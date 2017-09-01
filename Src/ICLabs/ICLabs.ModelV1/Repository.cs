using ICLabs.ModelV1;
using InterSystems.Data.CacheClient;
using InterSystems.Data.CacheTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ICLabs.ModelV1
{
    public class Repository
    {
        CacheConnection CacheConnect = null;
        CacheCommand Cmd;


        CacheConnection GetConnection(bool firstCall = true)
        {

            try
            {
                if (CacheConnect == null)
                {
                    string connStr = ConfigurationManager.ConnectionStrings["CacheDB"].ConnectionString;
                    CacheConnect = new CacheConnection();
                    CacheConnect.ConnectionString = connStr;
                }

                if (CacheConnect.State == System.Data.ConnectionState.Closed)
                {
                    CacheConnect.Open();
                }

            }
            catch(CacheInternalException e)
            {
                if (firstCall)
                {
                    Thread.Sleep(30);
                    GetConnection(false);
                }

            }
           

            return CacheConnect;
        }

        //public void AddResult()
        //{
        //    try
        //    {
        //        string strQuery = "INSERT Results.Results(Client,OrderCode,OrderId,ResultBinaryStream,ResultType,ResultName,ResultDate   ) VALUES(@Client,@OrderCode,@OrderId,@ResultBinaryStream  ,@ResultType,@ResultName,@ResultDate  )";
        //        Cmd = new CacheCommand(strQuery, GetConnection());
        //        Cmd.Parameters.Add("@Client", "test5");
        //        Cmd.Parameters.Add("@OrderCode", "test5");
        //        Cmd.Parameters.Add("@OrderId", "test5");
        //        //Cmd.Parameters.Add("@ResultBinaryFile", "N");
        //        Cmd.Parameters.Add("@ResultType", "F");
        //        Cmd.Parameters.Add("@ResultName", "test5");
        //        Cmd.Parameters.Add("@ResultDate", DateTime.Now.ToString("yyyyMMdd"));
        //        // Now insert two files from disk as streams
        //        // Open binary file and read into byte[]
        //        FileStream fbs = new System.IO.FileStream("test.pdf", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
        //        int filebLen = (int)fbs.Length;
        //        byte[] filebData = new byte[filebLen];
        //        fbs.Read(filebData, 0, (int)filebLen);
        //        fbs.Close();
        //        //Cmd.Parameters.Add()
        //        CacheParameter para = new CacheParameter("@ResultBinaryStream", filebData);
        //        para.Size = filebLen;
        //        Cmd.Parameters.Add(para);

        //        Cmd.ExecuteNonQuery();
        //    }
        //    catch(Exception e)
        //    { 
            
        //    }
        //    finally
        //    {
        //        CacheConnect.Close();
        //    }
        //}

        public ClsResultFile GetResultFile(ClsGetResultFile GetResult)
        {
            DataTable dt = new DataTable();
            //byte[] file = null;
            ClsResultFile result=new ClsResultFile();

            try
            {
                //Add Log
                //AddLog(GetResult.appId, "GetResultFile", "RemoteIP" + GetResult.remoteIP + " OrderId " + GetResult.orderId + " OrderCode " + GetResult.test);
                AddLog(GetResult.appId, "GetResultFile", " OrderId " + GetResult.orderId + " OrderCode " + GetResult.test);
                using (Cmd = new CacheCommand("select ResultBinaryStream,FileName from  Results.Results where orderId='" + GetResult.orderId + "' and OrderCode = '" +
                    GetResult.test + "' and locationId= '" + GetResult.locationId + "' and ResultName='" + GetResult.resultName + "'", GetConnection()))
                {
                    //Cmd.Parameters.Add("@orderId", GetResult.orderId);
                    //Cmd.Parameters.Add("@orderCode", GetResult.test);
                    CacheDataReader reader= Cmd.ExecuteReader();

                   while(reader.Read())
                   {
                       result.resultBinaryStream =  reader[0] as byte[];
                       result.fileName = reader[1] as string;
                   }
                }

            }           
            finally
            {
                CacheConnect.Close();
            }
            return result;
        }

        public void AddLog(String AppId, String recordType, String eventData)
        {
            try
            {
                string strQuery = "INSERT HICL.AuditLog(AppId,actDate,eventData,recordType ) VALUES('" + AppId + "','" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "','" + eventData + "','" + recordType + "')";
                Cmd = new CacheCommand(strQuery, GetConnection());
                //Cmd.Parameters.Add("@AppId", AppId);
                //Cmd.Parameters.Add("@actDate", );
                //Cmd.Parameters.Add("@eventData", eventData);
                //Cmd.Parameters.Add("@recordType", recordType);
                Cmd.ExecuteNonQuery();
            }
            finally
            {
                CacheConnect.Close();
            }
        }
    }
}
