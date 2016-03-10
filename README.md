AP .NET JustAPI Gateway
===========

This is an SDK that you will use to interface with the AnyPresence's JustAPIs technology.


Dependencies
===========

APGW_CORE library

System.Net.Http - From NuGet

Autofac - From NuGet


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

Unpack the zip file to find APGW_|platform|.dll and AGPW_CORE.dll. Place these as dependent assemblies into your app.

Bootstrap the SDK by doing:

```
Common.Config.Setup();
```

Examples
===========

Create an APGateway builder with a base url of http://localhost/api/v1/

```
    APGatewayBuilder builder = new APGatewayBuilder ();
    builder.Uri ("http://localhost/api/v1/");
    APGateway gw = builder.Build ();
```


Sends an asynchronous request with "/foo" appended to base url

```
    APGatewayBuilder builder = new APGatewayBuilder ();
    builder.Uri ("http://localhost/api/v1/");
    APGateway gw = builder.Build ();

    // Send the request to http://localhost/api/v1/foo
    gw.GetAsync (url: "/foo", callback: new APGW.StringCallback () {
        OnSuccess = (res) => {
            Console.WriteLine (res);
        },                 
        OnError = (error) => {
            Console.WriteLine(error.Message);
        }   
    });
```

```
    APGatewayBuilder builder = new APGatewayBuilder ();
    builder.Uri ("http://localhost/api/v1/");
    APGateway gw = builder.Build ();

    // Send the request to http://localhost/api/v1/foo
    gw.PostAsync (url: "/foo", callback: new APGW.StringCallback () {
        OnSuccess = (res) => {
            Console.WriteLine (res);
        },                 
        OnError = (error) => {
            Console.WriteLine(error.Message);
        }   
    });
```

Sends a synchronous request with "/foo" appended to base url

```
    APGatewayBuilder builder = new APGatewayBuilder ();
    builder.Uri ("http://localhost/api/v1/");
    APGateway gw = builder.Build ();

    System.Threading.ThreadPool.QueueUserWorkItem((s) => {
      // Send the request to http://localhost/api/v1/foo
      var result = gw.GetSync (url: "/foo");
    });
```

Certificate Pinning
===========


Certificate pinning allows you to tie certificates against specified domains. It defends against attacks on certificate authorities.
It has it's limitations as well. We require the base64 encoded hashed SubjectPublicKeyInfo. See the example below for google.com

Example:
```
        APGW.CertManager.addCert ("localhost", dataAsByteArray);

        APGatewayBuilder builder = new APGatewayBuilder ();
        builder.Uri ("http://localhost/api/v1/");
        APGateway gw = builder.Build ();

        gw.UsePinning (true).GetAsync (url: "/foo", callback: new APGW.StringCallback () {
            OnSuccess = (res) => {
                Console.WriteLine (res);
            },                 
            OnError = (error) => {
                Console.WriteLine(error.Message);
            }   
        });

```
