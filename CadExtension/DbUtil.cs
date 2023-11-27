using Autodesk.AutoCAD.Runtime;
using System.Data.SqlClient;


namespace CadExtension
{
    public static class DbUtil
    {

        [CommandMethod("DBRun")]
        public static void DBRun()
        {
            Main main =new Main();
            main.ShowDialog();
        }

        public static SqlConnection GetConnection()
        {
            string connStr = Settings1.Default.connstr;
            SqlConnection conn = new SqlConnection(connStr);
            return conn;
        }
    }
}
