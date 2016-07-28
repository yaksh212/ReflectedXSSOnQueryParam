using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XSSInterviewAssignment
{
    class Response
    {
        public static bool probeResponse(string responseFromServer)
        {
            bool ret = false;

            try
            {
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                responseFromServer = Regex.Replace(responseFromServer, @"( |\t|\r?\n)\1+", "$1");
                htmlDoc.LoadHtml(responseFromServer);

                if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
                {
                    return ret;

                }
                else
                {
                    if (htmlDoc.DocumentNode != null)
                    {
                        foreach (var node in htmlDoc.DocumentNode.Descendants())
                        {
                            if (node.InnerText.Contains("alert('XSSVul')") && !node.HasChildNodes)
                            {
                                if (AttackStrings.checkTags.Contains(node.ParentNode.Name.ToLower()))
                                {
                                    Console.WriteLine("Found in inner text: " + node.ParentNode.Name);
                                    return true;
                                }

                            }
                            if (node.HasAttributes)
                            {
                                foreach (var attr in node.Attributes)
                                {
                                    if (attr.Value.Contains("alert('XSSVul')") && AttackStrings.checkTags.Contains(attr.Name.ToLower()))
                                    {
                                        Console.WriteLine("Found in attr: " + attr.Value);
                                        return true;
                                    }
                                }
                            }
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                string innerMessage =
                (ex.InnerException != null) ?
                ex.InnerException.Message : String.Empty;
                Console.WriteLine("{0}\n{1}", ex.Message, innerMessage);
            }

            return ret;
        }
    }
}
