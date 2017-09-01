using ICLabs.Model;
using ICLabs.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;


namespace ICLabs.WS.Controllers
{
    [RoutePrefix("api/Order")]
    public class OrderController : BaseApiController
    {

        IICLabsService iICLabsService = new ICLabsService();

        /// <summary>
        /// HttpResponseMessage,to create and return HTTP responses like 200(OK),401(Unauthorized),400(Bad Request),408(RequestTimeout).
        /// </summary>
        private HttpResponseMessage response = new HttpResponseMessage();

        #region Order Methods
        /// <summary>
        /// Create a new Order data.
        /// </summary>        
        /// <param name="Order">Represents the instance of the POST Order Class</param>
        /// <returns>HTTP 200 OK with Order items / HTTP 400 with errors / Other HTTP responses</returns>
        [Authorize]
        [Route("")]
        public HttpResponseMessage Post(ClsOrder order)
        {
            string errMsg = string.Empty, errData = string.Empty;
            var errorsList = new JObject();
            var  message = new JObject();
            JArray errors = new JArray();
            string result = string.Empty;
            JArray messageList = new JArray();
            JArray infoList = new JArray();

            /// <summary>
            /// HttpResponseMessa
            try
            {
                ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
                var appId = principal.Claims.Where(c => c.Type == "clientId").Single().Value;
                var orgId = principal.Claims.Where(c => c.Type == "orgId").Single().Value;
                if (order == null)
                {
                    errorsList["Error"] = "Data required";
                    errors.Add(errorsList);
                    this.response = this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors, "info"));
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        order.appId = appId;
                        order.remoteIP = Request.GetClientIpAddress();
                        result = this.iICLabsService.PostOrder(order);

                        if (result == "200")
                        {
                            //message["Message"] = "Order created for Order Id=" + order.orderId + " locationId=" + order.locationId + " and tests=" + order.orders;

                            messageList.Add(GetJsonObject("OrderId", order.orderId));
                            messageList.Add(GetJsonObject("locationId", order.locationId));
                            messageList.Add(GetJsonObject("tests", order.orders));
                            this.response = this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("ok", messageList));
                        }
                        else if (result.Contains("200|"))
                        {
                            string[] splitValues = result.Split('|');
                            //errorsList[splitValues[0].ToString()] = splitValues[1].ToString(); 
                            //errors.Add(errorsList);
                            messageList.Add(GetJsonObject("OrderId", order.orderId));
                            messageList.Add(GetJsonObject("locationId", order.locationId));
                            messageList.Add(GetJsonObject("tests", splitValues[1]));

                            string[] splitValuesWarning = splitValues[2].Split(',');
                             for (int i = 0; i < splitValuesWarning.Count(); i++)
                                {

                                    if (!splitValuesWarning[i].Equals(String.Empty))
                                    {
                                        infoList.Add(GetJsonObject("Warning : Test not found", splitValuesWarning[i]));
                                    }                                   
                                }

                             this.response = this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("partial", messageList, infoList));
                        }
                        else if (result.Contains("400|"))
                        {
                            //string[] splitValues = result.Split('|');
                            //errorsList[splitValues[0].ToString()] = splitValues[1].ToString();
                            errors.Add(GetJsonObject("Error : Order exists", order.orderId));

                            this.response = this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors,"info"));
                        }
                    }
                    else if (!ModelState.IsValid)
                    {
                        foreach (var state in this.ModelState)
                        {
                            foreach (var error in state.Value.Errors)
                            {
                                errMsg = error.ErrorMessage;
                                errData = state.Key.ToString();
                                if (errMsg == string.Empty || errMsg == null)
                                {
                                    errMsg = "Incorrect JSON format !";
                                    errData = "OrderData";
                                }

                                result = errMsg + "|" + errData.Replace("Order+.", string.Empty).Replace(".String", string.Empty);
                                string[] splitValues = result.Split('|');
                                for (int i = 0; i < splitValues.Count(); i++)
                                {
                                    errorsList[splitValues[i].ToString()] = splitValues[i + 1].ToString();
                                    i++;
                                    errors.Add(errorsList);
                                    errorsList = new JObject();
                                }
                            }
                        }

                        this.response = this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors, "info"));
                    }
                }
            }
            catch(Exception e)
            {
                errorsList["Error"] = e.Message;
                errors.Add(errorsList);
                this.response = this.Request.CreateResponse(HttpStatusCode.BadRequest, GetResponseBody("error", errors, "info"));
            }

            return this.response;
        }


        #endregion

        #region Helpers

        /// <summary>
        /// /// Checks for HTTP status
        /// </summary>
        /// <param name="status">Contains HTTP status code</param>
        /// <returns>HTTP response</returns>
        public HttpResponseMessage CheckHTTPStatus(string status)
        {
            if (status == "401")
            {
                this.response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else if (status == "400")
            {
                this.response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else if (status == "408")
            {
                this.response = Request.CreateResponse(HttpStatusCode.RequestTimeout);
            }
            else if (status == "200")
            {
                this.response = Request.CreateResponse(HttpStatusCode.OK);
            }
            return this.response;
        }


        #endregion

    }


   
}
