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
using RBC.AppCodes;

namespace RBC.Controllers
{
    public class ReportController : ApiController
    {

        [Route("api/Report/GetReport")]
        [HttpPost]
        public ByteReportResponse GenerateByteReport(ByteReportReqBody byteReportReqBody)
        {
            ReportService reportService = new ReportService();
            ByteReportResponse response = new ByteReportResponse();
            byte[] binfile = null;

            if (byteReportReqBody.testcode == "abstractPageCount")
            {
                int pageNumber = reportService.GetAbstractPageCountNEW(byteReportReqBody);
                response.PageNumber = pageNumber;
                return response;
            }
            if (byteReportReqBody.testcode == "generatepdfExtractAbs")
            {
                return reportService.generatepdfExtractAbstract(byteReportReqBody);
            }
            else
            {
                binfile = reportService.generateReportBarcoder(byteReportReqBody);
            }
            response.ByteStream = binfile;
            return response;

        }

    }
}
