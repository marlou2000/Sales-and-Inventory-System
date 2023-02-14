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

namespace Sales_and_Inventory_System
{
    /// <summary>
    /// Interaction logic for order.xaml
    /// </summary>
    public partial class Order : Window
    {
        SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");

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

            //Hiding the first columns of every grid
            product_data_grid.Columns[0].MaxWidth = 0;
            customer_items_grid.Columns[0].MaxWidth = 0;
            //end**********

            //set total cost to 0 since user did not order yet
            total_cost.Content = totalCost.ToString();

            //disable place order button since user did not order yet
            int customerItemCount = customer_items_grid.Items.Count;
            
            if(customerItemCount == 0)
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
            getItemStockCMD.CommandText = "SELECT * FROM available_items INNER JOIN items ON available_items.item_id = items.item_id WHERE available_items.item_stock > 0";
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

            for(int updateCounter = 0; updateCounter < count; updateCounter++)
            {
                connection.Open();
                SqlCommand updateItemStockCMD = new SqlCommand();
                updateItemStockCMD.Connection = connection;
                updateItemStockCMD.CommandText = "UPDATE available_items SET stock_copy = '" + numberOfItemsStock[updateCounter] + "' WHERE item_id = '" + numberOfItemsID[updateCounter] + "'";
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

        private void process_order()
        {
            int countNumberOfItemsInCustomerGrid = customer_items_grid.Items.Count;

            int[] item_id = new int[countNumberOfItemsInCustomerGrid];

            DateTime dateTime = DateTime.Now;
            DateTime date = dateTime.Date;
            DayOfWeek today = DateTime.Now.DayOfWeek;

            int dayOfTheWeekIndex = 0;

            //week of days index positions
            if(today.ToString() == "Monday"){dayOfTheWeekIndex = 1;}
            else if (today.ToString() == "Tuesday") { dayOfTheWeekIndex = 2; }
            else if (today.ToString() == "Wednesday") { dayOfTheWeekIndex = 3; }
            else if (today.ToString() == "Thursday") { dayOfTheWeekIndex = 4; }
            else if (today.ToString() == "Friday") { dayOfTheWeekIndex = 5; }
            else if (today.ToString() == "Saturday") { dayOfTheWeekIndex = 6; }
            else if (today.ToString() == "Sunday") { dayOfTheWeekIndex = 7; }
            //*********

            int startingDayOfTheWeekInt = dayOfTheWeekIndex - 7;
            DateTime startingDayOfTheWeek = date.AddDays(startingDayOfTheWeekInt);

            int toAddendDayOfTheWeekInt = startingDayOfTheWeekInt * -1;

            int endDayOfTheWeekInt = (7 - toAddendDayOfTheWeekInt) - 1;
            DateTime endDayOfTheWeek = date.AddDays(endDayOfTheWeekInt);

            String dateFormatted = date.ToString("yyyy-MM-dd");
            String startingDayOfTheWeekFormatted = startingDayOfTheWeek.ToString("yyyy-MM-dd");
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


            String startingDayOfTheWeekString = monthStarting + " " + dayStartingInt + ", " + yearStarting;
            String endDayOfTheWeekString = monthEnd + " " + dayEndInt + ", " + yearEnd;

            String weekRangeOfWeekOfTheDay = startingDayOfTheWeekString + " to " + endDayOfTheWeekString;

            int totalQuantity = 0;

            Home.instance.purchased = true;

            connection.Open();

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


                String totalCostString = total_cost.Content.ToString();
                float totalCostFloat = Convert.ToInt32(totalCostString);


                //Getting dates
                //getting Year Now
                int indexOfYearNow = dateFormatted.IndexOf('-');
                int indexOfYearPlusOneNow = indexOfYearNow;
                string yearNow = dateFormatted.Substring(0, indexOfYearPlusOneNow);
                int yearNowInt = Convert.ToInt32(yearNow);
                string dateWithoutYearNow = dateFormatted.Replace(yearNow, "");

                //getting Month Now
                string removeFirstCharacter = dateWithoutYearNow.Length > 1 ? dateWithoutYearNow.Substring(1) : "";
                int indexOfMonthNow = removeFirstCharacter.IndexOf('-');
                string monthNow = removeFirstCharacter.Substring(0, indexOfMonthNow);
                int monthNowInt = Convert.ToInt32(monthNow);
                string dateWithoutMonthNow = removeFirstCharacter.Replace(monthNow, "");

                //getting day of the month now
                string removeFirstCharacterDay = dateWithoutMonthNow.Replace("-", "");
                int dayNowInt = Convert.ToInt32(removeFirstCharacterDay);

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
                //*********************

                //insert date into date tables

                int date_id = 0;
                SqlCommand insertDateCMD = new SqlCommand();
                insertDateCMD.Connection = connection;
                insertDateCMD.CommandText = "DECLARE @date_id_var INT;INSERT INTO date(date_ordered, year_ordered, month_ordered, week_ordered, week_range_month, week_range_day_of_the_week, day_ordered, day_of_the_week_ordered) " +
                    "VALUES('" + dateFormatted + "', '" + yearNowInt + "', '" + monthNowInt + "','" + ordered_week + "', '" + weekRange + "', '" + weekRangeOfWeekOfTheDay + "', '" + dayNowInt + "', '" + today + "');" +
                    "SET @date_id_var = SCOPE_IDENTITY();SELECT @date_id_var AS date_id;";
                date_id = (int)insertDateCMD.ExecuteScalar();
                connection.Close();
                //**************************


                //insert sales into table
                item_id[customerItemIDCounter] = customerItemIDInt;
                totalQuantity = totalQuantity + customerItemQuantityInt;
                float totalItemCostPerItem = customerItemQuantityInt * customerItemPriceInt;

                connection.Open();
                int sales_id = 0;
                SqlCommand insertSalesHistoryCMD = new SqlCommand();
                insertSalesHistoryCMD.Connection = connection;
                insertSalesHistoryCMD.CommandText = "DECLARE @sales_id_var INT;INSERT INTO sales_history(total_item_quantity, total_cost, date_ordered) " +
                    "VALUES('" + totalQuantity + "', '" + totalCostFloat + "','" + date_id + "'); SET @sales_id_var = SCOPE_IDENTITY(); SELECT @sales_id_var AS sales_id;";
                sales_id = (int)insertSalesHistoryCMD.ExecuteScalar();
                connection.Close();
                //****************


                //insert order int table
                connection.Open();
                SqlCommand insertOrderCMD = new SqlCommand();
                insertOrderCMD.Connection = connection;
                insertOrderCMD.CommandText = "INSERT INTO orders_history(item_id, sales_id, date_ordered, ordered_quantity, total_cost, customer_name) VALUES('" + customerItemIDInt + "', '" + sales_id + "', '" + date_id + "', '" + customerItemQuantityInt + "', '" + totalItemCostPerItem + "', '" + customer_name.Text + "')";
                SqlDataAdapter insertOrderDA = new SqlDataAdapter(insertOrderCMD);
                DataTable insertOrderDT = new DataTable();
                insertOrderDA.Fill(insertOrderDT);
                connection.Close();
                //****************


                //getting stocks
                int numberOfItemsInCustomerGridItems = customer_items_grid.Items.Count;
                int[] itemID = new int[numberOfItemsInCustomerGridItems];
                int[] itemStock = new int[numberOfItemsInCustomerGridItems];

                connection.Open();
                SqlCommand getItemStockCopyCMD = new SqlCommand();
                getItemStockCopyCMD.Connection = connection;
                getItemStockCopyCMD.CommandText = "SELECT * FROM available_items INNER JOIN items ON available_items.item_id = items.item_id WHERE available_items.item_id = '" + customerItemIDInt + "'";
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
                    updateItemStockCMD.CommandText = "UPDATE available_items SET item_stock = '" + itemStock[updateCounter] + "' WHERE item_id = '" + itemID[updateCounter] + "'";
                    SqlDataAdapter updateItemStockDA = new SqlDataAdapter(updateItemStockCMD);
                    DataTable updateItemStockDT = new DataTable();
                    updateItemStockDA.Fill(updateItemStockDT);
                    connection.Close();
                }

                fill_item_data_grid();

            }



