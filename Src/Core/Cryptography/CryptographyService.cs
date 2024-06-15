using Avesta.Core.System;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Avesta.Core.Cryptography
{
    public class CryptographyService
    {
        private readonly int _keySize;
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public CryptographyService(IOptions<CoreOptions> options)
        {
            _keySize = options.Value.Cryptography.KeySize;

            var key = options.Value.Cryptography.Key;

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(options.Value.Cryptography.Key));
            }
            else
            {
                _key = Encoding.UTF8.GetBytes(key);
            }

            var iv = options.Value.Cryptography.IV;

            if (string.IsNullOrWhiteSpace(iv))
            {
                throw new ArgumentNullException(nameof(options.Value.Cryptography.IV));
            }
            else
            {
                _iv = Encoding.UTF8.GetBytes(iv);
            }
        }

        public string Encrpyt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                throw new ArgumentNullException(nameof(plainText));

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = _keySize;
                aes.Key = _key;
                aes.IV = _iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] encrypted;

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }

                return Convert.ToBase64String(encrypted);
            }
        }

        public string Decrpyt(string cipherText) 
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                throw new ArgumentNullException(nameof(cipherText));

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = _keySize;
                aes.Key = _key;
                aes.IV = _iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            var plaintext = srDecrypt.ReadToEnd();

                            return plaintext;
                        }
                    }
                }
            }
        }
    }
}
