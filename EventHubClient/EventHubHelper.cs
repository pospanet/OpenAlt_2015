using System;
using Microsoft.ServiceBus;

namespace EventHubClient
{
    internal class EventHubHelper
    {
        public static string CreateSas(string serviceBusNamespace, string eventHubName, string publisher, string keyName,
            string keyValue, double ttlSeconds)
        {
            return
                SharedAccessSignatureTokenProvider.GetPublisherSharedAccessSignature(GetMethodUri(serviceBusNamespace),
                    eventHubName, publisher, keyName, keyValue, TimeSpan.FromSeconds(ttlSeconds));
        }

        public static Uri GetMethodUri(string serviceBusNamespace)
        {
            return ServiceBusEnvironment.CreateServiceUri("https", serviceBusNamespace, string.Empty);
        }

        public static Uri GetServiceUri(string serviceBusNamespace, string eventHub, string publisher)
        {
            return ServiceBusEnvironment.CreateServiceUri("https", serviceBusNamespace,
                string.Format("{0}/publishers/{1}/messages", (object) eventHub, (object) publisher));
        }
    }
}