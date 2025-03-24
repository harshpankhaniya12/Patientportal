using System.Security.Cryptography;
using System.Text;

namespace Patientportal.Model
{
    public class EncryptionHelper
    {
        private static readonly string EncryptionKey = "kN4gG6YfQpVzV1Tq9Ys9dFhT+WJGmZoZnlHqj8J64Xg=";

        public static string EncryptId(long id)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(id.ToString());
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 }).GetBytes(32);
                encryptor.GenerateIV(); // हर बार नया IV बनाएँ
                byte[] iv = encryptor.IV;

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length); // सबसे पहले IV स्टोर करें
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    return Convert.ToBase64String(ms.ToArray()).Replace("+", "-").Replace("/", "_").Replace("=", "");
                }
            }
        }

        public static long DecryptId(string encryptedId)
        {
            encryptedId = encryptedId.Replace("-", "+").Replace("_", "/");
            while (encryptedId.Length % 4 != 0)
            {
                encryptedId += "=";
            }

            byte[] cipherBytes = Convert.FromBase64String(encryptedId);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 }).GetBytes(32);

                byte[] iv = new byte[16];
                Array.Copy(cipherBytes, 0, iv, 0, iv.Length); // IV को पहले 16 बाइट्स से प्राप्त करें

                encryptor.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, iv.Length, cipherBytes.Length - iv.Length);
                        cs.Close();
                    }
                    return long.Parse(Encoding.UTF8.GetString(ms.ToArray()));
                }
            }

        }
    }
}
