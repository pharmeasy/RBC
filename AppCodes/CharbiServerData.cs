using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Thyrocare.IT.DataLayer
{
    public class CharbiServerData
    {
        string _connectionstring;
        string _connectionStringName;
        public SqlConnection webSqlcon { get; set; }
        int QueryTimeoutSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["QueryTimeoutSeconds"]);

        public DataSet GetResultOfAQuery(string _query)
        {
            try
            {
                webSqlcon.Open();
                //DataSet ds = SqlHelper.ExecuteDataset(webSqlcon, CommandType.Text, _query);
                DataSet ds = new DataSet();
                using (SqlCommand cmd = webSqlcon.CreateCommand())
                {
                    cmd.CommandText = _query;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = QueryTimeoutSeconds;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                if (_connectionStringName != "FailoverDBConnection" && ex.Message.StartsWith("Invalid object name"))
                {
                    return new CharbiServerData("FailoverDBConnection").GetResultOfAQuery(_query);
                }

                throw new Exception(_connectionStringName + ": " + ex.Message.ToString());
            }
            finally
            {
                webSqlcon.Close();
            }
        }
        public CharbiServerData(string connectionStringName = "")
        {
            _connectionStringName = connectionStringName;
            if (String.IsNullOrEmpty(_connectionStringName))
            {
                _connectionStringName = "WebDBConnection";
            }
            _connectionstring = ConfigurationManager.AppSettings[_connectionStringName];
            webSqlcon = new SqlConnection(_connectionstring);
        }
 
        public DataSet ExecuteSPWithParameters(string spName, SqlParameter[] _params)
        {
            //SqlConnection webSqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["WebDBConnection"].ToString());
            try
            {
                webSqlcon.Open();
                //DataSet ds = SqlHelper.ExecuteDataset(webSqlcon, spName, _params);
                DataSet ds = new DataSet();
                using (SqlCommand cmd = webSqlcon.CreateCommand())
                {
                    cmd.CommandText = spName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = QueryTimeoutSeconds;
                    foreach (SqlParameter param in _params)
                        cmd.Parameters.Add(param);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }

                return ds;
            }
            catch (Exception ex)
            {
                if (_connectionStringName != "FailoverDBConnection" && ex.Message.StartsWith("Invalid object name"))
                {
                    SqlParameter[] _paramsCopy = new SqlParameter[_params.Length];
                    //_params.CopyTo(_paramsCopy, 0);
                    for (int x = 0; x < _params.Length; x++)
                    {
                        _paramsCopy[x] = new SqlParameter(_params[x].ParameterName, _params[x].Value);
                    }
                    return new CharbiServerData("FailoverDBConnection").ExecuteSPWithParameters(spName, _paramsCopy);
                }

                throw new Exception(_connectionStringName + ": " + ex.Message.ToString());
            }
            finally
            {
                webSqlcon.Close();

            }
        }

    }
}

