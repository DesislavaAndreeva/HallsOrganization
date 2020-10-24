using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for AddTeachersPage.xaml
    /// </summary>
    public partial class AddTeachersPage : System.Windows.Controls.Page
    {
        public AddTeachersPage()
        {
            InitializeComponent();
        }

        private int checkTeacherInput()
        {
            if (teacher_name.Text == String.Empty)
            {
                MessageBox.Show("Enter teacher name!");
                return 1;
            }

            if (Regex.IsMatch(teacher_name.Text, @"^[a-zA-Z\s]+$") == false)
            {
                MessageBox.Show("Enter valid teacher name!");
                return 1;
            }

            if (faculty.Text == String.Empty)
            {
                MessageBox.Show("Enter faculty name!");
                return 1;
            }

            if (Regex.IsMatch(faculty.Text, @"^[A-Z]+$") == false)
            {
                MessageBox.Show("Enter valid faculty name!");
                return 1;
            }

            if (department.Text == String.Empty)
            {
                MessageBox.Show("Enter department name!");
                return 1;
            }

            if (Regex.IsMatch(department.Text, @"^[a-zA-Z\s]+$") == false)
            {
                MessageBox.Show("Enter valid department name!");
                return 1;
            }

            if (degree_title.Text == String.Empty)
            {
                MessageBox.Show("Enter degree title!");
                return 1;
            }

            if (Regex.IsMatch(degree_title.Text, "^[a-zA-Z.]+$") == false)
            {
                MessageBox.Show("Enter valid degree title!");
                return 1;
            }

            if (email.Text == String.Empty)
            {
                MessageBox.Show("Enter email!");
                return 1;
            }

            if(Teacher.IsValidEmail(email.Text) == false)
            {
                MessageBox.Show("Enter valid email!");
                return 1;
            }

            if (egn.Text == String.Empty)
            {
                MessageBox.Show("Enter EGN!");
                return 1;
            }

            if (Regex.IsMatch(egn.Text, "\\A[0-9]{10}\\z") == false)
            {
                MessageBox.Show("Enter valid EGN!");
                return 1;
            }

            return 0;
        }

        private void Button_Click_Delete_Teacher(object sender, RoutedEventArgs e)
        {
            var db = new DbHallContext();

            if (egn.Text == String.Empty && Regex.IsMatch(egn.Text, "\\A[0-9]{10}\\z"))
            {
                MessageBox.Show("Enter valid EGN!");
                return;
            }

            if (Teacher.CheckIfTeacherExists(egn.Text) == true)
            {
                var hall_schedules = (from s in db.Schedules
                                      join t in db.Teachers on s.TeacherId equals t.TeacherId
                                      where t.Egn == egn.Text
                                      select s).ToList();

                foreach (var schedule in hall_schedules)
                {
                    db.Schedules.Remove(schedule);
                    db.SaveChanges();
                }

                var teacher = (from d in db.Teachers where d.Egn == egn.Text select d).Single();
                db.Teachers.Remove(teacher);
                db.SaveChanges();
            }

            MessageBox.Show("Teacher removed successfully!");
        }

        private void Button_Click_Add_Teacher(object sender, RoutedEventArgs e)
        {
            if (checkTeacherInput() == 1)
                return;

            if (Teacher.CheckIfTeacherExists(egn.Text) == true)
            {
                MessageBox.Show("Teacher already exists!");
                return;
            }

            using (var db = new DbHallContext())
            {
                var teacher = new Teacher
                {
                    Full_Name = teacher_name.Text,
                    Egn = egn.Text,
                    Faculty = faculty.Text,
                    Department = department.Text,
                    Email = email.Text,
                    Degree_Title = degree_title.Text
                };

                db.Teachers.Add(teacher);
                db.SaveChanges();
            }

            MessageBox.Show("Teacher added successfully!");
        }

    }
    
}
