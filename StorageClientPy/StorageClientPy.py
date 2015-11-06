from azure.storage.blob import BlobService



blob_service = BlobService(account_name='<account_name>', account_key='<account_key>')

blob_service.create_container('datacontainer')

blob_service.create_container('datacontainer', x_ms_blob_public_access='container')

blob_service.set_container_acl('datacontainer', x_ms_blob_public_access='container')


blob_service.put_block_blob_from_path(
    'datacontainer',
    'datablob',
    'StorageClientPy.py',
    x_ms_blob_content_type='text/x-script.phyton'
)


blobs = []
marker = None
while True:
    batch = blob_service.list_blobs('datacontainer', marker=marker)
    blobs.extend(batch)
    if not batch.next_marker:
        break
    marker = batch.next_marker
for blob in blobs:
    print(blob.name)


blob_service.get_blob_to_path('datacontainer', 'datablob', 'out-StorageClientPy.py')


blob_service.delete_blob('datacontainer', 'datablob')