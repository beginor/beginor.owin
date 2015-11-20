using Microsoft.Owin.Security.DataProtection;
using System;

namespace Beginor.Owin.Security.Aes {

    public class AesDataProtectionProvider : IDataProtectionProvider {

        private readonly string appName;

        public string Key { get; set; }

        public AesDataProtectionProvider(string appName) {
            if (appName == null) {
                throw new ArgumentNullException(nameof(appName));
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
