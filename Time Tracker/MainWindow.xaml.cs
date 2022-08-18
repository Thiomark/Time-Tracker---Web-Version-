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
using Time_Tracker.Pages;

namespace Time_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int loggedInUserId;                 // Varible to store the user ID from the login/register page 
        int numberOfWeeks;                  // Varible to store the number of weeks from the login/register page 
        string dateFromRegistation;         // Varible to store date from the login/register page 
        string currentUser;                 // Varible to store username from the login/register page 

        public MainWindow()
        {
            InitializeComponent();

            // string currentUser = "admin";
            // numberOfWeeks = 15;

            // Getting saved information form the login or the register page (The user ID)

            GetSettings();

            // Getting the current user

            TimeTrackerDB db = new TimeTrackerDB();

            var docs = from d in db.Users select new
            { 
                name = d.Name,
                username = d.Username,
                id = d.Id
            } ;
            // loggedInUserId = docs.Where(x => x.username == currentUser).Single().id;

            this.userName.Content = docs.Where(x => x.username == currentUser).Single().name;
            this.userUsername.Content = docs.Where(x => x.username == currentUser).Single().username;


            // Getting the current user semester info
            ShowSemesterInfo();

            // Getting the current user modules
            ShowListOfModules();


            // Getting the current user's hours spent on modules
            // ShowListOfModuleHours();
        }

        private void Add_Module_Button_Click(object sender, RoutedEventArgs e)
        {
            TimeTrackerDB db = new TimeTrackerDB();

            if (this.code.Text.Length > 0 && this.name.Text.Length > 0 
                && this.numOfCredits.Text.Length > 0 && this.classHours.Text.Length > 0)
            {
                int _credits;
                int _hours;

                if(int.TryParse(this.numOfCredits.Text, out _credits) && int.TryParse(this.classHours.Text, out _hours))
                {
                    if(_credits > 0 && _hours > 0)
                    {
                        try
                        {
                            Module newModule = new Module()
                            {
                                Name = this.name.Text,
                                Code = this.code.Text,
                                NumberOfCredits = _credits,
                                ClassHoursPerWeek = _hours,
                                UserId = loggedInUserId,
                                CreatedAt = DateTime.Now
                            };
                            db.Modules.Add(newModule);
                            db.SaveChanges();

                            this.code.Text = "";
                            this.name.Text = "";
                            this.numOfCredits.Text = "";
                            this.classHours.Text = "";

                            ShowListOfModules();
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Module credits or the module class hours per week should be bigger than 0");
                    }
                }
                else
                {
                    MessageBox.Show("Module credits or the module class hours per week should be a number");
                }
            }
            else
            {
                MessageBox.Show("Please enter all Fields!!");
            }

        }

        private void ShowListOfModules()
        {
            // Getting the current user modules
            try
            {
                TimeTrackerDB db = new TimeTrackerDB();

                var docs = from d in db.Modules
                           select new
                           {
                               Name = d.Name,
                               HoursOfSelfStudyPerWeek = (d.NumberOfCredits * 10 / numberOfWeeks) - d.ClassHoursPerWeek,
                               UserId = d.UserId,
                               Id = d.Id
                           };

                this.selectedModules.ItemsSource = docs.Where(x => x.UserId == loggedInUserId).ToList();
                //this._hours_modle_name.DisplayMemberPath = "Name";
                //this._hours_modle_name.SelectedValuePath = "Id";
                this._hours_modle_name.ItemsSource = docs.Where(x => x.UserId == loggedInUserId).ToList();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowListOfModuleHours()
        {
            // Getting the current user modules hours
            try
            {
                TimeTrackerDB db = new TimeTrackerDB();

                var docs = from d in db.Hours select d;
                this._hours_sumOfHours.Content = "0";
                if (docs.ToString().Length > 0)
                {
                    this.selectedModulesHoursList.ItemsSource = docs.Where(x => x.UserId == loggedInUserId).ToList();
                    // this._hours_sumOfHours.Content = docs.Sum(item => item.Hours);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowSemesterInfo()
        {
            // Getting the current user semester info
            try
            {
                TimeTrackerDB db = new TimeTrackerDB();

                var docs = from d in db.Semesters select d;
                Semester semester = docs.Where(x => x.UserId == loggedInUserId).Single();

                if (semester.ToString().Length > 0)
                {
                    this._numberOfWeeks.Content = semester.NumberOfWeeksInTheSemester.ToString();
                    this._firstWeekOfTheSemester.Content = semester.StartDateForTheFirstWeek.ToString();
                    numberOfWeeks = semester.NumberOfWeeksInTheSemester;

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void SelectModule()
        {
            // Getting the current user semester info
            int selectedItemId;
            Module selectedModule;
            int hoursOfSelfStudyAlreadyRecored = 0;       // Store sum of hours from Hours table in the database
            try
            {
                if(int.TryParse(this.selectedModules.SelectedValue.ToString(), out selectedItemId))
                {
                    TimeTrackerDB db = new TimeTrackerDB();

                    var docs = from d in db.Modules select d;
                    selectedModule = docs.Where(x => x.UserId == loggedInUserId && x.Id == selectedItemId).Single();

                    if (selectedModule.ToString().Length > 0)
                    {
                        this._selectedName.Content = selectedModule.Name;
                        this._selectedCode.Content = selectedModule.Code;
                        this._selectedNumOfCredits.Content = selectedModule.NumberOfCredits;
                        this._selectedClassHours.Content = selectedModule.ClassHoursPerWeek;
                        this._selectedCreatedAt.Content = selectedModule.CreatedAt;
                        this._selectedSelfStudy.Content = (selectedModule.NumberOfCredits * 10 / numberOfWeeks) - selectedModule.ClassHoursPerWeek;

                        // Calculating remaing hours of selfStudy perWeek
                        var allModuleHours = from d in db.Hours where d.Name.Equals(selectedModule.Name) select d;
                        
                        if (allModuleHours.ToList().Count > 0)
                        {
                            this._hours_sumOfHours.Content = allModuleHours.Sum(item => item.Hours);
                            hoursOfSelfStudyAlreadyRecored = allModuleHours.Where(x => x.UserId == loggedInUserId).Sum(item => item.Hours);
                        }
                        int numberOfDaysInAWeek = 7 * 24;
                        this._selectedHoursOfSelfStudyRemaing.Content = numberOfDaysInAWeek - hoursOfSelfStudyAlreadyRecored;

                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
       
        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            LoginPage login = new LoginPage();

            login.Show();
            this.Close();
        }

        private void SearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Select_Module_Event(object sender, SelectionChangedEventArgs e)
        {
            SelectModule();
        }

        private void Add_Hours_Button_Click(object sender, RoutedEventArgs e)
        {
            int numberOfHours;
            if (this._hours_module_date.SelectedDate.HasValue && this._hours_modle_name.Text.Length > 0 && this._hours_module_hours.Text.Length > 0) 
            { 

                if (int.TryParse(this._hours_module_hours.Text, out numberOfHours))
                {
                    if (numberOfHours > 0 )
                    {
                        try
                        {
                            TimeTrackerDB db = new TimeTrackerDB();

                            Hour newHours = new Hour()
                            {
                                Name = this._hours_modle_name.Text,
                                Hours = numberOfHours,
                                ModuleDate = this._hours_module_date.SelectedDate.Value.Date.ToString(),
                                UserId = loggedInUserId
                            };
                            db.Hours.Add(newHours);
                            db.SaveChanges();

                            MessageBox.Show("Saved!");

                            ShowListOfModuleHours();
                            ShowListOfModules();
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Module credits or the module class hours per week should be a number");
                    }
                }
                else
                {
                    MessageBox.Show("Hourse spent on the module has to be a number and greator than 0");
                }
            }
            else
            {
                if (!this._hours_module_date.SelectedDate.HasValue)
                {
                    MessageBox.Show($"Please select date");
                }
                else if(this._hours_modle_name.Text.Length == 0)
                {
                    MessageBox.Show("Please select a module form the drop down combo box");
                }
                else
                {
                    MessageBox.Show("Please enter Hourse spent on the module");
                }
            }
            // _hours_module_hours
        }

        void GetSettings()
        {
            loggedInUserId = Properties.Settings.Default.USERID;
            currentUser = Properties.Settings.Default.USERNAME;
            dateFromRegistation = Properties.Settings.Default.SEMESTERDATE;

            // dateFromReger.Properties.Settings

            // MessageBox.Show($"{loggedInUserId}");
        }

        private void Update_Semester_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _semeWeeks;
                
                // Validation
                if (this._semester_module_date.SelectedDate.HasValue || this._semester_numberOFModules.Text.Length > 0)
                {
                    TimeTrackerDB db = new TimeTrackerDB();
                    var r = from d in db.Semesters where d.Id == loggedInUserId select d;
                    foreach (var item in r)
                    {

                        if (this._semester_numberOFModules.Text.Length > 0)
                        {
                            if (int.TryParse(this._semester_numberOFModules.Text, out _semeWeeks))
                            {
                                // item.NumberOfWeeksInTheSemester = 13;
                                
                            }
                        }
                        if (this._semester_module_date.SelectedDate.HasValue)
                        {
                            // MessageBox.Show("!!");
                            item.StartDateForTheFirstWeek = _semester_module_date.SelectedDate.Value.Date.ToString();
                            db.SaveChanges();
                        }

                        //Thread thread2 = new Thread()
                        MessageBox.Show("Updated!!");
                    }

                    // Update Semester
                    ShowSemesterInfo();
                }
                else
                {
                    MessageBox.Show("Please atleast of of the fields!!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
    }
}
