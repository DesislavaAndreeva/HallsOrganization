using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Net.Mail;
using System.Transactions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Data;

namespace EfDbHallsOrg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page
    {
        public MainWindow()
        {
            InitializeComponent();
            DbHallContext db = new DbHallContext();
            var halls = db.Halls;
            foreach (var hall in halls.ToList())
                hall_box.Items.Add(hall.Hall_Name);
        }

        private void schedulesNavigate(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("AddSchedulePage.xaml", UriKind.Relative));
        }

        private void teachersNavigate(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("AddTeachersPage.xaml", UriKind.Relative));
        }

        private void hallsNavigate(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("AddHallsPage.xaml", UriKind.Relative));
        }

        private void setTableViewForSchedule()
        {
            resultGrid.Columns[0].Visibility = System.Windows.Visibility.Hidden;
            resultGrid.Columns[3].Visibility = System.Windows.Visibility.Hidden;
            resultGrid.Columns[4].Visibility = System.Windows.Visibility.Hidden;
            resultGrid.Columns[6].Visibility = System.Windows.Visibility.Hidden;
            resultGrid.Columns[7].Visibility = System.Windows.Visibility.Hidden;
            resultGrid.Columns[8].Visibility = System.Windows.Visibility.Hidden;
            resultGrid.Columns[9].Visibility = System.Windows.Visibility.Hidden;

            resultGrid.Columns[1].Header = "Date";
            resultGrid.Columns[2].Header = "Day";
            resultGrid.Columns[5].Header = "Hour";
        }

        private int searchByNameAndEgn()
        {
            var db = new DbHallContext();

            if (oderByDate.IsChecked == true)
            {
                var hall_schedules = (from s in db.Schedules
                                        join t in db.Teachers on s.TeacherId equals t.TeacherId
                                        join h in db.Halls on s.HallId equals h.HallId
                                        where h.Hall_Name == hall_box.SelectedItem.ToString() && (t.Egn == egn_box.Text)
                                        orderby (s.Date_of_occupation)
                                        select s).ToList();

                resultGrid.ItemsSource = null;
                resultGrid.ItemsSource = hall_schedules;
                setTableViewForSchedule();
                exportQueryToPdf(hall_schedules);
                return 1;
            }

            if (oderByDate.IsChecked == false)
            {
                var hall_schedules = (from s in db.Schedules
                                        join t in db.Teachers on s.TeacherId equals t.TeacherId
                                        join h in db.Halls on s.HallId equals h.HallId
                                        where h.Hall_Name == hall_box.SelectedItem.ToString() && (t.Egn == egn_box.Text)
                                        orderby (s.Hour)
                                        select s).ToList();

                resultGrid.ItemsSource = null;
                resultGrid.ItemsSource = hall_schedules;
                setTableViewForSchedule();
                exportQueryToPdf(hall_schedules);
                return 1;
            }

            return 0;
        }

        private int searchAll()
        {
            var db = new DbHallContext();

            if (oderByDate.IsChecked == true)
            {
                var hall_schedules = (from s in db.Schedules
                                        join t in db.Teachers on s.TeacherId equals t.TeacherId
                                        join h in db.Halls on s.HallId equals h.HallId
                                        orderby (s.Date_of_occupation)
                                        select s).ToList();

                exportQueryToPdf(hall_schedules);
                resultGrid.ItemsSource = null;
                resultGrid.ItemsSource = hall_schedules;
                setTableViewForSchedule();
                return 1;
            }

            if (oderByDate.IsChecked == false)
            {
                var hall_schedules = (from s in db.Schedules
                                        join t in db.Teachers on s.TeacherId equals t.TeacherId
                                        join h in db.Halls on s.HallId equals h.HallId
                                        select s).ToList();

                exportQueryToPdf(hall_schedules);
                resultGrid.ItemsSource = null;
                resultGrid.ItemsSource = hall_schedules;
                setTableViewForSchedule();
                return 1;
            }

            return 0;
        }

        private int searchByHallName()
        {
            var db = new DbHallContext();
            if (oderByDate.IsChecked == true)
            {
                var hall_schedules = (from s in db.Schedules
                                        join t in db.Teachers on s.TeacherId equals t.TeacherId
                                        join h in db.Halls on s.HallId equals h.HallId
                                        where h.Hall_Name == hall_box.SelectedItem.ToString()
                                        orderby (s.Date_of_occupation)
                                        select s).ToList();

                exportQueryToPdf(hall_schedules);
                resultGrid.ItemsSource = null;
                resultGrid.ItemsSource = hall_schedules;
                setTableViewForSchedule();
                return 1;
            }

            if (oderByDate.IsChecked == false)
            {
                var hall_schedules = (from s in db.Schedules
                                        join t in db.Teachers on s.TeacherId equals t.TeacherId
                                        join h in db.Halls on s.HallId equals h.HallId
                                        where h.Hall_Name == hall_box.SelectedItem.ToString()
                                        select s).ToList();

                exportQueryToPdf(hall_schedules);
                resultGrid.ItemsSource = null;
                resultGrid.ItemsSource = hall_schedules;
                setTableViewForSchedule();
                return 1;
            }

            return 0;
        }

        private int searchByEgn()
        {
            var db = new DbHallContext();

            if (oderByDate.IsChecked == true)
            {
                var hall_schedules = (from s in db.Schedules
                                        join t in db.Teachers on s.TeacherId equals t.TeacherId
                                        join h in db.Halls on s.HallId equals h.HallId
                                        where t.Egn == egn_box.Text
                                        orderby (s.Date_of_occupation)
                                        select s).ToList();

                exportQueryToPdf(hall_schedules);
                resultGrid.ItemsSource = null;
                resultGrid.ItemsSource = hall_schedules;
                setTableViewForSchedule();
                return 1;
            }

            if (oderByDate.IsChecked == false)
            {
                var hall_schedules = (from s in db.Schedules
                                        join t in db.Teachers on s.TeacherId equals t.TeacherId
                                        join h in db.Halls on s.HallId equals h.HallId
                                        where t.Egn == egn_box.Text
                                        select s).ToList();

                exportQueryToPdf(hall_schedules);
                resultGrid.ItemsSource = null;
                resultGrid.ItemsSource = hall_schedules;
                setTableViewForSchedule();
                return 1;
            }

            return 0;
        }

        private void Button_Click_Search_Results(object sender, RoutedEventArgs e)
        {
            bool isEgnSelected = false;
            bool isHallSelected = false;

            if (hall_box.SelectedIndex > -1)
                isHallSelected = true;

            if (egn_box.Text != String.Empty)
            {
                if (Regex.IsMatch(egn_box.Text, "\\A[0-9]{10}\\z") == false)
                {
                    MessageBox.Show("Enter valid EGN!");
                    return;
                }

                isEgnSelected = true;
            }

            if (date_box.SelectedDate != null && isHallSelected == true)
            {
                var db = new DbHallContext();

                var hcnt = (from s in db.Schedules
                            join h in db.Halls on s.HallId equals h.HallId
                            where h.Hall_Name == hall_box.SelectedItem.ToString()
                            && (DateTime.Compare(date_box.SelectedDate.Value, s.Date_of_occupation) == 0)
                            select s).Count();
                
                MessageBox.Show("Hall " + hall_box.SelectedItem.ToString() + " has " + hcnt + " schedules assigned at " + date_box.SelectedDate.Value.ToString("yyyy-MM-dd"));
            }

            if (date_box.SelectedDate != null && isEgnSelected == true)
            {
               var ctx = new DbHallContext();

                var teacher = (from h in ctx.Teachers where h.Egn == egn_box.Text select h).Single();

                var tcnt = (from s in ctx.Schedules
                           join t in ctx.Teachers on s.TeacherId equals t.TeacherId
                           where t.Egn == egn_box.Text
                            && (DateTime.Compare(date_box.SelectedDate.Value, s.Date_of_occupation) == 0)
                            select s).Count();

                MessageBox.Show("Teacher " + teacher.Full_Name + " has " + tcnt + " schedules assigned at " + date_box.SelectedDate.Value.ToString("yyyy-MM-dd"));
            }

            if (isEgnSelected == true && isHallSelected == true)
            {
                if (searchByNameAndEgn() == 1)
                    return;
            }

            if (isEgnSelected == false && isHallSelected == false)
            {
                if (searchAll() == 1)
                    return;
            }

            if (isEgnSelected == false && isHallSelected == true)
            {
                if (searchByHallName() == 1)
                    return;
            }

            if (isEgnSelected == true && isHallSelected == false)
            {
                if (searchByEgn() == 1)
                    return;
            }
        }

        private void exportQueryToPdf(List<Schedule> schedules)
        {
            string file_name = "C:\\Users\\Public\\query.pdf";

            if (File.Exists(file_name))
            {
                File.SetAttributes(file_name, FileAttributes.Normal);
                try
                {
                    File.Delete(file_name);
                }
                catch (UnauthorizedAccessException)
                {
                    return;
                }
           
            }

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(file_name, FileMode.Create));

            doc.Open();
            PdfPTable pdfTable = new PdfPTable(5);
            pdfTable.AddCell(new Phrase("Day of the week"));
            pdfTable.AddCell(new Phrase("Day"));
            pdfTable.AddCell(new Phrase("Month"));
            pdfTable.AddCell(new Phrase("Year"));
            pdfTable.AddCell(new Phrase("Hour"));

            for (int i = 0; i < schedules.Count; i++)
            {
                pdfTable.AddCell(new Phrase(schedules[i].Day_of_the_week));
                pdfTable.AddCell(new Phrase(schedules[i].Date_of_occupation.Day.ToString()));
                pdfTable.AddCell(new Phrase(schedules[i].Month));
                pdfTable.AddCell(new Phrase(schedules[i].Year));
                pdfTable.AddCell(new Phrase(schedules[i].Hour));
            }

            doc.Add(pdfTable);
            doc.Close();
        }

        private void Button_Click_Generate_Pdf_File(object sender, RoutedEventArgs e)
        {
            if(pdf_file_name.Text == String.Empty)
            {
                MessageBox.Show("Enter PDF file name!");
                return;
            }

            if (Regex.IsMatch(pdf_file_name.Text, "^[a-zA-Z0-9]+.pdf$") == false)
            {
                MessageBox.Show("Enter valid file name (.pdf)!");
                return;
            }

            string file_name = "C:\\Users\\Public\\" + pdf_file_name.Text;
            if(File.Exists(file_name))
            {
                MessageBox.Show("File with that name already exists, enter another one!");
                return;
            }

            var context = new DbHallContext();
            var halls = context.Halls.ToList();
            var teachers = context.Teachers.ToList();
            var schedules = context.Schedules.ToList();

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(file_name, FileMode.Create));

            doc.Open();//Open Document to write

            Paragraph paragraphT = new Paragraph("Teachers data exported!");
            Paragraph paragraphH = new Paragraph("Halls data exported!");
            Paragraph paragraphS = new Paragraph("Schedules data exported!");
            PdfPTable pdfTableHalls = new PdfPTable(6);
            PdfPTable pdfTableSchedules = new PdfPTable(7);
            PdfPTable pdfTableTeachers = new PdfPTable(6);

            pdfTableHalls.DefaultCell.Padding = 3;
            pdfTableHalls.WidthPercentage = 100;
            pdfTableHalls.HorizontalAlignment = Element.ALIGN_LEFT;

            pdfTableSchedules.DefaultCell.Padding = 3;
            pdfTableSchedules.WidthPercentage = 100;
            pdfTableSchedules.HorizontalAlignment = Element.ALIGN_LEFT;

            pdfTableTeachers.DefaultCell.Padding = 3;
            pdfTableTeachers.WidthPercentage = 100;
            pdfTableTeachers.HorizontalAlignment = Element.ALIGN_LEFT;

            pdfTableTeachers.AddCell(new Phrase("Name"));
            pdfTableTeachers.AddCell(new Phrase("EGN"));
            pdfTableTeachers.AddCell(new Phrase("Faculty"));
            pdfTableTeachers.AddCell(new Phrase("Degree"));
            pdfTableTeachers.AddCell(new Phrase("Department"));
            pdfTableTeachers.AddCell(new Phrase("Email"));

            for (int i = 0; i < teachers.Count; i++)
            {
                pdfTableTeachers.AddCell(new Phrase(teachers[i].Full_Name));
                pdfTableTeachers.AddCell(new Phrase(teachers[i].Egn));
                pdfTableTeachers.AddCell(new Phrase(teachers[i].Faculty));
                pdfTableTeachers.AddCell(new Phrase(teachers[i].Degree_Title));
                pdfTableTeachers.AddCell(new Phrase(teachers[i].Department));
                pdfTableTeachers.AddCell(new Phrase(teachers[i].Email));
            }

            pdfTableHalls.AddCell(new Phrase("Name"));
            pdfTableHalls.AddCell(new Phrase("Seats"));
            pdfTableHalls.AddCell(new Phrase("Computers"));
            pdfTableHalls.AddCell(new Phrase("Board"));
            pdfTableHalls.AddCell(new Phrase("Internet"));
            pdfTableHalls.AddCell(new Phrase("Projector"));

            for (int i = 0; i < halls.Count; i++)
            {
                pdfTableHalls.AddCell(new Phrase(halls[i].Hall_Name));
                pdfTableHalls.AddCell(new Phrase(halls[i].Capacity.ToString()));
                pdfTableHalls.AddCell(new Phrase(halls[i].Comp_Count.ToString()));
                pdfTableHalls.AddCell(new Phrase(halls[i].Board_Color));
                pdfTableHalls.AddCell(new Phrase(halls[i].Internet.ToString()));
                pdfTableHalls.AddCell(new Phrase(halls[i].Projector.ToString()));
            }

            pdfTableSchedules.AddCell(new Phrase("Day of the week"));
            pdfTableSchedules.AddCell(new Phrase("Day"));
            pdfTableSchedules.AddCell(new Phrase("Month"));
            pdfTableSchedules.AddCell(new Phrase("Year")); 
            pdfTableSchedules.AddCell(new Phrase("Hour"));
            pdfTableSchedules.AddCell(new Phrase("Teacher Id"));
            pdfTableSchedules.AddCell(new Phrase("Hall Id"));
            for (int i = 0; i < schedules.Count; i++)
            {
                pdfTableSchedules.AddCell(new Phrase(schedules[i].Day_of_the_week));
                pdfTableSchedules.AddCell(new Phrase(schedules[i].Date_of_occupation.Day.ToString()));
                pdfTableSchedules.AddCell(new Phrase(schedules[i].Month));
                pdfTableSchedules.AddCell(new Phrase(schedules[i].Year));
                pdfTableSchedules.AddCell(new Phrase(schedules[i].Hour));
                pdfTableSchedules.AddCell(new Phrase(schedules[i].TeacherId.ToString()));
                pdfTableSchedules.AddCell(new Phrase(schedules[i].HallId.ToString()));
            }

            doc.Add(paragraphH);
            doc.Add(pdfTableHalls);
            doc.Add(paragraphT);
            doc.Add(pdfTableTeachers);
            doc.Add(paragraphS);
            doc.Add(pdfTableSchedules);
            doc.Close(); //Close document

            MessageBox.Show("PDF created successfully at " + file_name);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var context = new DbHallContext();
            var halls = context.Halls;

            resultGrid.ItemsSource = null;
            resultGrid.ItemsSource = halls.ToList();

            resultGrid.Columns[0].Visibility = System.Windows.Visibility.Hidden;
            resultGrid.Columns[1].Header = "Hall";
            resultGrid.Columns[2].Header = "Seats";
            resultGrid.Columns[5].Header = "Computers";
            resultGrid.Columns[6].Header = "Board";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var context = new DbHallContext();
            var t = context.Teachers;
            if (t == null)
                return;
            resultGrid.ItemsSource = null;
            resultGrid.ItemsSource = t.ToList();

            resultGrid.Columns[0].Visibility = System.Windows.Visibility.Hidden;
            resultGrid.Columns[1].Header = "EGN";
            resultGrid.Columns[2].Header = "Name";
            resultGrid.Columns[3].Header = "Faculty";
            resultGrid.Columns[4].Header = "Degree";
            resultGrid.Columns[5].Header = "Department";
            resultGrid.Columns[6].Header = "Email";
        }

        private void Button_Click_BackupDB(object sender, RoutedEventArgs e)
        {
            //backup
            using (var context = new DbHallContext())
            {
                string dbname = "DbHallContext";
                string filename = "C:\\Users\\Public\\backup.bak";

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    context.Database.UseTransaction(null);
                    string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                    context.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction,
                        string.Format(sqlCommand, dbname, filename));
                }

                MessageBox.Show("Successfull Database backup created at " + filename);
            }
        }

        private void Button_Click_RestoreDB(object sender, RoutedEventArgs e)
        {
            //restore
            using (var context = new DbHallContext())
            {
                string dbname = "DbHallContext";
                string filename = "C:\\Users\\Public\\backup.bak";

                if (File.Exists(filename) == false)
                {
                    MessageBox.Show("Execute backup DB first!");
                    return;
                }

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    context.Database.UseTransaction(null);
                    string sqlCommand = @"RESTORE DATABASE dbname FROM DISK = 'C:\\Users\\Public\\backup.bak' WITH
                                            MOVE 'DbHallContext' TO 'C:\\Users\\Public\\dataDbHallContext.mdf',
                                            MOVE 'DbHallContext_log' TO 'C:\\Users\\Public\\dataDbHallContext.ldf', REPLACE";
                    context.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction,
                        string.Format(sqlCommand, dbname, filename));
                }

                MessageBox.Show("Successfull Database restore procedure from " + filename);
            }
        }
    }
}
