using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Collections.Generic;
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

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }


        // todo: create hashmap of testcodes and path, instead of if conditions
        public static string getRptPathsByTestCode(string testcode)
        {
            Dictionary<String, String> rptPathConfig = new Dictionary<String, String>();
            return rptPathConfig[testcode];
        }
    }
}
