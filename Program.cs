using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XSSInterviewAssignment;

namespace XSS
{
    class Program
    {

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void Main(string[] args)
        {
            try
            {
                StreamReader inputReader = new StreamReader(File.OpenRead(@"C:\\Users\\yakshdeepkaul\\Documents\\Visual Studio 2015\\Projects\\ConsoleApplication1\\ConsoleApplication1\\bin\\Debug\\test.csv"));
                StreamWriter swr = new StreamWriter("Response.csv");
                List<string> inputList = new List<string>();

                while (!inputReader.EndOfStream)
                {
                    var inputLine = inputReader.ReadLine();
                    inputList.Add(inputLine);
                }

                inputReader.Close();

                if (inputList.Count == 0)
                {
                    Console.Write("Please supply some input");
                    return;
                }

                foreach (string input in inputList)
                {
                    string randString = RandomString(50);

                    if (!Request.GetResponseFromRequest(input, randString).Contains(randString))
                    {
                        Console.WriteLine("Input being fuzzed: " + randString);
                        continue;
                    }

                    foreach (KeyValuePair<string, string> inputParameter in AttackStrings.fuzzStrings)
                    {
                        StreamWriter sw = new StreamWriter("Response" + input.Length + ".html");
                        string responseFromServer = Request.GetResponseFromRequest(input, inputParameter.Key);

                        if (responseFromServer == null)
                        {
                            Console.WriteLine("Null Response received for input query" + input + inputParameter);
                            continue;
                        }

                        sw.WriteLine(responseFromServer);
                        string first, line;
                        string second = "OK";
                        first = input;

                        if (Response.probeResponse(responseFromServer))
                        {
                            second = "Unsafe";
                            line = string.Format("{0},{1}", first, second);
                            swr.WriteLine(line);
                            Process.Start("IExplore.exe", "C:\\Users\\yakshdeepkaul\\Documents\\Visual Studio 2015\\Projects\\ConsoleApplication1\\ConsoleApplication1\\bin\\Debug\\Response" + input.Length + ".html");
                            sw.Close();
                            break;
                        }

                        line = string.Format("{0},{1}", first, second);
                        swr.WriteLine(line);
                        sw.Close();
                    }
                }
                swr.Close();
            }

            catch (Exception ex)
            {
                string innerMessage =
                (ex.InnerException != null) ?
                ex.InnerException.Message : String.Empty;
                Console.WriteLine("{0}\n{1}", ex.Message, innerMessage);
            }

        }
    }
}
