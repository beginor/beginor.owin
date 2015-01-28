using System;

namespace Beginor.Owin.StaticFile {

    static class PathUtil {

        public static string CheckRequestPath(string requestPath, string defaultFile) {
            const string str = "/";
            if (requestPath.EndsWith(str, StringComparison.OrdinalIgnoreCase)) {
                requestPath += defaultFile;
            }
            if (requestPath.StartsWith(str, StringComparison.OrdinalIgnoreCase)) {
                requestPath = requestPath.Substring(1);
            }
            return requestPath;
        }
    }
}

