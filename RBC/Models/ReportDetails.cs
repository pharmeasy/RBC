using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC.Models
{
    public class ByteReportRequest
    {
        public string CustomerId { get; set; }
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
