using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDBClient
{
    class Program
    {
        private const string EndpointUrl = "https://<host_name>.documents.azure.com:443/";
        private const string AuthKey = "<access_key>";
        private const string DatabasebId = "data";
        private const string CollectionId = "default";

        static void Main(string[] args)
        {
            ConnectionPolicy policy = new ConnectionPolicy()
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };
            Uri endPoint = new Uri(EndpointUrl);
            using (DocumentClient client = new DocumentClient(endPoint, AuthKey, policy))
            {
                Database database =
                    client.CreateDatabaseQuery().Where(db => db.Id == DatabasebId).AsEnumerable().First();
                DocumentCollection collection =
                    client.CreateDocumentCollectionQuery(database.SelfLink)
                        .Where(c => c.Id == CollectionId)
                        .AsEnumerable()
                        .First();

                IEnumerable<DataClass> dataSet = GetDataObjects(100);

                Task<ResourceResponse<Document>>[] documentTasks =
                    dataSet.Select(d => client.CreateDocumentAsync(collection.DocumentsLink, d)).ToArray();

                Task.WaitAll(documentTasks);

                Document[] docs = documentTasks.Select(t => t.Result.Resource).ToArray();

                Document doc = docs.First();

                Task<ResourceResponse<Attachment>>[] attachmentTasks =
                    docs.Select(d => client.CreateAttachmentAsync(doc.AttachmentsLink, GetStream(1024))).ToArray();

                Task.WaitAll(attachmentTasks);

                DataClass[] data =
                    client.CreateDocumentQuery<DataClass>(collection.DocumentsLink).Select(d => d).ToArray();
                Attachment[] attachments = client.CreateAttachmentQuery(doc.AttachmentsLink).ToArray();

                string sql = "SELECT c.Name FROM c Where c.ObjectKey < 30 AND c.ObjectKey > 20";
                dynamic[] dataArray = client.CreateDocumentQuery(collection.DocumentsLink, sql).ToArray();
            }
        }

        private static Stream GetStream(int size)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(new byte[size], 0, size);
            return stream;
        }

        private static IEnumerable<DataClass> GetDataObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new DataClass()
                {
                    Name = string.Concat("Object #", i),
                    ObjectKey = i,
                    UniqueId = Guid.NewGuid(),
                    Date = DateTime.UtcNow
                };
            }
        }
    }
}