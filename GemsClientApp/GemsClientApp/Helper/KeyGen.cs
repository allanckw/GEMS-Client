using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.IO;


namespace Gems.UIWPF.Helper
{

    public class KeyGen
    {
        private KeyGen()
        {
        }

        #region "Private variables and Constants"

        private const string seedToUse = "T1GNacCS3213_t3@mO1_GEMS_@SkY1904@";

        /// The salt value used to strengthen the encryption.
        private static readonly byte[] SALT = Encoding.ASCII.GetBytes(seedToUse);
        private static readonly byte[] key;
        private static readonly byte[] initVector;

        private static readonly Rfc2898DeriveBytes rfcKeyGen;
        #endregion

        #region "Constructor"

        static KeyGen()
        {
            //We create the Rfc2898DerveBytes once as Rfc2898DeriveBytes uses a pseudo-random number generator based on HMACSHA1.

            rfcKeyGen = new Rfc2898DeriveBytes(seedToUse, SALT);
            //When calling GetBytes it initializes a new instance of HMAC, Later calls to GetBytes does not need to do this.
            key = rfcKeyGen.GetBytes(32);
            //Generate Initialization Vector - This will be less expensive as we have already intialized Rfc2898DeriveBytes
            initVector = rfcKeyGen.GetBytes(16);
        }

        #endregion

        #region "Encrypt/Decrypt Methods"

        //Encrypts any string using the Rijndael algorithm.
        //input text: The string to encrypt.
        // returns A Base64 encrypted string.
        public static string Encrypt(string inputText)
        {

            //Create RijndaelManaged cipher for the symmetric algorithm from the key and initVector 
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Key = key;
            rijndaelCipher.IV = initVector;


            byte[] plainText = Encoding.Unicode.GetBytes(inputText);

            using (ICryptoTransform enCrypter = rijndaelCipher.CreateEncryptor())
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, enCrypter, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainText, 0, plainText.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memStream.ToArray());
                    }
                }
            }
        }

        //Decrypts a previously encrypted string.
        //inputext: The encrypted string to decrypt.
        //Returns A decrypted string.
        public static string Decrypt(string inputText)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] encryptedData = Convert.FromBase64String(inputText);

            using (ICryptoTransform deCrypter = rijndaelCipher.CreateDecryptor(key, initVector))
            {
                using (MemoryStream memStream = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, deCrypter, CryptoStreamMode.Read))
                    {
                        byte[] plainText = new byte[encryptedData.Length];
                        int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                        return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                    }
                }
            }
        }

        #endregion

    }

}
