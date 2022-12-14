using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using Newtonsoft.Json;
using RBC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using Thyrocare.IT.DataLayer;

namespace RBC.AppCodes
{
    public class ReportService
    {
        public byte[] binFile = null;
        DataSet ds = null;

        public byte[] generateReportBarcoder(ByteReportReqBody byteReportReqBody)
        {
            try
            {
                CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
                ReportDocument rpt = new ReportDocument();

                string testcode = byteReportReqBody.testcode;

                ds = ExecuteReportGenerationSteps("ReportDB..usp_pdfgen_new", testcode, byteReportReqBody.labcode, byteReportReqBody.sampledate, byteReportReqBody.report_group_id, byteReportReqBody.slno);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return null;
                }

                rpt.Load(byteReportReqBody.rptFilePath);

                if (testcode == "HIVE" || testcode == "SAGHIVE" || testcode == "HIVESAG")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            rpt.SetParameterValue("GP160", ds.Tables[1].Rows[0]["GP160"].ToString());
                            rpt.SetParameterValue("GP120", ds.Tables[1].Rows[0]["GP120"].ToString());
                            rpt.SetParameterValue("P66", ds.Tables[1].Rows[0]["P66"].ToString());
                            rpt.SetParameterValue("P55", ds.Tables[1].Rows[0]["P55"].ToString());
                            rpt.SetParameterValue("P51", ds.Tables[1].Rows[0]["P51"].ToString());
                            rpt.SetParameterValue("GP41", ds.Tables[1].Rows[0]["GP41"].ToString());
                            rpt.SetParameterValue("P31", ds.Tables[1].Rows[0]["P31"].ToString());
                            rpt.SetParameterValue("P24", ds.Tables[1].Rows[0]["P24"].ToString());
                            rpt.SetParameterValue("P17", ds.Tables[1].Rows[0]["P17"].ToString());
                            rpt.SetParameterValue("HIV_2", ds.Tables[1].Rows[0]["HIV_2"].ToString());
                            rpt.SetParameterValue("COMMENTS", ds.Tables[1].Rows[0]["COMMENTS"].ToString());

                        }
                        else
                        {
                            rpt.SetParameterValue("GP160", "");
                            rpt.SetParameterValue("GP120", "");
                            rpt.SetParameterValue("P66", "");
                            rpt.SetParameterValue("P55", "");
                            rpt.SetParameterValue("P51", "");
                            rpt.SetParameterValue("GP41", "");
                            rpt.SetParameterValue("P31", "");
                            rpt.SetParameterValue("P24", "");
                            rpt.SetParameterValue("P17", "");
                            rpt.SetParameterValue("HIV_2", "");
                            rpt.SetParameterValue("COMMENTS", "");
                        }

                    }
                }

                rpt.SetDataSource(ds.Tables[0]);
                CrystalReportViewer1.ReportSource = rpt;

                string patient_name = ds.Tables[0].Rows[0]["patient"].ToString();
                rpt.SetParameterValue("pname", patient_name);
                string sdate = Convert.ToDateTime(ds.Tables[0].Rows[0]["sdate"].ToString()).ToString("dd MMM yyyy");
                rpt.SetParameterValue("sdate", sdate);
                rpt.SetParameterValue("barcode", ds.Tables[0].Rows[0]["barcode"].ToString());
                rpt.SetParameterValue("refby", ds.Tables[0].Rows[0]["ref_dr"].ToString());
                string tests = ds.Tables[0].Rows[0]["tests"].ToString();
       
                int endOfReport = byteReportReqBody.endOfReport;
                bool displayReport = byteReportReqBody.displayReport;
                int currPage = byteReportReqBody.currPage;
                int totalPages = byteReportReqBody.totalPages;
                string reportName = byteReportReqBody.reportName;
                string refferenceOrTechnology = byteReportReqBody.strname1;
                string tempfilepath = byteReportReqBody.tempfilepath;
                string sct = byteReportReqBody.sct;
                string bvt = byteReportReqBody.bvt;
                string rrt = byteReportReqBody.rrt;
                string customerid = byteReportReqBody.customerId;

                if(endOfReport == 1)
                {
                    rpt.SetParameterValue("endreport", "~~ end of report ~~");
                }
                else
                {
                    rpt.SetParameterValue("endreport", "");
                }

                rpt.SetParameterValue("page_no", "page : " + currPage + " of " + totalPages);

                if (reportName.ToUpper() == "allergy.rpt")
                    rpt.SetParameterValue("strname", refferenceOrTechnology);
                if (testcode == "ualb")
                {
                    if (tests.ToUpper().Contains("urog"))
                    {
                        //tests = tests.ToUpper().replace(",urog", "");
                        rpt.SetParameterValue("tests", tests);
                    }
                    else
                        rpt.SetParameterValue("tests", tests);
                }
                else if (testcode == "urog")
                {
                    if (tests.ToUpper().Contains("urog"))
                    {
                        rpt.SetParameterValue("tests", "urog");
                    }
                    else if (tests.ToUpper().Contains("complete urine analysis"))
                    {
                        rpt.SetParameterValue("tests", "complete urine analysis");
                    }
                    else
                    {
                        rpt.SetParameterValue("tests", tests);
                    }

                }
                else if (testcode == "hba")
                {
                    if (tests.ToUpper().Contains("hemo"))
                    {
                        rpt.SetParameterValue("tests", tests);
                    }
                    else
                        rpt.SetParameterValue("tests", tests);
                }
                else if (testcode == "hemo")
                {

                    if (tests.ToUpper().Contains("hemo"))
                        rpt.SetParameterValue("tests", "hemo");
                    else
                        rpt.SetParameterValue("tests", "hemogram");
                }
                else
                    rpt.SetParameterValue("tests", tests);

                if (testcode == "sag" || testcode == "ahcv" || testcode == "hive")
                {

                    rpt.SetParameterValue("pname", patient_name);
                    rpt.SetParameterValue("wp", " ");
                }
                else
                {
                    rpt.SetParameterValue("pname", patient_name);
                    rpt.SetParameterValue("wp", " ");
                }

                string labcode1 = ds.Tables[0].Rows[0]["lab_code"].ToString();
                rpt.SetParameterValue("labcode", labcode1);
                rpt.SetParameterValue("sct", sct.ToString());
                rpt.SetParameterValue("bvt", bvt.ToString());
                rpt.SetParameterValue("rrt", rrt.ToString());
                rpt.SetParameterValue("sample_type", ds.Tables[0].Rows[0]["sample_type"].ToString());
                rpt.SetParameterValue("remark", ds.Tables[0].Rows[0]["remarks"].ToString());

                // qr
                rpt.SetParameterValue("qrfilepath", tempfilepath);

                if (reportName.ToUpper() == "profilereport.rpt")
                    rpt.SetParameterValue("reference", refferenceOrTechnology);

                if (reportName.ToUpper() == "thyroidreport.rpt")
                    rpt.SetParameterValue("technology", refferenceOrTechnology);

                if (reportName.ToUpper() == "multireport.rpt")
                    rpt.SetParameterValue("strname", refferenceOrTechnology);

                if (reportName.ToUpper() == "pbs")
                    rpt.SetParameterValue("reference", refferenceOrTechnology);

                string[] _barcode = ds.Tables[0].Rows[0]["barcode"].ToString().ToUpper().Split('/');

                // convert rpt data into byte array
                System.IO.Stream oStream = null;
                oStream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                binFile = new byte[oStream.Length];
                oStream.Read(binFile, 0, Convert.ToInt32(oStream.Length - 1));

                ds.Tables[0].Clear();

                rpt.Close();
                rpt.Dispose();
                CrystalReportViewer1.Dispose();
                return binFile;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception during generateReportBarcoder" + ex);
                ErrorLogger.InsertErrorLog(ex);
                return null;
            }
        }
          
        public byte[] generatepdfExtractAbstract(ByteReportReqBody byteReportReqBody)
        {
            try
            {
                List<AllData> UserList = JsonConvert.DeserializeObject<List<AllData>>(byteReportReqBody.dataSet);
                DataTable dt = StringToDataTable.ToDataTable(UserList);

                //CharbiServerData charbiServerData = byteReportReqBody.ReportServerIdentifider == 1 ? new CharbiServerData() : new CharbiServerData("FailoverDBConnection");
                //DataSet woResultDetails = charbiServerData.GetResultOfAQuery("Select * from ReportDB..wo_result_detail (Nolock) where customerid='" + byteReportReqBody.customerId + "'");
                //DataSet reportAllData = charbiServerData.GetResultOfAQuery("exec ReportDB..USP_REPORTGEN_PATIENTWISE 'GETPATIENTDTL','" + byteReportReqBody.customerId + "',''");
                return binFile;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception during generatepdfExtractAbstract" + ex);
                ErrorLogger.InsertErrorLog(ex);
                return null;
            }
        }
        private DataSet ExecuteReportGenerationSteps(string _spName, string _keyword, string labcode = null, string sdate = null, string report_name = null, int slno = 0)
        {
            SqlParameter[] sqlParams = new SqlParameter[5];
            sqlParams[0] = new SqlParameter("@key_word", _keyword);
            sqlParams[1] = new SqlParameter("@LAB_CODE", labcode);
            sqlParams[2] = new SqlParameter("@SDATE", sdate);
            sqlParams[3] = new SqlParameter("@REPORT_NAME", report_name);
            sqlParams[4] = new SqlParameter("@slno", slno);
            return (new CharbiServerData().ExecuteSPWithParameters(_spName, sqlParams));
        }

        public byte[] AddSummaryPage(string rrt, string sct, string customerid, int DN, int flag_no)
        {
            StringBuilder tests_to_show_other = new StringBuilder();
            StringBuilder tests_to_show_serumn = new StringBuilder();
            CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
            ReportDocument rpt = new ReportDocument();
            try
            {
                DataSet datasheet_data = ExecuteReportGenerationSteps("ReportDB..usp_pdfgen_new", "GET_DATASHEET_NEW", customerid);
                string rptpath = ConfigurationManager.AppSettings["DataSheet"].ToString();
                string[] tempstore = datasheet_data.Tables[0].Rows[0]["PATIENT_NAME"].ToString().Split('(', '/', ')');
                int amountcollected = datasheet_data.Tables[0].Rows[0]["AMOUNT"].ToString().Trim() == "" ? 0 : Convert.ToInt32(datasheet_data.Tables[0].Rows[0]["AMOUNT"].ToString());

                // logic for tests 

                for (int r = 0; r < datasheet_data.Tables[1].Rows.Count; r++)
                {
                    if (datasheet_data.Tables[1].Rows[r]["SAMPLE_TYPE"].ToString() == "SERUM")
                    {
                        if (tests_to_show_serumn.ToString() == "")
                        {
                            tests_to_show_serumn.Append(datasheet_data.Tables[1].Rows[r]["TESTS"]);
                        }
                        else
                        {
                            tests_to_show_serumn.Append("," + datasheet_data.Tables[1].Rows[r]["TESTS"]);
                        }
                    }
                    else
                    {
                        if (tests_to_show_other.ToString() == "")
                        {
                            tests_to_show_other.Append(datasheet_data.Tables[1].Rows[r]["TESTS"]);
                        }
                        else
                        {
                            tests_to_show_other.Append("," + datasheet_data.Tables[1].Rows[r]["TESTS"]);
                        }
                    }
                }
                string tests = tests_to_show_serumn.ToString() == "" ? tests_to_show_other.ToString() : tests_to_show_serumn.ToString();
                string actualamount = "Rs." + amountcollected + "/-(" + CommonFunctions.NumberToWords(amountcollected) + " only)";
                rpt.Load(rptpath + @"\ReportSummary.rpt");
                CrystalReportViewer1.ReportSource = rpt;
                rpt.SetParameterValue("pname", tempstore[0].ToString());
                rpt.SetParameterValue("Age", tempstore[1].ToString());
                rpt.SetParameterValue("Sex", tempstore[2].ToString());
                rpt.SetParameterValue("customerID", customerid);
                rpt.SetParameterValue("barcodes", datasheet_data.Tables[1].Rows[0]["BARCODE"].ToString());
                rpt.SetParameterValue("refby", datasheet_data.Tables[0].Rows[0]["REF_BY"].ToString());
                rpt.SetParameterValue("remark", datasheet_data.Tables[0].Rows[0]["CLIENTADDRESS"].ToString());
                // rpt.SetParameterValue("TspAddress", datasheet_data.Tables[0].Rows[0]["TSP_ADDRESS"].ToString());
                rpt.SetParameterValue("Tests", tests);
                rpt.SetParameterValue("labcode", datasheet_data.Tables[1].Rows[0]["lab_code"].ToString());
                rpt.SetParameterValue("mobile", datasheet_data.Tables[0].Rows[0]["mobile"].ToString());
                if (datasheet_data.Tables[1].Rows.Count > 0)
                {
                    rpt.SetParameterValue("sampletype", datasheet_data.Tables[1].Rows[0]["sample_type"].ToString());
                }
                else
                {
                    rpt.SetParameterValue("sampletype", "");
                }
                if (datasheet_data.Tables[1].Rows.Count > 1)
                {
                    rpt.SetParameterValue("sampletype1", datasheet_data.Tables[1].Rows[1]["sample_type"].ToString());
                }
                else
                {
                    rpt.SetParameterValue("sampletype1", "");
                }
                if (datasheet_data.Tables[1].Rows.Count > 2)
                {
                    rpt.SetParameterValue("sampletype2", datasheet_data.Tables[1].Rows[2]["sample_type"].ToString());
                }
                else
                {
                    rpt.SetParameterValue("sampletype2", "");
                }
                if (datasheet_data.Tables[1].Rows.Count > 3)
                {
                    rpt.SetParameterValue("sampletype3", datasheet_data.Tables[1].Rows[3]["sample_type"].ToString());
                }
                else
                {
                    rpt.SetParameterValue("sampletype3", "");
                }
                if (datasheet_data.Tables[1].Rows.Count > 4)
                {
                    rpt.SetParameterValue("sampletype4", datasheet_data.Tables[1].Rows[4]["sample_type"].ToString());
                }
                else
                {
                    rpt.SetParameterValue("sampletype4", "");
                }
       
                rpt.SetParameterValue("sct", sct);
                rpt.SetParameterValue("rrt", rrt);
                rpt.SetParameterValue("amcollec", amountcollected == 0 ? "-" : actualamount);
                rpt.SetParameterValue("page_no", "Page : " + DN + " of " + flag_no);
                //flag_no--;
                System.IO.Stream oStream = null;
                oStream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                binFile = new byte[oStream.Length];
                oStream.Read(binFile, 0, Convert.ToInt32(oStream.Length - 1));
                rpt.Close();
                rpt.Dispose();
                CrystalReportViewer1.Dispose();
                return binFile;
            }
            catch (Exception Ex)
            {
                rpt.Close();
                rpt.Dispose();
                CrystalReportViewer1.Dispose();
                ErrorLogger.InsertErrorLog(Ex);
                return null;
            }

        }

    }
}