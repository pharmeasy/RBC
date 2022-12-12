using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Thyrocare.IT.DataLayer;

namespace RBC.AppCodes
{
    public class ErrorLogger
    {

        public static void InsertErrorLog(Exception strExc, string leadId = null, string toList = null, string ccList = null, string bccList = null)
        {
            CharbiServerData csdCon = new CharbiServerData();
            try
            {
                string page_name = "";
                try
                {
                    if (HttpContext.Current.Handler is System.Web.Http.ApiController)
                        page_name = (HttpContext.Current.Handler as System.Web.Http.ApiController).RequestContext.RouteData.Values["controller"].ToString();
                }
                catch (Exception ex)
                {

                }

                //HttpContext context = HttpContext.Current;
                //string IPAddress = context.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DataSet DsError = new DataSet();
                SqlParameter[] param3 = new SqlParameter[17];
                param3[0] = new SqlParameter("@type", "Insert_error");
                param3[1] = new SqlParameter("@err_id", null);
                param3[2] = new SqlParameter("@occ_date", DateTime.Now.ToString());
                param3[3] = new SqlParameter("@domain", "ReportAPI");
                param3[4] = new SqlParameter("@page_name", page_name);
                param3[5] = new SqlParameter("@page_link", HttpContext.Current.Request.Url.AbsoluteUri.ToString());
                param3[6] = new SqlParameter("@session_name", leadId);
                param3[7] = new SqlParameter("@exception_type", strExc.GetType().ToString());
                param3[8] = new SqlParameter("@err_desc", /*"TO:" + toList + ", CC:" + ccList + ", BCC:" + bccList + ", " +*/ strExc.Message + "" + strExc.StackTrace);
                param3[9] = new SqlParameter("@additional_info", null);
                param3[10] = new SqlParameter("@host", null);
                param3[11] = new SqlParameter("@mac", null);
                param3[12] = new SqlParameter("@ip", null);
                param3[13] = new SqlParameter("@device", null);
                param3[14] = new SqlParameter("@task_status", null);
                param3[15] = new SqlParameter("@task_id", "");
                param3[16] = new SqlParameter("@last_modify", DateTime.Now.ToString());
                //string strExec = Functions.GenerateScript(param3);
                DsError = csdCon.ExecuteSPWithParameters("ReportDB..USP_Error_Log", param3);
            }
            catch (Exception exc)
            {
                try
                {
                    csdCon.GetResultOfAQuery("exec ReportDB..USP_Error_Log 'INSERT_ERROR','','','ReportAPI','','','','','" + exc.ToString().Replace("'", "") + "','','','','','','','',''");
                }
                catch (Exception ex1)
                {

                }
            }
        }
    }
}