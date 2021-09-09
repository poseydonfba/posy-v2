using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Posy.V2.Infra.CrossCutting.Common.Conversions
{
    public class Conversion
    {
        public static byte[] StrToByteArray(string str)
        {
            //if (str == null) return null;

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            Encoding utf16 = Encoding.GetEncoding("utf-16");

            //byte[] utfBytes = utf8.GetBytes(str);
            //byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);

            byte[] utfBytes16 = utf16.GetBytes(str);
            byte[] isoBytes = Encoding.Convert(utf16, iso, utfBytes16);

            string msg = iso.GetString(isoBytes);
            string msg2 = iso.GetString(utfBytes16);
            string msg3 = utf16.GetString(utfBytes16);

            return utfBytes16;
            //return isoBytes;
        }
        public static string ByteArrayToStr(byte[] byteArr)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            Encoding utf16 = Encoding.GetEncoding("utf-16");

            string msg1 = iso.GetString(byteArr);
            string msg2 = utf8.GetString(byteArr);
            string msg3 = utf16.GetString(byteArr);

            return utf16.GetString(byteArr);
            //return iso.GetString(byteArr);
        }

        public static void DescobrirEncoding()
        {
            string exemplo = "";// "aáeêi!oôuú:!@#$¨%";
            byte[] arrBytes = Encoding.Unicode.GetBytes(exemplo);

            System.IO.StreamWriter writer = new System.IO.StreamWriter(@"D:\encoding.txt", false);
            foreach (EncodingInfo e in Encoding.GetEncodings().OrderBy(c => c.Name))
            {
                writer.WriteLine("{0} - {1}", e.Name, Encoding.GetEncoding(e.Name).GetString(arrBytes));
            }
            writer.Close();
        }

        public static string ImageToBase64(string pathImage)
        {
            byte[] imageArray = File.ReadAllBytes(pathImage);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return base64ImageRepresentation;
        }
    }
}
