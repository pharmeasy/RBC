using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBC.Models
{
    public class QRReqBody
    {
        public string customerId { get; set; }
    }
    public class QRResBody
    {
        public string aesEncryptionText { get; set; }
    }
}