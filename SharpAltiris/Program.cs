using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using System.Text.RegularExpressions;
using System.Net.Http;

namespace SharpAltiris
{
    class Program
    {

        public static void GetComputerGuidByName(string notificationServer, string computerName, string username = null, string password = null)
        {
            string webserviceUrl = "/altiris/ASDK.NS/ItemManagementService.asmx/GetItemsByName?itemName=" + computerName;
            string finalUrl = notificationServer + webserviceUrl;

            string requestOutput = HttpRequest(finalUrl, null, username, password);
            Regex re = new Regex("<guid>[a-zA-Z0-9\\-]*</guid>", RegexOptions.IgnoreCase);
            string match = re.Match(requestOutput,0).Value;
            Console.WriteLine("Name: " + computerName);
            Console.WriteLine(match);

        }

        //TO DO - Find a way of getting this info
        public static void FindUserComputer(string notificationServer, string targetUser, string username = null, string password = null)
        {
            string webserviceUrl = "/altiris/ASDK.NS/ResourceManagementService.asmx/GetResourceByContext?parameters=type:computer, %" + targetUser + "%";
            string finalUrl = notificationServer + webserviceUrl;
            string requestOutput = HttpRequest(finalUrl, null, username, password);
            Console.WriteLine(requestOutput);
        }

        public static void GetItemDetailsByGuid(string notificationServer, string guid, string username = null, string password = null)
        {
            string webserviceUrl = "/altiris/ASDK.NS/ItemManagementService.asmx/GetItemByGuid?itemGuid=" + guid;
            string finalUrl = notificationServer + webserviceUrl;
            string requestOutput = HttpRequest(finalUrl, null, username, password);
            Console.WriteLine(requestOutput);
        }

        public static void GetScriptTasks(string notificationServer, string username = null, string password = null)
        {
            string webserviceUrl = "/altiris/ASDK.NS/ItemManagementService.asmx/GetItemsByType?typeName=ScriptTask";
            string finalUrl = notificationServer + webserviceUrl;

            string requestOutput = HttpRequest(finalUrl, null, username, password);
            Console.WriteLine(requestOutput);
        }

        public static void GetScriptTaskXml(string notificationServer, string taskGuid, string username = null, string password = null)
        {
            string webserviceUrl = "/altiris/ASDK.NS/ItemManagementService.asmx/ExportItemXmlString?itemGuid=" + taskGuid;
            string finalUrl = notificationServer + webserviceUrl;

            string requestOutput = HttpRequest(finalUrl,  null, username, password);
            Console.WriteLine(requestOutput);
        }

        public static void ImportScriptXmlString(string notificationServer, string xmlTask, string username = null, string password = null)
        {
            string webserviceUrl = "/altiris/ASDK.NS/ItemManagementService.asmx/ImportItemXmlString";
            string finalUrl = notificationServer + webserviceUrl;
            string postData = "xml=" + xmlTask;
            string requestOutput = HttpRequest(finalUrl, postData, username, password);
            Console.WriteLine(requestOutput);
            Console.WriteLine("[+] Task imported successfully");

        }

        public static void ScheduleTask(string notificationServer, string taskGuid, string computerGuids, string executionName, string username = null, string password = null)
        {
            string webserviceUrl = "/altiris/ASDK.Task/TaskManagementService.asmx/ExecuteTask";
            string finalUrl = notificationServer + webserviceUrl;
            string scheduleXml = "<inputParameters><parameter><name>@AssignedResources</name><value>" + computerGuids + "</value></parameter><parameter><name>@CompRequirements</name><value><minWaitTime>2 minutes</minWaitTime><maxWaitTime>60 minutes</maxWaitTime><minCompletion>100 %</minCompletion></value></parameter></inputParameters>";
            string postData = "taskGuid=" + taskGuid + "&executionName=" + executionName + "&inputParameters=" + scheduleXml;

            string requestOutput = HttpRequest(finalUrl, postData, username, password);
            Console.WriteLine(requestOutput);
            Console.WriteLine("[+] Task Executed successfully");
        }

        public static void GetTaskStatus(string notificationServer, string taskExecutionGuid, string username = null, string password = null)
        {
            string webserviceUrl = "/altiris/ASDK.Task/TaskManagementService.asmx/GetTaskStatus?taskGuid=" + taskExecutionGuid;
            string finalUrl = notificationServer + webserviceUrl;
           
            string requestOutput = HttpRequest(finalUrl, null, username, password);
            Console.WriteLine(requestOutput);
        }

