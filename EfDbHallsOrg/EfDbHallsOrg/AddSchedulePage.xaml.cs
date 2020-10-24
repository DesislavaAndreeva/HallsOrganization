using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EfDbHallsOrg
{
    /// <summary>
    /// Interaction logic for AddSchedulePage.xaml
    /// </summary>
    public partial class AddSchedulePage : Page
    {
        public AddSchedulePage()
        {
            InitializeComponent();

            var db = new DbHallContext();

            var halls = db.Halls;
            foreach (var hall in halls.ToList())
                halls_combo_box.Items.Add(hall.Hall_Name);

            for (int i = 7; i < 12; i++)
             hours_combo_box.Items.Add(i + ":00 AM");

            for (int i = 12; i < 21; i++)
                hours_combo_box.Items.Add(i + ":00 PM");
        }

        private int checkScheduleInput()
        {
            if (halls_combo_box.SelectedIndex == -1)
            {
                MessageBox.Show("Select hall, if there are no options add halls!");
                return 1;
            }

            if (date.SelectedDate == null)
            {
                MessageBox.Show("Select date!");
                return 1;
            }

            if (hours_combo_box.SelectedIndex == -1)
            {
                MessageBox.Show("Select hour!");
                return 1;
            }

            if (teacher_egn.Text == String.Empty)
            {
                MessageBox.Show("Enter your EGN!");
                return 1;
            }

            if (Regex.IsMatch(teacher_egn.Text, "\\A[0-9]{10}\\z") == false)
            {
                MessageBox.Show("Enter valid EGN!");
                return 1;
            }

            return 0;
        }

        private bool checkIfReservationExists()
        {
            var db = new DbHallContext();

            var result = (from s in db.Schedules
                          join t in db.Teachers on s.TeacherId equals t.TeacherId
                          join h in db.Halls on s.HallId equals h.HallId
                          where (DateTime.Compare(date.SelectedDate.Value, s.Date_of_occupation) == 0)
                          && s.Hour == hours_combo_box.SelectedItem.ToString()
                          && (t.Egn == teacher_egn.Text) && (h.Hall_Name == halls_combo_box.SelectedItem.ToString())
                          select s).ToList();

            var result2 = (from s in db.Schedules
                           join h in db.Halls on s.HallId equals h.HallId
                           where (DateTime.Compare(date.SelectedDate.Value, s.Date_of_occupation) == 0)
                           && s.Hour == hours_combo_box.SelectedItem.ToString()
                           select s).ToList();

            if (result.Count > 0 || result2.Count > 0)
                return true;

            return false;
        }

        private int findReservationId()
        {
            var db = new DbHallContext();

            var result = (from s in db.Schedules
                          join t in db.Teachers on s.TeacherId equals t.TeacherId
                          join h in db.Halls on s.HallId equals h.HallId
                          where (DateTime.Compare(date.SelectedDate.Value, s.Date_of_occupation) == 0)
                          && s.Hour == hours_combo_box.SelectedItem.ToString()
                          && (t.Egn == teacher_egn.Text) && (h.Hall_Name == halls_combo_box.SelectedItem.ToString())
                          select s).SingleOrDefault();

            if (result != null)
                return result.ScheduleId;

            return 0;
        }

        private void Button_Click_Make_Reservation (object sender, RoutedEventArgs e)
        {
            if (checkScheduleInput() == 1)
                return;

            if (checkIfReservationExists() == true)
            {
                MessageBox.Show("Reservation done for selected date and time!");
                return;
            }

            if (Teacher.CheckIfTeacherExists(teacher_egn.Text) == false)
            {
                MessageBox.Show("Teacher with this EGN doesn't exists!");
                return;
            }

            var db = new DbHallContext();

            var tid = (from d in db.Teachers where d.Egn == teacher_egn.Text select d.TeacherId).Single();
            var hid = (from h in db.Halls where h.Hall_Name == halls_combo_box.SelectedItem.ToString() select h.HallId).Single();

            var schedule = new Schedule
            {
                Date_of_occupation = date.SelectedDate.Value,
                Day_of_the_week = date.SelectedDate.Value.DayOfWeek.ToString(),
                Hour = hours_combo_box.SelectedItem.ToString(),
                Month = date.SelectedDate.Value.Month.ToString(),
                Year = date.SelectedDate.Value.Year.ToString(),
                TeacherId = tid,
                HallId = hid
            };

            db.Schedules.Add(schedule);
            db.SaveChanges();

            var email = (from d in db.Teachers where d.Egn == teacher_egn.Text select d.Email).Single();
            sendAutoMail(email, true, halls_combo_box.SelectedItem.ToString(), date.SelectedDate.Value.DayOfWeek.ToString(), date.SelectedDate.Value, hours_combo_box.SelectedItem.ToString());

            MessageBox.Show("Reservation done successfully, check your email!");
        }

        //don't forget to turn on access for not secure application for sender meil
        private void sendAutoMail (string receiver_email, bool isRes, string hall_name, string day, DateTime date, string hour)
        {
            const string username = "xxxx@gmail.com"; /*If you test add gmail sender here*/
            const string password = "xxxxx"; /*If you test add gmail sender password here*/

            SmtpClient smtpclient = new SmtpClient();
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            MailAddress fromaddress = new MailAddress(username);

            smtpclient.Host = "smtp.gmail.com";
            smtpclient.UseDefaultCredentials = false;
            smtpclient.EnableSsl = true;
            smtpclient.Port = 587;
            mail.From = fromaddress;

            mail.To.Add(receiver_email);
            mail.IsBodyHtml = true;

            if (isRes == true)
            {
                mail.Subject = "Hall reservation!";
                mail.Body = ("Successfull reservation for hall " + hall_name + " on " + day + " " + date.Date.ToString("yyyy-MM-dd") + " " + hour);
            }
            else
            {
                mail.Subject = "Hall cancelation!";
                mail.Body = ("Successfull cancelation for hall " + hall_name + " on " + day + " " + date.Date.ToString("yyyy-MM-dd") + " " + hour);
            }

            smtpclient.Credentials = new System.Net.NetworkCredential(username, password);

            try
            {
                smtpclient.Send(mail);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void Button_Click_Delete_Reservation (object sender, RoutedEventArgs e)
        {
            if (checkScheduleInput() == 1)
                return;

            if (checkIfReservationExists() == false)
             {
                 MessageBox.Show("Schedule with selected parameters doesn't exists!");
                 return;
             }

            int id = findReservationId();
            if ( id != 0 )
            {
                using (var db = new DbHallContext())
                {
                    var schedule = (from d in db.Schedules where d.ScheduleId == id select d).Single();

                    db.Schedules.Remove(schedule);
                    db.SaveChanges();

                    var email = (from d in db.Teachers where d.Egn == teacher_egn.Text select d.Email).Single();
                    sendAutoMail(email, false, halls_combo_box.SelectedItem.ToString(), date.SelectedDate.Value.DayOfWeek.ToString(), date.SelectedDate.Value, hours_combo_box.SelectedItem.ToString());
                    MessageBox.Show("Reservation canceled successfully, check your email!");
                }
            }
        }
    }
}
