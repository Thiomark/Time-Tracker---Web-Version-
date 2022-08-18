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
using System.Windows.Shapes;

namespace Time_Tracker.Pages
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            // Varibles to validate info for the user 
            int _txtNumberOfSemeterWeeks;
            try
            {
                if (txtEmail.Text.Equals("") || txtPassword.Password.Equals("") || 
                    txtName.Text.Equals("") || txtNumberOfSemeterWeeks.Text.Equals("") || !datePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Please enter your name, number of semester weeks, semester start date, email and password");
                }
                else
                {
                    
                    if (int.TryParse(this.txtNumberOfSemeterWeeks.Text, out _txtNumberOfSemeterWeeks))
                    {
                        TimeTrackerDB db = new TimeTrackerDB();

                        if (_txtNumberOfSemeterWeeks > 0)
                        {
                            try
                            {
                                User newUser = new User()
                                {
                                    Name = this.txtName.Text,
                                    Username = this.txtEmail.Text,
                                    Password = txtPassword.Password
                                };
                                db.Users.Add(newUser);

                                // Saving the user
                                db.SaveChanges();
                                

                                // Getting that created users ID
                                var docs = from d in db.Users
                                           select new
                                           {
                                               name = d.Name,
                                               username = d.Username,
                                               id = d.Id
                                           };
                                int loggedInUserId = docs.Where(x => x.username == this.txtEmail.Text).Single().id;

                                // Creating a semester
                                Semester newSemester = new Semester()
                                {
                                    UserId = loggedInUserId,
                                    NumberOfWeeksInTheSemester = _txtNumberOfSemeterWeeks,
                                    StartDateForTheFirstWeek = datePicker.SelectedDate.Value.Date.ToShortDateString()
                                };
                                db.Semesters.Add(newSemester);

                                Properties.Settings.Default.USERID = loggedInUserId;
                                Properties.Settings.Default.USERNAME = docs.Where(x => x.username == this.txtEmail.Text).Single().username;

                                Properties.Settings.Default.SEMETERWEEKS = _txtNumberOfSemeterWeeks;
                                Properties.Settings.Default.SEMESTERDATE = datePicker.SelectedDate.Value.Date.ToShortDateString();

                                db.SaveChanges();
                                Properties.Settings.Default.Save();

                                MessageBox.Show("Account Created!!");
                                MainWindow main = new MainWindow();

                                main.Show();
                                this.Close();
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Account exist, use a diffrent username!!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("number of semeter weeks should be bigger than 0");
                        }
                    }
                    else
                    {
                        MessageBox.Show("number of semeter weeks should be a number");
                    }
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            LoginPage login = new LoginPage();

            login.Show();
            this.Close();
        }
    }
}
