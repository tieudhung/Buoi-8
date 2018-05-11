using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LTM_B8__02
{
    class Program
    {
        private class IgnoreBadCert : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint sp,
           X509Certificate cert, WebRequest request, int err)
            {
                return true;
            }
        }

        static string username = "huy14194";
        static string pass = "huy123456789";
        static void Main(string[] args)
        {
            ServicePointManager.CertificatePolicy = new
IgnoreBadCert();
            NetworkCredential cred = new NetworkCredential();
            cred.UserName = username;
            cred.Password = pass;
            WebRequest webr =
           WebRequest.Create("https://mail.google.com/mail/feed/atom");
            webr.Credentials = cred;
            Stream stream = webr.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string s = sr.ReadToEnd();
            s = s.Replace("<feed version=\"0.3\"xmlns =\"http://purl.org/atom/ns#\">", @"<feed>");
            StreamWriter sw = new StreamWriter("emaildata.txt");
            sw.Write(s);
            sr.Close();
            sw.Close();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load("emaildata.txt");
            string nr =
           xmldoc.SelectSingleNode(@"/feed/fullcount").InnerText;
            Console.WriteLine("Count: {0}", nr);
            foreach (XmlNode node in xmldoc.SelectNodes(@"/feed/entry"))
            {
                string title = node.SelectSingleNode("title").InnerText;
                string summary =
               node.SelectSingleNode("summary").InnerText;
                Console.WriteLine("{0} \n{1}\n", title, summary);
            }
        }
    }
}
