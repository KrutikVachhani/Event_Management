using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace Event_Management.DAL.SEC_Admin
{
    public class SEC_AdminDALBase : DAL_Helper
    {
        #region Counter
        public DataTable PR_Overview_Counter()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Overview_Counter");
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
