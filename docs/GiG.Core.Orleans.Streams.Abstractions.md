# GiG.Core.Orleans.Streams.Abstractions

This Library provides an API to register Stream Helpers.

## Namespace Prefix

### Stream Helper for Namespace

A helper can be used to construct the namespace including the `NamespacePrefix`.

#### Sample Usage

```
// dev.message-type
StreamHelper.GetNamespace("message-type");

// dev.domain.message-type.v1
StreamHelper.GetNamespace("domain", "message-type");

// dev.domain.message-type.v2
StreamHelper.GetNamespace("domain", "message-type", 2);
```

### Namespace Implicit Stream Subscription

The `NamespaceImplicitStreamSubscription` is used for implicit subscriptions using the specified stream namespace including the `NamespacePrefix`.

#### Sample Usage

```
// Same as [ImplicitStreamSubscription("dev.domain.message-type.v1")]
[NamespaceImplicitStreamSubscription("domain", "message-type")]
public class MockStreamGrain : Grain, IMockStreamGrain, IAsyncObserver<MockRequest>
{
    public override Task OnActivateAsync()
    {
        var streamProvider = GetStreamProvider(Constants.StreamProviderName);
        var mockRequestStream = streamProvider.GetStream<MockRequest>(this.GetPrimaryKey(), StreamHelper.GetNamespace("domain", "message-type"));
        mockRequestStream.SubscribeAsync(this);
    }

    ...
}
```