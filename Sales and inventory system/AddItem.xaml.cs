using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using Xceed.Wpf.Toolkit;
using Sales_and_Inventory_System;

namespace Sales_and_Inventory_System
{
    /// <summary>
    /// Interaction logic for addItem.xaml
    /// </summary>
    public partial class AddItem : Window
    {
        SqlConnection connection = ConnectionString.Connection;

        string filePath;
        string targetFile;

        bool uploadButtonClicked = false;

        int numberOfImages = 0;

        public AddItem()
        {
            InitializeComponent();

            item_price.FormatString = "0.00";
            item_price.Text = "0.00";
            item_stock.Text = "0";

            setDefaultItemImage();

            fill_item_data_grid();
        }

        //for losing focus on text box when click outside
        private void window_MouseDown(object sender, MouseButtonEventArgs e)
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

            Keyboard.ClearFocus();

        }
        //end ************************************


        //check if the textbox lost its focus
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
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


        private void setDefaultItemImage()
        {
            String sourceFilePath1 = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            String DefaultImagepath = sourceFilePath1 + "\\img\\item_images\\Default.png";

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(DefaultImagepath);
            bitmap.EndInit();

            Image image = new Image();
            item_image.Source = bitmap;
            file_name.Text = "Default.png";

            remove_image_btn.Visibility = Visibility.Hidden;

        }

        private void getFileNamesCount()
        {
            String sourceFilePath1 = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            String path = sourceFilePath1 + "\\img\\item_images\\";

            try
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    numberOfImages++;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            String[] imageNames = new String[numberOfImages];

            int loopImageArray = 0;

            try
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    string fileName = System.IO.Path.GetFileName(file);

                    int index = fileName.IndexOf('.');
                    string subString = fileName.Substring(0, index);

                    imageNames[loopImageArray] = subString;

                    loopImageArray++;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            String itemName= "Marlou";

            for(int imageLoop = 0; imageLoop < numberOfImages; imageLoop++)
            {
                if (imageNames[imageLoop] == itemName)
                {
                    System.Windows.MessageBox.Show("Found");
                }
            }
        }

        private void add_item_btn_Click(object sender, RoutedEventArgs e)
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


            bool add = true;

            if (string.IsNullOrEmpty(serial_number.Text))
            {
                System.Windows.MessageBox.Show("Item serial number should not be empty!", "Empty item serial number", MessageBoxButton.OK, MessageBoxImage.Error);
                add = false;
                serial_number.Focus();
            }

