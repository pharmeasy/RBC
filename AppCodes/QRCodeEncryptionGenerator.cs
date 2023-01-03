using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RBC.Models;

namespace RBC.AppCodes
{
    public class QRCodeEncryptionGenerator
    {
        public string CreateQRCodeEncryption(QRReqBody qrReqBody)
        {
            try
            {
                string encryptionText = Functions.Encrypt(null + "|" + qrReqBody.customerId);
                return encryptionText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during CreateQRCodeEncryption" + ex);
                ErrorLogger.InsertErrorLog(ex);
                return null;
            }
        }
    }
}