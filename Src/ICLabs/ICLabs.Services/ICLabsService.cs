using ICLabs.Model;
using ICLabs.ModelV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICLabs.Services
{
    public class ICLabsService : IICLabsService
    {
        ICLabs.Model.Repository repository = new ICLabs.Model.Repository();

        public ClsApplication IsAuthenticated(string clientId)
        {
            ClsGetApplication getApplication = new ClsGetApplication() { appId = clientId };
            ClsApplication application = repository.GetApplication(getApplication);
            return application;
        }

        /// <summary>
        /// Inserts an new Order record
        /// </summary>
        /// <param name="applicationKey">Vendor applicationKey</param>
        /// <param name="objOrganization">Represents the instance of the POST Vendor Class</param>
        /// <returns>Status code with Code</returns>
        public string PostOrder(ClsOrder objOrder)
        {
            String msg = string.Empty;
            string status = string.Empty;
            string found = string.Empty;
            string notFound = string.Empty;

            try
            {
                repository.AddOrder(objOrder);
                repository.AddOrderStatus(objOrder);
                if (objOrder.orders == null)
                {
                    msg = "Order created without test code";
                }
                else
                {
                    string[] testCodes = objOrder.orders.Split(',');
                    IList<ClsTest> list = repository.GetTestList();

                    foreach (var code in testCodes)
                    {
                        var test = list.Where(p => p.test == code).FirstOrDefault();

                        if (test == null)
                        {
                            //msg += code + " not exist in ICLabs system. ";
                            notFound += code + ",";
                        }
                        else
                        {
                            found += code + ",";
                            repository.AddResultsStatus(objOrder, code);
                        }
                    }
                }

                if (notFound.Equals(string.Empty))
                {
                    status = "200";
                }
                else
                {
                    status = "200|" + found + "|" + notFound;
                }

            }
            catch (Exception e)
            {
                status = "400|" + e.Message;
            }

            return status;
        }

        public IList<ClsOrderStatus> GetOrderStatus(ClsGetOrderStatus GetOrderStatus)
        {
            IList<ClsOrderStatus> orderStatus = null;


            try
            {
                orderStatus = repository.GetOrderStatus(GetOrderStatus);

            }
            catch (Exception e)
            {
                throw new Exception("400|" + e.Message, e);
            }

            return orderStatus;
        }

        public IList<ClsResultsStatus> GetResultStatus(ClsGetResultStatus GetResultStatus)
        {
            IList<ClsResultsStatus> resultsStatus = null;


            try
            {
                resultsStatus = repository.GetResultStatus(GetResultStatus);

            }
            catch (Exception e)
            {
                throw new Exception("400|" + e.Message, e);
            }

            return resultsStatus;
        }

        public IList<ClsResult> GetResult(ClsGetResult GetResult)
        {
            IList<ClsResult> result = null;


            try
            {
                result = repository.GetResult(GetResult);              
                

            }
            catch (Exception e)
            {
                throw new Exception("400|" + e.Message, e);
            }

            return result;
        }

        public ClsResultFile GetResultFile(ClsGetResultFile GetResultFile)
        {
            ICLabs.ModelV1.Repository repository = new ICLabs.ModelV1.Repository();
            ClsResultFile result;
            try
            {
                result = repository.GetResultFile(GetResultFile);


            }
            catch (Exception e)
            {
                throw new Exception("400|" + e.Message, e);
            }

            return result;
        }

        public void AddLog(String AppId, String recordType, String eventData)
        {
            try
            {
                repository.AddLog(AppId, recordType, eventData);
            }
            catch (Exception e)
            {
                throw new Exception("400|" + e.Message, e);
            }
        }


        public string PostResult(ClsPostResult result)
        {
            String error=String.Empty;

            try
            {
                repository.AddResult(result);

            }
            catch (Exception e)
            {
                error = e.Message;
                //throw new Exception("400|" + e.Message, e);
            }

            return error;
        }
    }
}
