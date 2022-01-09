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
using System.Data.Entity;
using Schedule_Administration;
using System.Data;
using static Employee_Schedule.Validation;

namespace Employee_Schedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AdministrationModel ctx = new AdministrationModel();
        CollectionViewSource employeeVSource;
        CollectionViewSource timetableVSource;
        CollectionViewSource employeeSchedulesVSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            employeeVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("employeeViewSource")));
            employeeVSource.Source = ctx.Employees.Local;
            ctx.Employees.Load();

            employeeSchedulesVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("employeeSchedulesViewSource")));
            //employeesScheduleVSource.Source = ctx.Schedules.Local;
            ctx.Schedules.Load();
            ctx.Timetables.Load();
            cmbEmployees.ItemsSource = ctx.Employees.Local;
            //cmbEmployees.DisplayMemberPath = "First Name";
            cmbEmployees.SelectedValuePath = "Id";

            cmbTimetable.ItemsSource = ctx.Timetables.Local;
            //cmbTimetable.DisplayMemberPath = "Close Hour";
            cmbTimetable.SelectedValuePath = "IdTime";

            BindDataGrid();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(identityNrTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(contractTypeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(salaryTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(openHourTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(openHourTextBox, TextBox.TextProperty);
            SetValidationBinding();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(identityNrTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(contractTypeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(salaryTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(openHourTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(openHourTextBox, TextBox.TextProperty);
            SetValidationBinding();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            employeeVSource.View.MoveCurrentToPrevious();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            employeeVSource.View.MoveCurrentToNext();
        }
        private void SaveEmployees()
        {
            Employee employee = null;
            if(action == ActionState.New)
            {
                try
                {
                    employee = new Employee()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim(),
                        IdentityNr = identityNrTextBox.Text.Trim(),
                        ContractType = contractTypeTextBox.Text.Trim(),
                        Salary = int.Parse("0"+salaryTextBox.Text)
                    };
                    ctx.Employees.Add(employee);
                    employeeVSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if(action == ActionState.Edit)
            {
                try
                {
                    employee = (Employee)employeeDataGrid.SelectedItem;
                    employee.FirstName = firstNameTextBox.Text.Trim();
                    employee.LastName = lastNameTextBox.Text.Trim();
                    employee.IdentityNr = identityNrTextBox.Text.Trim();
                    employee.ContractType = contractTypeTextBox.Text.Trim();
                    int result = 0;
                    if(int.TryParse(salaryTextBox.Text.Trim(), out result)) employee.Salary = result;
                    ctx.SaveChanges();
                    employeeVSource.View.Refresh();
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if(action == ActionState.Delete)
            {
                try
                {
                    employee = (Employee)employeeDataGrid.SelectedItem;
                    ctx.Employees.Remove(employee);
                    ctx.SaveChanges();
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                employeeVSource.View.Refresh();
            }
        }
        private void SaveTimetable()
        {
            Timetable timetable = null;
            if(action == ActionState.New)
            {
                try
                {
                    timetable = new Timetable()
                    {
                        OpenHour = TimeSpan.Parse(openHourTextBox.Text.Trim()),
                        CloseHour = TimeSpan.Parse(closeHourTextBox.Text.Trim())
                    };
                    ctx.Timetables.Add(timetable);
                    timetableVSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if(action == ActionState.Edit)
            {
                try
                {
                    timetable = (Timetable)timetableDataGrid.SelectedItem;
                    timetable.OpenHour = TimeSpan.Parse(openHourTextBox.Text.Trim());
                    timetable.CloseHour = TimeSpan.Parse(closeHourTextBox.Text.Trim());
                    ctx.SaveChanges();
                    timetableVSource.View.Refresh();
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if(action == ActionState.Delete)
            {
                try
                {
                    timetable = (Timetable)timetableDataGrid.SelectedItem;
                    ctx.Timetables.Remove(timetable);
                    ctx.SaveChanges();
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                timetableVSource.View.Refresh();
            }
        }
        private void gbOperations_Click(object sender, RoutedEventArgs e)
        {
            Button SelectedButton = (Button)e.OriginalSource;
            Panel panel = (Panel)SelectedButton.Parent;
            foreach(Button B in panel.Children.OfType<Button>())
            {
                if (B != SelectedButton)
                    B.IsEnabled = false;
            }
            gbActions.IsEnabled = true;
        }
        private void Reinitialize()
        {
            Panel panel = gbOperations.Content as Panel;
            foreach(Button B in panel.Children.OfType<Button>())
            {
                B.IsEnabled = true;
            }
            gbActions.IsEnabled = false;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Reinitialize();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = tbCtrlEmpSch.SelectedItem as TabItem;
            switch (ti.Header)
            {
                case "Employees":
                    SaveEmployees();
                    break;
                case "Timetable":
                    SaveTimetable();
                    break;
                case "Schedule":
                    break;
            }
            Reinitialize();
        }
        private void SaveSchedule()
        {
            Schedule schedule = null;
            if(action == ActionState.New)
            {
                try
                {
                    Employee employee = (Employee)cmbEmployees.SelectedItem;
                    Timetable timetable = (Timetable)cmbTimetable.SelectedItem;
                    schedule = new Schedule()
                    {
                        EmployeeID = employee.Id,
                        TimetableID = timetable.IdTime
                    };
                    ctx.Schedules.Add(schedule);
                    ctx.SaveChanges();
                    BindDataGrid();
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Edit)
            {
                dynamic selectedSchedule = schedulesDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedSchedule.IdSch;
                    var editedSchedule = ctx.Schedules.FirstOrDefault(s => s.IdSch == curr_id);
                    if(editedSchedule != null)
                    {
                        editedSchedule.EmployeeID = Int32.Parse(cmbEmployees.SelectedValue.ToString());
                        editedSchedule.TimetableID = Convert.ToInt32(cmbTimetable.SelectedValue.ToString());
                        ctx.SaveChanges();
                    }
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                employeeSchedulesVSource.View.MoveCurrentTo(selectedSchedule);
            }
            else if(action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedSchedule = schedulesDataGrid.SelectedItem;
                    int curr_id = selectedSchedule.IdSch;
                    var deletedSchedule = ctx.Schedules.FirstOrDefault(s => s.IdSch == curr_id);
                    if(deletedSchedule != null)
                    {
                        ctx.Schedules.Remove(deletedSchedule);
                        ctx.SaveChanges();
                        MessageBox.Show("Schedule deleted successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void BindDataGrid()
        {
            var querySchedule = from sch in ctx.Schedules
                                join emp in ctx.Employees on sch.EmployeeID equals
                                emp.Id join tmt in ctx.Timetables on sch.TimetableID
                                equals tmt.IdTime select new { sch.IdSch, sch.EmployeeID, 
                                    sch.TimetableID, emp.FirstName, emp.LastName, tmt.OpenHour, tmt.CloseHour };
            employeeSchedulesVSource.Source = querySchedule.ToList();
        }
        private void SetValidationBinding()
        {
            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = employeeVSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameValidationBinding);

            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = employeeVSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameValidationBinding);

            Binding contractTypeValidationBinding = new Binding();
            contractTypeValidationBinding.Source = employeeVSource;
            contractTypeValidationBinding.Path = new PropertyPath("ContractType");
            contractTypeValidationBinding.NotifyOnValidationError = true;
            contractTypeValidationBinding.Mode = BindingMode.TwoWay;
            contractTypeValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            contractTypeValidationBinding.ValidationRules.Add(new StringMaxLengthValidator());
            contractTypeTextBox.SetBinding(TextBox.TextProperty, contractTypeValidationBinding);
        }
    }
}
