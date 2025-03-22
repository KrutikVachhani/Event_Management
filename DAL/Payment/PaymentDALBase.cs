using Event_Management.Areas.Payment.Models;
using Event_Management.BAL;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace Event_Management.DAL.Payment
{
    public class PaymentDALBase : DAL_Helper
    {
        #region SelectByID
        public PaymentModel PR_Payment_SelectByID(int PriceID)
        {
            PaymentModel paymentModel = new PaymentModel();
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Price_SelectByID");
                sqlDatabase.AddInParameter(dbCommand, "@PriceID", DbType.Int32, PriceID);
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }

                foreach (DataRow dr in dataTable.Rows)
                {
                    paymentModel.PriceID = Convert.ToInt32(dr["PriceID"]);
                    //paymentModel.CardNumber = dr["CardNumber"].ToString();
                    //paymentModel.CardHolderName = dr["CardHolderName"].ToString();
                    //paymentModel.CardExpiryDate = DateTime.Parse(dr["CardExpiryDate"].ToString());
                    paymentModel.Price = Convert.ToInt32(dr["Price"].ToString());
                }
                return paymentModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region Insert/Update
        public bool PaymentSave(PaymentModel paymentModel)
        {
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            try
            {
                if (paymentModel.PaymentID == null)
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_PaymentInsert");
                    sqlDatabase.AddInParameter(dbCommand, "@UserID", DbType.Int32, CommonVariables.UserID());
                    sqlDatabase.AddInParameter(dbCommand, "@PriceID", DbType.Int32, paymentModel.PriceID);
                    sqlDatabase.AddInParameter(dbCommand, "@OrderID", DbType.String, paymentModel.OrderId);
                    sqlDatabase.AddInParameter(dbCommand, "@TransactionID", DbType.String, paymentModel.TransactionId);
                    bool isSuccess = Convert.ToBoolean(sqlDatabase.ExecuteNonQuery(dbCommand));
                    return isSuccess;
                }
                else
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Venue_Update");
                    sqlDatabase.AddInParameter(dbCommand, "@CardDetailID", DbType.Int32, paymentModel.PaymentID);
                    sqlDatabase.AddInParameter(dbCommand, "@UserID", DbType.Int32, paymentModel.UserID);
                    sqlDatabase.AddInParameter(dbCommand, "@OrderID", DbType.String, paymentModel.OrderId);
                    sqlDatabase.AddInParameter(dbCommand, "@TransactionID", DbType.String, paymentModel.TransactionId);
                    bool isSuccess = Convert.ToBoolean(sqlDatabase.ExecuteNonQuery(dbCommand));
                    return isSuccess;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Select Price

        public DataTable SelectPrice()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Price_SelectAll");
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
