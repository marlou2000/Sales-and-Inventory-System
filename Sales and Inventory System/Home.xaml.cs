using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;

namespace Sales_and_Inventory_System
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");

        int dropdown_counter_username = 0;
        int dropdown_counter_sales = 0;
        public static Home instance;

        bool didUserSelectedItem = false;

        public bool purchased = false;

        public String itemIDToEdit;

        bool isShowItemDetailsButtonIsClicked = false;

        int totalDailyIncome = 0, totalWeeklyIncome = 0, totalMonthlyIncome = 0, totalYearlyIncome = 0;

        int totalDaysInWeek = 7;

        int monday = 1, tuesday = 2, wednesday = 3, thursday = 4, friday = 5, saturday = 6, sunday = 7;
        
        public Home()
        {
            InitializeComponent();

            username_label.Content = Login.instance.username;

            //Hiding the first columns of every grid
            item_data_grid.Columns[0].MaxWidth = 0;
            //end**********

            instance = this;

            item_preview.Visibility = Visibility.Collapsed;
            item_details_column.Width = new GridLength(1, GridUnitType.Auto);

            refreshItemStocks();

            setDefaultItemImage();

            fill_item_data_grid();

            updateItemTotalCost();
            updateTotalItemCount();

            sales_label.Content = "Daily income";

            computeDailyIncome();
            //computeWeeklyIncome();
            computeMonthlyIncome();
            computeYearlyIncome();
        }

        private void refreshItemStocks()
        {
            int numberOfItemsInDatabase = 0;

            connection.Open();
            SqlCommand refreshItemStocksCMD = new SqlCommand();
            refreshItemStocksCMD.Connection = connection;
            refreshItemStocksCMD.CommandText = "SELECT * FROM available_items INNER JOIN items ON available_items.item_id = items.item_id WHERE available_items.item_stock > 0";
            refreshItemStocksCMD.ExecuteNonQuery();

            String itemIDFromDatabase;
            String itemStockFromDatabase;            

            SqlDataReader refreshItemStocksDR = refreshItemStocksCMD.ExecuteReader();
            while (refreshItemStocksDR.Read())
            {
                numberOfItemsInDatabase++;
            }
            refreshItemStocksDR.Close();
            connection.Close();

            string[] itemID = new string[numberOfItemsInDatabase];
            string[] itemStock = new string[numberOfItemsInDatabase];

            int itemLoop = 0;

            connection.Open();
            SqlCommand refreshItemStocks1CMD = new SqlCommand();
            refreshItemStocks1CMD.Connection = connection;
            refreshItemStocks1CMD.CommandText = "SELECT * FROM available_items INNER JOIN items ON available_items.item_id = items.item_id WHERE available_items.item_stock > 0";
            refreshItemStocks1CMD.ExecuteNonQuery();

            SqlDataReader refreshItemStocks1DR = refreshItemStocks1CMD.ExecuteReader();
            while (refreshItemStocks1DR.Read())
            {
                itemIDFromDatabase = refreshItemStocks1DR.GetValue(0).ToString();
                itemStockFromDatabase = refreshItemStocks1DR.GetValue(2).ToString();

                itemID[itemLoop] = itemIDFromDatabase;
                itemStock[itemLoop] = itemStockFromDatabase;

                itemLoop++;
            }
            refreshItemStocks1DR.Close();
            connection.Close();

            for (int updateCounter = 0; updateCounter < numberOfItemsInDatabase; updateCounter++)
            {
                connection.Open();
                SqlCommand updateItemStockCopyCMD = new SqlCommand();
                updateItemStockCopyCMD.Connection = connection;
                updateItemStockCopyCMD.CommandText = "UPDATE available_items SET stock_copy = '" + itemStock[updateCounter] + "' WHERE item_id = '" + itemID[updateCounter] + "'";
                SqlDataAdapter updateItemStockCopyDA = new SqlDataAdapter(updateItemStockCopyCMD);
                DataTable updateItemStockCopyDT = new DataTable();
                updateItemStockCopyDA.Fill(updateItemStockCopyDT);
                connection.Close();
            }


        }

        public void updateItemTotalCost()
        {
            String itemPriceFromDatabase;
            String itemStockFromDatabase;
            int itemPriceFromDatabaseInt;
            int itemStockFromDatabaseInt;

            int PartialValue = 0;
            int TotalValue = 0;

            connection.Open();
            SqlCommand refreshItemStocks1CMD = new SqlCommand();
            refreshItemStocks1CMD.Connection = connection;
            refreshItemStocks1CMD.CommandText = "SELECT * FROM items INNER JOIN available_items ON items.item_id = available_items.item_id WHERE available_items.item_stock > 0";
            refreshItemStocks1CMD.ExecuteNonQuery();

            SqlDataReader refreshItemStocks1DR = refreshItemStocks1CMD.ExecuteReader();
            while (refreshItemStocks1DR.Read())
            {
                itemPriceFromDatabase = refreshItemStocks1DR.GetValue(2).ToString();
                itemStockFromDatabase = refreshItemStocks1DR.GetValue(7).ToString();

                itemPriceFromDatabaseInt = Int32.Parse(itemPriceFromDatabase);
                itemStockFromDatabaseInt = Int32.Parse(itemStockFromDatabase);

                PartialValue = itemPriceFromDatabaseInt * itemStockFromDatabaseInt;
                TotalValue = TotalValue + PartialValue;
            }
            refreshItemStocks1DR.Close();
            connection.Close();

            total_value.Content = TotalValue.ToString();
        }

        public void updateTotalItemCount()
        {
            String itemStockFromDatabase;
            int itemStockFromDatabaseInt;

            int TotalItemCount = 0;

            connection.Open();
            SqlCommand refreshItemStocks1CMD = new SqlCommand();
            refreshItemStocks1CMD.Connection = connection;
            refreshItemStocks1CMD.CommandText = "SELECT * FROM items INNER JOIN available_items ON items.item_id = available_items.item_id WHERE available_items.item_stock > 0";
            refreshItemStocks1CMD.ExecuteNonQuery();

            SqlDataReader refreshItemStocks1DR = refreshItemStocks1CMD.ExecuteReader();
            while (refreshItemStocks1DR.Read())
            {
                itemStockFromDatabase = refreshItemStocks1DR.GetValue(7).ToString();
                itemStockFromDatabaseInt = Int32.Parse(itemStockFromDatabase);

                TotalItemCount = TotalItemCount + itemStockFromDatabaseInt;
            }
            refreshItemStocks1DR.Close();
            connection.Close();

            total_item.Content = TotalItemCount.ToString();
        }

        //for losing focus on text box and data grid cell when click outside
        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image myImage = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("img/Left arrow.png", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            show_item_details_image_btn.Source = bi;

            isShowItemDetailsButtonIsClicked = false;

            item_preview.Visibility = Visibility.Collapsed;
            item_details_column.Width = new GridLength(1, GridUnitType.Auto);

            logout_btn.Visibility = Visibility.Hidden;
            dropdown_counter_username = 0;

            sales_grid_dropdown.Margin = new Thickness(0, 0, 0, 0);
            dropdown_counter_sales = 0;

            didUserSelectedItem = false;

            edit_item_btn.IsEnabled = false;

            Keyboard.ClearFocus();

            setDefaultItemImage();
            name_textblock.Text = "Item name";
            price_textblock.Text = "0.00";
            description_textblock.Text = "Item description";
            stock_textblock.Text = "0";

            SolidColorBrush newInactiveSelectionBrush = new SolidColorBrush(Colors.Transparent);
            SolidColorBrush newInactiveSelectionTextBrush = new SolidColorBrush(Colors.Black);

            item_data_grid.Resources[SystemColors.InactiveSelectionHighlightBrushKey] = newInactiveSelectionBrush;
            item_data_grid.Resources[SystemColors.InactiveSelectionHighlightTextBrushKey] = newInactiveSelectionTextBrush;

            search_textbox.Text = "";

            fill_item_data_grid();
        }
        //end ************************************

        //maintain row of dattagrid still selected
        private void window_MouseDown_item_grid(object sender, RoutedEventArgs e)
        {
            logout_btn.Visibility = Visibility.Hidden;
            dropdown_counter_username = 0;

            sales_grid_dropdown.Margin = new Thickness(0, 0, 0, 0);
            dropdown_counter_sales = 0;

            if (didUserSelectedItem == false)
            {
                SolidColorBrush newInactiveSelectionBrushItemGrid = new SolidColorBrush(Colors.Transparent);
                SolidColorBrush newInactiveSelectionTextBrushItemGrid = new SolidColorBrush(Colors.Black);

                item_data_grid.Resources[SystemColors.InactiveSelectionHighlightBrushKey] = newInactiveSelectionBrushItemGrid;
                item_data_grid.Resources[SystemColors.InactiveSelectionHighlightTextBrushKey] = newInactiveSelectionTextBrushItemGrid;
            }

            else
            {
                SolidColorBrush newInactiveSelectionBrushItemGrid = new SolidColorBrush(Colors.Blue);
                SolidColorBrush newInactiveSelectionTextBrushItemGrid = new SolidColorBrush(Colors.White);

                item_data_grid.Resources[SystemColors.InactiveSelectionHighlightBrushKey] = newInactiveSelectionBrushItemGrid;
                item_data_grid.Resources[SystemColors.InactiveSelectionHighlightTextBrushKey] = newInactiveSelectionTextBrushItemGrid;
            }

            Keyboard.ClearFocus();
        }
        //end ************************************

        public void setDefaultItemImage()
        {
            String sourceFilePath1 = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            String DefaultImagepath = sourceFilePath1 + "\\img\\item_images\\Default.png";

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(DefaultImagepath);
            bitmap.EndInit();

            Image image = new Image();
            item_image.Source = bitmap;
        }

        //Dropdown username above top right header
        private void user_btn_Click(object sender, RoutedEventArgs e)
        {
            sales_grid_dropdown.Margin = new Thickness(0, 0, 0, 0);
            dropdown_counter_sales = 0;

            if (dropdown_counter_username == 0)
            {
                dropdown_counter_username = 1;
                logout_btn.Visibility = Visibility.Visible;
            }

            else
            {
                logout_btn.Visibility = Visibility.Hidden;
                dropdown_counter_username = 0;
            }

        }
        //end***************

        //Logout
        private void logout_btn_Click(object sender, RoutedEventArgs e)
        {
            //question before logout
            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to logout?", "Logout", System.Windows.MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                //redirect to another page
                Login loginWIndown = new Login();
                loginWIndown.Show();
                this.Close();
            }

            else
            {
                logout_btn.Visibility = Visibility.Hidden;
                dropdown_counter_username = 0;
            }

        }

        private void sales_btn_Click(object sender, RoutedEventArgs e)
        {
            logout_btn.Visibility = Visibility.Hidden;
            dropdown_counter_username = 0;

            if (dropdown_counter_sales == 0)
            {
                dropdown_counter_sales = 1;
                sales_grid_dropdown.Margin = new Thickness(0, 0, 0, -418);
            }

            else
            {
                sales_grid_dropdown.Margin = new Thickness(0, 0, 0, 0);
                dropdown_counter_sales = 0;
            }
        }
        //end***************

        private void daily_sales_btn_Click(object sender, RoutedEventArgs e)
        {
            income_total_main_daily.Visibility = Visibility.Visible;
            income_total_main_weekly.Visibility = Visibility.Collapsed;
            income_total_main_monthly.Visibility = Visibility.Collapsed;
            income_total_main_yearly.Visibility = Visibility.Collapsed;

            sales_label.Content = "Daily income";

            computeDailyIncome();

            sales_grid_dropdown.Margin = new Thickness(0, 0, 0, 0);
            dropdown_counter_sales = 0;
        }
        //end***************

        private void weekly_sales_btn_Click(object sender, RoutedEventArgs e)
        {
            income_total_main_daily.Visibility = Visibility.Collapsed;
            income_total_main_weekly.Visibility = Visibility.Visible;
            income_total_main_monthly.Visibility = Visibility.Collapsed;
            income_total_main_yearly.Visibility = Visibility.Collapsed;

            sales_label.Content = "Weekly income";

            computeWeeklyIncome();

            sales_grid_dropdown.Margin = new Thickness(0, 0, 0, 0);
            dropdown_counter_sales = 0;
        }
        //end***************

        private void monthly_sales_btn_Click(object sender, RoutedEventArgs e)
        {
            income_total_main_daily.Visibility = Visibility.Collapsed;
            income_total_main_weekly.Visibility = Visibility.Collapsed;
            income_total_main_monthly.Visibility = Visibility.Visible;
            income_total_main_yearly.Visibility = Visibility.Collapsed;

            sales_label.Content = "Monthly income";

            computeMonthlyIncome();

            sales_grid_dropdown.Margin = new Thickness(0, 0, 0, 0);
            dropdown_counter_sales = 0;
        }
        //end***************

        private void yearly_sales_btn_Click(object sender, RoutedEventArgs e)
        {
            income_total_main_daily.Visibility = Visibility.Collapsed;
            income_total_main_weekly.Visibility = Visibility.Collapsed;
            income_total_main_monthly.Visibility = Visibility.Collapsed;
            income_total_main_yearly.Visibility = Visibility.Visible;

            sales_label.Content = "Yearly income";

            computeYearlyIncome();

            sales_grid_dropdown.Margin = new Thickness(0, 0, 0, 0);
            dropdown_counter_sales = 0;
        }
        //end***************

        public void computeDailyIncome()
        {
            totalDailyIncome = 0;

            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;

            String dateFormatted = date.ToString("yyyy-MM-dd");

            connection.Open();
            SqlCommand countTotalDailySalesCMD = new SqlCommand();
            countTotalDailySalesCMD.Connection = connection;
            countTotalDailySalesCMD.CommandText = "SELECT * FROM date INNER JOIN sales_history ON date.date_id = sales_history.date_ordered WHERE date.date_ordered = '" + dateFormatted + "'";
            countTotalDailySalesCMD.ExecuteNonQuery();

            String totalCostString;
            int totalCostInt;

            SqlDataReader countTotalDailySalesDR = countTotalDailySalesCMD.ExecuteReader();
            while (countTotalDailySalesDR.Read())
            {
                totalCostString = countTotalDailySalesDR.GetValue(11).ToString();
                totalCostInt = Convert.ToInt32(totalCostString);

                totalDailyIncome = totalDailyIncome + totalCostInt;
            }
            countTotalDailySalesDR.Close();
            connection.Close();

            income_total_main_daily.Content = totalDailyIncome.ToString();
            daily_income_total.Content = totalDailyIncome.ToString();
        }

        public void computeWeeklyIncome()
        {
            totalWeeklyIncome = 0;

            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;

            String dateFormatted = date.ToString("yyyy-MM-dd");

            //getting Month Now
            int indexOfYearNow = dateFormatted.IndexOf('-');
            int indexOfYearPlusOneNow = indexOfYearNow;
            string yearNow = dateFormatted.Substring(0, indexOfYearPlusOneNow);
            int yearNowInt = Convert.ToInt32(yearNow);
            string dateWithoutYearNow = dateFormatted.Replace(yearNow, "");
            string removeFirstCharacter = dateWithoutYearNow.Length > 1 ? dateWithoutYearNow.Substring(1) : "";
            int indexOfMonthNow = removeFirstCharacter.IndexOf('-');
            string monthNow = removeFirstCharacter.Substring(0, indexOfMonthNow);
            int monthNowInt = Convert.ToInt32(monthNow);
            string dateWithoutMonthNow = removeFirstCharacter.Replace(monthNow, "");
            string removeFirstCharacterDay = dateWithoutMonthNow.Replace("-", "");
            int dayNowInt = Convert.ToInt32(removeFirstCharacterDay);

            connection.Open();
            SqlCommand countTotalWeeklySalesCMD = new SqlCommand();
            countTotalWeeklySalesCMD.Connection = connection;
            countTotalWeeklySalesCMD.CommandText = "SELECT * FROM sales_history WHERE month_ordered = '" + monthNowInt + "'";
            countTotalWeeklySalesCMD.ExecuteNonQuery();

            String orderedMonth;
            int orderedMonthInt;
            String orderedWeek;
            int orderedWeekInt;
            String totalWeeklySales;
            int totalWeeklySalesInt;

            SqlDataReader countTotalWeeklySalesDR = countTotalWeeklySalesCMD.ExecuteReader();
            while (countTotalWeeklySalesDR.Read())
            {
                orderedMonth = countTotalWeeklySalesDR.GetValue(6).ToString();
                orderedMonthInt = Convert.ToInt32(orderedMonth);
                orderedWeek = countTotalWeeklySalesDR.GetValue(7).ToString();
                orderedWeekInt = Convert.ToInt32(orderedWeek);
                totalWeeklySales = countTotalWeeklySalesDR.GetValue(3).ToString();
                totalWeeklySalesInt = Convert.ToInt32(totalWeeklySales);

                int lastDayOfTheMonth = 0;
                int week_now = 1;

                if (monthNow == "01") { lastDayOfTheMonth = 31; }

                else if (monthNow == "02") { lastDayOfTheMonth = 28; }

                else if (monthNow == "03") { lastDayOfTheMonth = 31; }

                else if (monthNow == "04") { lastDayOfTheMonth = 30; }

                else if (monthNow == "05") { lastDayOfTheMonth = 31; }

                else if (monthNow == "06") { lastDayOfTheMonth = 30; }

                else if (monthNow == "07") { lastDayOfTheMonth = 31; }

                else if (monthNow == "08") { lastDayOfTheMonth = 31; }

                else if (monthNow == "09") { lastDayOfTheMonth = 30; }

                else if (monthNow == "10") { lastDayOfTheMonth = 31; }

                else if (monthNow == "11") { lastDayOfTheMonth = 30; }

                else if (monthNow == "12") { lastDayOfTheMonth = 31; }

                if (lastDayOfTheMonth == 28)
                {
                    if (dayNowInt <= 7)
                    {
                        week_now = 1;
                    }

                    else if (dayNowInt <= 14)
                    {
                        week_now = 2;
                    }

                    else if (dayNowInt <= 21)
                    {
                        week_now = 3;
                    }

                    else
                    {
                        week_now = 4;
                    }
                }

                else if (lastDayOfTheMonth == 30 || lastDayOfTheMonth == 31)
                {
                    if (dayNowInt <= 7)
                    {
                        week_now = 1;
                    }

                    else if (dayNowInt <= 14)
                    {
                        week_now = 2;
                    }

                    else if (dayNowInt <= 21)
                    {
                        week_now = 3;
                    }

                    else if (dayNowInt <= 28)
                    {
                        week_now = 4;
                    }

                    else
                    {
                        week_now = 5;
                    }
                }

                if(week_now == orderedWeekInt)
                {
                    totalWeeklyIncome = totalWeeklyIncome + totalWeeklySalesInt;
                }


            }
            countTotalWeeklySalesDR.Close();
            connection.Close();

            income_total_main_weekly.Content = totalWeeklyIncome;
            weekly_income_total.Content = totalWeeklyIncome;
        }

        public void computeWeeklyIncome1()
        {
            //getting day of the week
            DayOfWeek today = DateTime.Now.DayOfWeek;

            connection.Open();
            SqlCommand daysOfTheWeekCMD = new SqlCommand();
            daysOfTheWeekCMD.Connection = connection;
            daysOfTheWeekCMD.CommandText = "SELECT * FROM sales_history WHERE day_of_the_week_ordered = '" + today + "'";
            daysOfTheWeekCMD.ExecuteNonQuery();

            String daysOfTheWeekString;

            SqlDataReader daysOfTheWeekDR = daysOfTheWeekCMD.ExecuteReader();
            while (daysOfTheWeekDR.Read())
            {
                daysOfTheWeekString = daysOfTheWeekDR.GetValue(9).ToString();

                 
            }
            daysOfTheWeekDR.Close();
            connection.Close();


        }

        public void computeMonthlyIncome()
        {
            totalMonthlyIncome = 0;

            income_total_main_monthly.Content = "0";
            monthly_income_total.Content = totalMonthlyIncome.ToString();

            /*DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;

            String dateFormatted = date.ToString("yyyy-MM-dd");

            connection.Open();
            SqlCommand countTotalDailySalesCMD = new SqlCommand();
            countTotalDailySalesCMD.Connection = connection;
            countTotalDailySalesCMD.CommandText = "SELECT * FROM sales_history WHERE order_date = '" + dateFormatted + "'";
            countTotalDailySalesCMD.ExecuteNonQuery();

            String totalCostString;
            int totalCostInt;

            int totalDailyIncome = 0;

            SqlDataReader countTotalDailySalesDR = countTotalDailySalesCMD.ExecuteReader();
            while (countTotalDailySalesDR.Read())
            {
                totalCostString = countTotalDailySalesDR.GetValue(3).ToString();
                totalCostInt = Convert.ToInt32(totalCostString);

                totalDailyIncome = totalDailyIncome + totalCostInt;
            }
            countTotalDailySalesDR.Close();
            connection.Close();*/
        }

        public void computeYearlyIncome()
        {
            totalYearlyIncome = 0;

            income_total_main_yearly.Content = "0";
            yearly_income_total.Content = totalYearlyIncome.ToString();

            /*DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;

            String dateFormatted = date.ToString("yyyy-MM-dd");

            connection.Open();
            SqlCommand countTotalDailySalesCMD = new SqlCommand();
            countTotalDailySalesCMD.Connection = connection;
            countTotalDailySalesCMD.CommandText = "SELECT * FROM sales_history WHERE order_date = '" + dateFormatted + "'";
            countTotalDailySalesCMD.ExecuteNonQuery();

            String totalCostString;
            int totalCostInt;

            int totalDailyIncome = 0;

            SqlDataReader countTotalDailySalesDR = countTotalDailySalesCMD.ExecuteReader();
            while (countTotalDailySalesDR.Read())
            {
                totalCostString = countTotalDailySalesDR.GetValue(3).ToString();
                totalCostInt = Convert.ToInt32(totalCostString);

                totalDailyIncome = totalDailyIncome + totalCostInt;
            }
            countTotalDailySalesDR.Close();
            connection.Close();*/
        }

        //Product data grid
        public void fill_item_data_grid()
        {
            updateItemTotalCost();
            updateTotalItemCount();

            //computeDailyIncome();
            //computeWeeklyIncome();
            computeMonthlyIncome();
            computeYearlyIncome();
            
            item_preview.Visibility = Visibility.Collapsed;
            item_details_column.Width = new GridLength(1, GridUnitType.Auto);

            Image myImage = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("img/Left arrow.png", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            show_item_details_image_btn.Source = bi;

            isShowItemDetailsButtonIsClicked = false;

            SqlConnection sqlCon = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();

                    SqlCommand fillProductTableCMD = new SqlCommand();
                    fillProductTableCMD.CommandText = "SELECT * FROM available_items INNER JOIN items ON available_items.item_id = items.item_id WHERE item_stock > 0";
                    fillProductTableCMD.Connection = sqlCon;
                    SqlDataAdapter fillProductTableDA = new SqlDataAdapter(fillProductTableCMD);
                    DataTable fillProductTableDT = new DataTable("items");
                    fillProductTableDA.Fill(fillProductTableDT);

                    item_data_grid.ItemsSource = fillProductTableDT.DefaultView;

                    sqlCon.Close();

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
                sqlCon.Close();
            }
        }

        private void add_item_Click(object sender, RoutedEventArgs e)
        {
            AddItem addItemWindow = new AddItem();
            addItemWindow.Show();
        }
        //end****************


        //view item detailes
        private void view_item_btn_Click(object sender, RoutedEventArgs e)
        {
            isShowItemDetailsButtonIsClicked = true;

            Image myImage = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("img/Right arrow.png", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            show_item_details_image_btn.Source = bi;

            didUserSelectedItem = true;

            edit_item_btn.IsEnabled = true;

            TextBlock itemID = item_data_grid.Columns[0].GetCellContent(item_data_grid.Items[item_data_grid.SelectedIndex]) as TextBlock;
            String itemIDString = itemID.Text;
            int itemIDInt = Convert.ToInt32(itemIDString);

            TextBlock itemName = item_data_grid.Columns[1].GetCellContent(item_data_grid.Items[item_data_grid.SelectedIndex]) as TextBlock;
            String itemNameString = itemName.Text;

            String sourceFilePathPartial = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            String sourceFilePathPartialFinal = sourceFilePathPartial + "\\img\\item_images\\";
            string[] fileNames = Directory.GetFiles(sourceFilePathPartialFinal);

            bool imageFound = false;

            foreach (string fileName in fileNames)
            {
                string fileNameWithExtension = System.IO.Path.GetFileName(fileName);

                int index = fileNameWithExtension.IndexOf('.');
                string fileNameFInal = fileNameWithExtension.Substring(0, index);

                if(itemNameString == fileNameFInal)
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

            if(imageFound == false)
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

            connection.Open();
            SqlCommand getItemInformationsCMD = new SqlCommand();
            getItemInformationsCMD.Connection = connection;
            getItemInformationsCMD.CommandText = "SELECT * FROM items INNER JOIN available_items ON items.item_id = available_items.item_id WHERE items.item_id = '" + itemIDInt + "'";            
            getItemInformationsCMD.ExecuteNonQuery();

            String itemNameInformation;
            String itemDescriptionInformation;
            String itemPriceInformation;
            String itemStockInformation;

            SqlDataReader getItemInformationsDR = getItemInformationsCMD.ExecuteReader();
            while (getItemInformationsDR.Read())
            {
                itemNameInformation = getItemInformationsDR.GetValue(1).ToString();
                itemPriceInformation = getItemInformationsDR.GetValue(2).ToString();
                itemDescriptionInformation = getItemInformationsDR.GetValue(3).ToString();
                itemStockInformation = getItemInformationsDR.GetValue(7).ToString();

                name_textblock.Text = itemNameInformation;
                price_textblock.Text = itemPriceInformation;
                description_textblock.Text = itemDescriptionInformation;
                stock_textblock.Text = itemStockInformation;
            }
            getItemInformationsDR.Close();
            connection.Close();

            item_preview.Visibility = Visibility.Visible;
            item_details_column.Width = new GridLength(317);

        }

        private void delete_item_btn_Click(object sender, RoutedEventArgs e)
        { 
            //question before adding item
            MessageBoxResult result1 = System.Windows.MessageBox.Show("Are you sure you want to delete this item?", "Delete item", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result1 == MessageBoxResult.Yes)
            {
                bool itemStillExist = false;

                TextBlock itemID = item_data_grid.Columns[0].GetCellContent(item_data_grid.Items[item_data_grid.SelectedIndex]) as TextBlock;
                String itemIDString = itemID.Text;
                int itemIDInt = Convert.ToInt32(itemIDString);

                connection.Open();
                //query for verifying pin code in database
                String verifyItemIfStillExist = "SELECT COUNT(1) FROM items WHERE item_id = '" + itemIDString + "'";
                SqlCommand sqlCmd = new SqlCommand(verifyItemIfStillExist, connection);
                sqlCmd.CommandType = CommandType.Text;
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                //if found similar pin code in database
                if (count == 1)
                {
                    itemStillExist = true;
                }
                connection.Close();

                if(itemStillExist == true)
                {
                    connection.Open();
                    SqlCommand deleteItemCMD = new SqlCommand();
                    deleteItemCMD.Connection = connection;
                    deleteItemCMD.CommandText = "DELETE FROM available_items WHERE item_id = '" + itemIDString + "' ";
                    SqlDataAdapter deleteItemDA = new SqlDataAdapter(deleteItemCMD);
                    DataTable deleteItemDT = new DataTable();
                    deleteItemDA.Fill(deleteItemDT);
                    connection.Close();

                    System.Windows.MessageBox.Show("Item has been deleted successfully!", "Item deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    System.Windows.MessageBox.Show("The item that you are trying to delete is not existing already", "Item deleted already", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                updateTotalItemCount();
                updateItemTotalCost();
                fill_item_data_grid();

            }
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

        private void item_data_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*DataGrid dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedCells.Count == 0)
            {
                System.Windows.MessageBox.Show("Null");
            }*/
        }

        private void order_btn_Click(object sender, RoutedEventArgs e)
        {
            Order orderWindow = new Order();
            orderWindow.Show();
        }

        private void edit_item_btn_Click(object sender, RoutedEventArgs e)
        {
            TextBlock itemID = item_data_grid.Columns[0].GetCellContent(item_data_grid.Items[item_data_grid.SelectedIndex]) as TextBlock;
            String itemIDString = itemID.Text;
            int itemIDInt = Convert.ToInt32(itemIDString);

            itemIDToEdit = itemIDString;

            EditItem editItemWindow = new EditItem();
            editItemWindow.Show();
        }

        private void show_item_details_btn_Click(object sender, RoutedEventArgs e)
        {

            if (isShowItemDetailsButtonIsClicked == false)
            {
                item_preview.Visibility = Visibility.Visible;
                item_details_column.Width = new GridLength(317);

                Image myImage = new Image();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("img/Right arrow.png", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                show_item_details_image_btn.Source = bi;

                isShowItemDetailsButtonIsClicked = true;
            }

            else
            {
                item_preview.Visibility = Visibility.Collapsed;
                item_details_column.Width = new GridLength(1, GridUnitType.Auto);

                Image myImage = new Image();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("img/Left arrow.png", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                show_item_details_image_btn.Source = bi;

                isShowItemDetailsButtonIsClicked = false;
            }
            
        }

        private void search_btn_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            SqlCommand searchItemCMD = new SqlCommand();
            searchItemCMD.CommandText = "SELECT * FROM available_items INNER JOIN items ON available_items.item_id = items.item_id WHERE items.item_name LIKE '%" + search_textbox.Text + "%' AND item_stock > 0";
            searchItemCMD.Connection = connection;
            SqlDataAdapter searchItemDA = new SqlDataAdapter(searchItemCMD);
            DataTable searchItemDT = new DataTable("available_items");
            searchItemDA.Fill(searchItemDT);

            item_data_grid.ItemsSource = searchItemDT.DefaultView;

            connection.Close();
        }
        //************

    }
}
