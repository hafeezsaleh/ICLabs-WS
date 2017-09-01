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
    /// <summary>
    ///  Retrieves ResultStatus record
    /// </summary>
     [RoutePrefix("api/ResultStatus")]
    public class ResultStatusController : BaseApiController
    {
        IICLabsService iICLabsService = new ICLabsService();

        /// <summary>
        /// HttpResponseMessage,to create and return HTTP responses like 200(OK),401(Unauthorized),400(Bad Request),408(RequestTimeout).
        /// </summary>
        private HttpResponseMessage response = new HttpResponseMessage();


        /// <summary>
        /// Retrieves ResultStatus record
        /// </summary>        
        /// <param name="ClsGetResultStatus">Represents the instance of the GET Result Status class</param>
        /// <returns>HTTP response 200 with Result Status record</returns>
        /// 
        [Authorize]
        [Route("")]
        public HttpResponseMessage GetResultStatus([FromUri]ClsGetResultStatus ResultStatus)
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
                if (ResultStatus == null)
                {
                    errorsList["Error"] = "Order Id and location Id is required";
                    errors.Add(errorsList);
                    this.response = this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors, "info"));
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        ResultStatus.appId = appId;
                        ResultStatus.remoteIP = Request.GetClientIpAddress();
                        var result = this.iICLabsService.GetResultStatus(ResultStatus);

                        if (result == null || result.Count == 0)
                        {
                            errorsList["Warning"] = "No record found for orderId =" + ResultStatus.orderId + " and locationId =" + ResultStatus.locationId;
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
            catch(Exception e)
            {
                errorsList["Error"] = e.Message;
                errors.Add(errorsList);
                return Request.CreateResponse(HttpStatusCode.BadRequest, GetResponseBody("error", errors, "info"));
            }

            return this.response;
        }

        protected JObject GetResponseBody(string status, IList<ClsResultsStatus> data)
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
