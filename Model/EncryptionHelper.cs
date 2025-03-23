using System.Security.Cryptography;
using System.Text;

namespace Patientportal.Model
{
    public class EncryptionHelper
    {
        private static readonly string EncryptionKey = "kN4gG6YfQpVzV1Tq9Ys9dFhT+WJGmZoZnlHqj8J64Xg="; // Change this key

        public static string EncryptId(long id)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(id.ToString());
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
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
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    return long.Parse(Encoding.UTF8.GetString(ms.ToArray()));
                }
            }

        }
    }
}
