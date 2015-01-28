using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beginor.Owin.StaticFile {

    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class StaticFileMiddlewareOptions {

        string defaultFile = "index.html";
        string rootDirectory = "wwwroot";
        bool enableETag = true;

        public string RootDirectory {
            get {
                return rootDirectory;
            }
            set {
                rootDirectory = value;
            }
        }

        public string DefaultFile {
            get {
                return defaultFile;
            }
            set {
                defaultFile = value;
            }
        }

        public bool EnableETag {
            get {
                return enableETag;
            }
            set {
                enableETag = value;
            }
        }

        public IMimeTypeProvider MimeTypeProvider { get; set; }

    }

}