        public static void GetTaskOutput(string notificationServer, string taskExecutionGuid, string computerGuid, string username = null, string password = null)
        {
            string webserviceUrl = "/altiris/ASDK.Task/TaskManagementService.asmx/GetTaskResourceStatus?taskInstanceGuid=" + taskExecutionGuid + "&resourceGuid=" + computerGuid;
            string finalUrl = notificationServer + webserviceUrl;

            string requestOutput = HttpRequest(finalUrl, null, username, password);
            Console.WriteLine(requestOutput);
        }

        static void Main(string[] args)
        {
            string func = args[0].ToLower();
            int argsLen = args.Length - 1;

            switch(func)
            {
                case "findusercomputer":
                    if (argsLen == 2)
                        FindUserComputer(args[1], args[2]);
                    else 
                        FindUserComputer(args[1], args[2], args[3], args[4]);
                    break;
                case "getitemdetailsbyguid":
                    if (argsLen == 2)
                        GetItemDetailsByGuid(args[1], args[2]);
                    else
                        GetItemDetailsByGuid(args[1], args[2], args[3], args[4]);
                    break;
                case "getcomputerguidbyname":
                    if (argsLen == 2)
                        GetComputerGuidByName(args[1], args[2]);
                    else
                        GetComputerGuidByName(args[1], args[2], args[3], args[4]);
                    break;
                case "getscripttasks":
                    if (argsLen == 1)
                        GetScriptTasks(args[1]);
                    else
                        GetScriptTasks(args[1], args[2], args[3]);
                    break;
                case "getscripttaskxml":
                    if (argsLen == 2)
                        GetScriptTaskXml(args[1], args[2]);
                    else
                        GetScriptTaskXml(args[1], args[2], args[3], args[4]);
                    break;
                case "importscriptxmlstring":
                    if(argsLen == 2)
                        ImportScriptXmlString(args[1], args[2]);
                    else
                        ImportScriptXmlString(args[1], args[2], args[3], args[4]);
                    break;
                case "scheduletask":
                    if (argsLen == 4)
                        ScheduleTask(args[1], args[2], args[3], args[4]);
                    else
                        ScheduleTask(args[1], args[2], args[3], args[4], args[5], args[6]);
                    break;
                case "gettaskstatus":
                    if (argsLen == 2)
                        GetTaskStatus(args[1], args[2]);
                    else
                        GetTaskStatus(args[1], args[2], args[3], args[4]);
                    break;
                case "gettaskoutput":
                    if (argsLen == 3)
                        GetTaskOutput(args[1], args[2], args[3]);
                    else
                        GetTaskOutput(args[1], args[2], args[3], args[4], args[5]);
                    break;
                default:
                    Console.WriteLine("[x] Unknown Function");
                    break;
            }
        }


        private static string HttpRequest(string url, string data = null, string user = null, string password = null)
        {
            // Configure connection
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertificates);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            
            foreach (var protocol in Enum.GetValues(typeof(SecurityProtocolType)))
            {
                ServicePointManager.SecurityProtocol |= (SecurityProtocolType)protocol;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response;
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";

            if (user != null)
                request.Credentials = new NetworkCredential(user, password);
            else
                request.Credentials = CredentialCache.DefaultCredentials;
            
           
            try
            {
                if (data != null)
                {
                    var body = Encoding.ASCII.GetBytes(data);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = body.Length;

                    using (Stream dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(body, 0, body.Length);
                    }
                    
                    
                    response = (HttpWebResponse)request.GetResponse();
                }
                else
                {
                    response = (HttpWebResponse)request.GetResponse();

                }
            }
            catch (WebException e)
            {
                response = e.Response as HttpWebResponse;
                if (response == null)
                {
                    Console.Error.WriteLine(e.Message);
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return null;
            }

           
            using (Stream dataStream = response.GetResponseStream())
            {     
                StreamReader reader = new StreamReader(dataStream);
                Console.WriteLine();
                string output = reader.ReadToEnd();
                return output;
            }
            
        }

        private static void IgnoreBadCertificates()
        {

        }

        private static bool AcceptAllCertificates(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
