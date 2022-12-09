using RBC.AppCode;
using RBC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RBC.Controllers
{
    public class ReportController : ApiController
    {
        [Route("api/Report/GetReport")]
        [HttpPost]
        public ByteReportResponse GetReportsInBytes(ByteReportRequest reportDetails)
        {
            ReportService reportService = new ReportService();
            ByteReportResponse byteReportResponse = new ByteReportResponse();
            return byteReportResponse;
        }
    }
}
