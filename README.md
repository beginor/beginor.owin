# Beginor.Owin.StaticFile

**Features:**

- Simple static file middleware for owin, just has reference to Owin.dll, no direct reference to Microsoft.Owin.*;
- Can handle most common static file types;
- ETag header support (optional), use file's last modified time as etag value;

**Usage:**

```c#
using Beginor.Owin.StaticFile;
```

```c#
app.UseStaticFile(new StaticFileMiddlewareOptions {
    RootDirectory = @"C:\inetpub\wwwroot",
    DefaultFile = "iisstart.htm",
    EnableETag = true,
    MimeTypeProvider = new MimeTypeProvider()
});
```
