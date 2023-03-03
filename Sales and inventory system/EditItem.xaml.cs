using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;

namespace Sales_and_Inventory_System
{
    /// <summary>
    /// Interaction logic for EditItem.xaml
    /// </summary>
    public partial class EditItem : Window
    {
        SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");

        String itemID;
        int itemIDInt;
        String itemName;
        String itemModel;
        String itemDescription;
        String itemPriceString;
        int itemPriceInt;
        String itemStockString;
        int itemStockInt;
        String itemWarranty;
        String itemWarrantyService;

        string filePath;
        string targetFile;

        bool didUserChangeImage = false;

        public EditItem()
        {
            InitializeComponent();

            getItemData();
        }

        private void getItemData()
        {
            connection.Open();
            SqlCommand getItemInformationCMD = new SqlCommand();
            getItemInformationCMD.Connection = connection;
            getItemInformationCMD.CommandText = "SELECT * FROM item INNER JOIN available_items ON item.item_serial_number = available_items.item_serial_number WHERE item.item_serial_number = '" + Home.instance.itemIDToEdit + "'";
            getItemInformationCMD.ExecuteNonQuery();

            SqlDataReader getItemInformationDR = getItemInformationCMD.ExecuteReader();
            while (getItemInformationDR.Read()){ 
                itemID = getItemInformationDR.GetValue(0).ToString();
                itemName = getItemInformationDR.GetValue(1).ToString();
                itemModel = getItemInformationDR.GetValue(2).ToString();
                itemDescription = getItemInformationDR.GetValue(3).ToString();
                itemPriceString = getItemInformationDR.GetValue(4).ToString();
                itemStockString = getItemInformationDR.GetValue(12).ToString();
                itemWarranty = getItemInformationDR.GetValue(5).ToString();
                itemWarrantyService = getItemInformationDR.GetValue(7).ToString();
                itemStockInt = Int32.Parse(itemStockString);
            }
            getItemInformationDR.Close();
            connection.Close();

            item_serial_number.Text = itemID;
            item_name.Text = itemName;
            item_model.Text = itemModel;
            item_description.Text = itemDescription;
            item_price.Text = itemPriceString;
            item_stock.Text = itemStockString;
            item_warranty.Text = itemWarranty;
            item_warranty_service.Text = itemWarrantyService;

            if (item_price.Text.Contains("."))
            {
                item_price.FormatString = "";
            }

            else
            {
                item_price.FormatString = "0.00";
            }

            getItemImage(itemName);
        }

