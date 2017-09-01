using ICLabs.ModelV1;
using ICLabs.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http;

namespace ICLabs.WS.Controllers
{
     [RoutePrefix("api/ResultFile")]
    public class ResultFileController: BaseApiController
    {
        IICLabsService iICLabsService = new ICLabsService();

        /// <summary>
        /// HttpResponseMessage,to create and return HTTP responses like 200(OK),401(Unauthorized),400(Bad Request),408(RequestTimeout).
        /// </summary>
        private HttpResponseMessage response = new HttpResponseMessage();

        /// <summary>
        /// Retrieves Result File record
        /// </summary>        
        /// <param name="ResultFile">Represents the instance of the GET Result File class</param>
        /// <returns>HTTP response 200 with Result File record</returns>
        /// 

        [Authorize]
        [Route("")]
        public HttpResponseMessage GetResultFile([FromUri]ClsGetResultFile ResultStatus)
        {
            string str = string.Empty;
            string errMsg = string.Empty, errData = string.Empty;
            var errorsList = new JObject();
            JArray errors = new JArray();
            MemoryStream responseStream = new MemoryStream();

            try
            {
                ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
                var appId = principal.Claims.Where(c => c.Type == "clientId").Single().Value;
                var orgId = principal.Claims.Where(c => c.Type == "orgId").Single().Value;
                if (ResultStatus == null)
                {
                    errorsList["Error"] = "Order Id, Location Id, Test and Result Name is required";
                    errors.Add(errorsList);
                    this.response = this.Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors, "info"));
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        ResultStatus.appId = appId;

                        //ResultStatus.appId = "ZOHO";
                        ResultStatus.remoteIP = Request.GetClientIpAddress();
                        ClsResultFile resultFile = this.iICLabsService.GetResultFile(ResultStatus);

                        if (resultFile.fileName == null)
                        {
                            errorsList["Warning"] = "No record found for orderId=" + ResultStatus.orderId + " and locationId=" + ResultStatus.locationId + " and test=" + ResultStatus.test +" and resultName=" + ResultStatus.resultName;
                            errors.Add(errorsList);
                            this.response = Request.CreateResponse(HttpStatusCode.OK, GetResponseBody("error", errors, "info"));
                        }
                        else
                        {
                            //byte[] buffer = result;
                            responseStream.Write(resultFile.resultBinaryStream, 0, resultFile.resultBinaryStream.Length);
                            //this.response = Request.CreateResponse(HttpStatusCode.OK, result);
                            HttpResponseMessage response = new HttpResponseMessage();
                            response.StatusCode = HttpStatusCode.OK;
                            response.Content = new ByteArrayContent(responseStream.ToArray());
                            response.Content.Headers.ContentDisposition
                                = new ContentDispositionHeaderValue("attachment");
                            response.Content.Headers.ContentDisposition.FileName = (resultFile.fileName == null) ? "RESULT_FILE" : resultFile.fileName;
                            response.Content.Headers.ContentType
                                = new MediaTypeHeaderValue("application/octet-stream");
                            response.Content.Headers.ContentLength
                                    = resultFile.resultBinaryStream.Length;

                            return response;
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

        
    
    }
}
