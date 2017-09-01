using ICLabs.Model;
using ICLabs.ModelV1;
using ICLabs.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ICLabs.WS.Controllers
{
    [RoutePrefix("api/Result")]
    public class ResultController : BaseApiController
    {
        IICLabsService iICLabsService = new ICLabsService();
        JObject errorsList = new JObject();
        JArray errors = new JArray();
        JArray message = new JArray();

        /// <summary>
        /// HttpResponseMessage,to create and return HTTP responses like 200(OK),401(Unauthorized),400(Bad Request),408(RequestTimeout).
        /// </summary>
        private HttpResponseMessage response = new HttpResponseMessage();

        /// <summary>
        /// Retrieves Result record
        /// </summary>        
        /// <param name="GetResult">Represents the instance of the GET Result class</param>
        /// <returns>HTTP response 200 with Result Status record</returns>
        /// 
        [Authorize]
        [Route("")]
        public HttpResponseMessage GetResult([FromUri]ClsGetResult GetResult)
        {
            string str = string.Empty;
            string errMsg = string.Empty, errData = string.Empty;


            try
            {
                ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
                var appId = principal.Claims.Where(c => c.Type == "clientId").Single().Value;
                var orgId = principal.Claims.Where(c => c.Type == "orgId").Single().Value;
                if (GetResult == null)
                {
                    errorsList["Error"] = "Data required";
                    errors.Add(errorsList);
                    this.response = this.Request.CreateResponse(HttpStatusCode.BadRequest, GetResponseBody("error", errors, "info"));
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        GetResult.appId = appId;
                        GetResult.remoteIP = Request.GetClientIpAddress();
                        var result = this.iICLabsService.GetResult(GetResult);

                        if (result.Count == 0)
                        {
                            errorsList["Warning"] = "No record found for orderId=" + GetResult.orderId + " and locationId=" + GetResult.locationId;
                            errors.Add(errorsList);

                            this.response = Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors, "info"));
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

       
        /// <summary>
        /// Create a new Result data.
        /// </summary>        
        /// <param name="Result">Represents the instance of the POST rsult Class</param>
        /// <returns>HTTP 200 OK  / HTTP 400 with errors / Other HTTP responses</returns>
        /// 
        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> PostResult()
        {
            ClsPostResult result = new ClsPostResult();
            byte[] filebData = null;
            string errorMsg = String.Empty;
            var messageList = new JObject();

            //ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            //var appId = principal.Claims.Where(c => c.Type == "clientId").Single().Value;
            //var orgId = principal.Claims.Where(c => c.Type == "orgId").Single().Value;

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                result.appId = "appId";
                result.remoteIP = Request.GetClientIpAddress();

                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                if (provider.FileData.Count == 0)
                {
                    errorMsg = "Result file is required; ";
                   
                }
                else
                {
                    MultipartFileData file = provider.FileData[0];
                    //result.fileName = file.Headers.ContentDisposition.FileName;


                    if ( provider.FormData.GetValues("filename") == null )
                    {
                        result.fileName = "Result.pdf";
                    }
                    //else
                    //{

                    //}
                    //String fileName = provider.FormData.GetValues("filename").FirstOrDefault();

                    //if (String.IsNullOrEmpty(fileName))
                    //{
                    //    result.fileName = "Result.pdf";
                    //}
                    else
                    {
                        result.fileName = provider.FormData.GetValues("filename").FirstOrDefault();
                    }
                    FileStream fbs = new System.IO.FileStream(file.LocalFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                    int filebLen = (int)fbs.Length;
                    filebData = new byte[filebLen];
                    fbs.Read(filebData, 0, (int)filebLen);
                    fbs.Close();
                    result.resultBinaryStream = filebData;
                }

                if (provider.FormData.GetValues("clientId") == null)
                {
                    errorMsg += "clientId is required; ";                  
                }
                else
                {
                    result.clientId = provider.FormData.GetValues("clientId").FirstOrDefault();
                }

                if (provider.FormData.GetValues("orderId") == null)
                {
                    errorMsg += "orderId is required; ";
                }
                else
                {
                    result.orderId = provider.FormData.GetValues("orderId").FirstOrDefault();
                }


                if (provider.FormData.GetValues("orderCode") == null)
                {
                    errorMsg += "orderCode is required; ";
                }
                else
                {
                    result.orderCode = provider.FormData.GetValues("orderCode").FirstOrDefault();
                }

                if (provider.FormData.GetValues("resultFlag") == null)
                {
                    errorMsg += "resultFlag is required; ";
                }
                else
                {
                    result.resultFlag = provider.FormData.GetValues("resultFlag").FirstOrDefault();
                }

                if (provider.FormData.GetValues("normalRange") != null)
                {
                    result.normalRange = provider.FormData.GetValues("normalRange").FirstOrDefault();
                }

                //if (provider.FormData.GetValues("resultDate") != null)
                //{
                //    result.resultDate = provider.FormData.GetValues("resultDate").FirstOrDefault();
                //}

                if (provider.FormData.GetValues("resultName") == null)
                {
                    errorMsg += "resultName is required; ";
                }
                else
                {
                    result.resultName = provider.FormData.GetValues("resultName").FirstOrDefault();
                }

                if (provider.FormData.GetValues("resultText") != null)
                {
                    result.resultText = provider.FormData.GetValues("resultText").FirstOrDefault();
                }

                if (provider.FormData.GetValues("resultType") == null)
                {
                    errorMsg += "resultType is required; ";
                }
                else
                {
                    result.resultType = provider.FormData.GetValues("resultType").FirstOrDefault();
                }

                if (provider.FormData.GetValues("resultUnits") != null)
                {
                    result.resultUnits = provider.FormData.GetValues("resultUnits").FirstOrDefault();
                }

                if (errorMsg.Equals(String.Empty))
                {
                    errorMsg = this.iICLabsService.PostResult(result);

                    if (errorMsg.Equals(String.Empty))
                    {
                        messageList["Message"] = "Result added for Order Id:" + result.orderId + " and Order code:" + result.orderCode;
                        message.Add(messageList);

                        this.response = this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("ok", message));
                        
                    }
                    else
                    {
                        errorsList["Error"] = errorMsg;
                        errors.Add(errorsList);
                        return this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors));
                    }
                }
                else
                {
                    errorsList["Error"] = errorMsg;
                    errors.Add(errorsList);
                    return this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors));
                }
            }
            catch (Exception e)
            {
                errorsList["Error"] = e.Message;
                errors.Add(errorsList);
                return Request.CreateResponse(HttpStatusCode.BadRequest, GetResponseBody("error", errors));
            }


            return response;
        }

        
        protected JObject GetResponseBody(string status, IList<ClsResult> data)
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
