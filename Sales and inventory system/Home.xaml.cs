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
using Sales_and_Inventory_System;

namespace Sales_and_Inventory_System
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        SqlConnection connection = ConnectionString.Connection;

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

            instance = this;

            item_preview.Visibility = Visibility.Collapsed;
            item_details_column.Width = new GridLength(1, GridUnitType.Auto);

            refreshItemStocks();

            setDefaultItemImage();

            //fill_item_data_grid();

            //updateItemTotalCost();
            //updateTotalItemCount();

            sales_label.Content = "Daily income";


            computeDailyIncome();
            computeWeeklyIncome1();
            computeMonthlyIncome();
            computeYearlyIncome();

            //computeDailyIncome();
            //computeWeeklyIncome();
            //computeMonthlyIncome();
            //computeYearlyIncome();

            //currentMonthWeeks();

        }

        private void refreshItemStocks()
        {
            int numberOfItemsInDatabase = 0;

            connection.Open();
            SqlCommand refreshItemStocksCMD = new SqlCommand();
            refreshItemStocksCMD.Connection = connection;
            refreshItemStocksCMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE available_items.item_stock > 0";
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
            refreshItemStocks1CMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE available_items.item_stock > 0";
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
                updateItemStockCopyCMD.CommandText = "UPDATE available_items SET stock_copy = '" + itemStock[updateCounter] + "' WHERE item_serial_number = '" + itemID[updateCounter] + "'";
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
            refreshItemStocks1CMD.CommandText = "SELECT * FROM item INNER JOIN available_items ON item.item_serial_number = available_items.item_serial_number WHERE available_items.item_stock > 0";
            refreshItemStocks1CMD.ExecuteNonQuery();

            SqlDataReader refreshItemStocks1DR = refreshItemStocks1CMD.ExecuteReader();
            while (refreshItemStocks1DR.Read())
            {
                itemPriceFromDatabase = refreshItemStocks1DR.GetValue(4).ToString();
                itemStockFromDatabase = refreshItemStocks1DR.GetValue(12).ToString();

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
            refreshItemStocks1CMD.CommandText = "SELECT * FROM item INNER JOIN available_items ON item.item_serial_number = available_items.item_serial_number WHERE available_items.item_stock > 0";
            refreshItemStocks1CMD.ExecuteNonQuery();

            SqlDataReader refreshItemStocks1DR = refreshItemStocks1CMD.ExecuteReader();
            while (refreshItemStocks1DR.Read())
            {
                itemStockFromDatabase = refreshItemStocks1DR.GetValue(12).ToString();
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

            computeWeeklyIncome1();

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
            countTotalDailySalesCMD.CommandText = "SELECT * FROM date INNER JOIN sales_history ON date.date_id = sales_history.date_id WHERE date.date_ordered = '" + dateFormatted + "'";
            countTotalDailySalesCMD.ExecuteNonQuery();

            String totalCostString;
            int totalCostInt;

            SqlDataReader countTotalDailySalesDR = countTotalDailySalesCMD.ExecuteReader();
            while (countTotalDailySalesDR.Read())
            {
                totalCostString = countTotalDailySalesDR.GetValue(12).ToString();
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
            //WEEK OF THE DAY , DAY OF MONTH BASIS
            int weekTodayIndex = weekTodayIndex_dayOfMonthBasis();

            String totalCostString;
            int totalCostInt = 0;
            int totalWeeklySales = 0;

            connection.Open();
            SqlCommand countTotalWeeklySalesCMD = new SqlCommand();
            countTotalWeeklySalesCMD.Connection = connection;
            countTotalWeeklySalesCMD.CommandText = "SELECT * FROM date INNER JOIN sales_history ON date.date_id = sales_history.date_id WHERE date.week_ordered = '" + weekTodayIndex + "'";
            countTotalWeeklySalesCMD.ExecuteNonQuery();

            SqlDataReader countTotalWeeklySalesDR = countTotalWeeklySalesCMD.ExecuteReader();
            while (countTotalWeeklySalesDR.Read())
            {
                totalCostString = countTotalWeeklySalesDR.GetValue(12).ToString();
                totalCostInt = Convert.ToInt32(totalCostString);

                totalWeeklySales = totalWeeklySales + totalCostInt;
            }
            countTotalWeeklySalesDR.Close();
            connection.Close();

            income_total_main_weekly.Content = totalWeeklySales.ToString();
            weekly_income_total.Content = totalWeeklySales.ToString();

        }

        public void computeWeeklyIncome1()
        {
            int weekTodayIndex = weekTodayIndex_WeekOfTheDayMonthBasis();

            String totalCostString;
            int totalCostInt = 0;
            int totalWeeklySales = 0;

            connection.Open();
            SqlCommand countTotalWeeklySalesCMD = new SqlCommand();
            countTotalWeeklySalesCMD.Connection = connection;
            countTotalWeeklySalesCMD.CommandText = "SELECT * FROM date INNER JOIN sales_history ON date.date_id = sales_history.date_id WHERE date.week_ordered_dayOfTheWeek_basis = '" + weekTodayIndex + "'";
            countTotalWeeklySalesCMD.ExecuteNonQuery();

            SqlDataReader countTotalWeeklySalesDR = countTotalWeeklySalesCMD.ExecuteReader();
            while (countTotalWeeklySalesDR.Read())
            {
                totalCostString = countTotalWeeklySalesDR.GetValue(12).ToString();
                totalCostInt = Convert.ToInt32(totalCostString);

                totalWeeklySales = totalWeeklySales + totalCostInt;
            }
            countTotalWeeklySalesDR.Close();
            connection.Close();

            income_total_main_weekly.Content = totalWeeklySales.ToString();
            weekly_income_total.Content = totalWeeklySales.ToString();
        }

        public void computeMonthlyIncome()
        {
            int monthNow = monthNowInt();

            String totalCostString;
            int totalCostInt = 0;
            int totalMonthlySales = 0;

            connection.Open();
            SqlCommand countTotalMonthlySalesCMD = new SqlCommand();
            countTotalMonthlySalesCMD.Connection = connection;
            countTotalMonthlySalesCMD.CommandText = "SELECT * FROM date INNER JOIN sales_history ON date.date_id = sales_history.date_id WHERE date.month_ordered = '" + monthNow + "'";
            countTotalMonthlySalesCMD.ExecuteNonQuery();

            SqlDataReader countTotalMonthSalesDR = countTotalMonthlySalesCMD.ExecuteReader();
            while (countTotalMonthSalesDR.Read())
            {
                totalCostString = countTotalMonthSalesDR.GetValue(12).ToString();
                totalCostInt = Convert.ToInt32(totalCostString);

                totalMonthlySales = totalMonthlySales + totalCostInt;
            }
            countTotalMonthSalesDR.Close();
            connection.Close();

            income_total_main_monthly.Content = totalMonthlySales.ToString();
            monthly_income_total.Content = totalMonthlySales.ToString();
        }

        public void computeYearlyIncome()
        {
            int yearNow = yearNowInt();

            String totalCostString;
            int totalCostInt = 0;
            int totalYearlySales = 0;

            connection.Open();
            SqlCommand countTotalYearlySalesCMD = new SqlCommand();
            countTotalYearlySalesCMD.Connection = connection;
            countTotalYearlySalesCMD.CommandText = "SELECT * FROM date INNER JOIN sales_history ON date.date_id = sales_history.date_id WHERE date.year_ordered = '" + yearNow + "'";
            countTotalYearlySalesCMD.ExecuteNonQuery();

            SqlDataReader countTotalYearlySalesDR = countTotalYearlySalesCMD.ExecuteReader();
            while (countTotalYearlySalesDR.Read())
            {
                totalCostString = countTotalYearlySalesDR.GetValue(12).ToString();
                totalCostInt = Convert.ToInt32(totalCostString);

                totalYearlySales = totalYearlySales + totalCostInt;
            }
            countTotalYearlySalesDR.Close();
            connection.Close();

            income_total_main_yearly.Content = totalYearlySales.ToString();
            yearly_income_total.Content = totalYearlySales.ToString();

            
        }

        private int monthNowInt()
        {
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

            return monthNowInt;
        }

        private int yearNowInt()
        {
            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;

            String dateFormatted = date.ToString("yyyy-MM-dd");

            //GETTING WEEK INDEX ON THIS WEEK******************

            //getting Month Now
            int indexOfYearNow = dateFormatted.IndexOf('-');
            int indexOfYearPlusOneNow = indexOfYearNow;
            string yearNow = dateFormatted.Substring(0, indexOfYearPlusOneNow);
            int yearNowInt = Convert.ToInt32(yearNow);

            return yearNowInt;
        }

        private int weekTodayIndex_dayOfMonthBasis()
        {
            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;

            String dateFormatted = date.ToString("yyyy-MM-dd");

            //GETTING WEEK INDEX ON THIS WEEK******************

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

            //getting days in month
            int lastDay = DateTime.DaysInMonth(yearNowInt, monthNowInt);

            //week 1
            int firstWeekStartingDayToMinus = (dayNowInt - 1) * -1;
            DateTime firstWeekStartingDay = date.AddDays(firstWeekStartingDayToMinus);
            DateTime endDayOfTheFirstOfWeek = firstWeekStartingDay.AddDays(7);

            //week 2
            DateTime week2StartingDay = endDayOfTheFirstOfWeek.AddDays(1);
            DateTime week2EndDay = endDayOfTheFirstOfWeek.AddDays(7);

            //week 3
            DateTime week3StartingDay = week2EndDay.AddDays(1);
            DateTime week3EndDay = week2EndDay.AddDays(7);

            //week 4
            DateTime week4StartingDay = week3EndDay.AddDays(1);
            DateTime week4EndDay = week3EndDay.AddDays(7);

            //week 5
            DateTime week5StartingDay = week4EndDay.AddDays(1);
            DateTime week5EndDay = week4EndDay.AddDays(7);

            int weekToday = 0;

            //gathering the weeks of month today
            if (date >= firstWeekStartingDay && date <= endDayOfTheFirstOfWeek)
            {
                weekToday = 1;
            }

            else if (date >= week2StartingDay && date <= week2EndDay)
            {
                weekToday = 2;
            }

            else if (date >= week3StartingDay && date <= week3EndDay)
            {
                weekToday = 3;
            }

            else if (date >= week4StartingDay && date <= week4EndDay)
            {
                weekToday = 4;
            }

            else if (date >= week5StartingDay && date <= week5EndDay)
            {
                weekToday = 5;
            }

            return weekToday;
        }


        //CurrentMonthWeeks
        private int weekTodayIndex_WeekOfTheDayMonthBasis()
        {
            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;

            String dateFormatted = date.ToString("yyyy-MM-dd");

            //getting Month Now
            int indexOfYearNow = dateFormatted.IndexOf('-');
            int indexOfYearPlusOneNow = indexOfYearNow;
            string yearNow = dateFormatted.Substring(0, indexOfYearPlusOneNow);
            int yearNowInt = Convert.ToInt32(yearNow);

            string dateWithoutYearNow = dateFormatted.Substring(yearNow.Length + 1);
            int indexOfMonthNow = dateWithoutYearNow.IndexOf('-');
            string monthNow = dateWithoutYearNow.Substring(0, indexOfMonthNow);
            int monthNowInt = Convert.ToInt32(monthNow);

            string dateWithoutMonthNow = dateWithoutYearNow.Substring(monthNow.Length + 1);
            int dayNowInt = Convert.ToInt32(dateWithoutMonthNow);

            int firstWeekStartingDayToMinus = (dayNowInt - 1) * -1;
            DateTime firstWeekStartingDay = date.AddDays(firstWeekStartingDayToMinus);
            String firstWeekStartingDayString = firstWeekStartingDay.ToString("yyyy-MM-dd");

            DayOfWeek firstWeekStartingDay_dayOfTheWeek = firstWeekStartingDay.DayOfWeek;

            int dayOfTheWeekIndex = 0;

            //week of days index positions
            if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Monday") { dayOfTheWeekIndex = 1; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Tuesday") { dayOfTheWeekIndex = 2; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Wednesday") { dayOfTheWeekIndex = 3; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Thursday") { dayOfTheWeekIndex = 4; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Friday") { dayOfTheWeekIndex = 5; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Saturday") { dayOfTheWeekIndex = 6; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Sunday") { dayOfTheWeekIndex = 7; }
            //*********

            //this is for getting the starting day of day 1 of month
            int startingDayOfWeekInt = (dayOfTheWeekIndex - 1) * -1;
            DateTime startingDayOfTheFirstOfWeek = firstWeekStartingDay.AddDays(startingDayOfWeekInt);
            String startingDayOfTheFirstOfWeekString = startingDayOfTheFirstOfWeek.ToString("yyyy-MM-dd");

            //end day of day 1 
            int endDayOfWeekInt = 7 - dayOfTheWeekIndex;
            DateTime endDayOfTheFirstOfWeek = firstWeekStartingDay.AddDays(endDayOfWeekInt);
            String endDayOfTheFirstOfWeekString = endDayOfTheFirstOfWeek.ToString("yyyy-MM-dd");

            //getting days in month
            int lastDay = DateTime.DaysInMonth(yearNowInt, monthNowInt);

            //week 2
            DateTime week2StartingDay = endDayOfTheFirstOfWeek.AddDays(1);
            DateTime week2EndDay = endDayOfTheFirstOfWeek.AddDays(7);

            //week 3
            DateTime week3StartingDay = week2EndDay.AddDays(1);
            DateTime week3EndDay = week2EndDay.AddDays(7);

            //week 4
            DateTime week4StartingDay = week3EndDay.AddDays(1);
            DateTime week4EndDay = week3EndDay.AddDays(7);

            //week 5
            DateTime week5StartingDay = week4EndDay.AddDays(1);
            DateTime week5EndDay = week4EndDay.AddDays(7);

            int weekToday = 0;

            //gathering the weeks of month today
            if (date >= startingDayOfTheFirstOfWeek && date <= endDayOfTheFirstOfWeek)
            {
                weekToday = 1;
            }


            else if(date >= week2StartingDay && date <= week2EndDay)
            {
                weekToday = 2;
            }

            else if (date >= week3StartingDay && date <= week3EndDay)
            {
                weekToday = 3;
            }

            else if (date >= week4StartingDay && date <= week4EndDay)
            {
                weekToday = 4;
            }

            else if (date >= week5StartingDay && date <= week5EndDay)
            {
                weekToday = 5;
            }

            // get the week of ordered similar to the weekToday and add it for the weekly sales

            return weekToday;
        }


        //CurrentMonthWeeks
        private void currentMonthWeeks()
        {
            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;

            String dateFormatted = date.ToString("yyyy-MM-dd");

            //getting Month Now
            int indexOfYearNow = dateFormatted.IndexOf('-');
            int indexOfYearPlusOneNow = indexOfYearNow;
            string yearNow = dateFormatted.Substring(0, indexOfYearPlusOneNow);
            int yearNowInt = Convert.ToInt32(yearNow);

            string dateWithoutYearNow = dateFormatted.Substring(yearNow.Length + 1);
            int indexOfMonthNow = dateWithoutYearNow.IndexOf('-');
            string monthNow = dateWithoutYearNow.Substring(0, indexOfMonthNow);
            int monthNowInt = Convert.ToInt32(monthNow);

            string dateWithoutMonthNow = dateWithoutYearNow.Substring(monthNow.Length + 1);
            int dayNowInt = Convert.ToInt32(dateWithoutMonthNow);

            int firstWeekStartingDayToMinus = (dayNowInt - 1) * -1;
            DateTime firstWeekStartingDay = date.AddDays(firstWeekStartingDayToMinus);
            String firstWeekStartingDayString = firstWeekStartingDay.ToString("yyyy-MM-dd");

            DayOfWeek firstWeekStartingDay_dayOfTheWeek = firstWeekStartingDay.DayOfWeek;

            int dayOfTheWeekIndex = 0;

            //week of days index positions
            if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Monday") { dayOfTheWeekIndex = 1; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Tuesday") { dayOfTheWeekIndex = 2; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Wednesday") { dayOfTheWeekIndex = 3; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Thursday") { dayOfTheWeekIndex = 4; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Friday") { dayOfTheWeekIndex = 5; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Saturday") { dayOfTheWeekIndex = 6; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Sunday") { dayOfTheWeekIndex = 7; }
            //*********

            //this is for getting the starting day of day 1 of month
            int startingDayOfWeekInt = (dayOfTheWeekIndex - 1) * -1;
            DateTime startingDayOfTheFirstOfWeek = firstWeekStartingDay.AddDays(startingDayOfWeekInt);
            String startingDayOfTheFirstOfWeekString = startingDayOfTheFirstOfWeek.ToString("yyyy-MM-dd");

            //end day of day 1 
            int endDayOfWeekInt = 7 - dayOfTheWeekIndex;
            DateTime endDayOfTheFirstOfWeek = firstWeekStartingDay.AddDays(endDayOfWeekInt);
            String endDayOfTheFirstOfWeekString = endDayOfTheFirstOfWeek.ToString("yyyy-MM-dd");

            //getting days in month
            int lastDay = DateTime.DaysInMonth(yearNowInt, monthNowInt);

            //week 2
            DateTime week2StartingDay = endDayOfTheFirstOfWeek.AddDays(1);
            DateTime week2EndDay = endDayOfTheFirstOfWeek.AddDays(7);

            //week 3
            DateTime week3StartingDay = week2EndDay.AddDays(1);
            DateTime week3EndDay = week2EndDay.AddDays(7);

            //week 4
            DateTime week4StartingDay = week3EndDay.AddDays(1);
            DateTime week4EndDay = week3EndDay.AddDays(7);

            //week 5
            DateTime week5StartingDay = week4EndDay.AddDays(1);
            DateTime week5EndDay = week4EndDay.AddDays(7);

            int weekToday = 0;

            //gathering the weeks of month today
            if (date >= startingDayOfTheFirstOfWeek && date <= endDayOfTheFirstOfWeek)
            {
                weekToday = 1;
            }

            else if(date >= week2StartingDay && date <= week2EndDay)
            {
                weekToday = 2;
            }

            else if (date >= week3StartingDay && date <= week3EndDay)
            {
                weekToday = 3;
            }

            else if (date >= week4StartingDay && date <= week4EndDay)
            {
                weekToday = 4;
            }

            else if (date >= week5StartingDay && date <= week5EndDay)
            {
                weekToday = 5;
            }

            // get the week of ordered similar to the weekToday and add it for the weekly sales


        }



        //Product data grid
        public void fill_item_data_grid()
        {
            updateItemTotalCost();
            updateTotalItemCount();

            computeDailyIncome();
            computeWeeklyIncome1();
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

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                    SqlCommand fillProductTableCMD = new SqlCommand();
                    fillProductTableCMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE item_stock > 0";
                    fillProductTableCMD.Connection = connection;
                    SqlDataAdapter fillProductTableDA = new SqlDataAdapter(fillProductTableCMD);
                    DataTable fillProductTableDT = new DataTable("items");
                    fillProductTableDA.Fill(fillProductTableDT);

                    item_data_grid.ItemsSource = fillProductTableDT.DefaultView;

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
            getItemInformationsCMD.CommandText = "SELECT * FROM item INNER JOIN available_items ON item.item_serial_number = available_items.item_serial_number WHERE item.item_serial_number = '" + itemIDInt + "'";            
            getItemInformationsCMD.ExecuteNonQuery();

            String itemSerialNumberInformation;
            String itemNameInformation;
            String itemModelInformation;
            String itemDescriptionInformation;
            String itemPriceInformation;
            String itemStockInformation;
            String itemWarrantyInformation;
            String itemWarrantyServiceInformation;

            SqlDataReader getItemInformationsDR = getItemInformationsCMD.ExecuteReader();
            while (getItemInformationsDR.Read())
            {
                itemSerialNumberInformation = getItemInformationsDR.GetValue(0).ToString();
                itemNameInformation = getItemInformationsDR.GetValue(1).ToString();
                itemModelInformation = getItemInformationsDR.GetValue(2).ToString();
                itemDescriptionInformation = getItemInformationsDR.GetValue(3).ToString();
                itemPriceInformation = getItemInformationsDR.GetValue(4).ToString();
                itemStockInformation = getItemInformationsDR.GetValue(12).ToString();
                itemWarrantyInformation = getItemInformationsDR.GetValue(6).ToString();
                itemWarrantyServiceInformation = getItemInformationsDR.GetValue(8).ToString();

                serial_number_textblock.Text = itemSerialNumberInformation;
                name_textblock.Text = itemNameInformation;
                model_textblock.Text = itemModelInformation;
                description_textblock.Text = itemDescriptionInformation;
                price_textblock.Text = itemPriceInformation;
                stock_textblock.Text = itemStockInformation;
                warranty_textblock.Text = itemWarrantyInformation;
                warranty_service_textblock.Text = itemWarrantyServiceInformation;
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
                String verifyItemIfStillExist = "SELECT COUNT(1) FROM item WHERE item_serial_number = '" + itemIDString + "'";
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
                    deleteItemCMD.CommandText = "DELETE FROM available_items WHERE item_serial_number = '" + itemIDString + "' ";
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
            searchItemCMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE item.item_name LIKE '%" + search_textbox.Text + "%' AND item_stock > 0";
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
