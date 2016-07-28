using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XSSInterviewAssignment
{
    class Request
    {
        public static string GetResponseFromRequest(string uri, string queryParamVal)
        {
            string responseFromServer = null;
            try
            {
                WebRequest request = WebRequest.Create(uri + queryParamVal);
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
            }

            catch (Exception ex)
            {
                string innerMessage =
                (ex.InnerException != null) ?
                ex.InnerException.Message : String.Empty;
                Console.WriteLine("{0}\n{1}", ex.Message, innerMessage);
            }

            return responseFromServer;
        }
    }
}
