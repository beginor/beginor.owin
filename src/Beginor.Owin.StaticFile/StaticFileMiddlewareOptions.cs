namespace Beginor.Owin.StaticFile {

    public class StaticFileMiddlewareOptions {

        public string RootDirectory { get; set; } = "wwwroot";

        public string DefaultFile { get; set; } = "index.html";

        public bool EnableETag { get; set; } = true;

        public bool EnableHtml5LocationMode { get; set; } = false;

        public IMimeTypeProvider MimeTypeProvider { get; set; }

        public IETagProvider ETagProvider { get; set; }

    }

}