        private void getItemImage(String itemImageName)
        {
            String sourceFilePathPartial = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            String sourceFilePathPartialFinal = sourceFilePathPartial + "\\img\\item_images\\";
            string[] fileNames = Directory.GetFiles(sourceFilePathPartialFinal);

            bool imageFound = false;

            foreach (string fileName in fileNames)
            {
                string fileNameWithExtension = System.IO.Path.GetFileName(fileName);

                int index = fileNameWithExtension.IndexOf('.');
                string fileNameFInal = fileNameWithExtension.Substring(0, index);

                if (itemImageName == fileNameFInal)
                {
                    imageFound = true;

                    String directoryPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
                    String directoryPathForItemImage = directoryPath + "\\img\\item_images\\" + fileNameWithExtension;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(directoryPathForItemImage);
                    bitmap.EndInit();

                    Image image = new Image();
                    item_image.Source = bitmap;
                    break;
                }
            }

            if (imageFound == false)
            {
                String directoryPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
                String directoryPathForItemImage = directoryPath + "\\img\\item_images\\Default.png";

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(directoryPathForItemImage);
                bitmap.EndInit();

                Image image = new Image();
                item_image.Source = bitmap;
            }
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            if (item_price.Text.ToString().Contains("."))
            {
                if (item_price.Text.ToString().Contains(".00"))
                {
                    item_price.FormatString = "0.00";
                }

                else
                {
                    item_price.FormatString = "";
                }
            }

            else
            {
                item_price.FormatString = "0.00";
            }

            if (string.IsNullOrEmpty(item_serial_number.Text))
            {
                System.Windows.MessageBox.Show("Item serial number should not be empty!", "Empty item serial number", MessageBoxButton.OK, MessageBoxImage.Error);
                item_serial_number.Focus();
            }

            else
            {
                if (string.IsNullOrEmpty(item_name.Text))
                {
                    System.Windows.MessageBox.Show("Item name should not be empty!", "Empty item name", MessageBoxButton.OK, MessageBoxImage.Error);
                    item_name.Focus();
                }

                else
                {
                    if (string.IsNullOrEmpty(item_model.Text))
                    {
                        System.Windows.MessageBox.Show("Item model should not be empty!", "Empty item model", MessageBoxButton.OK, MessageBoxImage.Error);
                        item_model.Focus();
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(item_stock.Text) || item_stock.Text == "0")
                        {
                            System.Windows.MessageBox.Show("Item stock should not be 0 or empty!", "Empty item stock", MessageBoxButton.OK, MessageBoxImage.Error);
                            item_stock.Focus();
                        }

                        else
                        {
                            if (string.IsNullOrEmpty(item_warranty.Text))
                            {
                                System.Windows.MessageBox.Show("Item warranty should not be empty!", "Empty item warranty", MessageBoxButton.OK, MessageBoxImage.Error);
                                item_warranty.Focus();
                            }

                            else
                            {
                                if (string.IsNullOrEmpty(item_warranty_service.Text))
                                {
                                    System.Windows.MessageBox.Show("Item warranty service should not be empty!", "Empty item warranty service", MessageBoxButton.OK, MessageBoxImage.Error);
                                    item_warranty_service.Focus();
                                }

                                else
                                {
                                    MessageBoxResult result1 = System.Windows.MessageBox.Show("Are you sure you want to update the information of this item?", "Update item information", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                    if (result1 == MessageBoxResult.Yes)
                                    {
                                        String warranty_word = warrantyInWord();
                                        String warranty_service_word = warrantyServiceInWord();

                                        String warranty = item_warranty.Text;
                                        String warranty_service = item_warranty_service.Text;

                                        DateTime warranty_date = DateTime.Parse(warranty);
                                        DateTime warranty_service_date = DateTime.Parse(warranty_service);

                                        String warrantyDateFormatted = warranty_date.ToString("yyyy-MM-dd");
                                        String warrantyServiceDateFormatted = warranty_service_date.ToString("yyyy-MM-dd");

                                        System.Windows.MessageBox.Show(item_warranty_service.Text);

                                        SqlCommand updateItemCMD = new SqlCommand();
                                        updateItemCMD.Connection = connection;
                                        updateItemCMD.CommandText = "UPDATE item SET item_serial_number = '" + item_serial_number.Text +"', item_name = '" + item_name.Text + "', item_model = '" + item_model.Text +"', item_description = '" + item_description.Text + "', item_price = '" + item_price.Text + "', item_warranty = '" + item_warranty.Text +"', item_warranty_word = '" + warranty_word + "', item_warranty_service = '" + item_warranty_service.Text + "', item_warranty_service_word = '" + warranty_service_word + "' WHERE item_serial_number = '" + Home.instance.itemIDToEdit + "'";
                                        SqlDataAdapter updateItemDA = new SqlDataAdapter(updateItemCMD);
                                        DataTable updateItemDT = new DataTable();
                                        updateItemDA.Fill(updateItemDT);
                                        connection.Close();

                                        SqlCommand updateItemStockCMD = new SqlCommand();
                                        updateItemStockCMD.Connection = connection;
                                        updateItemStockCMD.CommandText = "UPDATE available_items SET item_stock = '" + item_stock.Text + "', stock_copy = '" + item_stock.Text + "' WHERE item_serial_number = '" + Home.instance.itemIDToEdit + "'";
                                        SqlDataAdapter updateItemStockDA = new SqlDataAdapter(updateItemStockCMD);
                                        DataTable updateItemStockmDT = new DataTable();
                                        updateItemStockDA.Fill(updateItemStockmDT);
                                        connection.Close();

                                        if (didUserChangeImage == true)
                                        {
                                            File.Copy(filePath, targetFile, true);
                                        }

                                        System.Windows.MessageBox.Show("Item information update successfully", "Updated successfully", MessageBoxButton.OK, MessageBoxImage.Information);

                                        Home.instance.updateTotalItemCount();
                                        Home.instance.updateItemTotalCost();
                                        Home.instance.fill_item_data_grid();

                                        Home.instance.setDefaultItemImage();
                                        Home.instance.serial_number_textblock.Text = "Item serial number";
                                        Home.instance.name_textblock.Text = "Item name";
                                        Home.instance.model_textblock.Text = "Item model";
                                        Home.instance.price_textblock.Text = "0.00";
                                        Home.instance.description_textblock.Text = "Item description";
                                        Home.instance.stock_textblock.Text = "0";
                                        Home.instance.warranty_textblock.Text = "01/01/2023";
                                        Home.instance.warranty_service_textblock.Text = "01/01/2023";
                                        Home.instance.edit_item_btn.IsEnabled = false;

                                        this.Close();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private String warrantyInWord()
        {
            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;

            DateTime warrantyDate = DateTime.Parse(item_warranty.Text);

            String dateNow = date.ToString("yyyy-MM-dd");
            String warrantyDateFormatted = warrantyDate.ToString("yyyy-MM-dd");

            int indexOfYearNow = dateNow.IndexOf('-');
            int indexOfYearPlusOneNow = indexOfYearNow;
            string yearNow = dateNow.Substring(0, indexOfYearPlusOneNow);
            int yearNowInt = Convert.ToInt32(yearNow);

            string dateWithoutYearNow = dateNow.Substring(yearNow.Length + 1);
            int indexOfMonthNow = dateWithoutYearNow.IndexOf('-');
            string monthNow = dateWithoutYearNow.Substring(0, indexOfMonthNow);
            int monthNowInt = Convert.ToInt32(monthNow);

            string dateWithoutMonthNow = dateWithoutYearNow.Substring(monthNow.Length + 1);
            int dayNowInt = Convert.ToInt32(dateWithoutMonthNow);

            int numberOfDaysOfMonthToday = DateTime.DaysInMonth(yearNowInt, monthNowInt);

            String warranty_word = "";

            TimeSpan numberOfWarrantyDayTimeSpan = warrantyDate - date;
            double numberOfWarrantyDayDouble = numberOfWarrantyDayTimeSpan.TotalDays;
            int numberOfWarrantyDay = Convert.ToInt32(numberOfWarrantyDayDouble);

            if (numberOfWarrantyDay < 7)
            {
                //days lang
                if (numberOfWarrantyDay > 1)
                {
                    warranty_word = numberOfWarrantyDay + " Days";
                }

                else
                {
                    warranty_word = numberOfWarrantyDay + " Day";
                }

            }

            else if (numberOfWarrantyDay >= 7 && numberOfWarrantyDay < numberOfDaysOfMonthToday)
            {
                int totalWeeksCount = 7;// this is in loop so that we can determine how many weeks it counted base from the total number of warranty day
                int totalWeeks = 0;// this is to count the total weeks 

                for (; ; )
                {
                    if (numberOfWarrantyDay >= totalWeeksCount)
                    {
                        totalWeeks++;
                        totalWeeksCount = totalWeeksCount + 7;
                    }

                    else
                    {
                        int numberOfRemainingDays = numberOfWarrantyDay - (totalWeeks * 7);

                        if (totalWeeks > 1)
                        {
                            warranty_word = totalWeeks + " Weeks";

                            /*if (numberOfRemainingDays == 0)
                            {
                                warranty_word = totalWeeks + " Weeks";
                            }

                            else if (numberOfRemainingDays > 1)
                            {
                                warranty_word = totalWeeks + " Weeks and " + numberOfRemainingDays + " Days";
                            }

                            else
                            {
                                warranty_word = totalWeeks + " Weeks and " + numberOfRemainingDays + " Day";
                            }*/
                        }

                        else
                        {
                            warranty_word = totalWeeks + " Week";

                            /*if (numberOfRemainingDays == 0)
                            {
                                warranty_word = totalWeeks + " Week";
                            }

                            else if(numberOfRemainingDays > 1)
                            {
                                warranty_word = totalWeeks + " Week and " + numberOfRemainingDays + " Days";
                            }

                            else
                            {
                                warranty_word = totalWeeks + " Week and " + numberOfRemainingDays + " Day";
                            }*/
                        }

                        break;
                    }
                }
            }

            else if (numberOfWarrantyDay >= numberOfDaysOfMonthToday && numberOfWarrantyDay < 365)
            {
                int totalMonthsCount = numberOfDaysOfMonthToday;// this is in loop so that we can determine how many months it counted base from the total number of warranty day
                int totalMonths = 0;// this is to count the total months 
                int forLoopCounter = 1;

                for (; ; )
                {
                    if (numberOfWarrantyDay >= totalMonthsCount)
                    {
                        int monthInt = monthNowInt + forLoopCounter;
                        int numberOfDaysOfMonths = DateTime.DaysInMonth(yearNowInt, monthInt);

                        totalMonthsCount = totalMonthsCount + numberOfDaysOfMonths;
                        totalMonths++;
                    }

                    else
                    {
                        int numberOfRemainingWeeksPartial = 0;

                        for (int monthLoop = 0; monthLoop < totalMonths; monthLoop++)
                        {
                            int monthInt = monthNowInt + monthLoop;
                            int numberOfDaysOfMonths = DateTime.DaysInMonth(yearNowInt, monthInt);

                            numberOfRemainingWeeksPartial = numberOfWarrantyDay - numberOfDaysOfMonths;
                        }

                        int numberOfRemainingWeeksFinal = numberOfRemainingWeeksPartial / 7;
                        int numberOfRemainingDays = numberOfRemainingWeeksPartial - (numberOfRemainingWeeksFinal * 7);

                        if (totalMonths > 1)
                        {
                            warranty_word = totalMonths + " Months";

                            /*if (numberOfRemainingWeeksFinal == 0)
                            {
                                if(numberOfRemainingDays == 0)
                                {
                                    warranty_word = totalMonths + " Months";
                                }

                                else if(numberOfRemainingDays > 1)
                                {
                                    warranty_word = totalMonths + " Months and " + numberOfRemainingDays + " Days";
                                }

                                else if(numberOfRemainingDays == 1)
                                {
                                    warranty_word = totalMonths + " Months and " + numberOfRemainingDays + " Day";
                                }
                                
                            }

                            else if (numberOfRemainingWeeksFinal > 1)
                            {
                                if(numberOfRemainingDays == 0)
                                {
                                    warranty_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Weeks";
                                }

                                else if (numberOfRemainingDays > 1)
                                {
                                    warranty_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Weeks and " + numberOfRemainingDays + " Days";
                                }

                                else if(numberOfRemainingDays == 1)
                                {
                                    warranty_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Weeks and " + numberOfRemainingDays + " Day";
                                }

                            }

                            else if (numberOfRemainingWeeksFinal == 1)
                            {
                                if (numberOfRemainingDays == 0)
                                {
                                    warranty_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Week";
                                }

                                else if (numberOfRemainingDays > 1)
                                {
                                    warranty_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Week and " + numberOfRemainingDays + " Days";
                                }

                                else if (numberOfRemainingDays == 1)
                                {
                                    warranty_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Week and " + numberOfRemainingDays + " Day";
                                }
                            }*/

                        }

                        else
                        {
                            warranty_word = totalMonths + " Month";

                            /*if (numberOfRemainingWeeksFinal == 0)
                            {
                                if (numberOfRemainingDays == 0)
                                {
                                    warranty_word = totalMonths + " Month";
                                }

                                else if (numberOfRemainingDays > 1)
                                {
                                    warranty_word = totalMonths + " Month and " + numberOfRemainingDays + " Days";
                                }

                                else if (numberOfRemainingDays == 1)
                                {
                                    warranty_word = totalMonths + " Month and " + numberOfRemainingDays + " Day";
                                }

                            }

                            else if (numberOfRemainingWeeksFinal > 1)
                            {
                                if (numberOfRemainingDays == 0)
                                {
                                    warranty_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Weeks";
                                }

                                else if (numberOfRemainingDays > 1)
                                {
                                    warranty_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Weeks and " + numberOfRemainingDays + " Days";
                                }

                                else if (numberOfRemainingDays == 1)
                                {
                                    warranty_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Weeks and " + numberOfRemainingDays + " Day";
                                }

                            }

                            else if (numberOfRemainingWeeksFinal == 1)
                            {
                                if (numberOfRemainingDays == 0)
                                {
                                    warranty_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Week";
                                }

                                else if (numberOfRemainingDays > 1)
                                {
                                    warranty_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Week and " + numberOfRemainingDays + " Days";
                                }

                                else if (numberOfRemainingDays == 1)
                                {
                                    warranty_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Week and " + numberOfRemainingDays + " Day";
                                }
                            }*/
                        }

                        break;
                    }

                    forLoopCounter++;
                }

            }
            return warranty_word;
        }


        private String warrantyServiceInWord()
        {
            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;

            DateTime warrantyServiceDate = DateTime.Parse(item_warranty_service.Text);

            String dateNow = date.ToString("yyyy-MM-dd");
            String warrantyServiceDateFormatted = warrantyServiceDate.ToString("yyyy-MM-dd");

            int indexOfYearNow = dateNow.IndexOf('-');
            int indexOfYearPlusOneNow = indexOfYearNow;
            string yearNow = dateNow.Substring(0, indexOfYearPlusOneNow);
            int yearNowInt = Convert.ToInt32(yearNow);

            string dateWithoutYearNow = dateNow.Substring(yearNow.Length + 1);
            int indexOfMonthNow = dateWithoutYearNow.IndexOf('-');
            string monthNow = dateWithoutYearNow.Substring(0, indexOfMonthNow);
            int monthNowInt = Convert.ToInt32(monthNow);

            string dateWithoutMonthNow = dateWithoutYearNow.Substring(monthNow.Length + 1);
            int dayNowInt = Convert.ToInt32(dateWithoutMonthNow);

            int numberOfDaysOfMonthToday = DateTime.DaysInMonth(yearNowInt, monthNowInt);

            String warranty_service_word = "";

            TimeSpan numberOfWarrantyServiceDayTimeSpan = warrantyServiceDate - date;
            double numberOfWarrantyServiceDayDouble = numberOfWarrantyServiceDayTimeSpan.TotalDays;
            int numberOfWarrantyServiceDay = Convert.ToInt32(numberOfWarrantyServiceDayDouble);

            if (numberOfWarrantyServiceDay < 7)
            {
                //days lang
                if (numberOfWarrantyServiceDay > 1)
                {
                    warranty_service_word = numberOfWarrantyServiceDay + " Days";
                }

                else
                {
                    warranty_service_word = numberOfWarrantyServiceDay + " Day";
                }

            }

            else if (numberOfWarrantyServiceDay >= 7 && numberOfWarrantyServiceDay < numberOfDaysOfMonthToday)
            {
                int totalWeeksCount = 7;// this is in loop so that we can determine how many weeks it counted base from the total number of warranty day
                int totalWeeks = 0;// this is to count the total weeks 

                for (; ; )
                {
                    if (numberOfWarrantyServiceDay >= totalWeeksCount)
                    {
                        totalWeeks++;
                        totalWeeksCount = totalWeeksCount + 7;
                    }

                    else
                    {
                        int numberOfRemainingDays = numberOfWarrantyServiceDay - (totalWeeks * 7);

                        if (totalWeeks > 1)
                        {
                            warranty_service_word = totalWeeks + " Weeks";

                            /*if (numberOfRemainingDays == 0)
                            {
                                warranty_service_word = totalWeeks + " Weeks";
                            }

                            else if (numberOfRemainingDays > 1)
                            {
                                warranty_service_word = totalWeeks + " Weeks and " + numberOfRemainingDays + " Days";
                            }

                            else
                            {
                                warranty_service_word = totalWeeks + " Weeks and " + numberOfRemainingDays + " Day";
                            }*/
                        }

                        else
                        {
                            warranty_service_word = totalWeeks + " Week";

                            /*if (numberOfRemainingDays == 0)
                            {
                                warranty_service_word = totalWeeks + " Week";
                            }

                            else if(numberOfRemainingDays > 1)
                            {
                                warranty_service_word = totalWeeks + " Week and " + numberOfRemainingDays + " Days";
                            }

                            else
                            {
                                warranty_service_word = totalWeeks + " Week and " + numberOfRemainingDays + " Day";
                            }*/
                        }

                        break;
                    }
                }
            }

            else if (numberOfWarrantyServiceDay >= numberOfDaysOfMonthToday && numberOfWarrantyServiceDay < 365)
            {
                int totalMonthsCount = numberOfDaysOfMonthToday;// this is in loop so that we can determine how many months it counted base from the total number of warranty day
                int totalMonths = 0;// this is to count the total months 
                int forLoopCounter = 1;

                for (; ; )
                {
                    if (numberOfWarrantyServiceDay >= totalMonthsCount)
                    {
                        int monthInt = monthNowInt + forLoopCounter;
                        int numberOfDaysOfMonths = DateTime.DaysInMonth(yearNowInt, monthInt);

                        totalMonthsCount = totalMonthsCount + numberOfDaysOfMonths;
                        totalMonths++;
                    }

                    else
                    {
                        int numberOfRemainingWeeksPartial = 0;

                        for (int monthLoop = 0; monthLoop < totalMonths; monthLoop++)
                        {
                            int monthInt = monthNowInt + monthLoop;
                            int numberOfDaysOfMonths = DateTime.DaysInMonth(yearNowInt, monthInt);

                            numberOfRemainingWeeksPartial = numberOfWarrantyServiceDay - numberOfDaysOfMonths;
                        }

                        int numberOfRemainingWeeksFinal = numberOfRemainingWeeksPartial / 7;
                        int numberOfRemainingDays = numberOfRemainingWeeksPartial - (numberOfRemainingWeeksFinal * 7);

                        if (totalMonths > 1)
                        {
                            warranty_service_word = totalMonths + " Months";

                            /*if (numberOfRemainingWeeksFinal == 0)
                            {
                                if(numberOfRemainingDays == 0)
                                {
                                    warranty_service_word = totalMonths + " Months";
                                }

                                else if(numberOfRemainingDays > 1)
                                {
                                    warranty_service_word = totalMonths + " Months and " + numberOfRemainingDays + " Days";
                                }

                                else if(numberOfRemainingDays == 1)
                                {
                                    warranty_service_word = totalMonths + " Months and " + numberOfRemainingDays + " Day";
                                }
                                
                            }

                            else if (numberOfRemainingWeeksFinal > 1)
                            {
                                if(numberOfRemainingDays == 0)
                                {
                                    warranty_service_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Weeks";
                                }

                                else if (numberOfRemainingDays > 1)
                                {
                                    warranty_service_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Weeks and " + numberOfRemainingDays + " Days";
                                }

                                else if(numberOfRemainingDays == 1)
                                {
                                    warranty_service_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Weeks and " + numberOfRemainingDays + " Day";
                                }

                            }

                            else if (numberOfRemainingWeeksFinal == 1)
                            {
                                if (numberOfRemainingDays == 0)
                                {
                                    warranty_service_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Week";
                                }

                                else if (numberOfRemainingDays > 1)
                                {
                                    warranty_service_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Week and " + numberOfRemainingDays + " Days";
                                }

                                else if (numberOfRemainingDays == 1)
                                {
                                    warranty_service_word = totalMonths + " Months and " + numberOfRemainingWeeksFinal + " Week and " + numberOfRemainingDays + " Day";
                                }
                            }*/

                        }

                        else
                        {
                            warranty_service_word = totalMonths + " Month";

                            /*if (numberOfRemainingWeeksFinal == 0)
                            {
                                if (numberOfRemainingDays == 0)
                                {
                                    warranty_service_word = totalMonths + " Month";
                                }

                                else if (numberOfRemainingDays > 1)
                                {
                                    warranty_service_word = totalMonths + " Month and " + numberOfRemainingDays + " Days";
                                }

                                else if (numberOfRemainingDays == 1)
                                {
                                    warranty_service_word = totalMonths + " Month and " + numberOfRemainingDays + " Day";
                                }

                            }

                            else if (numberOfRemainingWeeksFinal > 1)
                            {
                                if (numberOfRemainingDays == 0)
                                {
                                    warranty_service_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Weeks";
                                }

                                else if (numberOfRemainingDays > 1)
                                {
                                    warranty_service_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Weeks and " + numberOfRemainingDays + " Days";
                                }

                                else if (numberOfRemainingDays == 1)
                                {
                                    warranty_service_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Weeks and " + numberOfRemainingDays + " Day";
                                }

                            }

                            else if (numberOfRemainingWeeksFinal == 1)
                            {
                                if (numberOfRemainingDays == 0)
                                {
                                    warranty_service_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Week";
                                }

                                else if (numberOfRemainingDays > 1)
                                {
                                    warranty_service_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Week and " + numberOfRemainingDays + " Days";
                                }

                                else if (numberOfRemainingDays == 1)
                                {
                                    warranty_service_word = totalMonths + " Month and " + numberOfRemainingWeeksFinal + " Week and " + numberOfRemainingDays + " Day";
                                }
                            }*/
                        }

                        break;
                    }

                    forLoopCounter++;
                }

            }
            return warranty_service_word;
        }



        private void change_image_btn_click(object sender, RoutedEventArgs e)
        {
            didUserChangeImage = true;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.gif)|*.png;*.jpeg;*.jpg;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(filePath);
                bitmapImage.EndInit();

                String sourceFilePath = bitmapImage.ToString();

                string fileName = System.IO.Path.GetFileName(filePath);

                int indexOfDot = fileName.IndexOf('.');
                string imageExtension = fileName.Substring(indexOfDot + 1);


                String sourceFilePath1 = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
                String sourceFilePathFinal = sourceFilePath1 + "\\img\\item_images\\";
                targetFile = System.IO.Path.Combine(sourceFilePathFinal, item_name.Text + "." + imageExtension);
                string destinationFilePath = sourceFilePath;

                file_name.Content = fileName;
                item_image.Source = bitmapImage;

                remove_image_btn.Visibility = Visibility.Visible;
                file_name.Visibility = Visibility.Visible;
            }
        }

        private void remove_image_btn_click(object sender, RoutedEventArgs e)
        {
            didUserChangeImage = false;

            remove_image_btn.Visibility = Visibility.Collapsed;
            file_name.Content = "";
            file_name.Visibility = Visibility.Collapsed;

            getItemImage(itemName);
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //check if the textbox lost its focus
        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(item_price.Text) || item_price.Text == "0")
            {
                item_price.FormatString = "0.00";
                item_price.Text = "0.00";
            }

            if ((string.IsNullOrEmpty(item_stock.Text) || item_stock.Text == "0") && !string.IsNullOrEmpty(item_price.Text))
            {
                item_stock.Text = "0";
            }

            else if ((string.IsNullOrEmpty(item_stock.Text) || item_stock.Text == "0") && (string.IsNullOrEmpty(item_price.Text) || item_price.Text == "0"))
            {
                item_price.FormatString = "0.00";
                item_price.Text = "0.00";
                item_stock.Text = "0";
            }
        }
        //***********

        //for number input only in pin code input
        private void NumberWithFloatOnly(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                double? value = ((DoubleUpDown)sender).Value;
                if (value.HasValue && !value.ToString().Contains("."))
                {
                    approvedDecimalPoint = true;
                }
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }
    }
}
