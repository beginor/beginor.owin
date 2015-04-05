namespace Beginor.Owin.StaticFile {

    public interface IETagProvider {

        /// <summary>
        /// Get ETAG for file; if file does not exists or can not
        /// compute file etag, just return string.Empty, do not
        /// throw exception;
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string GetETag(string filePath);

        bool CompareETag(string filePath, string etag);

    }

}
