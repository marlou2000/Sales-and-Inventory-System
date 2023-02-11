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
        String itemDescription;
        String itemPriceString;
        int itemPriceInt;
        String itemStockString;
        int itemStockInt;

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
            getItemInformationCMD.CommandText = "SELECT * FROM items INNER JOIN available_items ON items.item_id = available_items.item_id WHERE items.item_id = '" + Home.instance.itemIDToEdit + "'";
            getItemInformationCMD.ExecuteNonQuery();

            SqlDataReader getItemInformationDR = getItemInformationCMD.ExecuteReader();
            while (getItemInformationDR.Read())
            {
                itemName = getItemInformationDR.GetValue(1).ToString();
                itemDescription = getItemInformationDR.GetValue(3).ToString();
                itemPriceString = getItemInformationDR.GetValue(2).ToString();
                itemStockString = getItemInformationDR.GetValue(6).ToString();
                itemStockInt = Int32.Parse(itemStockString);
            }
            getItemInformationDR.Close();
            connection.Close();

            item_name.Text = itemName;
            item_description.Text = itemDescription;
            item_price.Text = itemPriceString;
            item_stock.Text = itemStockString;

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

            if (string.IsNullOrEmpty(item_name.Text))
            {
                System.Windows.MessageBox.Show("Item name should not be empty!", "Empty item name", MessageBoxButton.OK, MessageBoxImage.Error);
                item_name.Focus();
            }

            else
            {
                if (string.IsNullOrEmpty(item_stock.Text) || item_stock.Text == "0") {
                    System.Windows.MessageBox.Show("Item stock should not be 0 or empty!", "Empty item stock", MessageBoxButton.OK, MessageBoxImage.Error);
                    item_stock.Focus();
                }

                else
                {
                    MessageBoxResult result1 = System.Windows.MessageBox.Show("Are you sure you want to update the information of this item?", "Update item information", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        SqlCommand updateItemCMD = new SqlCommand();
                        updateItemCMD.Connection = connection;
                        updateItemCMD.CommandText = "UPDATE items SET item_name = '" + item_name.Text + "', item_description = '" + item_description.Text + "', item_price = '" + item_price.Text + "' WHERE item_id = '" + Home.instance.itemIDToEdit + "'";
                        SqlDataAdapter updateItemDA = new SqlDataAdapter(updateItemCMD);
                        DataTable updateItemDT = new DataTable();
                        updateItemDA.Fill(updateItemDT);
                        connection.Close();

                        SqlCommand updateItemStockCMD = new SqlCommand();
                        updateItemStockCMD.Connection = connection;
                        updateItemStockCMD.CommandText = "UPDATE available_items SET item_stock = '" + item_stock.Text + "', stock_copy = '" + item_stock.Text + "' WHERE item_id = '" + Home.instance.itemIDToEdit + "'";
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
                        Home.instance.name_textblock.Text = "Item name";
                        Home.instance.price_textblock.Text = "0.00";
                        Home.instance.description_textblock.Text = "Item description";
                        Home.instance.stock_textblock.Text = "0";
                        Home.instance.edit_item_btn.IsEnabled = false;

                        this.Close();
                    }
                }
            }
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
