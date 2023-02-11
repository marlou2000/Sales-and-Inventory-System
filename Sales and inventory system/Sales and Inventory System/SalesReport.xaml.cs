using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using CrystalDecisions.CrystalReports.Engine;

namespace Sales_and_Inventory_System
{
    /// <summary>
    /// Interaction logic for SalesReport.xaml
    /// </summary>
    public partial class SalesReport : Window
    {
        ReportDocument crystalReport = new ReportDocument();
        SqlConnection connection = new SqlConnection("Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");
        SqlDataAdapter sda;

        public SalesReport()
        {
            InitializeComponent();

            connection.Open();
            sda = new SqlDataAdapter("SELECT * FROM orders INNER JOIN items ON orders.item_id = items.item_id;", connection);

            DataSet dst = new DataSet();
            sda.Fill(dst, "Sales");
            crystalReport.Load("SalesCrystalReport.rpt");
            crystalReport.SetDataSource(dst);
            SalesCrystalReportViewer.ViewerCore.ReportSource = crystalReport;
        }
    }
}
