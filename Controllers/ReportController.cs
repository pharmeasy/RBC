using RBC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.Reporting;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using Thyrocare.IT.DataLayer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web;
using System.Configuration;

namespace RBC.Controllers
{
    public class ReportController : ApiController
    {
        public byte[] binFile = null;
        public byte[] binFile2 = null;
        public byte[] binFile3 = null;
        DataSet ds = null;

        [Route("api/Report/GetReport")]
        [HttpPost]
        public ByteReportResponse GenerateByteReport(ByteReportReqBody byteReportReqBody)
        {
            CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
            ReportDocument rpt = new ReportDocument();
            string rptpath = @"C:\inetpub\wwwroot\TRPT\";
            string testcode = byteReportReqBody.testcode;

            rpt.Load(rptpath + @"\HIVQN.rpt");
            ds = ExecuteReportGenerationSteps("ReportDB..usp_pdfgen_new", testcode, byteReportReqBody.labcode, byteReportReqBody.sampledate, byteReportReqBody.report_group_id, byteReportReqBody.slno);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
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
            int barcodeCount = byteReportReqBody.cnt;
            int flag = byteReportReqBody.flag;
            int endOfReport = byteReportReqBody.endOfReport;
            bool displayReport = byteReportReqBody.displayReport;
            //int RK = byteReportReqBody.RK;
            int currPage = byteReportReqBody.DN;
            int totalPages = byteReportReqBody.flag_no;
            int barcodeItr = byteReportReqBody.a;
            string reportName = byteReportReqBody.report_name;
            string refferenceOrTechnology = byteReportReqBody.strname1;
            string tempfilepath = byteReportReqBody.tempfilepath;
            string sct = byteReportReqBody.sct;
            string bvt = byteReportReqBody.bvt;
            string rrt = byteReportReqBody.rrt;
            string customerid = byteReportReqBody.customerid;


            if (flag == 1)
            {
                if (barcodeItr >= barcodeCount - 1)
                {

                    endOfReport = 1;
                    rpt.SetParameterValue("endreport", "~~ end of report ~~");
                }
                else
                {
                    //rk = currPage + 1;
                    rpt.SetParameterValue("endreport", "");

                }
            }
            else
            {
                rpt.SetParameterValue("endreport", "");
            }

            rpt.SetParameterValue("page_no", "page : " + currPage + " of " + totalPages);
            if (currPage <= totalPages)
            {
                currPage++;
            }

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

            if (binFile2 == null)
            {
                binFile2 = binFile;
            }
            else if (binFile3 == null)
            {
                binFile3 = meargePDF(binFile2, binFile);
            }
            else
            {
                binFile3 = meargePDF(binFile3, binFile);
            }
            //rpt.Close();
            //rpt.Dispose();
            //CrystalReportViewer1.Dispose();

            if (endOfReport == 1 && displayReport == true)
            {
                AddSummaryPage(rrt, sct, customerid, currPage, totalPages, ref binFile3, ref binFile2);
            }

            ds.Tables[0].Clear();

            //Debug.WriteLine("rpt CrystalReportViewer1 ->" + oStream);
            //Debug.Flush();
            //Console.ReadLine();
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();

            ByteReportResponse res = new ByteReportResponse();
            res.ByteStream = binFile3;
            return res;
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

        public byte[] meargePDF(byte[] MainTream, byte[] NextPDFmem)
        {
            try
            {
                MemoryStream stream1 = new MemoryStream(MainTream);
                MemoryStream stream2 = new MemoryStream(NextPDFmem);
                //Create three MemoryStreams
                MemoryStream[] streams = { stream1, stream2 };
                //I don't have a web server handy so I'm going to write my final MemoryStream to a byte array and then to disk
                byte[] bytes;
                //Create our final combined MemoryStream
                MemoryStream finalStream = new MemoryStream();
                {
                    //Create our copy object
                    PdfCopyFields copy = new PdfCopyFields(finalStream);
                    //Loop through each MemoryStream
                    foreach (MemoryStream ms in streams)
                    {
                        //Reset the position back to zero
                        ms.Position = 0;
                        //Add it to the copy object
                        copy.AddDocument(new PdfReader(ms));
                        //Clean up
                        ms.Dispose();
                    }
                    //Close the copy object
                    copy.Close();
                    //Get the raw bytes to save to disk
                    bytes = finalStream.ToArray();
                }
                return bytes;
            }
            catch (Exception ex)
            {
                RBC.AppCodes.ErrorLogger.InsertErrorLog(ex);
                return null;
            }
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public void AddSummaryPage(string rrt, string sct, string customerid, int DN, int flag_no, ref byte[] Report, ref byte[] Reportt)
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
                string actualamount = "Rs." + amountcollected + "/-(" + NumberToWords(amountcollected) + " only)";
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
                //rpt.SetParameterValue("sampletype", datasheet_data.Tables[1].Rows[0]["sample_type"].ToString());
                //string labcode = datasheet_data.Tables[0].Rows[0]["lab_code"].ToString();
                //rpt.SetParameterValue("lab_code", labcode);
                rpt.SetParameterValue("sct", sct);
                rpt.SetParameterValue("rrt", rrt);
                rpt.SetParameterValue("amcollec", amountcollected == 0 ? "-" : actualamount);
                rpt.SetParameterValue("page_no", "Page : " + DN + " of " + flag_no);
                //flag_no--;
                System.IO.Stream oStream = null;
                oStream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                binFile = new byte[oStream.Length];
                oStream.Read(binFile, 0, Convert.ToInt32(oStream.Length - 1));
                if (Report != null)
                {
                    Report = meargePDF(Report, binFile);
                }
                else
                {
                    Report = meargePDF(Reportt, binFile);
                }
                rpt.Close();
                rpt.Dispose();
                CrystalReportViewer1.Dispose();
            }
            catch (Exception Ex)
            {
                rpt.Close();
                rpt.Dispose();
                CrystalReportViewer1.Dispose();
                RBC.AppCodes.ErrorLogger.InsertErrorLog(Ex);
            }

        }
    }
}