            connection.Open();
            SqlCommand deleteEmptyStockCMD = new SqlCommand();
            deleteEmptyStockCMD.Connection = connection;
            deleteEmptyStockCMD.CommandText = "DELETE FROM available_items WHERE item_stock = 0";
            SqlDataAdapter deleteEmptyStockDA = new SqlDataAdapter(deleteEmptyStockCMD);
            DataTable deleteEmptyStockDT = new DataTable();
            deleteEmptyStockDA.Fill(deleteEmptyStockDT);
            connection.Close();

            MessageBox.Show("Ordered successfully, Thankyou " + customer_name.Text);
            total_cost.Content = "0";
            customer_name.Text = "";
            Home.instance.fill_item_data_grid();
            place_order_btn.IsEnabled = false;
            customer_items_grid.Items.Clear();
        }

        private void fill_item_data_grid()
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    if(searchBtn == false)
                    {
                        sqlCon.Open();

                        SqlCommand fillProductTableCMD = new SqlCommand();
                        fillProductTableCMD.CommandText = "SELECT * FROM available_items INNER JOIN items ON available_items.item_id = items.item_id WHERE available_items.stock_copy > 0";
                        fillProductTableCMD.Connection = sqlCon;
                        SqlDataAdapter fillProductTableDA = new SqlDataAdapter(fillProductTableCMD);
                        DataTable fillProductTableDT = new DataTable("items");
                        fillProductTableDA.Fill(fillProductTableDT);

                        product_data_grid.ItemsSource = fillProductTableDT.DefaultView;

                        sqlCon.Close();
                    }

