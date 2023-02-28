using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using Sales_and_Inventory_System;

namespace Sales_and_Inventory_System
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public static Login instance;

        SqlConnection connection = ConnectionString.Connection;

        public string username;
        public Login()
        {
            InitializeComponent();
            instance = this;    
        }

        //code for water mark 
        //hide and hide two inputs every type
        //If user input something
        private void watermark_GotFocus(object sender, RoutedEventArgs e)
        {
            pin_code.Visibility = Visibility.Visible;
            pin_code_place_holder.Visibility = Visibility.Collapsed;
            pin_code.Focus();
        }
        //end************************************


        //for losing focus on text box when click outside
        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(pin_code.Password))
            {
                Keyboard.ClearFocus();
                pin_code.Visibility = Visibility.Visible;
                pin_code_place_holder.Visibility = Visibility.Collapsed;
            }

            else
            {
                Keyboard.ClearFocus();
                pin_code.Visibility = Visibility.Collapsed;
                pin_code_place_holder.Visibility = Visibility.Visible;
            }

        }
        //end ************************************

        //query for validation
        private void enter_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;

            //check if pin code is empty or not
            if (string.IsNullOrEmpty(pin_code.Password))
            {
                pin_code.Visibility = Visibility.Visible;
                pin_code_place_holder.Visibility = Visibility.Collapsed;
                pin_code.Focus();
                MessageBox.Show("Pin code is required");
            }

            else
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    //query for verifying pin code in database
                    String verifyPinCode = "SELECT COUNT(1) FROM users WHERE user_pin_code = @pin_code";
                    SqlCommand sqlCmd = new SqlCommand(verifyPinCode, connection);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@pin_code", pin_code.Password); //getting the input pin code of user
                    count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    //if found similar pin code in database
                    if (count == 1)
                    {
                        count = 1;
                    }

                    //if nothing found
                    else
                    {
                        MessageBox.Show("Invalid pin code", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        pin_code.Password = "";
                        pin_code.Focus();
                        input_grid.BorderBrush = Brushes.Red;

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


            if(count == 1)
            {
                connection.Open();

                //query for storing the username of usesr
                String getUsername = "SELECT * FROM users WHERE user_pin_code = @pin_code";
                SqlCommand sqlCmdgetUsername = new SqlCommand(getUsername, connection);
                sqlCmdgetUsername.CommandType = CommandType.Text;
                sqlCmdgetUsername.Parameters.AddWithValue("@pin_code", pin_code.Password); //getting the input pin code of user
                SqlDataReader reader_getUsername;
                reader_getUsername = sqlCmdgetUsername.ExecuteReader();

                if (reader_getUsername.Read())
                {
                    username = reader_getUsername["username"].ToString();
                    input_grid.BorderBrush = Brushes.Green;
                    Keyboard.ClearFocus();

                    connection.Close();

                    //redirect to another page
                    Home homeWindow = new Home();
                    homeWindow.Show();
                    this.Close();

                }

                else
                {
                    MessageBox.Show("I'm sorry, no data found", "Not found", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                connection.Close();
            }

        }
        //end************************************


        //for number input only in pin code input
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        //end************************************
    }
}
