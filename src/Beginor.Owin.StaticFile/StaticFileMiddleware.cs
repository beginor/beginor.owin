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
                return WriteFileToOwin(fileInfo, env);
            }
            if (options.EnableHtml5LocationMode) {
                var parentFile = FindFileRecursively(fileInfo.Directory, options.DefaultFile);
                if (parentFile != null) {
                    return WriteFileToOwin(parentFile, env);
                }
            }
            return next.Invoke(env);
        }

        private static FileInfo FindFileRecursively(DirectoryInfo dir, string fileName) {
            FileInfo fileInfo = null;
            while (dir.Exists) {
                var filePath = Path.Combine(dir.FullName, fileName);
                if (File.Exists(filePath)) {
                    fileInfo = new FileInfo(filePath);
                    break;
                }
                dir = dir.Parent;
            }
            return fileInfo;
        }

        private Task WriteFileToOwin(FileInfo fileInfo, IDictionary<string, object> env) {
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

    }

}

