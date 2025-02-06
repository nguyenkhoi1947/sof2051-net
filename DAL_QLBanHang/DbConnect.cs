using System.Data.SqlClient;  

namespace DAL_QLBanHang
{
    public class DbConnect
    {
        protected SqlConnection _conn = new SqlConnection(@"Data Source=DESKTOP-5D9R1R3\SQLEXPRESS;Initial Catalog=SOF2051_QLBanHang1;Integrated Security=True;");
    }
}
