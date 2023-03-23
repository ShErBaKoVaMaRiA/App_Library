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
    /// Логика взаимодействия для Window_ExtraditionBooks.xaml
    /// </summary>
    public partial class Window_ExtraditionBooks : Window
    {
        //Ссылки на датасет и данные
        DataSet1 dataSet1;
        tb_extradition_booksTableAdapter tb_Extradition_BooksTableAdapter;
        pr_extradition_booksTableAdapter pr_Extradition_BooksTableAdapter;

        pr_booksTableAdapter pr_BooksTableAdapter;
        pr_library_cardsTableAdapter pr_Library_CardsTableAdapter;
        public Window_ExtraditionBooks()
        {
            InitializeComponent();
            dataSet1 = new DataSet1();
            tb_Extradition_BooksTableAdapter = new tb_extradition_booksTableAdapter();
            tb_Extradition_BooksTableAdapter.Fill(dataSet1.tb_extradition_books);
            pr_Extradition_BooksTableAdapter = new pr_extradition_booksTableAdapter();
            pr_Extradition_BooksTableAdapter.Fill(dataSet1.pr_extradition_books);

            pr_BooksTableAdapter = new pr_booksTableAdapter();
            pr_BooksTableAdapter.Fill(dataSet1.pr_books);
            pr_Library_CardsTableAdapter = new pr_library_cardsTableAdapter();
            pr_Library_CardsTableAdapter.Fill(dataSet1.pr_library_cards);

            datagrid.ItemsSource = dataSet1.pr_extradition_books.DefaultView;
            datagrid.SelectedValuePath = "ID_Extradition";
            datagrid.CanUserAddRows = false;
            datagrid.CanUserDeleteRows = false;
            datagrid.SelectionMode = DataGridSelectionMode.Single;

            cb_cards.ItemsSource = dataSet1.pr_library_cards.DefaultView;
            cb_cards.SelectedValuePath = "ID_library_card";
            cb_cards.SelectedIndex = -1;
            cb_book.ItemsSource = dataSet1.pr_books.DefaultView;
            cb_book.SelectedValuePath = "ID_Book";
            cb_book.SelectedIndex = -1;
        }
        ProjectHelper projectHelper = new ProjectHelper();
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((cb_book.SelectedIndex != -1) && (cb_cards.SelectedIndex != -1) && (tb_end.Text != "") && (tb_start.Text != ""))
                {
                    int id_book = (int)cb_book.SelectedValue;
                    int id_cards = (int)cb_cards.SelectedValue;
                    DateTime start = DateTime.ParseExact(tb_start.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(tb_end.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    if (start < end)
                    {
                        tb_Extradition_BooksTableAdapter.Extradition_Insert(start,end,id_book,id_cards);
                        pr_Extradition_BooksTableAdapter.Fill(dataSet1.pr_extradition_books);
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
                if ((datagrid.SelectedItem != null) && (cb_book.SelectedIndex != -1) && (cb_cards.SelectedIndex != -1) && (tb_end.Text != "") && (tb_start.Text != ""))
                {
                    int id_book = (int)cb_book.SelectedValue;
                    int id_cards = (int)cb_cards.SelectedValue;
                    DateTime start = DateTime.ParseExact(tb_start.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(tb_end.SelectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    int id = (int)datagrid.SelectedValue;
                    if (start < end)
                    {
                        tb_Extradition_BooksTableAdapter.Extradotion_Update(id,start, end, id_book, id_cards);
                        pr_Extradition_BooksTableAdapter.Fill(dataSet1.pr_extradition_books);
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
                MessageBoxResult rsltMessageBox = MessageBox.Show("Вы уверены, что хотите удалить данные выданных книг?", "Предупреждение!", btnMessageBox, MessageBoxImage.Question);
                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        {
                            tb_Extradition_BooksTableAdapter.Extradition_Delete((int)datagrid.SelectedValue);
                            pr_Extradition_BooksTableAdapter.Fill(dataSet1.pr_extradition_books);
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
                    tb_start.Text = selectedRow.Row.ItemArray[1].ToString();
                    tb_end.Text = selectedRow.Row.ItemArray[2].ToString();

                    cb_book.SelectedValue = selectedRow.Row.ItemArray[3];
                    cb_cards.SelectedValue = selectedRow.Row.ItemArray[8];
                }
            }
            catch { MessageBox.Show(@"Ошибка!"); }
        }
        //Отображение таблицы БД
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid.Columns[0].Visibility = Visibility.Hidden; //индекс выдачи
            datagrid.Columns[1].Header = "Дата получения";
            datagrid.Columns[2].Header = "Дата возврата";
            datagrid.Columns[3].Visibility = Visibility.Hidden; //индекс книги
            datagrid.Columns[4].Header = "Название";
            datagrid.Columns[5].Header = "Фамилия автора";
            datagrid.Columns[6].Header = "Имя автора";
            datagrid.Columns[7].Header = "Отчество автора";
            datagrid.Columns[8].Header = "Номер билета";
            datagrid.Columns[9].Visibility = Visibility.Hidden; //открыт
            datagrid.Columns[10].Visibility = Visibility.Hidden; //закрыт
            datagrid.Columns[11].Visibility = Visibility.Hidden; //индекс читателя
            datagrid.Columns[12].Header = "Фамилия читателя";
            datagrid.Columns[13].Header = "Имя читателя";
            datagrid.Columns[14].Header = "Отчество читателя";
            datagrid.Columns[15].Header = "Телефон читателя";
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
        private void btn_backup_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxResult rsltMessageBox = MessageBox.Show("Вы хотите выгрузить данные из таблицы?", "Выгрузка данных", btnMessageBox, MessageBoxImage.Question);
            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    {
                        try
                        {
                            string sql = "select * from pr_extradition_books;";
                            DataTable table = projectHelper.Select(sql);
                            projectHelper.ToCSV(table);
                            MessageBox.Show("Выгрузка завершена!");
                        }
                        catch { MessageBox.Show("Ошибка!"); }
                        break;
                    }
                case MessageBoxResult.No:
                    return;
            }
        }
    }
}
