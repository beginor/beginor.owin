using System.IO;

namespace Beginor.Owin.StaticFile {

    public class LastWriteTimeETagProvider : IETagProvider {

        public virtual string GetETag(string filePath) {
            var ticks = GetLastWriteTimeUtcTicks(filePath);
            return ticks > 0 ? ticks.ToString() : string.Empty;
        }

        private static long GetLastWriteTimeUtcTicks(string filePath) {
            var fileInfo = new FileInfo(filePath);
            return fileInfo.Exists ? fileInfo.LastWriteTimeUtc.Ticks
                                    : 0;
        }

        public virtual bool CompareETag(string filePath, string etag) {
            long tag;
            if (long.TryParse(etag, out tag)) {
                var ticks = GetLastWriteTimeUtcTicks(filePath);
                return ticks == tag;
            }
            return false;
        }

    }
}
