using System.Security.Cryptography;

namespace New_Project_Backend.Extensions
{
    public class EncryptDecrypt
    {

        /// <summary>
        /// Ref. URL: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/
        /// </summary>
        //static string m_privateKey = "D87F01C168064266A5EB89AC68E3789C";
        //Encoding.UTF8.GetBytes(m_privateKey);
        //string keyString = String.Join(", ", aes.Key);

        //Actual Key for Encrypt and Decrypt - D87F01C168064266A5EB89AC68E3789C
        static byte[] m_byteKey = new byte[] { 68, 56, 55, 70, 48, 49, 67, 49, 54, 56, 48, 54, 52, 50, 54, 54, 65, 53, 69, 66, 56, 57, 65, 67, 54, 56, 69, 51, 55, 56, 57, 67 };


        public static string EncryptString(string inputData)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = m_byteKey;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(inputData);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.Key = m_byteKey;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
