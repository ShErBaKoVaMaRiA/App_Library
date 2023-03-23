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
    /// Логика взаимодействия для Window_Applicants.xaml
    /// </summary>
    public partial class Window_Applicants : Window
    {
        //Ссылки на датасет и данные
        DataSet1 dataSet1;
        tb_applicantsTableAdapter tb_ApplicantsTableAdapter;
        pr_applicantsTableAdapter pr_ApplicantsTableAdapter;

        tb_positionTableAdapter tb_PositionTableAdapter;
        //Присвоение для переменных ссылок данных, отображенеи комбобоксов и таблицы
        public Window_Applicants()
        {
            InitializeComponent();
            dataSet1 = new DataSet1();
            tb_ApplicantsTableAdapter = new tb_applicantsTableAdapter();
            tb_ApplicantsTableAdapter.Fill(dataSet1.tb_applicants);
            pr_ApplicantsTableAdapter = new pr_applicantsTableAdapter();
            pr_ApplicantsTableAdapter.Fill(dataSet1.pr_applicants);

            tb_PositionTableAdapter = new tb_positionTableAdapter();
            tb_PositionTableAdapter.Fill(dataSet1.tb_position);

            datagrid.ItemsSource = dataSet1.pr_applicants.DefaultView;
            datagrid.SelectedValuePath = "ID_Applicant";
            datagrid.CanUserAddRows = false;
            datagrid.CanUserDeleteRows = false;
            datagrid.SelectionMode = DataGridSelectionMode.Single;

            cb_positions.ItemsSource = dataSet1.tb_position.DefaultView;
            cb_positions.SelectedValuePath = "ID_Position";
            cb_positions.DisplayMemberPath = "Name_Position";
            cb_positions.SelectedIndex = -1;
        }
        ProjectHelper projectHelper = new ProjectHelper();
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb_work.Text == "") tb_work.Text = "0";
                if ((cb_positions.SelectedIndex != -1) && (tb_surname.Text != "") && (tb_name.Text != "") && (!tb_telefon.Text.Contains("_")) && (tb_birthday.Text != "") && (tb_address.Text != "") && (!tb_pasport.Text.Contains("_")) && (tb_work.Text != ""))
                {
                    int id_position = (int)cb_positions.SelectedValue;
                    string surname = tb_surname.Text;
                    string name = tb_name.Text;
                    string middlename = tb_middlename.Text;
                    string telefon = tb_telefon.Text;
                    DateTime birthday = DateTime.ParseExact(tb_birthday.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    string address = tb_address.Text;
                    string pasport = tb_pasport.Text;
                    int work = Convert.ToInt32(tb_work.Text);

                    tb_ApplicantsTableAdapter.Applicant_Insert(surname, name, middlename, pasport, telefon, work, address, birthday, id_position);
                    pr_ApplicantsTableAdapter.Fill(dataSet1.pr_applicants);
                }
                else { MessageBox.Show(@"Ошибка! Проверьте ввод данных!"); }
            }catch { MessageBox.Show("Erroe!"); }
    
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb_work.Text == "") tb_work.Text = "0";
                if ((datagrid.SelectedItem != null) && (cb_positions.SelectedIndex != -1) && (tb_surname.Text != "") && (tb_name.Text != "") && (!tb_telefon.Text.Contains("_")) && (tb_birthday.Text != "") && (tb_address.Text != "") && (!tb_pasport.Text.Contains("_")) && (tb_work.Text != ""))
                {
                    int id_position = (int)cb_positions.SelectedValue;
                    string surname = tb_surname.Text;
                    string name = tb_name.Text;
                    string middlename = tb_middlename.Text;
                    string telefon = tb_telefon.Text;
                    DateTime birthday = DateTime.ParseExact(tb_birthday.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    string address = tb_address.Text;
                    string pasport = tb_pasport.Text;
                    int work = Convert.ToInt32(tb_work.Text);
                    int id = (int)datagrid.SelectedValue;

                    tb_ApplicantsTableAdapter.Applicant_Update(id,surname, name, middlename, pasport, telefon, work, address, birthday, id_position);
                    pr_ApplicantsTableAdapter.Fill(dataSet1.pr_applicants);
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
                MessageBoxResult rsltMessageBox = MessageBox.Show("Вы уверены, что хотите удалить данные соискателей?", "Предупреждение!", btnMessageBox, MessageBoxImage.Question);
                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        {
                            tb_ApplicantsTableAdapter.Applicant_Delete((int)datagrid.SelectedValue);
                            pr_ApplicantsTableAdapter.Fill(dataSet1.pr_applicants);
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
                    tb_surname.Text = selectedRow.Row.ItemArray[1].ToString();
                    tb_name.Text = selectedRow.Row.ItemArray[2].ToString();
                    tb_middlename.Text = selectedRow.Row.ItemArray[3].ToString();
                    tb_telefon.Text = selectedRow.Row.ItemArray[5].ToString();
                    tb_birthday.Text = selectedRow.Row.ItemArray[8].ToString();
                    tb_address.Text = selectedRow.Row.ItemArray[7].ToString();
                    tb_pasport.Text = selectedRow.Row.ItemArray[4].ToString();
                    tb_work.Text = selectedRow.Row.ItemArray[6].ToString();

                    cb_positions.SelectedValue = selectedRow.Row.ItemArray[9];
                }
            }
            catch { MessageBox.Show(@"Ошибка!"); }
        }
        //Отображение таблицы БД
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid.Columns[1].Header = "Фамилия";
            datagrid.Columns[0].Visibility = Visibility.Hidden; //Номер 
            datagrid.Columns[2].Header = "Имя";
            datagrid.Columns[3].Header = "Отчество";
            datagrid.Columns[4].Header = "Паспорт";
            datagrid.Columns[5].Header = "Телефон";
            datagrid.Columns[6].Header = "Опыт работы";
            datagrid.Columns[7].Header = "Адрес проживания";
            datagrid.Columns[8].Header = "Дата рождения";
            datagrid.Columns[9].Visibility = Visibility.Hidden; //индекс сотрудника
            datagrid.Columns[10].Header = "Должность";
            datagrid.Columns[11].Header = "Зарплата";
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
