# Beginor.Owin.Security.Aes

`DpapiDataProtectionProvider` is the default data protection provider used by Microsoft's
Owin security middleware, and it uses `crypt32.dll` to protect data, and can not works
with mono.

Aes data protection provider for Microsoft.Owin.Security, fully managed C# code, works
with mono;

simple usage:

```c#
// just add one line code before using the cookie authentication middleware of microsoft
app.UseAesDataProtectionProvider();
// cookie auth;
app.UseCookieAuthentication(new CookieAuthenticationOptions{
    AuthenticationType = CookieAuthenticationDefaults.AuthenticationType
});
// other config, like web api...
```

BTW:

Microsoft.Owin.Security.*.dll need app property `host.AppName` to work, but some thirdpart owin server [jexus](http://jexus.org/) or [nowin](https://github.com/Bobris/Nowin) does not provide this property, if you use jexus or nowin, please add `host.AppName` to the `app.Properties` dictionary before call to `Configure` function.
