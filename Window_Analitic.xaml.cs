using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace App_Library
{
    /// <summary>
    /// Логика взаимодействия для Window_Analitic.xaml
    /// </summary>
    public partial class Window_Analitic : Window
    {
        ProjectHelper projectHelper = new ProjectHelper();
        public Window_Analitic()
        {
            InitializeComponent();
            //Получение данных для статистики
            string sql1 = "select b.[Name_Book], count(e.[ID_Extradition]) as Count_Extraditions from [dbo].[pr_extradition_books] e LEFT JOIN [dbo].[pr_books] b on b.[ID_Book]=e.Book_ID group by b.[Name_Book],e.[Book_ID],b.[Surname_Author],b.[Name_Author],b.[MiddleName_Author];";
            DataTable table1 = projectHelper.Select(sql1);
            string events = "";
            string[,] dataEvents = new string[table1.Rows.Count, table1.Columns.Count];
            for (int i = 0; i < table1.Rows.Count; i++)
            {
                for (int j = 0; j < table1.Columns.Count; j++)
                {
                    dataEvents[i, j] = table1.Rows[i].ItemArray[j] + "";
                    events = events + " " + i + j + " " + table1.Rows[i].ItemArray[j];
                }
            }
            //перенос данных для статистики
            int countrow = dataEvents.GetLength(0);
            KeyValuePair<string, int>[] cp = new KeyValuePair<string, int>[countrow];

            for (int i = 0; i < countrow; i++)
            {
                cp[i] = new KeyValuePair<string, int>(dataEvents[i, 0], Convert.ToInt32(dataEvents[i, 1]));
            }
            ((ColumnSeries)analitic_events.Series[0]).ItemsSource = cp;

            //KeyValuePair<string, int>[] cp1 = new KeyValuePair<string, int>[countrow];

            //for (int i = 0; i < countrow; i++)
            //{
            //    cp1[i] = new KeyValuePair<string, int>(dataEvents[i, 0], Convert.ToInt32(dataEvents[i, 1]));
            //}
            //((ColumnSeries)analitic_events.Series[1]).ItemsSource = cp1;
        }

        // Кнопка перехода в каталог
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            Window_CatalogBook menu = new Window_CatalogBook();
            menu.Show();
            this.Close();
        }
    }
}
