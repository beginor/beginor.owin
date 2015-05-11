using Microsoft.Owin.Security.DataProtection;
using System;

namespace Beginor.Owin.Security.Aes {

    public class AesDataProtectionProvider : IDataProtectionProvider {

        private string appName;

        public string Key { get; set; }

        public AesDataProtectionProvider() : this(Guid.NewGuid().ToString()) {
        }

        public AesDataProtectionProvider(string appName) {
            if (appName == null) {
                throw new ArgumentNullException("appName");
            }
            this.appName = appName;
        }

        public IDataProtector Create(params string[] purposes) {
            var key = GetProtectorKey();
            return new AesDataProtector(key);
        }

        private string GetProtectorKey() {
            return string.IsNullOrEmpty(Key) ? appName : Key;
        }
    }
}
