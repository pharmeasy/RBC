using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBC.Models
{
    public class ByteReportReqBody
    {
        public string CustomerId { get; set; }
        public string labcode { get; set; }
        public string sampledate { get; set; }
        public string report_group_id { get; set; }
        public int slno { get; set; }
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