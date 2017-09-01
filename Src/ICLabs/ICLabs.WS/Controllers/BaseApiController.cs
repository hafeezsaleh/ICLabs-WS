using ICLabs.Model;
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
    public class BaseApiController : ApiController
    {

        protected string Serialize<T>(MediaTypeFormatter formatter, T value)
        {
            // Create a dummy HTTP Content.
            Stream stream = new MemoryStream();
            var content = new StreamContent(stream);
            /// Serialize the object.
            formatter.WriteToStreamAsync(typeof(T), value, stream, content, null).Wait();
            // Read the serialized string.
            stream.Position = 0;
            return content.ReadAsStringAsync().Result;
        }

        protected T Deserialize<T>(MediaTypeFormatter formatter, string str) where T : class
        {
            // Write the serialized string to a memory stream.
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            // Deserialize to an object of type T
            return formatter.ReadFromStreamAsync(typeof(T), stream, null, null).Result as T;
        }


        protected JObject GetResponseBody(string status, JArray data , string key="data")
        {
            JObject body = new JObject();
            body.Add("status", status.ToString());
            body.Add(key, data.ToString());           
            return body;
        }
        
        protected JObject GetResponseBody(string status, JArray data, JArray info)
        {
            JObject body = new JObject();
            body.Add("status", status.ToString());
            body.Add("data", data.ToString());
            body.Add("info", info.ToString());
            return body;
        }

        protected JObject GetJsonObject(string key,string value)
        {
            JObject body = new JObject();
            body.Add(key, value);            
            return body;
        }
    
    }
}