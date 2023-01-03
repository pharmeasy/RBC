using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Security.Cryptography;

namespace RBC.AppCodes
{
    public class Functions
    {
        public Functions()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static string GenerateScript(SqlParameter[] param)
        {
            StringBuilder b = new StringBuilder();
            int i;

            //#region forloop
            for (i = 0; i < param.Length; i++)
            {
                #region try
                try
                {

                    if (param[i] != null)
                    {
                        string paramname = (param[i].ParameterName).ToString();
                        string paramvalue = "";
                        if (param[i].Value != null)
                        {
                            paramvalue = (param[i].Value).ToString();
                        }
                        if (param[i].SqlDbType.ToString().ToLower().Contains("varchar")) //For varchar as parameter datatype
                        {
                            if (!(param[i].ParameterName.ToLower().Contains("@CustomErrorMessage"))) //@custome error message as output variable as varchar
                            {
                                paramvalue = "'" + paramvalue + "'";
                                b.AppendLine(" " + paramname + "=" + paramvalue + ",");
                            }
                        }

                        else if (param[i].SqlDbType.ToString().ToLower().Contains("xml")) //For xml as parameter datatype
                        {
                            paramvalue = "'" + paramvalue + "'";
                            b.AppendLine(" " + paramname + "=" + paramvalue + ",");
                        }

                        else if (param[i].SqlDbType.ToString().ToLower().Contains("int")) //For int as parameter datatype
                        {
                            if (!(param[i].ParameterName.ToLower().Contains("@outputidentity")) && !(param[i].ParameterName.ToLower().Contains("@issuccess")))  //@issuccess & @outputidentity error message as output variable
                            {
                                paramvalue = "'" + paramvalue + "'";
                                b.AppendLine(" " + paramname + "=" + paramvalue + ",");
                            }
                        }
                        else if (param[i].SqlDbType.ToString().ToLower().Contains("datetime")) //For xml as parameter datatype
                        {
                            paramvalue = "'" + paramvalue + "'";
                            b.AppendLine(" " + paramname + "=" + paramvalue + ",");
                        }
                        else
                        {
                            paramvalue = "'" + paramvalue + "'";
                            b.AppendLine(" " + paramname + "=" + paramvalue + ",");
                        }

                    }
                }
                #endregion
                #region catch
                catch
                {
                    return string.Empty;
                }
                #endregion
            }
            //#endregion
            return b.ToString();

        }

        public static string Encode(string ecode)
        {
            try
            {
                int x, y;
                string abto = "", encode = "", ABFrom = "";
                for (x = 0; x <= 25; x++)
                {
                    ABFrom = ABFrom + Convert.ToChar(65 + x);
                }
                for (x = 0; x <= 25; x++)
                {
                    ABFrom = ABFrom + Convert.ToChar(97 + x);
                }
                for (x = 0; x <= 9; x++)
                {
                    ABFrom = ABFrom + Convert.ToString(x);
                }
                int len = ABFrom.Length - 13;
                abto = abto + ABFrom.Substring(13, len);
                abto = abto + ABFrom.Substring(0, 13);

                for (x = 0; x < ecode.Length; x++)
                {
                    y = ABFrom.IndexOf(ecode.Substring(x, 1));
                    if (y == 0)
                    {
                        encode = encode + ecode.Substring(x, 1);
                    }
                    else
                    {
                        encode = encode + abto.Substring(y, 1);
                    }
                }
                return encode;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string key = "456as4d6a73a2fghHJS4865a87932d(d4586qzxxiwopdGKQPGT712lsa4d4sadas8";

        public static string Encrypt(string clearText, string portal = "")
        {

            string message;

            try
            {
                // Convert String to Byte

                byte[] MsgBytes = Encoding.UTF8.GetBytes(clearText);
                byte[] KeyBytes = Encoding.UTF8.GetBytes(key);

                // Hash the password with SHA256
                //Secure Hash Algorithm
                //Operation And, Xor, Rot,Add (mod 232),Or, Shr
                //block size 1024
                //Rounds 80
                //rotation operator , rotates point1 to point2 by theta1=> p2=rot(t1)p1
                //SHR shift to right
                KeyBytes = SHA256.Create().ComputeHash(KeyBytes);

                byte[] bytesEncrypted = AES_Encryption(MsgBytes, KeyBytes);

                string encryptionText = Convert.ToBase64String(bytesEncrypted);



                message = encryptionText;

            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return message;
        }

        public static byte[] AES_Encryption(byte[] Msg, byte[] Key)
        {
            byte[] encryptedBytes = null;

            //salt is generated randomly as an additional number to 
            //hash password or message in order o dictionary attack
            //against pre computed rainbow table
            //dictionary attack is a systematic way to test 
            //all of possibilities words in dictionary weather or not is true?
            //to find decryption key
            //rainbow table is precomputed key for cracking password
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.  == 16 bits
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(Key, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream
                          (ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(Msg, 0, Msg.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        public static string Decode(string qry_str)
        {
            try
            {
                int x, y;
                string abto = "", Decode = "", ABFrom = "";
                for (x = 0; x <= 25; x++)
                {
                    ABFrom = ABFrom + Convert.ToChar(65 + x);
                }
                for (x = 0; x <= 25; x++)
                {
                    ABFrom = ABFrom + Convert.ToChar(97 + x);
                }
                for (x = 0; x <= 9; x++)
                {
                    ABFrom = ABFrom + Convert.ToString(x);
                }
                int len = ABFrom.Length - 13;
                abto = abto + ABFrom.Substring(13, len);
                abto = abto + ABFrom.Substring(0, 13);

                for (x = 0; x < qry_str.Length; x++)
                {
                    y = abto.IndexOf(qry_str.Substring(x, 1));
                    if (y == 0)
                    {
                        Decode = Decode + qry_str.Substring(x, 1);
                    }
                    else
                    {
                        Decode = Decode + ABFrom.Substring(y, 1);
                    }
                }
                return Decode;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static System.Data.SqlTypes.SqlXml CreateXml(DataTable dt)
        {
            StringWriter swReq = new StringWriter();
            dt.WriteXml(swReq);
            return ConvertStringToXML(swReq.ToString());
            // dt.WriteXml("items.xml");  // Uncomment this if you want to save this as XML file

        }
        public static System.Data.SqlTypes.SqlXml ConvertStringToXML(string xmlData)
        {
            System.Data.SqlTypes.SqlXml objData;
            try
            {
                objData = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(xmlData, System.Xml.XmlNodeType.Document, null));
            }
            catch
            {
                throw;
            }
            return objData;
        }
    }
}