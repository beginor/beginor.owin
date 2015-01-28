using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace Beginor.Owin.StaticFile {

    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class ETagMiddleware {

        readonly AppFunc next;
        readonly StaticFileMiddlewareOptions options;

        public ETagMiddleware(AppFunc next, StaticFileMiddlewareOptions options) {
            this.next = next;
            this.options = options;
        }

        public async Task Invoke(IDictionary<string, object> env) {
            var requestPath = (string)env["owin.RequestPath"];
            requestPath = PathUtil.CheckRequestPath(requestPath, options.DefaultFile);
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, options.RootDirectory, requestPath);
            var fileInfo = new FileInfo(filePath);
            var requestHeaders = (IDictionary<string, string[]>)env["owin.RequestHeaders"];

            if (fileInfo.Exists) {
                var ticks = fileInfo.LastWriteTime.Ticks;
                if (requestHeaders.ContainsKey("If-None-Match")) {
                    var tagValue = requestHeaders["If-None-Match"];
                    if (tagValue != null && tagValue.Length > 0) {
                        long tag;
                        if (long.TryParse(tagValue[0], out tag)) {
                            if (ticks == tag) {
                                env["owin.ResponseStatusCode"] = (int)HttpStatusCode.NotModified;
                                env["owin.ResponseReasonPhrase"] = "Not Modified";
                                return;
                            }
                        }
                    }
                }
                else {
                    await next.Invoke(env);
                    var responseHeaders = (IDictionary<string, string[]>)env["owin.ResponseHeaders"];
                    responseHeaders["ETag"] = new [] { ticks.ToString() };
                    return;
                }
            }
            await next.Invoke(env);
        }
    }
}