                    else
                    {
                        sqlCon.Open();

                        SqlCommand fillProductTableCMD = new SqlCommand();
                        fillProductTableCMD.CommandText = "SELECT * FROM available_items INNER JOIN items ON available_items.item_id = items.item_id WHERE items.item_name LIKE '%" + searchTextCopy + "%' AND item_stock > 0";
                        fillProductTableCMD.Connection = sqlCon;
                        SqlDataAdapter fillProductTableDA = new SqlDataAdapter(fillProductTableCMD);
                        DataTable fillProductTableDT = new DataTable("items");
                        fillProductTableDA.Fill(fillProductTableDT);

                        product_data_grid.ItemsSource = fillProductTableDT.DefaultView;

                        sqlCon.Close();
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
                sqlCon.Close();
            }

        }

        private void search_btn_Click(object sender, RoutedEventArgs e)
        {
            searchBtn = true;
            searchTextCopy = search_input.Text;


            SqlConnection sqlCon1 = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");

            try
            {
                if (sqlCon1.State == ConnectionState.Closed)
                {
                    sqlCon1.Open();

                    SqlCommand refreshItemCMD = new SqlCommand();
                    refreshItemCMD.CommandText = "SELECT * FROM available_items INNER JOIN items ON available_items.item_id = items.item_id WHERE items.item_name LIKE '%" + search_input.Text + "%' AND item_stock > 0";
                    refreshItemCMD.Connection = sqlCon1;
                    SqlDataAdapter refreshItemDA = new SqlDataAdapter(refreshItemCMD);
                    DataTable refreshItemDT = new DataTable("items");
                    refreshItemDA.Fill(refreshItemDT);

                    product_data_grid.ItemsSource = refreshItemDT.DefaultView;

                    sqlCon1.Close();

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
                sqlCon1.Close();
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

                SqlConnection sqlConUpdateStock = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");

                sqlConUpdateStock.Open();
                SqlCommand updateItemStockCMD = new SqlCommand();
                updateItemStockCMD.Connection = sqlConUpdateStock;
                updateItemStockCMD.CommandText = "UPDATE available_items SET stock_copy = '" + getItemStockInt + "' WHERE item_id = '" + getItemIDInt + "'";
                SqlDataAdapter updateItemStockDA = new SqlDataAdapter(updateItemStockCMD);
                DataTable updateItemStockDT = new DataTable();
                updateItemStockDA.Fill(updateItemStockDT);
                sqlConUpdateStock.Close();

                fill_item_data_grid();

                if(getItemStockInt < 0)
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

                    SqlConnection sqlConUpdateStock1 = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");

                    sqlConUpdateStock1.Open();
                    SqlCommand updateItemStockCMD1 = new SqlCommand();
                    updateItemStockCMD1.Connection = sqlConUpdateStock1;
                    updateItemStockCMD1.CommandText = "UPDATE available_items SET stock_copy = '" + getItemStockInt1 + "' WHERE item_id = '" + getItemIDInt1 + "'";
                    SqlDataAdapter updateItemStockDA1 = new SqlDataAdapter(updateItemStockCMD1);
                    DataTable updateItemStockDT1 = new DataTable();
                    updateItemStockDA1.Fill(updateItemStockDT1);
                    sqlConUpdateStock1.Close();

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
            for(int countDataGridItems = 0; countDataGridItems < itemsGridCount; countDataGridItems++)
            {
                TextBlock getItemID = product_data_grid.Columns[0].GetCellContent(product_data_grid.Items[countDataGridItems]) as TextBlock;
                String getItemIDString = getItemID.Text;
                int getItemIDInt = Convert.ToInt32(getItemIDString);

                if(getItemIDInt == getSelectedCustomerItemIDInt)
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

            for(int counterItems = 0; counterItems < countNumberOfItems; counterItems++)
            {
                TextBlock getItemID = product_data_grid.Columns[0].GetCellContent(product_data_grid.Items[counterItems]) as TextBlock;
                String getItemIDString = getItemID.Text;
                int getItemIDInt = Convert.ToInt32(getItemIDString);

                if(getItemIDInt == getCustomerItemIDInt)
                {
                    counterFoundSimilar++;
                }
            }

            if(counterFoundSimilar > 0)
            {
                TextBlock getItemStock = product_data_grid.Columns[3].GetCellContent(product_data_grid.Items[matchedItemID]) as TextBlock;
                String getItemStockString = getItemStock.Text;
                int getItemStockInt = Convert.ToInt32(getItemStockString);
                getItemStockInt++;

                //updating stock amount in database
                SqlConnection sqlConUpdateStock = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");

                sqlConUpdateStock.Open();
                SqlCommand updateItemStockCMD = new SqlCommand();
                updateItemStockCMD.Connection = sqlConUpdateStock;
                updateItemStockCMD.CommandText = "UPDATE available_items SET stock_copy = '" + getItemStockInt + "' WHERE item_id = '" + getCustomerItemIDInt + "'";
                SqlDataAdapter updateItemStockDA = new SqlDataAdapter(updateItemStockCMD);
                DataTable updateItemStockDT = new DataTable();
                updateItemStockDA.Fill(updateItemStockDT);
                sqlConUpdateStock.Close();

                fill_item_data_grid();
                //**********
            }

            else
            { //updating stock amount in database
                SqlConnection sqlConUpdateStock = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");

                sqlConUpdateStock.Open();
                SqlCommand updateItemStockCMD = new SqlCommand();
                updateItemStockCMD.Connection = sqlConUpdateStock;
                updateItemStockCMD.CommandText = "UPDATE available_items SET stock_copy = 1 WHERE item_id = '" + getCustomerItemIDInt + "'";
                SqlDataAdapter updateItemStockDA = new SqlDataAdapter(updateItemStockCMD);
                DataTable updateItemStockDT = new DataTable();
                updateItemStockDA.Fill(updateItemStockDT);
                sqlConUpdateStock.Close();

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
