using App_Library.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace App_Library
{
    /// <summary>
    /// Логика взаимодействия для Window_DeliveryMTO.xaml
    /// </summary>
    public partial class Window_DeliveryMTO : Window
    {
        //Ссылки на датасет и данные
        DataSet1 dataSet1;
        tb_delivery_mtsTableAdapter tb_Delivery_MtsTableAdapter;
        pr_delivery_mtsTableAdapter pr_Delivery_MtsTableAdapter;

        tb_type_mtsTableAdapter tb_Type_MtsTableAdapter;
        tb_employeesTableAdapter tb_EmployeesTableAdapter;
        public Window_DeliveryMTO()
        {
            InitializeComponent();
            dataSet1 = new DataSet1();
            tb_Delivery_MtsTableAdapter = new tb_delivery_mtsTableAdapter();
            tb_Delivery_MtsTableAdapter.Fill(dataSet1.tb_delivery_mts);
            pr_Delivery_MtsTableAdapter = new pr_delivery_mtsTableAdapter();
            pr_Delivery_MtsTableAdapter.Fill(dataSet1.pr_delivery_mts);

            tb_Type_MtsTableAdapter = new tb_type_mtsTableAdapter();
            tb_Type_MtsTableAdapter.Fill(dataSet1.tb_type_mts);
            tb_EmployeesTableAdapter = new tb_employeesTableAdapter();
            tb_EmployeesTableAdapter.Fill(dataSet1.tb_employees);

            datagrid.ItemsSource = dataSet1.pr_delivery_mts.DefaultView;
            datagrid.SelectedValuePath = "ID_Delivery";
            datagrid.CanUserAddRows = false;
            datagrid.CanUserDeleteRows = false;
            datagrid.SelectionMode = DataGridSelectionMode.Single;

            cb_type.ItemsSource = dataSet1.tb_type_mts.DefaultView;
            cb_type.SelectedValuePath = "ID_Type_MTS";
            cb_type.DisplayMemberPath = "Name_Type_MTS";
            cb_type.SelectedIndex = -1;
            cb_employee.ItemsSource = dataSet1.tb_employees.DefaultView;
            cb_employee.SelectedValuePath = "ID_Employee";
            cb_employee.SelectedIndex = -1;
        }
        ProjectHelper projectHelper = new ProjectHelper();
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb_count.Text == "") tb_count.Text = "1";
                if ((cb_employee.SelectedIndex != -1) && (cb_type.SelectedIndex != -1) && (tb_count.Text != "") && (tb_name.Text != "") && (Convert.ToInt32(tb_count.Text) > 0)  && (!tb_start.Text.Contains("_"))&&(!tb_start.Text.Contains("_")))
                {
                    int id_type = (int)cb_type.SelectedValue;
                    int id_employee = (int)cb_employee.SelectedValue;
                    int count = Convert.ToInt32(tb_count.Text);
                    string name = tb_name.Text;
                    DateTime start = DateTime.ParseExact(tb_start.Text, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(tb_end.Text, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    if (start < end)
                    {
                        tb_Delivery_MtsTableAdapter.Delivery_Insert(name, id_type, count, start, end, id_employee);
                        pr_Delivery_MtsTableAdapter.Fill(dataSet1.pr_delivery_mts);
                    }else
                    { MessageBox.Show(@"Ошибка! Проверьте даты!"); }
                }
                else { MessageBox.Show(@"Ошибка! Проверьте ввод данных!"); }

            }
            catch { MessageBox.Show(@"Ошибка!"); }
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb_count.Text == "") tb_count.Text = "1";
                if ((datagrid.SelectedItem != null) && (cb_employee.SelectedIndex != -1) && (cb_type.SelectedIndex != -1) && (tb_count.Text != "") && (tb_name.Text != "") && (Convert.ToInt32(tb_count.Text) > 0) && (!tb_start.Text.Contains("_")) && (!tb_start.Text.Contains("_")))
                {
                    int id_type = (int)cb_type.SelectedValue;
                    int id_employee = (int)cb_employee.SelectedValue;
                    int count = Convert.ToInt32(tb_count.Text);
                    string name = tb_name.Text;
                    DateTime start = DateTime.ParseExact(tb_start.Text, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(tb_end.Text, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    int id = (int)datagrid.SelectedValue;
                    if (start < end)
                    {
                        tb_Delivery_MtsTableAdapter.Delivery_Update(id,name, id_type, count, start, end, id_employee);
                        pr_Delivery_MtsTableAdapter.Fill(dataSet1.pr_delivery_mts);
                    }else
                    { MessageBox.Show(@"Ошибка! Проверьте даты!"); }
                }
                else { MessageBox.Show(@"Ошибка! Проверьте ввод данных!"); }
            }
            catch { MessageBox.Show(@"Ошибка!"); }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem != null)
            {
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxResult rsltMessageBox = MessageBox.Show("Вы уверены, что хотите удалить данные поставки МТО?", "Предупреждение!", btnMessageBox, MessageBoxImage.Question);
                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        {
                            int id = (int)datagrid.SelectedValue;
                            tb_Delivery_MtsTableAdapter.Delivery_Delete(id);
                            pr_Delivery_MtsTableAdapter.Fill(dataSet1.pr_delivery_mts);
                            break;
                        }
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)datagrid.SelectedItem;
                if (selectedRow != null)
                {
                    tb_name.Text = selectedRow.Row.ItemArray[1].ToString();
                    tb_count.Text = selectedRow.Row.ItemArray[4].ToString();
                    tb_start.Text = selectedRow.Row.ItemArray[5].ToString();
                    tb_end.Text = selectedRow.Row.ItemArray[6].ToString();
                    cb_employee.SelectedValue = selectedRow.Row.ItemArray[7];
                    cb_type.SelectedValue = selectedRow.Row.ItemArray[2];
                }
            }
            catch { MessageBox.Show(@"Ошибка!"); }
        }
        //Отображение таблицы БД
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid.Columns[1].Header = "Наименование";
            datagrid.Columns[0].Visibility = Visibility.Hidden; //Номер 
            datagrid.Columns[2].Visibility = Visibility.Hidden; //индекс типа
            datagrid.Columns[3].Header = "Тип";
            datagrid.Columns[4].Header = "Количество";
            datagrid.Columns[5].Header = "Дата заказа";
            datagrid.Columns[6].Header = "Дата получения";
            datagrid.Columns[7].Visibility = Visibility.Hidden; //индекс сотруника
            datagrid.Columns[8].Header = "Фамилия";
            datagrid.Columns[9].Header = "Имя";
            datagrid.Columns[10].Header = "Отчество";
        }
        private void btn_catalog_Click(object sender, RoutedEventArgs e)
        {
            Window_CatalogBook window = new Window_CatalogBook();
            window.Show();
            this.Close();
        }

        private void btn_profile_Click(object sender, RoutedEventArgs e)
        {
            Window_Profile window = new Window_Profile();
            window.Show();
            this.Close();
        }

        private void btn_authexit_Click(object sender, RoutedEventArgs e)
        {
            projectHelper.func_Auth();
        }

        private void btn_admin_Click(object sender, RoutedEventArgs e)
        {
            Window_MenuAdmin window = new Window_MenuAdmin();
            window.Show();
            this.Close();
        }

        private void btn_mtolibrary_Click(object sender, RoutedEventArgs e)
        {
            Window_MTOLibrary window = new Window_MTOLibrary();
            window.Show();
            this.Close();
        }
        private void btn_deliverymto_Click(object sender, RoutedEventArgs e)
        {
            Window_DeliveryMTO window = new Window_DeliveryMTO();
            window.Show();
            this.Close();
        }
        private void btn_applicants_Click(object sender, RoutedEventArgs e)
        {
            Window_Applicants window = new Window_Applicants();
            window.Show();
            this.Close();
        }
        private void btn_employees_Click(object sender, RoutedEventArgs e)
        {
            Window_Employees window = new Window_Employees();
            window.Show();
            this.Close();
        }
        private void btn_readers_Click(object sender, RoutedEventArgs e)
        {
            Window_Readers window = new Window_Readers();
            window.Show();
            this.Close();
        }
        private void btn_librarycards_Click(object sender, RoutedEventArgs e)
        {
            Window_LibraryCards window = new Window_LibraryCards();
            window.Show();
            this.Close();
        }
        private void btn_books_Click(object sender, RoutedEventArgs e)
        {
            Window_Books window = new Window_Books();
            window.Show();
            this.Close();
        }
        private void btn_bookslibrary_Click(object sender, RoutedEventArgs e)
        {
            Window_BooksLibrary window = new Window_BooksLibrary();
            window.Show();
            this.Close();
        }
        private void btn_extraditionbooks_Click(object sender, RoutedEventArgs e)
        {
            Window_ExtraditionBooks window = new Window_ExtraditionBooks();
            window.Show();
            this.Close();
        }
    }
}
