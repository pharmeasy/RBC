using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBC.Models
{
    public class ByteReportReqBody
    {
        public string testcode { get; set; }
        public string labcode { get; set; }
        public string sampledate { get; set; }
        public string report_group_id { get; set; }
        public int slno { get; set; }
        public int cnt { get; set; }
        public int flag { get; set; }
        public int endOfReport { get; set; }
        public int DN { get; set; }
        public int flag_no { get; set; }
        public int a { get; set; }
        public int RK { get; set; }
        public bool displayReport { get; set; }
        public string report_name { get; set; }
        public string strname1 { get; set; }
        public string tempfilepath { get; set; }
        public string sct { get; set; }
        public string bvt { get; set; }
        public string rrt { get; set; }
        public string customerid { get; set; }
    }

    public class ByteReportResponse
    {
        public string CustomerId { get; set; }
        public string Barcodes { get; set; }
        public int isReportAvailable { get; set; }
        public string Status { get; set; }
        public string Filename { get; set; }
        public string reportPath { get; set; }
        public byte[] ByteStream { get; set; }
        public string ByteStreamBase64 { get; set; }
        public string Exception { get; set; }
        public int ByteLength { get; set; }
    }
}