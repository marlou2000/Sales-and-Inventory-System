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

                        if (item_price.Text == "0.00" || String.IsNullOrEmpty(item_stock.Text))
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

                                if (warranty.Text == "0" || String.IsNullOrEmpty(item_stock.Text))
                                {
                                    System.Windows.MessageBox.Show("Item warranty should not be empty or 0!", "Empty item warranty", MessageBoxButton.OK, MessageBoxImage.Error);
                                    add = false;
                                    item_stock.Focus();
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

                                    String[] itemNames = new string[itemCount];

                                    connection.Open();
                                    SqlCommand getItemNameCMD = new SqlCommand();
                                    getItemNameCMD.Connection = connection;
                                    getItemNameCMD.CommandText = "SELECT * FROM item";
                                    getItemNameCMD.ExecuteNonQuery();

                                    String itemName;
                                    int loop = 0;

                                    SqlDataReader getItemNameDR = getItemNameCMD.ExecuteReader();
                                    while (getItemNameDR.Read())
                                    {
                                        itemName = getItemNameDR.GetValue(1).ToString();
                                        itemNames[loop] = itemName;

                                        loop++;
                                    }
                                    getItemNameDR.Close();
                                    connection.Close();

                                    bool checkIfFound = false;

                                    for (int loopNameCounter = 0; loopNameCounter < itemCount; loopNameCounter++)
                                    {
                                        if (item_name.Text == itemNames[loopNameCounter])
                                        {
                                            //question before adding item
                                            MessageBoxResult result = System.Windows.MessageBox.Show("This item is already been added, proceeding this process will update the price, description, image and stock of this item, do you still want to add this item?", "Add exsisting item", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                            if (result == MessageBoxResult.Yes)
                                            {
                                                connection.Open();
                                                SqlCommand getItemIDCMD = new SqlCommand();
                                                getItemIDCMD.Connection = connection;
                                                getItemIDCMD.CommandText = "SELECT * FROM available_items INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE item_name = '" + item_name.Text + "'";
                                                getItemIDCMD.ExecuteNonQuery();

                                                String itemID;
                                                int itemIDInt = 0;
                                                String itemStock;
                                                int itemStockInt = 0;

                                                SqlDataReader getItemIDDR = getItemIDCMD.ExecuteReader();
                                                while (getItemIDDR.Read())
                                                {
                                                    itemID = getItemIDDR.GetValue(0).ToString();
                                                    itemIDInt = Int32.Parse(itemID);
                                                    itemStock = getItemIDDR.GetValue(2).ToString();
                                                    itemStockInt = Int32.Parse(itemStock);
                                                }
                                                getItemIDDR.Close();
                                                connection.Close();

                                                int item_stock_int = Convert.ToInt32(item_stock.Text);
                                                int totalStock = item_stock_int + itemStockInt;

                                                SqlCommand insertAvailableItemCMD = new SqlCommand();
                                                insertAvailableItemCMD.Connection = connection;
                                                insertAvailableItemCMD.CommandText = "UPDATE available_items SET item_stock = '" + totalStock + "', stock_copy = '" + totalStock + "' WHERE item_serial_number = '" + itemIDInt + "' ";
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
                                                updateItemCMD.CommandText = "UPDATE item SET item_description = '" + itemDescription + "', item_price = '" + item_price.Text + "' WHERE item_serial_number = '" + itemIDInt + "'";
                                                SqlDataAdapter updateItemDA = new SqlDataAdapter(updateItemCMD);
                                                DataTable updateItemDT = new DataTable();
                                                updateItemDA.Fill(updateItemDT);
                                                connection.Close();

                                                if (uploadButtonClicked == true)
                                                {
                                                    File.Copy(filePath, targetFile, true);
                                                }

                                                System.Windows.MessageBox.Show("Item stock updated successfully");
                                                item_name.Text = "";
                                                item_price.FormatString = "0.00";
                                                item_price.Text = "0.00";
                                                item_stock.Text = "0";
                                                item_description.Text = "";
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
                                            insertItemCMD.CommandText = "INSERT INTO item(item_serial_number,item_name, item_model, item_price, item_description, item_warranty, item_added_date) VALUES('" + serial_number.Text + "', '" + item_name.Text + "', '" + model.Text + "', '" + item_price.Text + "', '" + item_description.Text + "', '" + formattedDate1 + "', '" + dateFormatted + "')";
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
                                            item_name.Text = "";
                                            item_price.FormatString = "0.00";
                                            item_price.Text = "0.00";
                                            item_stock.Text = "0";
                                            item_description.Text = "";
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
