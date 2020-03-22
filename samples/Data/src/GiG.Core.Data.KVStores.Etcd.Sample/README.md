# KV etcd Sample

## Countries

The countries sample is used to demo the Write functionality via `IDataWriter` and Get functionality via `IDataRetriever` APIs.

## Languages

The languages sample is used to demo the Watch functionality via `IDataRetriever` API.

You can use the following CLI command to update etcd value for Languages.  The updated value will always be displayed in the Console logging.

```sh
curl -L http://localhost:2379/v3/kv/put -X POST -d '{"key": "bGFuZ3VhZ2Vz", "value": "W3sgIk5hbWUiOiAiTWFsdGVzZSIsICJBbHBoYTJDb2RlIjogIm10IiB9LCB7ICJOYW1lIjogIkVuZ2xpc2giLCAiQWxwaGEyQ29kZSI6ICJlbiIgfV0="}'
```

## Documentation
- [GiG.Core.Data.KVStores](docs/GiG.Core.Data.KVStores.md) - Provides an API to register the required services needed by the KV Stores Data Providers.
- [GiG.Core.Data.KVStores.Providers.Etcd](docs/GiG.Core.Data.KVStores.Providers.Etcd.md) - Provides an API to register data providers which will read data from etcd.