            else
            {
                add = true;

                if (string.IsNullOrEmpty(item_name.Text))
                {
                    System.Windows.MessageBox.Show("Item name should not be empty!", "Empty item name", MessageBoxButton.OK, MessageBoxImage.Error);
                    add = false;
                    item_name.Focus();
                }

                else
                {
                    add = true;

                    if (string.IsNullOrEmpty(model.Text))
                    {
                        System.Windows.MessageBox.Show("Item model should not be empty!", "Empty item model", MessageBoxButton.OK, MessageBoxImage.Error);
                        add = false;
                        model.Focus();
                    }

                    else
                    {
                        add = true;

                        if (item_price.Text == "0.00" || String.IsNullOrEmpty(item_price.Text))
                        {
                            System.Windows.MessageBox.Show("Item price should not be empty or 0!", "Empty item price", MessageBoxButton.OK, MessageBoxImage.Error);
                            add = false;
                            item_stock.Focus();
                        }

                        else
                        {
                            add = true;

                            if (item_stock.Text == "0" || String.IsNullOrEmpty(item_stock.Text))
                            {
                                System.Windows.MessageBox.Show("Item stock should not be empty or 0!", "Empty item stock", MessageBoxButton.OK, MessageBoxImage.Error);
                                add = false;
                                item_stock.Focus();
                            }

                            else
                            {
                                add = true;

                                if (String.IsNullOrEmpty(item_stock.Text))
                                {
                                    System.Windows.MessageBox.Show("Item warranty should not be empty!", "Empty item warranty", MessageBoxButton.OK, MessageBoxImage.Error);
                                    add = false;
                                    item_stock.Focus();
                                }

                                else
                                {

                                    if (String.IsNullOrEmpty(warranty_service.Text))
                                    {
                                        System.Windows.MessageBox.Show("Item service warranty should not be empty!", "Empty item service warranty", MessageBoxButton.OK, MessageBoxImage.Error);
                                        add = false;
                                        warranty_service.Focus();
                                    }

                                    else
                                    {
                                        add = true;

                                        connection.Open();
                                        String countItems = "SELECT COUNT(*) FROM item";
                                        SqlCommand sqlCmd = new SqlCommand(countItems, connection);
                                        sqlCmd.CommandType = CommandType.Text;
                                        int itemCount = Convert.ToInt32(sqlCmd.ExecuteScalar());
                                        connection.Close();

                                        String[] itemSerialNumberArray = new string[itemCount];

                                        connection.Open();
                                        SqlCommand getItemNameCMD = new SqlCommand();
                                        getItemNameCMD.Connection = connection;
                                        getItemNameCMD.CommandText = "SELECT * FROM item";
                                        getItemNameCMD.ExecuteNonQuery();

                                        String itemSerialNumber;
                                        int loop = 0;

                                        SqlDataReader getItemNameDR = getItemNameCMD.ExecuteReader();
                                        while (getItemNameDR.Read())
                                        {
                                            itemSerialNumber = getItemNameDR.GetValue(0).ToString();
                                            itemSerialNumberArray[loop] = itemSerialNumber;

                                            loop++;
                                        }
                                        getItemNameDR.Close();
                                        connection.Close();

                                        bool checkIfFound = false;

                                        String warranty_word = warrantyInWord();
                                        String warranty_service_word = warrantyServiceInWord();

                                        for (int loopNameCounter = 0; loopNameCounter < itemCount; loopNameCounter++)
                                        {
                                            if (serial_number.Text == itemSerialNumberArray[loopNameCounter])
                                            {
                                                //question before adding item
                                                MessageBoxResult result = System.Windows.MessageBox.Show("This item is already been added, proceeding this process will update the name, model, warranty, price, description, image and stock of this item, do you want to update this item?", "Add exsisting item", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                                if (result == MessageBoxResult.Yes)
                                                {
                                                    connection.Open();
                                                    SqlCommand getItemIDCMD = new SqlCommand();
                                                    getItemIDCMD.Connection = connection;
                                                    getItemIDCMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE item.item_serial_number = '" + serial_number.Text + "'";
                                                    getItemIDCMD.ExecuteNonQuery();

                                                    String itemID;
                                                    int itemIDInt = 0;

                                                    SqlDataReader getItemIDDR = getItemIDCMD.ExecuteReader();
                                                    while (getItemIDDR.Read())
                                                    {
                                                        itemID = getItemIDDR.GetValue(1).ToString();
                                                        itemIDInt = Int32.Parse(itemID);
                                                    }
                                                    getItemIDDR.Close();
                                                    connection.Close();

                                                    int item_stock_int = Convert.ToInt32(item_stock.Text);

                                                    SqlCommand insertAvailableItemCMD = new SqlCommand();
                                                    insertAvailableItemCMD.Connection = connection;
                                                    insertAvailableItemCMD.CommandText = "UPDATE available_items SET item_stock = '" + item_stock_int + "', stock_copy = '" + item_stock_int + "' WHERE item_serial_number = '" + itemIDInt + "' ";
                                                    SqlDataAdapter insertAvailableItemDA = new SqlDataAdapter(insertAvailableItemCMD);
                                                    DataTable insertAvailableItemDT = new DataTable();
                                                    insertAvailableItemDA.Fill(insertAvailableItemDT);
                                                    connection.Close();

                                                    String itemDescription;

                                                    if (String.IsNullOrEmpty(item_description.Text))
                                                    {
                                                        itemDescription = "No description";
                                                    }

                                                    else
                                                    {
                                                        itemDescription = item_description.Text;
                                                    }

                                                    SqlCommand updateItemCMD = new SqlCommand();
                                                    updateItemCMD.Connection = connection;
                                                    updateItemCMD.CommandText = "UPDATE item SET item_name='" + item_name.Text + "',item_model = '" + model.Text + "', item_description = '" + itemDescription + "', item_price = '" + item_price.Text + "', item_warranty = '" + warranty.Text + "', item_warranty_word = '" + warranty_word + "', item_warranty_service = '" + warranty_service.Text + "', item_warranty_service_word = '" + warranty_service_word + "' WHERE item_serial_number = '" + itemIDInt + "'";
                                                    SqlDataAdapter updateItemDA = new SqlDataAdapter(updateItemCMD);
                                                    DataTable updateItemDT = new DataTable();
                                                    updateItemDA.Fill(updateItemDT);
                                                    connection.Close();

                                                    if (uploadButtonClicked == true)
                                                    {
                                                        File.Copy(filePath, targetFile, true);
                                                    }

                                                    System.Windows.MessageBox.Show("Item updated successfully");
                                                    serial_number.Text = "";
                                                    item_name.Text = "";
                                                    model.Text = "";
                                                    item_description.Text = "";
                                                    item_price.FormatString = "0.00";
                                                    item_price.Text = "0.00";
                                                    item_stock.Text = "0";
                                                    warranty.Text = "";
                                                    warranty_service.Text = "";
                                                    fill_item_data_grid();
                                                    setDefaultItemImage();
                                                    Home.instance.fill_item_data_grid();
                                                }

                                                checkIfFound = true;
                                                break;
                                            }
                                        }

                                        if (checkIfFound == false)
                                        {
                                            //question before adding item
                                            MessageBoxResult result1 = System.Windows.MessageBox.Show("Are you sure you want to add this item?", "Add item", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                            if (result1 == MessageBoxResult.Yes)
                                            {
                                                String itemDescription;

                                                if (String.IsNullOrEmpty(item_description.Text))
                                                {
                                                    itemDescription = "No description";
                                                }

                                                else
                                                {
                                                    itemDescription = item_description.Text;
                                                }

                                                DateTime dateTime = DateTime.Now;
                                                DateTime date = dateTime.Date;
                                                string formattedDate1 = "";

                                                DateTime dateValue;
                                                if (DateTime.TryParse(warranty.Text, out dateValue))
                                                {
                                                    // Format the date as a string in the desired format
                                                    formattedDate1 = dateValue.ToString("yyyy-MM-dd"); // Replace the format string with the desired format

                                                }

                                                String dateFormatted = date.ToString("yyyy-MM-dd");

                                                int item_id_variable = 0;

                                                connection.Open();
                                                SqlCommand insertItemCMD = new SqlCommand();
                                                insertItemCMD.Connection = connection;
                                                insertItemCMD.CommandText = "INSERT INTO item(item_serial_number,item_name, item_model, item_description, item_price, item_warranty, item_warranty_word, item_warranty_service, item_warranty_service_word, item_added_date) VALUES('" + serial_number.Text + "', '" + item_name.Text + "', '" + model.Text + "', '" + item_description.Text + "', '" + item_price.Text + "', '" + warranty.Text + "', '" + warranty_word + "', '" + warranty_service.Text + "', '" + warranty_service_word + "', '" + dateFormatted + "')";
                                                SqlDataAdapter insertItemDA = new SqlDataAdapter(insertItemCMD);
                                                DataTable insertItemDT = new DataTable();
                                                insertItemDA.Fill(insertItemDT);
                                                connection.Close();

                                                connection.Open();
                                                SqlCommand getItemIDCMD = new SqlCommand();
                                                getItemIDCMD.Connection = connection;
                                                getItemIDCMD.CommandText = "SELECT * FROM item WHERE item_name = '" + item_name.Text + "'";
                                                getItemIDCMD.ExecuteNonQuery();

                                                String itemID;
                                                int itemIDInt = 0;

                                                SqlDataReader getItemIDDR = getItemIDCMD.ExecuteReader();
                                                while (getItemIDDR.Read())
                                                {
                                                    itemID = getItemIDDR.GetValue(0).ToString();
                                                    itemIDInt = Int32.Parse(itemID);
                                                }
                                                getItemIDDR.Close();
                                                connection.Close();

                                                SqlCommand insertAvailableItemCMD = new SqlCommand();
                                                insertAvailableItemCMD.Connection = connection;
                                                insertAvailableItemCMD.CommandText = "INSERT INTO available_items(item_serial_number, item_stock, stock_copy) VALUES('" + itemIDInt + "', '" + item_stock.Text + "', '" + item_stock.Text + "')";
                                                SqlDataAdapter insertAvailableItemDA = new SqlDataAdapter(insertAvailableItemCMD);
                                                DataTable insertAvailableItemDT = new DataTable();
                                                insertAvailableItemDA.Fill(insertAvailableItemDT);
                                                connection.Close();

                                                if (uploadButtonClicked == true)
                                                {
                                                    File.Copy(filePath, targetFile, true);
                                                }

                                                System.Windows.MessageBox.Show("Item added successfully");
                                                serial_number.Text = "";
                                                item_name.Text = "";
                                                model.Text = "";
                                                item_description.Text = "";
                                                item_price.FormatString = "0.00";
                                                item_price.Text = "0.00";
                                                item_stock.Text = "0";
                                                warranty.Text = "";
                                                warranty_service.Text = "";
                                                fill_item_data_grid();
                                                setDefaultItemImage();
                                                Home.instance.fill_item_data_grid();
                                            }
                                        }
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

            DateTime warrantyDate = DateTime.Parse(warranty.Text);

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
                if(numberOfWarrantyDay > 1)
                {
                    warranty_word = numberOfWarrantyDay + " Days";
                }

                else
                {
                    warranty_word = numberOfWarrantyDay + " Day";
                }
               
            }

            else if(numberOfWarrantyDay >= 7 && numberOfWarrantyDay < numberOfDaysOfMonthToday)
            {
                int totalWeeksCount = 7;// this is in loop so that we can determine how many weeks it counted base from the total number of warranty day
                int totalWeeks = 0;// this is to count the total weeks 

                for(;;)
                {   
                    if(numberOfWarrantyDay >= totalWeeksCount)
                    {
                        totalWeeks++;
                        totalWeeksCount = totalWeeksCount + 7;
                    }

                    else
                    {
                        int numberOfRemainingDays = numberOfWarrantyDay - (totalWeeks * 7);

                        if(totalWeeks > 1)
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

            else if(numberOfWarrantyDay >= numberOfDaysOfMonthToday && numberOfWarrantyDay < 365)
            {
                int totalMonthsCount = numberOfDaysOfMonthToday;// this is in loop so that we can determine how many months it counted base from the total number of warranty day
                int totalMonths = 0;// this is to count the total months 
                int forLoopCounter = 1;
               
                for (;;)
                {
                    if(numberOfWarrantyDay >= totalMonthsCount)
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

            DateTime warrantyServiceDate = DateTime.Parse(warranty_service.Text);

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



        //for number input only in pin code input
        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

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
        //************

        private void upload_item_image_Click(object sender, RoutedEventArgs e)
        {
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

                file_name.Text = fileName;
                item_image.Source = bitmapImage;

                uploadButtonClicked = true;
                remove_image_btn.Visibility = Visibility.Visible;
            }
        }
        //end************************************

        private void fill_item_data_grid()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                    SqlCommand fillProductTableCMD = new SqlCommand();
                    fillProductTableCMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE available_items.stock_copy > 0";
                    fillProductTableCMD.Connection = connection;
                    SqlDataAdapter fillProductTableDA = new SqlDataAdapter(fillProductTableCMD);
                    DataTable fillProductTableDT = new DataTable("item");
                    fillProductTableDA.Fill(fillProductTableDT);

                    product_data_grid.ItemsSource = fillProductTableDT.DefaultView;

                    connection.Close();

                }

                //if nothing found
                else
                {

                }
            }

            catch (Exception ex)
            {
                //for Connection error in data base
                System.Windows.MessageBox.Show("Connection error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            finally
            {
                connection.Close();
            }

        }


        //Remove image button
        private void remove_image_btn_click(object sender, MouseButtonEventArgs e)
        {
            setDefaultItemImage();
            remove_image_btn.Visibility = Visibility.Hidden;
        }
        //***
    }
}
