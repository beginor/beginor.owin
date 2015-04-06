# Beginor.Owin.StaticFile

**Features:**

- Simple static file middleware for owin, just has reference to Owin.dll, no reference to Microsoft.Owin.*;
- Can handle most common static file types;
- ETag header support (optional), use file's last modified time as etag value;
- ETag custom provider support;

**Usage:**

Usage of this middleware is very simple:

```c#
// Use this static file middleware first is recomented
app.UseStaticFile(new StaticFileMiddlewareOptions {
    RootDirectory = @"C:\inetpub\wwwroot",
    DefaultFile = "iisstart.htm",
    EnableETag = true,
    // when EnableETag is true, default ETag provider is LastWriteTimeETagProvider
    // you can write your owin provider
    // ETagProvider = new LastWriteTimeETagProvider();
    MimeTypeProvider = new MimeTypeProvider()
});

// Add other middleware/framework here, WebAPI/Nancy etc.
app.UseWebApi(...);
```
