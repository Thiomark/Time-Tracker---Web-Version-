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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (txtEmail.Text.Equals("") || txtPassword.Password.Equals(""))
                {
                    MessageBox.Show("Please enter your email and password");
                }
                else
                {
                    TimeTrackerDB db = new TimeTrackerDB();

                    var docs = from d in db.Users where d.Username.Equals(txtEmail.Text) select d;
                    if(docs.ToString().Length > 0)
                    {
                        User semester = docs.Where(x => x.Username.Equals(txtEmail.Text) && x.Password.Equals(txtPassword.Password)).Single();
                        if(semester.ToString().Length > 0)
                        {
                            // Saving the setting so that the main page can have the user ID
                            Properties.Settings.Default.USERID = semester.Id;
                            Properties.Settings.Default.USERNAME = semester.Username;
                            
                            Properties.Settings.Default.Save();

                            MainWindow main = new MainWindow();

                            main.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Credentials!!!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Credentials!!!");
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Invalid Credentials!!!");
            }
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            RegisterPage register = new RegisterPage();

            register.Show();
            this.Close();
        }
    }
}
