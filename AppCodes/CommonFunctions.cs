using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RBC.AppCodes
{
    public class CommonFunctions
    {
        public static byte[] MergePDFs(params byte[][] Streams)
        {
            //written new function with new code for performance improvement
            //https://stackoverflow.com/questions/6029142/merging-multiple-pdfs-using-itextsharp-in-c-net

            try
            {
                MemoryStream finalStream = new MemoryStream();

                using (Document document = new Document())
                using (PdfCopy pdf = new PdfCopy(document, finalStream))
                {
                    PdfReader reader = null;
                    try
                    {
                        document.Open();
                        foreach (byte[] bt in Streams.Where(r => r != null))
                        {
                            reader = new PdfReader(bt);
                            pdf.AddDocument(reader);
                            reader.Dispose();
                        }
                    }
                    catch (Exception ex1)
                    {
                        ErrorLogger.InsertErrorLog(ex1);

                        if (reader != null)
                        {
                            reader.Dispose();
                        }
                    }
                }
                byte[] mergedBytes = finalStream.ToArray();
                finalStream.Dispose();
                return mergedBytes;
            }
            catch (Exception ex)
            {
                ErrorLogger.InsertErrorLog(ex);
                return null;
            }
        }
    }
}
