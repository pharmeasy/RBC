using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.Reporting;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

namespace RBC
{
    class Program
    {
        public static byte[] binFile;
        static void Main(string[] args)
        {
      
            //CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
            //ReportDocument rpt = new ReportDocument();
            //rpt.Load(@"C:\inetpub\wwwroot\COVER" + @"\cover.rpt");
            //CrystalReportViewer1.ReportSource = rpt;
            //rpt.SetParameterValue("page_no", "Page : " + "2" + " of " + "4");
            //System.IO.Stream oStream = null;
            //oStream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //binFile = new byte[oStream.Length];
            //oStream.Read(binFile, 0, Convert.ToInt32(oStream.Length - 1));
            //Debug.WriteLine("rpt CrystalReportViewer1 ->" + oStream);
            //Debug.Flush();
            //Console.ReadLine();
            //rpt.Close();
            //rpt.Dispose();
            //CrystalReportViewer1.Dispose();
        }
    }
}