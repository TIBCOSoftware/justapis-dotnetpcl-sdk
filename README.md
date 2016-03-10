AP .NET JustAPI Gateway
===========

This is an SDK that you will use to interface with the AnyPresence's JustAPIs technology.


Dependencies
===========

APGW_CORE library

System.Net.Http - From NuGet


Introduction
==========


The main class to work with is APGateway. You will use an instance of this class to make requests.

An instance should be created using a builder (APGatewayBuilder). 

```
        APGatewayBuilder builder = new APGatewayBuilder();
        builder.url("http://foo.lvh.me:3000/api/v1/foo");

        // Provide the application  context and build the gateway object
        APGateway gw = builder.build();
```        


Setup
===========

Examples
===========

Sends an asynchnronous request

```
    APGatewayBuilder builder = new APGatewayBuilder ();
    builder.Uri ("http://localhost/api/v1/foo");
    APGateway gw = builder.Build ();

    gw.GetAsync ("/foo", new APGW.StringCallback () {
        OnSuccess = (res) => {
            Console.WriteLine (res);
        },                 ,
        OnError = (error) => {
            Console.WriteLine(error.Message);
        }   
    });
```

Sends a synchronous request

```
    APGatewayBuilder builder = new APGatewayBuilder ();
    builder.Uri ("http://localhost/api/v1/foo");
    APGateway gw = builder.Build ();

    System.Threading.ThreadPool.QueueUserWorkItem((s) => {
      var result = gw.GetSync ("/foo");
    });
```

Certificate Pinning
===========

Caching
===========