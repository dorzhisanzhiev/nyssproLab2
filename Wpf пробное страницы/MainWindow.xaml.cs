using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace Wpf_пробное_страницы
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int numberOfRecPerPage; //Initialize our Variable, Classes and the List

        static Paging PagedTable = new Paging();

        static ThreatList ThreatList = new ThreatList();

        IList<ThreatList.Threat> myList = ThreatList.GetData();

        public MainWindow()
        {
            InitializeComponent();
            PagedTable.PageIndex = 1; //Sets the Initial Index to a default value

            int[] RecordsToShow = { 15, 30, 50, 100 }; //This Array can be any number groups

            foreach (int RecordGroup in RecordsToShow)
            {
                NumberOfRecords.Items.Add(RecordGroup); //Fill the ComboBox with the Array
            }

            NumberOfRecords.SelectedItem = 15; //Initialize the ComboBox

            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem); //Convert the 
                                                                                //Combobox Output to type int

            DataTable firstTable = PagedTable.SetPaging(myList, numberOfRecPerPage); //Fill a 
                                                                                     //DataTable with the First set based on the numberOfRecPerPage

            dataGrid.ItemsSource = firstTable.DefaultView; //Fill the dataGrid with the 
                                                           //DataTable created previously


        }

        public string PageNumberDisplay()
        {
            int PagedNumber = numberOfRecPerPage * (PagedTable.PageIndex + 1);
            if (PagedNumber > myList.Count)
            {
                PagedNumber = myList.Count;
            }
            return "Showing " + PagedNumber + " of " + myList.Count; //This dramatically 
                                                                     //reduced the number of times I had to write this string statement
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) //кнопки пагинации
        {
            dataGrid.ItemsSource = PagedTable.Next(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = PagedTable.Previous(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = PagedTable.Last(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void Refresh_Click(object sender, RoutedEventArgs e) // 2 пункт
        {
            try
            {
                IList<ThreatList.Threat> myList2 = ThreatList.GetNewData();
                string fullmessage = "";
                string message = "";
                int count = 0;
                for (int i = 0; i < myList2.Count; i++)
                {
                    if (myList[i].Name != myList2[i].Name)
                    {
                        message += "Название было: " + myList[i].Name + ".\nНазвание стало: " + myList2[i].Name + ".\n";
                    }
                    if (myList[i].Description != myList2[i].Description)
                    {
                        message += "Описание было: " + myList[i].Description + ".\nОписание стало: " + myList2[i].Description + ".\n";
                    }
                    if (myList[i].Source != myList2[i].Source)
                    {
                        message += "Источник угрозы был: " + myList[i].Source + ".\nИсточник угрозы стал: " + myList2[i].Source + ".\n";
                    }
                    if (myList[i].ImpactObj != myList2[i].ImpactObj)
                    {
                        message += "Объект воздействия был: " + myList[i].ImpactObj + ".\nОбъект воздействия стал: " + myList2[i].ImpactObj + ".\n";
                    }
                    if (myList[i].Confidentiality != myList2[i].Confidentiality)
                    {
                        if (myList[i].Confidentiality == true)
                        {
                            message += "Нарушение конфиденциальности было: да, стало: нет.\n";
                        }
                        message += "Нарушение конфиденциальности было: нет, стало: да.\n";
                    }
                    if (myList[i].Integrity != myList2[i].Integrity)
                    {
                        if (myList[i].Integrity == true)
                        {
                            message += "Нарушение целостности было: да, стало: нет.\n";
                        }
                        message += "Нарушение целостности было: нет, стало: да.\n";
                    }
                    if (myList[i].Availability != myList2[i].Availability)
                    {
                        if (myList[i].Availability == true)
                        {
                            message += "Нарушение доступности было: да, стало: нет.\n";
                        }
                        message += "Нарушение доступности было: нет, стало: да.\n";
                    }
                    if (message != "")
                    {
                        message = "Изменилась угроза " + myList2[i].Id + ".\n" + message;
                        fullmessage += message;
                        message = "";
                        count++;
                    }
                }
                if (count != 0)
                {
                    fullmessage = "Статус: Успешно.\nОбновлённых записей: " + count + ".\n" + fullmessage;
                    MessageBox.Show(fullmessage);
                    myList = myList2;
                } else MessageBox.Show("Статус: Успешно.\nОбновлённых записей: 0.\nБаза данных не изменилась.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Статус: Ошибка.\nПричина: " + ex.Message);
            }
        }
        private void NumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            dataGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void myDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) // 5 пункт
        {
            if (dataGrid.SelectedItem == null) return;
            DataRowView datarow = (DataRowView)dataGrid.SelectedItem;
            string datarowId = datarow["Id"].ToString();
            int id = Convert.ToInt32(datarowId.Substring(4, 3));
            ThreatList.Threat threat = myList[id-1];
            string message = "Идентификатор УБИ: " + threat.Id + "\nНаименование УБИ: " + threat.Name + "\nОписание: " + threat.Description + "\nИсточник угрозы (характеристика и потенциал нарушителя): " + threat.Source + "\nОбъект воздействия: " + threat.ImpactObj;
            if (threat.Confidentiality == true)
            {
                message += "\nНарушение конфиденциальности: да";
            } else message += "\nНарушение конфиденциальности: нет";
            if (threat.Integrity == true)
            {
                message += "\nНарушение целостности: да";
            }
            else message += "\nНарушение целостности: нет";
            if (threat.Availability == true)
            {
                message += "\nНарушение доступности: да";
            }
            else message += "\nНарушение доступности: нет";
            MessageBox.Show(message);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
