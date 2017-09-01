using ICLabs.Model;
using ICLabs.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Web.Http;

namespace ICLabs.WS.Controllers
{
    [RoutePrefix("api/OrderStatus")]
    public class OrderStatusController : BaseApiController
    {

        IICLabsService iICLabsService = new ICLabsService();

        /// <summary>
        /// HttpResponseMessage,to create and return HTTP responses like 200(OK),401(Unauthorized),400(Bad Request),408(RequestTimeout).
        /// </summary>
        private HttpResponseMessage response = new HttpResponseMessage();


        /// <summary>
        /// Retrieves OrderStatus record
        /// </summary>        
        /// <param name="OrderStatus">Represents the instance of the GET OrderStatus class</param>
        /// <returns>HTTP response 200 with OrderStatus record</returns>
        /// 
        [Authorize]
        [Route("")]
        public HttpResponseMessage GetOrderStatus([FromUri]ClsGetOrderStatus OrderStatus)
        {
            string str = string.Empty;
            string errMsg = string.Empty, errData = string.Empty;
            var errorsList = new JObject();
            JArray errors = new JArray();

            try
            {
                ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
                var appId = principal.Claims.Where(c => c.Type == "clientId").Single().Value;
                var orgId = principal.Claims.Where(c => c.Type == "orgId").Single().Value;
                if (OrderStatus == null)
                {
                    errorsList["Error"] = "Order Id and location Id is required";
                    errors.Add(errorsList);
                    this.response = this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors, "info"));
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        OrderStatus.appId = appId;
                        OrderStatus.remoteIP = Request.GetClientIpAddress();
                        var result = this.iICLabsService.GetOrderStatus(OrderStatus);
                        if (result.Count == 0 )
                        {
                            errorsList["Warning"] = "No record found for orderId =" + OrderStatus.orderId + " and locationId =" + OrderStatus.locationId;
                            errors.Add(errorsList);
                            this.response = Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors,"info"));
                        }
                        else
                        {
                            this.response = Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("ok", result));
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
                                string result = errMsg + "|" + errData.Replace("Vendor.", string.Empty);
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

                        this.response = Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors, "info"));
                    }
                }                
            
            }
            catch (Exception e)
            {
                errorsList["Error"] = e.Message;
                errors.Add(errorsList);
                return Request.CreateResponse(HttpStatusCode.BadRequest, GetResponseBody("error", errors, "info"));
            }


            return this.response;
        }

        protected JObject GetResponseBody(string status, IList<ClsOrderStatus> data)
        {
            string str = string.Empty;
            var json = new JsonMediaTypeFormatter();
            str = Serialize(json, data);
            JObject body = new JObject();
            body.Add("status", status);
            body.Add("data", str);
            return body;
        }

    }
}
