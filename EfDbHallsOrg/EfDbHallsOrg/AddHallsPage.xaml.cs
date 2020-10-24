using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for AddHallsPage.xaml
    /// </summary>
    public partial class AddHallsPage : Page
    {
        public AddHallsPage()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        { }

        private void Button_Click_Add_Hall(object sender, RoutedEventArgs e)
        {
            bool internet = false;
            bool projector = false;
            string board_color;

            if (hall_name.Text == String.Empty)
            {
                MessageBox.Show("Enter hall name!");
                return;
            }

            if (int.Parse(hall_name.Text) < 1000 || int.Parse(hall_name.Text) > 2000)
            {
                MessageBox.Show("Enter valid hall name (1000 to 2000)!");
                return;
            }

            if (capacity.Text == String.Empty)
            {
                MessageBox.Show("Enter hall capacity!");
                return;
            }

            if (int.Parse(capacity.Text) < 10 || int.Parse(capacity.Text) > 200)
            {
                MessageBox.Show("Enter seats capacity (10 to 200 seats)!");
                return;
            }

            if (projector_yes_checkbox.IsChecked == false && projector_no_checkbox.IsChecked == false)
            {
                MessageBox.Show("Select if hall has projector!");
                return;
            }

            if (internet_yes_checkbox.IsChecked == false && internet_no_checkbox.IsChecked == false)
            {
                MessageBox.Show("Select if hall has internet conectivity!");
                return;
            }

            if (comp_count.Text == String.Empty)
            {
                MessageBox.Show("Enter hall computer count!");
                return;
            }

            if (int.Parse(comp_count.Text) > 50)
            {
                MessageBox.Show("Enter computer count (max 50 computers)!");
                return;
            }

            if (board_white.IsChecked == false && board_black.IsChecked == false)
            {
                MessageBox.Show("Select board color!");
                return;
            }

            if (projector_yes_checkbox.IsChecked == true)
                projector = true;
            else
                projector = false;

            if (internet_yes_checkbox.IsChecked == true)
                internet = true;
            else
                internet = false;

            if (board_white.IsChecked == true)
                board_color = "white";
            else
                board_color = "black";

            if (Hall.CheckIfHallNameExists(hall_name.Text) == false)
            {
                using (var db = new DbHallContext())
                {
                    var hall = new Hall
                    {
                        Hall_Name = hall_name.Text,
                        Capacity = int.Parse(capacity.Text),
                        Internet = internet,
                        Projector = projector,
                        Comp_Count = int.Parse(comp_count.Text),
                        Board_Color = board_color
                    };

                    db.Halls.Add(hall);
                    db.SaveChanges();
                }
            }

            MessageBox.Show("Hall added successfully!");
        }

        private void Button_Click_Delete_Hall(object sender, RoutedEventArgs e)
        {
            var db = new DbHallContext();

            if (hall_name.Text == String.Empty)
            {
                MessageBox.Show("Enter hall name to delete!");
                return;
            }

            if (Hall.CheckIfHallNameExists(hall_name.Text) == true)
            {
                var hall_schedules = (from s in db.Schedules
                                      join h in db.Halls on s.HallId equals h.HallId
                                      where h.Hall_Name == hall_name.Text
                                      select s).ToList();

                foreach (var schedule in hall_schedules)
                {
                    db.Schedules.Remove(schedule);
                    db.SaveChanges();
                }

                var hall = (from d in db.Halls where d.Hall_Name == hall_name.Text select d).Single();
                db.Halls.Remove(hall);
                db.SaveChanges();
            }

            MessageBox.Show("Hall removed successfully!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var context = new DbHallContext();

            if (hall_name.Text == String.Empty)
            {
                MessageBox.Show("Enter hall name!");
                return;
            }

            if (int.Parse(hall_name.Text) < 1000 || int.Parse(hall_name.Text) > 2000)
            {
                MessageBox.Show("Enter valid hall name (1000 to 2000)!");
                return;
            }

            if (capacity.Text != String.Empty)
            { 
                if (int.Parse(capacity.Text) >= 10 || int.Parse(capacity.Text) <= 200)
                {
                    var hall_capacity = (from d in context.Halls where d.Hall_Name == hall_name.Text select d.Capacity).SingleOrDefault();

                    if (hall_capacity != int.Parse(capacity.Text))
                    {
                        Hall.ChangeHallByName(hall_name.Text, (int)HallFields.CAPACITY, "", false, int.Parse(capacity.Text));
                        MessageBox.Show("Capacity of hall " + hall_name.Text + " updated to " + capacity.Text + " seats!");
                    }
                }  
            }

            if (comp_count.Text != String.Empty)
            {
                var hall_comp = (from d in context.Halls where d.Hall_Name == hall_name.Text select d.Comp_Count).SingleOrDefault();

                if (hall_comp != int.Parse(comp_count.Text))
                {
                    if (int.Parse(comp_count.Text) < 50)
                    {
                        Hall.ChangeHallByName(hall_name.Text, (int)HallFields.COMPUTERS, "", false, int.Parse(comp_count.Text));
                        MessageBox.Show("Computers of hall " + hall_name.Text + " updated to " + comp_count.Text);
                    }
                }
            }
        }
    } 
}
