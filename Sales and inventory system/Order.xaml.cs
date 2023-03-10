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
using System.Data.SqlTypes;
using System.Runtime.InteropServices.ComTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using ControlzEx.Standard;
using Sales_and_Inventory_System;
using Sales_and_Inventory_System.Properties;
using System.Drawing;

namespace Sales_and_Inventory_System
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    /// 
    public partial class Order : Page
    {
        SqlConnection connection = ConnectionString.Connection;

        int foundSimilarCounter = 0;
        float totalCost = 0;

        string searchTextCopy;
        bool searchBtn = false;

        public struct customerItems
        {
            public int id { set; get; }
            public string item_name { set; get; }
            public float item_price { set; get; }
            public int item_quantity { set; get; }

        }
        public class MyDataSource
        {
            public bool IsButtonEnabled { get; set; }
        }

        public Order()
        {
            InitializeComponent();

            Home.instance.purchased = false;

            //set total cost to 0 since user did not order yet
            total_cost.Content = totalCost.ToString();

            //disable place order button since user did not order yet
            int customerItemCount = customer_items_grid.Items.Count;

            if (customerItemCount == 0)
            {
                place_order_btn.IsEnabled = false;
            }

            fill_item_data_grid();

            refresh_stock_copy();

        }

        private void refresh_stock_copy()
        {
            connection.Open();
            String countAllItems = "SELECT COUNT(*) FROM available_items WHERE item_stock > 0";
            SqlCommand sqlCmd = new SqlCommand(countAllItems, connection);
            sqlCmd.CommandType = CommandType.Text;
            int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
            connection.Close();

            int[] numberOfItemsID = new int[count];
            int[] numberOfItemsStock = new int[count];

            connection.Open();
            SqlCommand getItemStockCMD = new SqlCommand();
            getItemStockCMD.Connection = connection;
            getItemStockCMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE available_items.item_stock > 0";
            getItemStockCMD.ExecuteNonQuery();

            String itemID;
            int itemIDInt = 0;
            String itemStock;
            int itemStockInt = 0;

            int loopCounter = 0;

            SqlDataReader getItemStockDR = getItemStockCMD.ExecuteReader();
            while (getItemStockDR.Read())
            {

                itemID = getItemStockDR.GetValue(1).ToString();
                itemIDInt = Int32.Parse(itemID);
                itemStock = getItemStockDR.GetValue(2).ToString();
                itemStockInt = Int32.Parse(itemStock);

                numberOfItemsID[loopCounter] = itemIDInt;
                numberOfItemsStock[loopCounter] = itemStockInt;

                loopCounter++;

            }
            getItemStockDR.Close();
            connection.Close();

            for (int updateCounter = 0; updateCounter < count; updateCounter++)
            {
                connection.Open();
                SqlCommand updateItemStockCMD = new SqlCommand();
                updateItemStockCMD.Connection = connection;
                updateItemStockCMD.CommandText = "UPDATE available_items SET stock_copy = '" + numberOfItemsStock[updateCounter] + "' WHERE item_serial_number = '" + numberOfItemsID[updateCounter] + "'";
                SqlDataAdapter updateItemStockDA = new SqlDataAdapter(updateItemStockCMD);
                DataTable updateItemStockDT = new DataTable();
                updateItemStockDA.Fill(updateItemStockDT);
                connection.Close();
            }

            fill_item_data_grid();

        }

        private void place_order_btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(customer_name.Text))
            {
                //question before placing order
                MessageBoxResult result = MessageBox.Show("Are you sure you want to place your order without customer name?", "Empty Customer Name", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    int totalQuantity = 0;
                    //question before placing order
                    MessageBoxResult result1 = MessageBox.Show("Are you sure you want to place your order?", "Place order", MessageBoxButton.YesNo);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        process_order();
                    }

                }
            }

            else
            {

                //question before placing order
                MessageBoxResult result1 = MessageBox.Show("Are you sure you want to place your order?", "Place order", MessageBoxButton.YesNo);
                if (result1 == MessageBoxResult.Yes)
                {
                    process_order();
                }
            }
        }

        private String weekToday_DayOfTheWeekBasis()
        {
            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;
            DayOfWeek today = DateTime.Now.DayOfWeek;

            int dayOfTheWeekIndex = 0;

            //week of days index positions
            if (today.ToString() == "Monday") { dayOfTheWeekIndex = 1; }
            else if (today.ToString() == "Tuesday") { dayOfTheWeekIndex = 2; }
            else if (today.ToString() == "Wednesday") { dayOfTheWeekIndex = 3; }
            else if (today.ToString() == "Thursday") { dayOfTheWeekIndex = 4; }
            else if (today.ToString() == "Friday") { dayOfTheWeekIndex = 5; }
            else if (today.ToString() == "Saturday") { dayOfTheWeekIndex = 6; }
            else if (today.ToString() == "Sunday") { dayOfTheWeekIndex = 7; }
            //*********

            int endDayOfTheWeekInt = 7 - dayOfTheWeekIndex;
            DateTime endDayOfTheWeek = date.AddDays(endDayOfTheWeekInt);

            int startDayOfTheWeekInt = ((7 - endDayOfTheWeekInt) - 1) * -1;
            DateTime startDayOfTheWeek = date.AddDays(startDayOfTheWeekInt);

            String dateFormatted = date.ToString("yyyy-MM-dd");
            String startingDayOfTheWeekFormatted = startDayOfTheWeek.ToString("yyyy-MM-dd");
            String endDayOfTheWeekFormatted = endDayOfTheWeek.ToString("yyyy-MM-dd");

            //Getting dates
            //getting Year Starting
            int indexOfYearStarting = startingDayOfTheWeekFormatted.IndexOf('-');
            int indexOfYearPlusOneStarting = indexOfYearStarting;
            string yearStarting = startingDayOfTheWeekFormatted.Substring(0, indexOfYearPlusOneStarting);
            int yearStartingInt = Convert.ToInt32(yearStarting);
            string dateWithoutYearStarting = startingDayOfTheWeekFormatted.Replace(yearStarting, "");

            //getting Year End
            int indexOfYearEnd = endDayOfTheWeekFormatted.IndexOf('-');
            int indexOfYearPlusOneEnd = indexOfYearEnd;
            string yearEnd = endDayOfTheWeekFormatted.Substring(0, indexOfYearPlusOneEnd);
            int yearEndInt = Convert.ToInt32(yearEnd);
            string dateWithoutYearEnd = endDayOfTheWeekFormatted.Replace(yearEnd, "");

            //getting Month Starting
            string removeFirstCharacterStarting = dateWithoutYearStarting.Length > 1 ? dateWithoutYearStarting.Substring(1) : "";
            int indexOfMonthStarting = removeFirstCharacterStarting.IndexOf('-');
            string monthStarting = removeFirstCharacterStarting.Substring(0, indexOfMonthStarting);
            int monthStartingInt = Convert.ToInt32(monthStarting);
            string dateWithoutMonthStarting = removeFirstCharacterStarting.Replace(monthStarting, "");

            //getting Month End
            string removeFirstCharacterEnd = dateWithoutYearEnd.Length > 1 ? dateWithoutYearEnd.Substring(1) : "";
            int indexOfMonthEnd = removeFirstCharacterEnd.IndexOf('-');
            string monthEnd = removeFirstCharacterEnd.Substring(0, indexOfMonthEnd);
            int monthEndInt = Convert.ToInt32(monthEnd);
            string dateWithoutMonthEnd = removeFirstCharacterEnd.Replace(monthEnd, "");

            //getting day of the month Starting
            string removeFirstCharacterDayStarting = dateWithoutMonthStarting.Replace("-", "");
            int dayStartingInt = Convert.ToInt32(removeFirstCharacterDayStarting);

            //getting day of the month End
            string removeFirstCharacterDayEnd = dateWithoutMonthEnd.Replace("-", "");
            int dayEndInt = Convert.ToInt32(removeFirstCharacterDayEnd);

            //Starting date
            if (monthStarting == "01") { monthStarting = "January"; }

            else if (monthStarting == "02") { monthStarting = "February"; }

            else if (monthStarting == "03") { monthStarting = "March"; }

            else if (monthStarting == "04") { monthStarting = "April"; }

            else if (monthStarting == "05") { monthStarting = "May"; }

            else if (monthStarting == "06") { monthStarting = "June"; }

            else if (monthStarting == "07") { monthStarting = "July"; }

            else if (monthStarting == "08") { monthStarting = "August"; }

            else if (monthStarting == "09") { monthStarting = "September"; }

            else if (monthStarting == "10") { monthStarting = "October"; }

            else if (monthStarting == "11") { monthStarting = "November"; }

            else if (monthStarting == "12") { monthStarting = "December"; }

            //end date
            if (monthEnd == "01") { monthEnd = "January"; }

            else if (monthEnd == "02") { monthEnd = "February"; }

            else if (monthEnd == "03") { monthEnd = "March"; }

            else if (monthEnd == "04") { monthEnd = "April"; }

            else if (monthEnd == "05") { monthEnd = "May"; }

            else if (monthEnd == "06") { monthEnd = "June"; }

            else if (monthEnd == "07") { monthEnd = "July"; }

            else if (monthEnd == "08") { monthEnd = "August"; }

            else if (monthEnd == "09") { monthEnd = "September"; }

            else if (monthEnd == "10") { monthEnd = "October"; }

            else if (monthEnd == "11") { monthEnd = "November"; }

            else if (monthEnd == "12") { monthEnd = "December"; }


            // getting Month Now
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

            int dayOfTheWeekIndex1 = 0;

            //week of days index positions
            if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Monday") { dayOfTheWeekIndex1 = 1; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Tuesday") { dayOfTheWeekIndex1 = 2; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Wednesday") { dayOfTheWeekIndex1 = 3; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Thursday") { dayOfTheWeekIndex1 = 4; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Friday") { dayOfTheWeekIndex1 = 5; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Saturday") { dayOfTheWeekIndex1 = 6; }
            else if (firstWeekStartingDay_dayOfTheWeek.ToString() == "Sunday") { dayOfTheWeekIndex1 = 7; }
            //*********

            //this is for getting the starting day of day 1 of month
            int startingDayOfWeekInt = (dayOfTheWeekIndex1 - 1) * -1;
            DateTime startingDayOfTheFirstOfWeek = firstWeekStartingDay.AddDays(startingDayOfWeekInt);
            String startingDayOfTheFirstOfWeekString = startingDayOfTheFirstOfWeek.ToString("yyyy-MM-dd");

            //end day of day 1 
            int endDayOfWeekInt = 7 - dayOfTheWeekIndex1;
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


            String startingDayOfTheWeekString = monthStarting + " " + dayStartingInt + ", " + yearStarting;
            String endDayOfTheWeekString = monthEnd + " " + dayEndInt + ", " + yearEnd;
            String weekRangeOfWeekOfTheDay = startingDayOfTheWeekString + " to " + endDayOfTheWeekString;

            String finalReturn = weekRangeOfWeekOfTheDay + "%" + weekToday.ToString();

            return finalReturn;
        }

        private String weekToday_DayOfTheMonthBasis()
        {
            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;
            DayOfWeek today = DateTime.Now.DayOfWeek;

            String dateFormatted = date.ToString("yyyy-MM-dd");

            //Getting dates
            //getting Year Now
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

            int lastDayOfTheMonth = 0;
            int week_now = 1;
            int ordered_week = 1;

            int startOfWeekDay = 0;
            int endOfWeekDay = 0;
            String monthInWord = "";

            if (monthNow == "01") { lastDayOfTheMonth = 31; monthInWord = "January"; }

            else if (monthNow == "02") { lastDayOfTheMonth = 28; monthInWord = "February"; }

            else if (monthNow == "03") { lastDayOfTheMonth = 31; monthInWord = "March"; }

            else if (monthNow == "04") { lastDayOfTheMonth = 30; monthInWord = "April"; }

            else if (monthNow == "05") { lastDayOfTheMonth = 31; monthInWord = "May"; }

            else if (monthNow == "06") { lastDayOfTheMonth = 30; monthInWord = "June"; }

            else if (monthNow == "07") { lastDayOfTheMonth = 31; monthInWord = "July"; }

            else if (monthNow == "08") { lastDayOfTheMonth = 31; monthInWord = "August"; }

            else if (monthNow == "09") { lastDayOfTheMonth = 30; monthInWord = "September"; }

            else if (monthNow == "10") { lastDayOfTheMonth = 31; monthInWord = "October"; }

            else if (monthNow == "11") { lastDayOfTheMonth = 30; monthInWord = "November"; }

            else if (monthNow == "12") { lastDayOfTheMonth = 31; monthInWord = "December"; }

            if (lastDayOfTheMonth == 28)
            {
                if (dayNowInt <= 7)
                {
                    startOfWeekDay = 1;
                    endOfWeekDay = 7;
                    ordered_week = 1;
                }

                else if (dayNowInt <= 14)
                {
                    startOfWeekDay = 8;
                    endOfWeekDay = 14;
                    ordered_week = 2;
                }

                else if (dayNowInt <= 21)
                {
                    startOfWeekDay = 15;
                    endOfWeekDay = 21;
                    ordered_week = 3;
                }

                else
                {
                    startOfWeekDay = 22;
                    endOfWeekDay = 28;
                    ordered_week = 4;
                }
            }

            else if (lastDayOfTheMonth == 30 || lastDayOfTheMonth == 31)
            {
                if (dayNowInt <= 7)
                {
                    startOfWeekDay = 1;
                    endOfWeekDay = 7;
                    ordered_week = 1;
                }

                else if (dayNowInt <= 14)
                {
                    startOfWeekDay = 8;
                    endOfWeekDay = 14;
                    ordered_week = 2;
                }

                else if (dayNowInt <= 21)
                {
                    startOfWeekDay = 15;
                    endOfWeekDay = 21;
                    ordered_week = 3;
                }

                else if (dayNowInt <= 28)
                {
                    startOfWeekDay = 22;
                    endOfWeekDay = 28;
                    ordered_week = 4;
                }

                else
                {
                    startOfWeekDay = 29;

                    if (lastDayOfTheMonth == 30)
                    {
                        endOfWeekDay = 30;
                    }

                    else
                    {
                        endOfWeekDay = 31;
                    }

                    ordered_week = 5;
                }
            }

            String weekRange = monthInWord + " " + startOfWeekDay + ", " + yearNow + " to " + monthInWord + " " + endOfWeekDay + ", " + yearNow;

            String finalReturn = weekRange + "%" + ordered_week.ToString();

            return finalReturn;
        }

        private void process_order()
        {
            int countNumberOfItemsInCustomerGrid = customer_items_grid.Items.Count;

            int[] item_id = new int[countNumberOfItemsInCustomerGrid];


            //GETTING THE WEEKS VALUES OF DATE TODAY***********************

            String weekToday_dayMonthPartial = weekToday_DayOfTheMonthBasis();
            String weekToday_dayOfTheWeekMonthPartial = weekToday_DayOfTheWeekBasis();

            String weekToday_dayMonth;
            String weekToday_dayOfTheWeekMonth;
            int weekToday_dayMonth_weekIndex = 0;
            int weekToday_dayOfTheWeekMonth_weekIndex = 0;

            //getting weekToday_dayMonth
            int week_DayOfMonthBasisIndex = weekToday_dayMonthPartial.IndexOf('%');
            weekToday_dayMonth = weekToday_dayMonthPartial.Substring(0, week_DayOfMonthBasisIndex);

            //getting weekToday_dayMonth
            int week_weekOfDayBasisIndex = weekToday_dayOfTheWeekMonthPartial.IndexOf('%');
            weekToday_dayOfTheWeekMonth = weekToday_dayOfTheWeekMonthPartial.Substring(0, week_weekOfDayBasisIndex);


            //getting the week index of week today in day of month basis
            string remove_WeekToday_dayMonth_String = weekToday_dayMonthPartial.Replace(weekToday_dayMonth + "%", "");
            weekToday_dayMonth_weekIndex = int.Parse(remove_WeekToday_dayMonth_String);

            //getting the week index of week today in day of of the week month basis
            string remove_weekToday_dayOfTheWeekMonth_String = weekToday_dayOfTheWeekMonthPartial.Replace(weekToday_dayOfTheWeekMonth + "%", "");

            weekToday_dayOfTheWeekMonth_weekIndex = int.Parse(remove_weekToday_dayOfTheWeekMonth_String);

            weekToday_dayOfTheWeekMonth_weekIndex = int.Parse(remove_WeekToday_dayMonth_String);


            //GETTING THE WEEKS VALUES OF DATE TODAY****************************




            //GETTING  YEAR MONTH AND DAY TODAY INCLUDING DAY OF THE WEEK*******************
            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;
            DayOfWeek today = DateTime.Now.DayOfWeek;

            String dateFormatted = date.ToString("yyyy-MM-dd");

            //Getting dates
            //getting Year Now
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

            DayOfWeek dayOfTheWeekToday = date.DayOfWeek;

            //GETTING  YEAR MONTH AND DAY TODAY INCLUDING DAY OF THE WEEK *******************




            //INSERTING DATA IN DATE TABLE ********************
            int date_id = 0;
            connection.Open();
            SqlCommand insertDateCMD = new SqlCommand();
            insertDateCMD.Connection = connection;
            insertDateCMD.CommandText = "DECLARE @date_id_var INT;INSERT INTO ordered_date(date_ordered, year_ordered, month_ordered, week_ordered, week_ordered_dayOfTheWeek_basis, week_range_month, week_range_day_of_the_week, day_ordered, day_of_the_week_ordered) " +
                "VALUES('" + dateFormatted + "', '" + yearNowInt + "', '" + monthNowInt + "','" + weekToday_dayMonth_weekIndex + "', '" + weekToday_dayOfTheWeekMonth_weekIndex + "', '" + weekToday_dayMonth + "', '" + weekToday_dayOfTheWeekMonth + "', '" + dayNowInt + "', '" + dayOfTheWeekToday + "');" +
                "SET @date_id_var = SCOPE_IDENTITY(); SELECT @date_id_var AS date_id;";
            date_id = (int)insertDateCMD.ExecuteScalar();
            connection.Close();
            //INSERTING DATA IN DATE TABLE ********************





            //COUNTING THE TOTAL ITEM COST AND INSERTING IT INTO DATABASE ***************************************
            int totalQuantity = 0;
            float totalCost = 0;

            Home.instance.purchased = true;
            int order_total_id = 0;

            for (int customerItemIDCounter = 0; customerItemIDCounter < countNumberOfItemsInCustomerGrid; customerItemIDCounter++)
            {
                //getting the table item values
                TextBlock customerItemID = customer_items_grid.Columns[0].GetCellContent(customer_items_grid.Items[customerItemIDCounter]) as TextBlock;
                String customerItemIDString = customerItemID.Text;
                int customerItemIDInt = Convert.ToInt32(customerItemIDString);

                TextBlock customerItemQuantity = customer_items_grid.Columns[3].GetCellContent(customer_items_grid.Items[customerItemIDCounter]) as TextBlock;
                String customerItemQuantityString = customerItemQuantity.Text;
                int customerItemQuantityInt = Convert.ToInt32(customerItemQuantityString);

                TextBlock customerItemPrice = customer_items_grid.Columns[2].GetCellContent(customer_items_grid.Items[customerItemIDCounter]) as TextBlock;
                String customerItemPriceString = customerItemPrice.Text;
                int customerItemPriceInt = Convert.ToInt32(customerItemPriceString);
                //*********************


                item_id[customerItemIDCounter] = customerItemIDInt; // stroing item id into an array

                totalQuantity = totalQuantity + customerItemQuantityInt;
                float totalItemCostPerItem = customerItemQuantityInt * customerItemPriceInt;
                totalCost = totalCost + totalItemCostPerItem;


                //INSERTING INTO TABELS IN DATABSE ***********************



                if (customerItemIDCounter == 0)
                {
                    //sales insert data
                    connection.Open();
                    SqlCommand insertSalesHistoryCMD = new SqlCommand();
                    insertSalesHistoryCMD.Connection = connection;
                    insertSalesHistoryCMD.CommandText = "DECLARE @order_total_id_var INT;INSERT INTO customer_order_total(total_item_quantity, total_cost, date_id) " +
                        "VALUES('" + totalQuantity + "', '" + totalCost + "','" + date_id + "'); SET @order_total_id_var = SCOPE_IDENTITY(); SELECT @order_total_id_var AS order_total_id;";
                    order_total_id = (int)insertSalesHistoryCMD.ExecuteScalar();
                    connection.Close();
                    //****************
                }

                else
                {
                    //update salessssssssss
                    SqlCommand updateSalesCMD = new SqlCommand();
                    updateSalesCMD.Connection = connection;
                    updateSalesCMD.CommandText = "UPDATE customer_order_total SET total_item_quantity = '" + totalQuantity + "', total_cost = '" + totalCost + "' WHERE order_total_id = '" + order_total_id + "'";
                    SqlDataAdapter updateSalesDA = new SqlDataAdapter(updateSalesCMD);
                    DataTable updateSalesDT = new DataTable();
                    updateSalesDA.Fill(updateSalesDT);
                    connection.Close();
                    //****************
                }


                //insert order int table
                connection.Open();
                SqlCommand insertOrderCMD = new SqlCommand();
                insertOrderCMD.Connection = connection;
                insertOrderCMD.CommandText = "INSERT INTO customer_order(item_serial_number, ordered_quantity, total_cost_per_item, customer_name, order_total_id) VALUES('" + customerItemIDInt + "', '" + customerItemQuantityInt + "', '" + totalItemCostPerItem + "', '" + customer_name.Text + "', '" + order_total_id + "')";
                SqlDataAdapter insertItemDA = new SqlDataAdapter(insertOrderCMD);
                DataTable insertItemDT = new DataTable();
                insertItemDA.Fill(insertItemDT);
                connection.Close();
                //****************

                //INSERTING INTO TABELS IN DATABSE ***********************




                //UPDATING DATAGRID TABLE BY FETCHING DATA INTO DATABASE AFTER PURCHASED **************

                //getting stocks
                int numberOfItemsInCustomerGridItems = customer_items_grid.Items.Count;
                int[] itemID = new int[numberOfItemsInCustomerGridItems];
                int[] itemStock = new int[numberOfItemsInCustomerGridItems];

                connection.Open();
                SqlCommand getItemStockCopyCMD = new SqlCommand();
                getItemStockCopyCMD.Connection = connection;
                getItemStockCopyCMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE available_items.item_serial_number = '" + customerItemIDInt + "'";
                getItemStockCopyCMD.ExecuteNonQuery();

                String stock_copy;
                int stock_copyInt = 0;
                String itemID1;
                int itemIDInt1 = 0;

                int loopCounter1 = 0;

                SqlDataReader getItemStockCopyDR = getItemStockCopyCMD.ExecuteReader();
                while (getItemStockCopyDR.Read())
                {
                    itemID1 = getItemStockCopyDR.GetValue(1).ToString();
                    itemIDInt1 = Int32.Parse(itemID1);
                    stock_copy = getItemStockCopyDR.GetValue(3).ToString();
                    stock_copyInt = Int32.Parse(stock_copy);

                    itemID[loopCounter1] = itemIDInt1;
                    itemStock[loopCounter1] = stock_copyInt;

                    loopCounter1++;
                }
                getItemStockCopyDR.Close();
                connection.Close();

                //updating stocks
                for (int updateCounter = 0; updateCounter < numberOfItemsInCustomerGridItems; updateCounter++)
                {
                    connection.Open();
                    SqlCommand updateItemStockCMD = new SqlCommand();
                    updateItemStockCMD.Connection = connection;
                    updateItemStockCMD.CommandText = "UPDATE available_items SET item_stock = '" + itemStock[updateCounter] + "' WHERE item_serial_number = '" + itemID[updateCounter] + "'";
                    SqlDataAdapter updateItemStockDA = new SqlDataAdapter(updateItemStockCMD);
                    DataTable updateItemStockDT = new DataTable();
                    updateItemStockDA.Fill(updateItemStockDT);
                    connection.Close();
                }

                //UPDATING DATAGRID TABLE BY FETCHING DATA INTO DATABASE AFTER PURCHASED **************


            }
            //COUNTING THE TOTAL ITEM COST AND INSERTING IT INTO DATABASE ***************************************




            //UPDATING DATA GRID BY FILLING ITS TABLE *****
            fill_item_data_grid();
            //UPDATING DATA GRID BY FILLING ITS TABLE *****


            //DELETING ALL ITEMS IN TABLE IF STOCK IS 0 *****
            connection.Open();
            SqlCommand deleteEmptyStockCMD = new SqlCommand();
            deleteEmptyStockCMD.Connection = connection;
            deleteEmptyStockCMD.CommandText = "DELETE FROM available_items WHERE item_stock = 0";
            SqlDataAdapter deleteEmptyStockDA = new SqlDataAdapter(deleteEmptyStockCMD);
            DataTable deleteEmptyStockDT = new DataTable();
            deleteEmptyStockDA.Fill(deleteEmptyStockDT);
            connection.Close();
            //DELETING ALL ITEMS IN TABLE IF STOCK IS 0 *****


            MessageBox.Show("Ordered successfully, Thankyou " + customer_name.Text);
            total_cost.Content = "0";
            customer_name.Text = "";
            Home.instance.fill_item_data_grid();
            place_order_btn.IsEnabled = false;
            customer_items_grid.Items.Clear();
        }

        private void fill_item_data_grid()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    if (searchBtn == false)
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

                    else
                    {
                        connection.Open();

                        SqlCommand fillProductTableCMD = new SqlCommand();
                        fillProductTableCMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE item.item_name LIKE '%" + searchTextCopy + "%' AND item_stock > 0";
                        fillProductTableCMD.Connection = connection;
                        SqlDataAdapter fillProductTableDA = new SqlDataAdapter(fillProductTableCMD);
                        DataTable fillProductTableDT = new DataTable("item");
                        fillProductTableDA.Fill(fillProductTableDT);

                        product_data_grid.ItemsSource = fillProductTableDT.DefaultView;

                        connection.Close();
                    }

                }

                //if nothing found
                else
                {

                }
            }

            catch (Exception ex)
            {
                //for Connection error in data base
                MessageBox.Show("Connection error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            finally
            {
                connection.Close();
            }

        }

        private void search_btn_Click(object sender, RoutedEventArgs e)
        {
            searchBtn = true;
            searchTextCopy = search_input.Text;


            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                    SqlCommand refreshItemCMD = new SqlCommand();
                    refreshItemCMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE item.item_name LIKE '%" + search_input.Text + "%' AND item_stock > 0";
                    refreshItemCMD.Connection = connection;
                    SqlDataAdapter refreshItemDA = new SqlDataAdapter(refreshItemCMD);
                    DataTable refreshItemDT = new DataTable("item");
                    refreshItemDA.Fill(refreshItemDT);

                    product_data_grid.ItemsSource = refreshItemDT.DefaultView;

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
                MessageBox.Show("Connection error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            finally
            {
                connection.Close();
            }
        }
        //end****************


        //Selected item******
        private void select_btn_Click(object sender, RoutedEventArgs e)
        {
            place_order_btn.IsEnabled = true;

            //adding item to customer item grid
            var selectedIndex = product_data_grid.SelectedIndex;

            foundSimilarCounter = 0;

            TextBlock selectedItemIndex = product_data_grid.Columns[0].GetCellContent(product_data_grid.Items[selectedIndex]) as TextBlock;
            TextBlock selectedItemName = product_data_grid.Columns[1].GetCellContent(product_data_grid.Items[selectedIndex]) as TextBlock;
            TextBlock selectedItemPrice = product_data_grid.Columns[2].GetCellContent(product_data_grid.Items[selectedIndex]) as TextBlock;
            String selectedItemIndexString = selectedItemIndex.Text;
            String selectedItemPriceString = selectedItemPrice.Text;
            int selectedItemIndexInt = Convert.ToInt32(selectedItemIndexString);
            float selectedItemPriceFloat = Convert.ToInt32(selectedItemPriceString);

            int number = customer_items_grid.Items.Count;

            //if customer item is empty
            if (customer_items_grid.Items.IsEmpty)
            {
                //Reducing stock of item in database 
                TextBlock getItemID = product_data_grid.Columns[0].GetCellContent(product_data_grid.Items[product_data_grid.SelectedIndex]) as TextBlock;
                TextBlock getItemStock = product_data_grid.Columns[3].GetCellContent(product_data_grid.Items[product_data_grid.SelectedIndex]) as TextBlock;
                String getItemStockString = getItemStock.Text;
                String getItemIDString = getItemID.Text;
                int getItemStockInt = Convert.ToInt32(getItemStockString);
                int getItemIDInt = Convert.ToInt32(getItemIDString);

                getItemStockInt--;

                int numberOfGridItems = product_data_grid.Items.Count;
                int itemIDofSelectedItem = 0;

                for (int countGridItems = 0; countGridItems < numberOfGridItems; countGridItems++)
                {
                    TextBlock selectedItemID = product_data_grid.Columns[0].GetCellContent(product_data_grid.Items[countGridItems]) as TextBlock;
                    String selectedItemIDString = selectedItemID.Text;
                    int selectedItemIDInt = Convert.ToInt32(selectedItemIDString);

                    if (selectedItemIDInt == getItemIDInt)
                    {
                        itemIDofSelectedItem = countGridItems;
                    }
                }

                TextBlock selectedItemStock = product_data_grid.Columns[3].GetCellContent(product_data_grid.Items[itemIDofSelectedItem]) as TextBlock;
                String selectedItemStockString = selectedItemStock.Text;

                int selectedItemStockInt = Convert.ToInt32(selectedItemStockString);
                bool soldOut = false;

                customer_items_grid.Items.Add(new customerItems { id = selectedItemIndexInt, item_name = selectedItemName.Text, item_price = selectedItemPriceFloat, item_quantity = 1 });

                connection.Open();
                SqlCommand updateItemStockCMD = new SqlCommand();
                updateItemStockCMD.Connection = connection;
                updateItemStockCMD.CommandText = "UPDATE available_items SET stock_copy = '" + getItemStockInt + "' WHERE item_serial_number = '" + getItemIDInt + "'";
                SqlDataAdapter updateItemStockDA = new SqlDataAdapter(updateItemStockCMD);
                DataTable updateItemStockDT = new DataTable();
                updateItemStockDA.Fill(updateItemStockDT);
                connection.Close();

                fill_item_data_grid();

                if (getItemStockInt < 0)
                {
                    fill_item_data_grid();
                }

                else
                {
                    //get the price and add it in total
                    totalCost = totalCost + selectedItemPriceFloat;
                    total_cost.Content = totalCost.ToString();
                }

            }
            //************


            else
            {
                //Reducing stock of item in database 
                TextBlock getItemID = product_data_grid.Columns[0].GetCellContent(product_data_grid.Items[product_data_grid.SelectedIndex]) as TextBlock;
                TextBlock getItemStock = product_data_grid.Columns[3].GetCellContent(product_data_grid.Items[product_data_grid.SelectedIndex]) as TextBlock;
                String getItemStockString = getItemStock.Text;
                String getItemIDString = getItemID.Text;
                int getItemStockInt = Convert.ToInt32(getItemStockString);
                int getItemIDInt = Convert.ToInt32(getItemIDString);

                getItemStockInt--;

                if (getItemStockInt < 0)
                {
                    fill_item_data_grid();
                }

                else
                {
                    //get the price and add it in total
                    totalCost = totalCost + selectedItemPriceFloat;
                    total_cost.Content = totalCost.ToString();

                    //checking if what item is already been added and simply add it to item quantity
                    for (int columnCounter = 0; columnCounter < number; columnCounter++)
                    {
                        TextBlock customerGridItemIndex = customer_items_grid.Columns[0].GetCellContent(customer_items_grid.Items[columnCounter]) as TextBlock;
                        String customerGridItemIndexString = customerGridItemIndex.Text;
                        int selectedCustomerItemIndexInt = Convert.ToInt32(customerGridItemIndexString);

                        if (selectedItemIndexInt == selectedCustomerItemIndexInt)
                        {
                            foundSimilarCounter++;

                            TextBlock getQuantityValueOfSimilarItem = customer_items_grid.Columns[3].GetCellContent(customer_items_grid.Items[columnCounter]) as TextBlock;
                            String getQuantityValueOfSimilarItemString = getQuantityValueOfSimilarItem.Text;
                            int getQuantityValueOfSimilarItemInt = Convert.ToInt32(getQuantityValueOfSimilarItemString);
                            getQuantityValueOfSimilarItemInt++;

                            customer_items_grid.SelectedIndex = columnCounter;
                            customer_items_grid.Items.RemoveAt(customer_items_grid.SelectedIndex);
                            customer_items_grid.Items.Insert(columnCounter, new customerItems { id = selectedItemIndexInt, item_name = selectedItemName.Text, item_price = selectedItemPriceFloat, item_quantity = getQuantityValueOfSimilarItemInt });
                        }
                    }
                    //*****************

                    //if there is no similar item already added to customer item grid
                    if (foundSimilarCounter == 0)
                    {
                        customer_items_grid.Items.Add(new customerItems { id = selectedItemIndexInt, item_name = selectedItemName.Text, item_price = selectedItemPriceFloat, item_quantity = 1 });
                    }
                    //

                    customer_items_grid.Items.Refresh();

                    //Reeducing item stock in database
                    TextBlock getItemID1 = product_data_grid.Columns[0].GetCellContent(product_data_grid.Items[product_data_grid.SelectedIndex]) as TextBlock;
                    TextBlock getItemStock1 = product_data_grid.Columns[3].GetCellContent(product_data_grid.Items[product_data_grid.SelectedIndex]) as TextBlock;
                    String getItemStockString1 = getItemStock1.Text;
                    String getItemIDString1 = getItemID1.Text;
                    int getItemStockInt1 = Convert.ToInt32(getItemStockString1);
                    int getItemIDInt1 = Convert.ToInt32(getItemIDString1);

                    getItemStockInt1--;

                    connection.Open();
                    SqlCommand updateItemStockCMD1 = new SqlCommand();
                    updateItemStockCMD1.Connection = connection;
                    updateItemStockCMD1.CommandText = "UPDATE available_items SET stock_copy = '" + getItemStockInt1 + "' WHERE item_serial_number = '" + getItemIDInt1 + "'";
                    SqlDataAdapter updateItemStockDA1 = new SqlDataAdapter(updateItemStockCMD1);
                    DataTable updateItemStockDT1 = new DataTable();
                    updateItemStockDA1.Fill(updateItemStockDT1);
                    connection.Close();

                    fill_item_data_grid();
                    //************
                }

            }

            customer_items_grid.Items.Refresh();
        }
        //end****************



        //Selected item******
        private void remove_btn_Click(object sender, RoutedEventArgs e)
        {

            //getting the item id of item selected in customer id
            TextBlock getSelectedCustomerItemID = customer_items_grid.Columns[0].GetCellContent(customer_items_grid.Items[customer_items_grid.SelectedIndex]) as TextBlock;
            TextBlock getSelectedCustomerItemPrice = customer_items_grid.Columns[2].GetCellContent(customer_items_grid.Items[customer_items_grid.SelectedIndex]) as TextBlock;
            String getSelectedCustomerItemIDString = getSelectedCustomerItemID.Text;
            String getSelectedCustomerItemPriceString = getSelectedCustomerItemPrice.Text;
            int getSelectedCustomerItemIDInt = Convert.ToInt32(getSelectedCustomerItemIDString);
            float getSelectedCustomerItemPriceInt = Convert.ToInt32(getSelectedCustomerItemPriceString);

            //get the item price and minus it since the user reduce or remove the quantity
            totalCost = totalCost - getSelectedCustomerItemPriceInt;
            total_cost.Content = totalCost.ToString();

            int itemsGridCount = product_data_grid.Items.Count;
            int matchedItemID = 0;

            //checking what item id is being selected in customer item grid
            for (int countDataGridItems = 0; countDataGridItems < itemsGridCount; countDataGridItems++)
            {
                TextBlock getItemID = product_data_grid.Columns[0].GetCellContent(product_data_grid.Items[countDataGridItems]) as TextBlock;
                String getItemIDString = getItemID.Text;
                int getItemIDInt = Convert.ToInt32(getItemIDString);

                if (getItemIDInt == getSelectedCustomerItemIDInt)
                {
                    matchedItemID = countDataGridItems;
                    break;
                }
            }
            //******************

            //getting item id of item being selected in customer and grid and getting the total stock of item in
            //product grid and simply add +1 to its stock every time user reduce the amoutn of item being purchased
            TextBlock getCustomerItemID = customer_items_grid.Columns[0].GetCellContent(customer_items_grid.Items[customer_items_grid.SelectedIndex]) as TextBlock;
            String getCustomerItemIDString = getCustomerItemID.Text;
            int getCustomerItemIDInt = Convert.ToInt32(getCustomerItemIDString);

            int countNumberOfItems = product_data_grid.Items.Count;
            int counterFoundSimilar = 0;

            for (int counterItems = 0; counterItems < countNumberOfItems; counterItems++)
            {
                TextBlock getItemID = product_data_grid.Columns[0].GetCellContent(product_data_grid.Items[counterItems]) as TextBlock;
                String getItemIDString = getItemID.Text;
                int getItemIDInt = Convert.ToInt32(getItemIDString);

                if (getItemIDInt == getCustomerItemIDInt)
                {
                    counterFoundSimilar++;
                }
            }

            if (counterFoundSimilar > 0)
            {
                TextBlock getItemStock = product_data_grid.Columns[3].GetCellContent(product_data_grid.Items[matchedItemID]) as TextBlock;
                String getItemStockString = getItemStock.Text;
                int getItemStockInt = Convert.ToInt32(getItemStockString);
                getItemStockInt++;

                //updating stock amount in database

                connection.Open();
                SqlCommand updateItemStockCMD = new SqlCommand();
                updateItemStockCMD.Connection = connection;
                updateItemStockCMD.CommandText = "UPDATE available_items SET stock_copy = '" + getItemStockInt + "' WHERE item_serial_number = '" + getCustomerItemIDInt + "'";
                SqlDataAdapter updateItemStockDA = new SqlDataAdapter(updateItemStockCMD);
                DataTable updateItemStockDT = new DataTable();
                updateItemStockDA.Fill(updateItemStockDT);
                connection.Close();

                fill_item_data_grid();
                //**********
            }

            else
            { //updating stock amount in database
                connection.Open();
                SqlCommand updateItemStockCMD = new SqlCommand();
                updateItemStockCMD.Connection = connection;
                updateItemStockCMD.CommandText = "UPDATE available_items SET stock_copy = 1 WHERE item_serial_number = '" + getCustomerItemIDInt + "'";
                SqlDataAdapter updateItemStockDA = new SqlDataAdapter(updateItemStockCMD);
                DataTable updateItemStockDT = new DataTable();
                updateItemStockDA.Fill(updateItemStockDT);
                connection.Close();

                fill_item_data_grid();
            }

            //reducing the quantity of item being selected every time reduce button is selected
            var selectedIndexCustomerItem = customer_items_grid.SelectedIndex;
            TextBlock selectedCustomerItemIndex = customer_items_grid.Columns[0].GetCellContent(customer_items_grid.Items[selectedIndexCustomerItem]) as TextBlock;
            TextBlock selectedCustomerItemName = customer_items_grid.Columns[1].GetCellContent(customer_items_grid.Items[selectedIndexCustomerItem]) as TextBlock;
            TextBlock selectedCustomerItemPrice = customer_items_grid.Columns[2].GetCellContent(customer_items_grid.Items[selectedIndexCustomerItem]) as TextBlock;
            TextBlock selectedCustomerItemQuantityIndex = customer_items_grid.Columns[3].GetCellContent(customer_items_grid.Items[selectedIndexCustomerItem]) as TextBlock;
            String getQuantityValueOfCustomerItem = selectedCustomerItemQuantityIndex.Text;
            String selectedCustomerItemIndexString = selectedCustomerItemIndex.Text;
            int selectedCustomerItemIndexInt = Convert.ToInt32(selectedCustomerItemIndexString);
            String selectedCustomerItemNameString = selectedCustomerItemName.Text;
            String selectedCustomerItemPriceString = selectedCustomerItemPrice.Text;
            float selectedCustomerItemPriceFloat = Convert.ToInt32(selectedCustomerItemPriceString);
            int getQuantityValueOfCustomerItemInt = Convert.ToInt32(getQuantityValueOfCustomerItem);
            getQuantityValueOfCustomerItemInt--;

            if (getQuantityValueOfCustomerItemInt == 0)
            {
                customer_items_grid.Items.RemoveAt(customer_items_grid.SelectedIndex);
            }

            else
            {
                customer_items_grid.Items.Insert(customer_items_grid.SelectedIndex, new customerItems { id = selectedCustomerItemIndexInt, item_name = selectedCustomerItemNameString, item_price = selectedCustomerItemPriceFloat, item_quantity = getQuantityValueOfCustomerItemInt });
                customer_items_grid.Items.RemoveAt(customer_items_grid.SelectedIndex);
            }

            customer_items_grid.Items.Refresh();
            //****************

            //disable place order button since user did not order yet
            int customerItemCount = customer_items_grid.Items.Count;

            if (customerItemCount == 0)
            {
                place_order_btn.IsEnabled = false;
            }

        }
    }
}

