using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales_and_Inventory_System
{
    internal class ConnectionString
    {
        public static SqlConnection Connection = new SqlConnection(@"Data Source=LAPTOP-KQMHEG3A;Initial Catalog=sales_inventory;Integrated Security=True");
    }
}
