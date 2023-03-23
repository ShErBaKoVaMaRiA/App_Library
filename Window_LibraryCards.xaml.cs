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
    /// Логика взаимодействия для Window_LibraryCards.xaml
    /// </summary>
    public partial class Window_LibraryCards : Window
    {
        //Ссылки на датасет и данные
        DataSet1 dataSet1;
        tb_library_cardsTableAdapter tb_Library_CardsTableAdapter;
        pr_library_cardsTableAdapter pr_Library_CardsTableAdapter;

        tb_readersTableAdapter tb_ReadersTableAdapter;
        public Window_LibraryCards()
        {
            InitializeComponent();

            dataSet1 = new DataSet1();
            tb_Library_CardsTableAdapter = new tb_library_cardsTableAdapter();
            tb_Library_CardsTableAdapter.Fill(dataSet1.tb_library_cards);
            pr_Library_CardsTableAdapter = new pr_library_cardsTableAdapter();
            pr_Library_CardsTableAdapter.Fill(dataSet1.pr_library_cards);

            tb_ReadersTableAdapter = new tb_readersTableAdapter();
            tb_ReadersTableAdapter.Fill(dataSet1.tb_readers);

            datagrid.ItemsSource = dataSet1.pr_library_cards.DefaultView;
            datagrid.SelectedValuePath = "ID_library_card";
            datagrid.CanUserAddRows = false;
            datagrid.CanUserDeleteRows = false;
            datagrid.SelectionMode = DataGridSelectionMode.Single;

            cb_reader.ItemsSource = dataSet1.tb_readers.DefaultView;
            cb_reader.SelectedValuePath = "ID_Reader";
            cb_reader.SelectedIndex = -1;
        }
        ProjectHelper projectHelper = new ProjectHelper();
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((cb_reader.SelectedIndex != -1) && (tb_open.Text != "") && (tb_close.Text != ""))
                {
                    int id_reader = (int)cb_reader.SelectedValue;
                    DateTime open = DateTime.ParseExact(tb_open.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime close = DateTime.ParseExact(tb_close.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    if (open < close)
                    {
                        tb_Library_CardsTableAdapter.Cards_Insert(open,close,id_reader);
                        pr_Library_CardsTableAdapter.Fill(dataSet1.pr_library_cards);
                    }
                    else { MessageBox.Show(@"Ошибка! Проверьте даты!"); }
                }
                else { MessageBox.Show(@"Ошибка! Проверьте ввод данных!"); }
            }
            catch { MessageBox.Show("Erroe!"); }

        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((datagrid.SelectedItem != null) && (cb_reader.SelectedIndex != -1) && (tb_open.Text != "") && (tb_close.Text != ""))
                {
                    int id_reader = (int)cb_reader.SelectedValue;
                    DateTime open = DateTime.ParseExact(tb_open.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime close = DateTime.ParseExact(tb_close.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    int id = (int)datagrid.SelectedValue;
                    if (open < close)
                    {
                        tb_Library_CardsTableAdapter.Cards_Update(id,open, close, id_reader);
                        pr_Library_CardsTableAdapter.Fill(dataSet1.pr_library_cards);
                    }
                    else { MessageBox.Show(@"Ошибка! Проверьте даты!"); }
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
                MessageBoxResult rsltMessageBox = MessageBox.Show("Вы уверены, что хотите удалить данные читательского билета?", "Предупреждение!", btnMessageBox, MessageBoxImage.Question);
                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        {
                            tb_Library_CardsTableAdapter.Cards_Delete((int)datagrid.SelectedValue);
                            pr_Library_CardsTableAdapter.Fill(dataSet1.pr_library_cards);
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
                    tb_number.Text = selectedRow.Row.ItemArray[0].ToString();
                    tb_open.Text = selectedRow.Row.ItemArray[1].ToString();
                    tb_close.Text = selectedRow.Row.ItemArray[2].ToString();

                    cb_reader.SelectedValue = selectedRow.Row.ItemArray[3];
                }
            }
            catch { MessageBox.Show(@"Ошибка!"); }
        }
        //Отображение таблицы БД
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid.Columns[4].Header = "Фамилия";
            datagrid.Columns[5].Header = "Имя";
            datagrid.Columns[6].Header = "Отчество";
            datagrid.Columns[1].Header = "Дата открытия";
            datagrid.Columns[2].Header = "Дата закрытия";
            datagrid.Columns[7].Header = "Телефон";
            datagrid.Columns[3].Visibility = Visibility.Hidden; //индекс читателя
            datagrid.Columns[0].Header = "Номер";
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
