//using ICLabs.Model.Proxy;
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

namespace ICLabs.Model
{
    public class Repository
    {
        CacheConnection CacheConnect = null;
        CacheCommand Cmd;
        DataTable dt = new DataTable();

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

        #region Organization
        public DataTable GetOrganization()
        {
            DataTable dt = new DataTable();
            try
            {
                using (Cmd = new CacheCommand("select * from  HICL.Organization", GetConnection()))
                {
                    dt.Load(Cmd.ExecuteReader());
                }
            }
            finally
            {
                CacheConnect.Close();
            }
            return dt;
        }

        public void AddOrganization()
        {
            try
            {
                using (Cmd = new CacheCommand("INSERT HICL.Organization VALUES('001','Dr. Prouse','CLIENT')", GetConnection()))
                {
                    Cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CacheConnect.Close();
            }
        }

        #endregion

        #region Application
        public ClsApplication GetApplication(ClsGetApplication app)
        {
            DataTable dt = new DataTable();
            ClsApplication ap = null;
            try
            {
                using (Cmd = new CacheCommand("select * from  HICL.Application where appId='" + app.appId + "'", GetConnection()))
                {
                    //Cmd.Parameters.Add("@appId", app.appId);
                    dt.Load(Cmd.ExecuteReader());

                    var query = from p in dt.AsEnumerable()
                                select new ClsApplication
                                {
                                    appId = p.Field<string>("appId"),
                                    orgId = p.Field<string>("orgId"),
                                    clientSecret = p.Field<string>("clientSecret"),
                                    accessLevel = p.Field<string>("accessLevel"),
                                    ipAddress = p.Field<string>("ipAddress"),
                                    lastAccessed = p.Field<string>("lastAccessed")
                                };

                    ap = query.FirstOrDefault();
                }

            }
            finally
            {
                CacheConnect.Close();
            }
            return ap;
        }

        public void AddApplication()
        {
            try
            {
                using (Cmd = new CacheCommand("INSERT HICL.Application VALUES('RW','TS','" + Helper.GetHash("supplier") + "', null,null,'002' )", GetConnection()))
                {
                    Cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CacheConnect.Close();
            }
        }
        #endregion

        #region Order
        public DataTable GetOrder()
        {
            DataTable dt = new DataTable();
            try
            {
                using (Cmd = new CacheCommand("select * from Orders.Login", GetConnection()))
                {
                    dt.Load(Cmd.ExecuteReader());

                }
            }
            finally
            {
                CacheConnect.Close();
            }
            return dt;
        }


        public string AddOrder(ClsOrder order)
        {
            try
            {

                //Add Log
                AddLog(order.appId, "AddOrder", "RemoteIP:" + order.remoteIP + ";" + order.ToString());

                string strQuery = "INSERT Orders.Login(OrderId,Alt1,Alt10,Alt2,Alt3,Alt4,Alt5,Alt6,Alt7,Alt9,AppId,ClientComments,ClientFileNumber,ClientId,ClientName,CollectionDate,CollectionTime,DOB,FirstName,LastName," +
                "MiddleName,Orders,PatientAddress1,PatientAddress2,PatientAddressCity,PatientAddressCountry,PatientAddressPostalCode,PatientAddressProvince,PatientCellPhone,PatientHomePhone,PatientPHN,Priority,Received,Sex, Locationid) " +
                " VALUES(@OrderId,@Alt1,@Alt10,@Alt2,@Alt3,@Alt4,@Alt5,@Alt6,@Alt7,@Alt9,@AppId,@ClientComments,@ClientFileNumber,@ClientId,@ClientName,@CollectionDate,@CollectionTime,@DOB,@FirstName,@LastName," +
                "@MiddleName,@Orders,@PatientAddress1,@PatientAddress2,@PatientAddressCity,@PatientAddressCountry,@PatientAddressPostalCode,@PatientAddressProvince,@PatientCellPhone,@PatientHomePhone,@PatientPHN,@Priority,@Received,@Sex, @Locationid)";

                using (Cmd = new CacheCommand(strQuery, GetConnection()))
                {
                    Cmd.Parameters.Add("@OrderId", order.orderId);
                    Cmd.Parameters.Add("@Alt1", order.alt1);
                    Cmd.Parameters.Add("@Alt10", order.alt10);
                    Cmd.Parameters.Add("@Alt2", order.alt2);
                    Cmd.Parameters.Add("@Alt3", order.alt3);
                    Cmd.Parameters.Add("@Alt4", order.alt4);
                    Cmd.Parameters.Add("@Alt5", order.alt5);
                    Cmd.Parameters.Add("@Alt6", order.alt6);
                    Cmd.Parameters.Add("@Alt7", order.alt7);
                    Cmd.Parameters.Add("@Alt9", order.alt9);
                    Cmd.Parameters.Add("@AppId", order.appId);
                    Cmd.Parameters.Add("@ClientComments", order.clientComments);
                    Cmd.Parameters.Add("@ClientFileNumber", order.clientFileNumber);
                    Cmd.Parameters.Add("@ClientId", order.clientId);
                    Cmd.Parameters.Add("@ClientName", order.clientName);
                    Cmd.Parameters.Add("@CollectionDate", order.collectionDate);
                    Cmd.Parameters.Add("@CollectionTime", order.collectionTime);
                    Cmd.Parameters.Add("@DOB", order.dOB);
                    Cmd.Parameters.Add("@FirstName", order.firstName);
                    Cmd.Parameters.Add("@LastName", order.lastName);
                    Cmd.Parameters.Add("@MiddleName", order.middleName);
                    Cmd.Parameters.Add("@Orders", order.orders);
                    Cmd.Parameters.Add("@PatientAddress1", order.patientAddress1);
                    Cmd.Parameters.Add("@PatientAddress2", order.patientAddress2);
                    Cmd.Parameters.Add("@PatientAddressCity", order.patientAddressCity);
                    Cmd.Parameters.Add("@PatientAddressCountry", order.patientAddressCountry);
                    Cmd.Parameters.Add("@PatientAddressPostalCode", order.patientAddressPostalCode);
                    Cmd.Parameters.Add("@PatientAddressProvince", order.patientAddressProvince);
                    Cmd.Parameters.Add("@PatientCellPhone", order.patientCellPhone);
                    Cmd.Parameters.Add("@PatientHomePhone", order.patientHomePhone);
                    Cmd.Parameters.Add("@PatientPHN", order.patientPHN);
                    Cmd.Parameters.Add("@Priority", order.priority);
                    Cmd.Parameters.Add("@Received", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                    Cmd.Parameters.Add("@Sex", order.sex);
                    Cmd.Parameters.Add("@Locationid", order.locationId);
                    Cmd.ExecuteNonQuery();
                }

             }
            catch(Exception e)
            {
                if (e.Message.Contains("OrderIdIndex"))
                {
                    throw new Exception("Order with orderId: " + order.orderId + " exists in the system.");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                CacheConnect.Close();
            }
            return "";
        }

        #endregion

        public string AddOrderStatus(ClsOrder order)
        {
            try
            {
                string strQuery = "INSERT Orders.OrderStatus(orderId,accessioned,demographicChange,ordersModified,partialResults,referredtosupplier,resultsfinal, locationId) " +
                " VALUES(@orderId,@accessioned ,@demographicChange,@ordersModified,@partialResults,@referredtosupplier,@resultsfinal, @locationId)";

                using (Cmd = new CacheCommand(strQuery, GetConnection()))
                {
                    Cmd.Parameters.Add("@OrderId", order.orderId);
                    Cmd.Parameters.Add("@accessioned", "N");
                    Cmd.Parameters.Add("@demographicChange", "N");
                    Cmd.Parameters.Add("@ordersModified", "N");
                    Cmd.Parameters.Add("@partialResults", "N");
                    Cmd.Parameters.Add("@referredtosupplier", "N");
                    Cmd.Parameters.Add("@resultsfinal", "N");
                    Cmd.Parameters.Add("@locationId", order.locationId);

                    Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception("ERROR: AddOrderStatus() " + e.Message);
            }
            finally
            {
               CacheConnect.Close();
            }
            return "";
        }


        public string AddResultsStatus(ClsOrder order, string test)
        {
            try
            {
                string strQuery = "INSERT Orders.ResultsStatus(orderId,resultsStatus,test, locationId) " +
                " VALUES(@orderId,@resultsStatus ,@test, @locationId)";

                using (Cmd = new CacheCommand(strQuery, GetConnection()))
                {
                    Cmd.Parameters.Add("@OrderId", order.orderId);
                    Cmd.Parameters.Add("@resultsStatus", "Pending");
                    Cmd.Parameters.Add("@test", test);
                    Cmd.Parameters.Add("@locationId", order.locationId);

                    Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception("ERROR: AddResultsStatus() " + e.Message);
            }
            finally
            {
               CacheConnect.Close();
            }
            return "";
        }


        #region Test
        public IList<ClsTest> GetTestList()
        {
            DataTable dt = new DataTable();
            IList<ClsTest> list = null;
            try
            {
                using (Cmd = new CacheCommand("select * from  Orders.Tests", GetConnection()))
                {
                    dt.Load(Cmd.ExecuteReader());

                    var query = from p in dt.AsEnumerable()
                                select new ClsTest
                                {
                                    test = p.Field<string>("ordercode"),
                                    testName = p.Field<string>("testName"),
                                    testPrice = p.Field<object>("testPrice")
                                };

                    list = query.ToList();
                }
            }
            finally
            {
               CacheConnect.Close();
            }
            return list;
        }

        public void AddTest()
        {
            try
            {
                Cmd = new CacheCommand("INSERT Orders.Tests VALUES('FSH','FSH','500')", GetConnection());
                Cmd.ExecuteNonQuery();

                Cmd = new CacheCommand("INSERT Orders.Tests VALUES('TSH','TSH','500')", GetConnection());
                Cmd.ExecuteNonQuery();

                Cmd = new CacheCommand("INSERT Orders.Tests VALUES('LH','LH','500')", GetConnection());
                Cmd.ExecuteNonQuery();

                Cmd = new CacheCommand("INSERT Orders.Tests VALUES('VAL','VAL','500')", GetConnection());
                Cmd.ExecuteNonQuery();
            }
            finally
            {
               CacheConnect.Close();
            }
        }
        #endregion


        public IList<ClsOrderStatus> GetOrderStatus(ClsGetOrderStatus OrderStatus)
        {
            DataTable dt = new DataTable();
            IList<ClsOrderStatus> os = null;
            try
            {
                //Add Log
                AddLog(OrderStatus.appId, "GetOrderStatus", "RemoteIP:" + OrderStatus.remoteIP + ";OrderId:" + OrderStatus.orderId);

                //using (Cmd = new CacheCommand("select * from  Orders.OrderStatus where orderId='" + OrderStatus.orderId + "'", GetConnection()))
                using (Cmd = new CacheCommand("select * from  Orders.OrderStatus where orderId=@orderId and locationId=@locationId", GetConnection()))
                {
                    Cmd.Parameters.Add("@orderId", OrderStatus.orderId);
                    Cmd.Parameters.Add("@locationid", OrderStatus.locationId);
                    dt.Load(Cmd.ExecuteReader());

                    var query = from p in dt.AsEnumerable()
                                select new ClsOrderStatus
                                {
                                    orderId = p.Field<string>("OrderId"),
                                    locationId = p.Field<string>("LocationId"),
                                    accessioned = p.Field<string>("Accessioned"),
                                    demographicChange = p.Field<string>("DemographicChange"),
                                    ordersModified = p.Field<string>("OrdersModified"),
                                    partialResults = p.Field<string>("Partialresults"),
                                    referredToSupplier = p.Field<string>("ReferredToSupplier"),
                                    resultsFinal = p.Field<string>("ResultsFinal")
                                };

                    os = query.ToList<ClsOrderStatus>();
                }
               
            }
            finally
            {
               CacheConnect.Close();
            }
            return os;
        }

        public IList<ClsResultsStatus> GetResultStatus(ClsGetResultStatus GetResultStatus)
        {
            
            IList<ClsResultsStatus> os = null;
            try
            {
                //Add Log
                AddLog(GetResultStatus.appId, "GetResultStatus", "RemoteIP:" + GetResultStatus.remoteIP + ";OrderId:" + GetResultStatus.orderId + ";Test:" + GetResultStatus.test);

                if (GetResultStatus.test == null)
                {
                    Cmd = new CacheCommand("select * from  Orders.ResultsStatus where orderId=@orderId and locationId=@locationId", GetConnection());
                    Cmd.Parameters.Add("@orderId", GetResultStatus.orderId);
                    Cmd.Parameters.Add("@locationid", GetResultStatus.locationId);
                }
                else
                {
                    Cmd = new CacheCommand("select * from  Orders.ResultsStatus where orderId= @orderId and test = @test and locationId=@locationId", GetConnection());
                    Cmd.Parameters.Add("@orderId", GetResultStatus.orderId);
                    Cmd.Parameters.Add("@test", GetResultStatus.test);
                    Cmd.Parameters.Add("@locationid", GetResultStatus.locationId);
                }

                dt.Load(Cmd.ExecuteReader());

                var query = from p in dt.AsEnumerable()
                            select new ClsResultsStatus
                            {
                                orderId = p.Field<string>("OrderId"),
                                resultsStatus = p.Field<string>("ResultsStatus"),
                                test = p.Field<string>("Test")
                            };

                os = query.ToList<ClsResultsStatus>();

            }
            finally
            {
                CacheConnect.Close();
            }
            return os;
        }


        public void AddResult(ClsPostResult result)
        {
            try
            {
                //Add Log
                AddLog(result.appId, "AddOrder", "RemoteIP:" + result.remoteIP + ";" + result.orderId);

                //Check if result exist
                Cmd = new CacheCommand("DELETE  from  Results.Results where orderId= '" + result.orderId + "' and OrderCode = '" + result.orderCode + "'", GetConnection());
                //dt.Load(Cmd.ExecuteReader());
                Cmd.ExecuteNonQuery();

                //if (dt.Rows.Count == 0)
                //{

                    string strQuery = "INSERT Results.Results(FileName,Client,OrderCode,OrderId,ResultBinaryStream,ResultType,ResultName,ResultDate,Flag , NormalRange ,Units ,ResultText , locationId  )" +
                    " VALUES(@FileName,@Client,@OrderCode,@OrderId,@ResultBinaryStream ,@ResultType,@ResultName,@ResultDate , @Flag , @NormalRange ,@Units , @ResultText , @locationId )";
                    Cmd = new CacheCommand(strQuery, GetConnection());
                //}
                //else
                //{
                //    string strQuery = "UPDATE Results.Results SET FileName = @FileName, Client=@Client ,ResultBinaryStream = @ResultBinaryStream, ResultType=@ResultType,ResultName=@ResultName,ResultDate=@ResultDate,Flag=@Flag , NormalRange=@NormalRange , Units=@Units, ResultText=@ResultText " +
                //   " Where OrderId = @OrderId and OrderCode=@OrderCode";

                //    Cmd = new CacheCommand(strQuery, GetConnection());

                //}

                Cmd.Parameters.Add("@locationId", result.locationId);

                Cmd.Parameters.Add("@Flag", result.resultFlag);
                Cmd.Parameters.Add("@NormalRange", result.normalRange);
                Cmd.Parameters.Add("@Units", result.resultUnits);

                Cmd.Parameters.Add("@Client", result.clientId);
                Cmd.Parameters.Add("@OrderCode", result.orderCode);
                Cmd.Parameters.Add("@OrderId", result.orderId);
                Cmd.Parameters.Add("@ResultType", result.resultType);
                Cmd.Parameters.Add("@ResultName", result.resultName);
                Cmd.Parameters.Add("@ResultDate", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                Cmd.Parameters.Add("@ResultText", result.resultText);


                // Now insert two files from disk as streams
                // Open binary file and read into byte[]

                Cmd.Parameters.Add("@FileName", result.fileName);   

                //FileStream fbs = new System.IO.FileStream("test.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                //int filebLen = (int)fbs.Length;
                //byte[] filebData = new byte[file
                //fbs.Read(filebData, 0, (int)filebLen);
                //fbs.Close();


                //Cmd.Parameters.Add()
                CacheParameter para = new CacheParameter("@ResultBinaryStream", result.resultBinaryStream);
                para.Size = result.resultBinaryStream.Length;
                Cmd.Parameters.Add(para);

                Cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                if (e.Message.Contains("ClientIndex"))
                {
                    throw new Exception("Result for order Id: " + result.orderId + " and order code:" + result.orderCode + " exists in the system.");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                CacheConnect.Close();
            }
        }

        public IList<ClsResult> GetResult(ClsGetResult GetResult)
        {
            DataTable dt = new DataTable();
            IList<ClsResult> os = null;
            String str;

            try
            {
                //Add Log
                AddLog(GetResult.appId, "GetResult", "RemoteIP:" + GetResult.remoteIP + ";OrderId:" + GetResult.orderId);

                if (GetResult.test == null)
                {
                    str = "select clientId,resultName,resultDate,resultType,resultText,flag,units,normalRange,OrderCode , OrderId, resCode, supplierSite    from  Results.Results where orderId=@orderId and locationid=@locationid";                   
                }
                else
                {
                    str = "select clientId,resultName,resultDate,resultType,resultText,flag,units,normalRange,OrderCode , OrderId, resCode, supplierSite    from  Results.Results where orderId=@orderId and OrderCode = @test and locationid=@locationid";                   
                }

                using (Cmd = new CacheCommand(str, GetConnection()))
                {

                    if (GetResult.test == null)
                    {                       
                        Cmd.Parameters.Add("@orderId", GetResult.orderId);
                        Cmd.Parameters.Add("@locationid", GetResult.locationId);
                    }
                    else
                    {                        
                       Cmd.Parameters.Add("@orderId", GetResult.orderId);
                       Cmd.Parameters.Add("@test", GetResult.test);
                       Cmd.Parameters.Add("@locationid", GetResult.locationId);
                    }

               
                    //Cmd.Parameters.Add("@orderId", GetResult.orderId);
                    //Cmd.Parameters.Add("@orderCode", GetResult.test);
                    //dt.Load(Cmd.ExecuteReader());
                    CacheDataAdapter da = new CacheDataAdapter(Cmd);
                    da.Fill(dt);

                    var query = from p in dt.AsEnumerable()
                                select new ClsResult
                                {
                                    clientId = p.Field<string>("clientId"),
                                    resultName = p.Field<string>("ResultName"),
                                    resultDate = p.Field<string>("ResultDate"),
                                    resultType = p.Field<string>("ResultType"),
                                    resultText = p.Field<string>("ResultText"),
                                    resultFlag = p.Field<string>("Flag"),
                                    resultUnits = p.Field<string>("Units"),
                                    normalRange = p.Field<string>("NormalRange"),
                                    orderCode = p.Field<string>("OrderCode"),
                                    orderId = p.Field<string>("OrderId"),
                                    resCode = p.Field<string>("resCode"),
                                    supplierSite = p.Field<string>("supplierSite")
                                };

                   // os = query.FirstOrDefault();
                    os = query.ToList();
                }

            }            
            finally
            {
                CacheConnect.Close();
            }
            return os;
        }

        public void AddLog(String AppId, String recordType, String eventData)
        {
            try
            {
                string strQuery = "INSERT HICL.AuditLog(AppId,actDate,eventData,recordType ) VALUES(@AppId,@actDate,@eventData,@recordType )";
                Cmd = new CacheCommand(strQuery, GetConnection());
                Cmd.Parameters.Add("@AppId", AppId);
                Cmd.Parameters.Add("@actDate", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                Cmd.Parameters.Add("@eventData", eventData);
                Cmd.Parameters.Add("@recordType", recordType);
                Cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {}
            finally
            {
                CacheConnect.Close();
            }
        }



       

    }
}
