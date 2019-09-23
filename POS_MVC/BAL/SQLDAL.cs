using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RiceMill_MVC.BAL
{
    public class SQLDAL
    {
        private SqlConnection connection;

        public SQLDAL()
        {
            connection = new SqlConnection(GlobalConnection());
        }

        public string GlobalConnection()
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //string providerConnectionString = new EntityConnectionStringBuilder(entityConnectionString).ProviderConnectionString;
            return entityConnectionString;
        }

        #region Query Execute
        public Result ExecuteQuery(string SQL)
        {
            Result oResult = new Result();
            SqlCommand oCmd = null;

            try
            {

                if (connection != null)
                {
                    connection.Open();
                    oCmd = new SqlCommand(SQL, connection);
                    oCmd.CommandTimeout = 0;
                    oCmd.ExecuteNonQuery();
                    oResult.ExecutionState = true;
                }
            }
            catch (Exception ex)
            {
                oResult.ExecutionState = false;
                oResult.Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return oResult;
        }

        public Result ExecuteQuery(List<string> SQL)
        {
            Result oResult = new Result();
            SqlTransaction oTransaction = null;
            SqlCommand oCmd = null;

            try
            {

                if (connection != null)
                {
                    connection.Open();
                    oTransaction = connection.BeginTransaction();
                    foreach (string s in SQL)
                    {
                        oCmd = new SqlCommand(s, connection);
                        oCmd.Transaction = oTransaction;
                        oCmd.ExecuteNonQuery();
                        oResult.ExecutionState = true;
                    }
                    oTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                oResult.ExecutionState = false;
                oResult.Error = ex.Message;
                oTransaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
            return oResult;
        }
        #endregion

        public Result Select(string SQL)
        {
            Result oResult = new Result();
            SqlCommand oCmd = null;
            try
            {

                if (connection != null)
                {
                    connection.Open();
                    oCmd = new SqlCommand(SQL, connection);
                    oCmd.CommandTimeout = 0;
                    SqlDataAdapter adapter = new SqlDataAdapter(oCmd);
                    DataSet dt = new DataSet();
                    adapter.Fill(dt);
                    oResult.ExecutionState = true;
                    oResult.Data = dt.Tables[0];
                    oResult.ds = dt;
                }
            }
            catch (Exception ex)
            {
                oResult.ExecutionState = false;
                oResult.Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return oResult;
        }

    }
}
