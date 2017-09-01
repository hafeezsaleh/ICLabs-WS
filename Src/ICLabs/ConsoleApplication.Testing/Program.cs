//using ICLabs.ModelV1;

using ICLabs.Model;
//using ICLabs.Model;
//using ICLabs.ModelV1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Testing
{
    class Program
    {

        public static async Task<string> GetStringAsync()
        {

            using (HttpClient client = new HttpClient())
            {

                return await client
                   .GetStringAsync("https://localhost:44304/api/cars");
            }
        }

        static void Main(string[] args)
        {
            

            Repository rep = new Repository();

            //ICLabs.ModelV1.Repository repV1 = new ICLabs.ModelV1.Repository();
            ClsPostResult result = new ClsPostResult();

            FileStream fbs = new System.IO.FileStream("test.pdf", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            int filebLen = (int)fbs.Length;
            byte[] filebData = new byte[filebLen];
            fbs.Read(filebData, 0, (int)filebLen);
            fbs.Close();

            result.orderId = "XYZ000002";
            result.orderCode = "LP";
            result.clientId = "Test";
            result.resultBinaryStream = filebData;
            result.resultType = "F";
            result.resultName = "result name";
            rep.AddResult(result);

            //result.
            //rep.AddOrganization();


            //ClsGetApplication app = new ClsGetApplication();
            //app.appId = "ZOHO";
            //app.clientSecret = "z4DNiu1ILV0VJ9fccvzv+E5jJlkoSER9LcCw6H38mpA=";

            //rep.AddApplication();
            //var ob=rep.GetApplication(app);


            //var ob = rep.GetOrder();

            //ClsOrder order = new ClsOrder();
            //order.appId = "ZOHO";
            //order.clientId = "001";
            //order.orderId = "ABC0007";
            //order.clientName = "Test Client";
            //order.collectionDate = DateTime.Now.Date.ToString("yyyyMMdd");
            //order.collectionTime = DateTime.Now.ToLocalTime().ToString("hhmm");
            //order.firstName = "patient first name";
            //order.lastName = "patient last name";
            //order.orders = "BloodTest";
            //order.priority = "ASAP";
            //order.sex = "M";
            //order.dOB = "20140101";
            //string state = rep.AddOrder(order);


            //var l = rep.GetTestList();


            
            //ClsGetResultFile rf = new ClsGetResultFile();
            //rf.orderId = "test5";
            //rf.test = "test5";
            //rep.GetResultFile(rf);


            //string connStr = ConfigurationManager.ConnectionStrings["CacheDB"].ConnectionString;            
            //CacheConnection cnCache = new CacheConnection(connStr);
            //cnCache.Open();

            ////DataTable dt = new DataTable();
            ////ClsResult os = null;
            //try
            //{
            //    using (CacheCommand Cmd = new CacheCommand("select ResultBinaryStream from  Results.Results where orderId='test4' and OrderCode = 'test4'", cnCache))
            //    {
            //        //Cmd.Parameters.Add("@orderId", GetResult.orderId);
            //       // /Cmd.Parameters.Add("@orderCode", GetResult.test);
            //        CacheDataReader reader = Cmd.ExecuteReader();

            //        while (reader.Read())
            //        {
            //            var file = reader[0];
            //        }
            //    }

            //}
            //catch (Exception e)
            //{
            //}
            //finally
            //{
            //    cnCache.Close();
            //}
            ////return os;

            //rep.AddResult();


        }
    }
}
