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

namespace RBC.Controllers
{
    public class ReportController : ApiController
    {
        public byte[] binFile = null;
        DataSet ds = null;

        [Route("api/Report/GetReport")]
        [HttpPost]
        public ByteReportResponse GenerateByteReport(ByteReportReqBody byteReportReqBody)
        {
            CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
            ReportDocument rpt = new ReportDocument();
            string rptpath = @"C:\inetpub\wwwroot\TMAIL_RPT\PE\";

            rpt.Load(rptpath + @"\PartialReport.rpt");
            ds = ExecuteReportGenerationSteps("ReportDB..usp_pdfgen_new", "HIVQN", byteReportReqBody.labcode, byteReportReqBody.sampledate, byteReportReqBody.report_group_id, byteReportReqBody.slno);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            rpt.SetDataSource(ds.Tables[0]);
            rpt.SetParameterValue("pname", "dummy");
            rpt.SetParameterValue("refby", "dummyRef");
            rpt.SetParameterValue("remark", "dummyRemark");

            //CrystalReportViewer1.ReportSource = rpt;
            System.IO.Stream oStream = null;
            oStream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            binFile = new byte[oStream.Length];
            oStream.Read(binFile, 0, Convert.ToInt32(oStream.Length - 1));

            Debug.WriteLine("rpt CrystalReportViewer1 ->" + oStream);
            Debug.Flush();
            Console.ReadLine();
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();

            ByteReportResponse res = new ByteReportResponse();
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
    }
}
