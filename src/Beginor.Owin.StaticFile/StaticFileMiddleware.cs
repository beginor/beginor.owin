using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Beginor.Owin.StaticFile {

    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class StaticFileMiddleware {

        readonly AppFunc next;
        readonly StaticFileMiddlewareOptions options;

        public StaticFileMiddleware(AppFunc next, StaticFileMiddlewareOptions options) {
            this.next = next;
            this.options = options;
        }

        public Task Invoke(IDictionary<string, object> env) {
            var requestPath = (string)env["owin.RequestPath"];
            requestPath = PathUtil.CheckRequestPath(requestPath, options.DefaultFile);

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, options.RootDirectory, requestPath);
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists) {
                var responseHeaders = (IDictionary<string, string[]>)env["owin.ResponseHeaders"];
                responseHeaders["Content-Type"] = new[] { options.MimeTypeProvider.GetMimeType(fileInfo.Extension) };
                responseHeaders["Content-Length"] = new[] { fileInfo.Length.ToString() };
                //
                var fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                var responseBodyStream = (Stream)env["owin.ResponseBody"];
                return fileStream.CopyToAsync(responseBodyStream).ContinueWith(t => {
                    fileStream.Close();
                });
            }
            return next.Invoke(env);
        }

    }

}

