import pydocumentdb.document_client as document_client
import DocumentDBClientPy
import config

#f = file('simple.cfg')
#Fcfg = Config(f)

documentDb_host = 'https://<host_name>.documents.azure.com:443/'
documentDb_key = '<access_key>'
documentDb_db = 'data'
documentDb_collection = 'default'
documentDb_document = 'testDoc'

client = document_client.DocumentClient(documentDb_host, {'masterKey': documentDb_key})

# Attempt to delete the database.  This allows this to be used to recreate as well as create
#try:
#    db = next((data for data in client.ReadDatabases() if data['id'] == documentDb_db))
#    client.DeleteDatabase(db['_self'])
#except:
#    pass

# Create database
db = client.CreateDatabase({ 'id': documentDb_db })
# Create collection
collection = client.CreateCollection(db['_self'],{ 'id': documentDb_collection }, { 'offerType': 'S1' })
# Create document
document = client.CreateDocument(collection['_self'],
    { 'id': documentDb_document,
    'Web Site': 0,
    'Cloud Service': 0,
    'Virtual Machine': 0,
    'name': documentDb_document })