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
