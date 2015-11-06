using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EventHubClient
{
    public class EventHub
    {
        private const string ContentType = "application/atom+xml;type=entry;charset=utf-8";
        private const string Method = "POST";
        private const string AuthorizationHeaderKey = "Authorization";
        private const string UnderlyingTypeKey = "UnderlyingType";
        private readonly string _sas;
        private readonly Uri _serviceUri;

        public EventHub(string serviceBusNamespace, string eventHubName, string publisher, string keyName,
            string keyValue, double ttlSeconds)
        {
            _sas = EventHubHelper.CreateSas(serviceBusNamespace, eventHubName, publisher, keyName, keyValue, ttlSeconds);
            _serviceUri = EventHubHelper.GetServiceUri(serviceBusNamespace, eventHubName, publisher);
        }

        public async Task<bool> SendEventHubEventAsync(object obj)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(_serviceUri);
            request.Method = Method;
            request.Headers.Add(AuthorizationHeaderKey, _sas);
            request.ContentType = ContentType;
            request.Headers.Add(UnderlyingTypeKey, obj.GetType().FullName);
            string serializedString = JsonConvert.SerializeObject(obj);
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(serializedString);
            }
            using (WebResponse webResponse = await request.GetResponseAsync())
            {
                HttpWebResponse response = webResponse as HttpWebResponse;
                HttpStatusCode statusCode = response.StatusCode;
                return statusCode.Equals(HttpStatusCode.Created);
            }
        }
    }
}