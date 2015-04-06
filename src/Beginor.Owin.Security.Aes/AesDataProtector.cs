using Microsoft.Owin.Security.DataProtection;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Beginor.Owin.Security.Aes {

    //public IDataProtectionProvider

    public class AesDataProtector : IDataProtector {

        private readonly byte[] key;

        public AesDataProtector(string key) {
            using (var sha1 = new SHA256Managed()) {
                this.key = sha1.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }

        public byte[] Protect(byte[] userData) {
            byte[] dataHash;
            using (var sha = new SHA256Managed()) {
                dataHash = sha.ComputeHash(userData);
            }

            using (AesManaged aesAlg = new AesManaged()) {
                aesAlg.Key = key;
                aesAlg.GenerateIV();

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (var msEncrypt = new MemoryStream()) {
                    msEncrypt.Write(aesAlg.IV, 0, 16);

                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var bwEncrypt = new BinaryWriter(csEncrypt)) {
                        bwEncrypt.Write(dataHash);
                        bwEncrypt.Write(userData.Length);
                        bwEncrypt.Write(userData);
                    }
                    var protectedData = msEncrypt.ToArray();
                    return protectedData;
                }
            }
        }

        public byte[] Unprotect(byte[] protectedData) {
            using (AesManaged aesAlg = new AesManaged()) {
                aesAlg.Key = key;

                using (var msDecrypt = new MemoryStream(protectedData)) {
                    byte[] iv = new byte[16];
                    msDecrypt.Read(iv, 0, 16);

                    aesAlg.IV = iv;

                    using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var brDecrypt = new BinaryReader(csDecrypt)) {
                        var signature = brDecrypt.ReadBytes(32);
                        var len = brDecrypt.ReadInt32();
                        var data = brDecrypt.ReadBytes(len);

                        byte[] dataHash;
                        using (var sha = new SHA256Managed()) {
                            dataHash = sha.ComputeHash(data);
                        }

                        if (!dataHash.SequenceEqual(signature)) {
                            throw new SecurityException("Signature does not match the computed hash");
                        }

                        return data;
                    }
                }
            }
        }
    }
}
