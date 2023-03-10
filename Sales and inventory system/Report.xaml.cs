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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sales_and_Inventory_System;
using SAPBusinessObjects.WPF.Viewer;

namespace Sales_and_Inventory_System
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Page
    {
        SqlConnection connection = ConnectionString.Connection;

        bool didUserSelectedItem = false;

        public Report()
        {

            InitializeComponent();

            sales_report_combo_box.Text = "Daily";

        }

        //for losing focus on text box and data grid cell when click outside
        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();

            SolidColorBrush newInactiveSelectionBrush = new SolidColorBrush(Colors.Transparent);
            SolidColorBrush newInactiveSelectionTextBrush = new SolidColorBrush(Colors.Black);

            report_grid.Resources[SystemColors.InactiveSelectionHighlightBrushKey] = newInactiveSelectionBrush;
            report_grid.Resources[SystemColors.InactiveSelectionHighlightTextBrushKey] = newInactiveSelectionTextBrush;
        }
        //end ************************************

        //maintain row of dattagrid still selected
        private void window_MouseDown_item_grid(object sender, RoutedEventArgs e)
        {
            if (didUserSelectedItem == false)
            {
                SolidColorBrush newInactiveSelectionBrushItemGrid = new SolidColorBrush(Colors.Transparent);
                SolidColorBrush newInactiveSelectionTextBrushItemGrid = new SolidColorBrush(Colors.Black);

                report_grid.Resources[SystemColors.InactiveSelectionHighlightBrushKey] = newInactiveSelectionBrushItemGrid;
                report_grid.Resources[SystemColors.InactiveSelectionHighlightTextBrushKey] = newInactiveSelectionTextBrushItemGrid;
            }

            else
            {
                SolidColorBrush newInactiveSelectionBrushItemGrid = new SolidColorBrush(Colors.Blue);
                SolidColorBrush newInactiveSelectionTextBrushItemGrid = new SolidColorBrush(Colors.White);

                report_grid.Resources[SystemColors.InactiveSelectionHighlightBrushKey] = newInactiveSelectionBrushItemGrid;
                report_grid.Resources[SystemColors.InactiveSelectionHighlightTextBrushKey] = newInactiveSelectionTextBrushItemGrid;
            }

            Keyboard.ClearFocus();
        }
        //end ************************************


        private void displayReportData(String sales_report_combo_box_value)
        {
            if(sales_report_combo_box_value == "Daily")
            {
                /*connection.Open();

                SqlCommand fillProductTableCMD = new SqlCommand();
                fillProductTableCMD.CommandText = "SELECT * FROM ordered_date INNER JOIN item ON available_items.item_serial_number = item.item_serial_number WHERE item_stock > 0";
                fillProductTableCMD.Connection = connection;
                SqlDataAdapter fillProductTableDA = new SqlDataAdapter(fillProductTableCMD);
                DataTable fillProductTableDT = new DataTable("items");
                fillProductTableDA.Fill(fillProductTableDT);

                report_grid.ItemsSource = fillProductTableDT.DefaultView;

                connection.Close();*/
            }

            else if (sales_report_combo_box_value == "Weekly")
            {
                MessageBox.Show("Weekly");
            }

            else if (sales_report_combo_box_value == "Monthly")
            {
                MessageBox.Show("Monthly");
            }

            else if (sales_report_combo_box_value == "Yearly")
            {
                MessageBox.Show("Yearly");
            }
        }

        private void sales_report_combo_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedValue_SalesReportComboBox = ((System.Windows.Controls.ComboBoxItem)sales_report_combo_box.SelectedItem).Content as string;
            displayReportData(selectedValue_SalesReportComboBox);
        }
    }
}
