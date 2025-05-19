using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {
        public string Decrypt(string cipherText, List<string> key)
        {
            DES desInstance = new DES();

            string firstDecryption = desInstance.Decrypt(cipherText, key[0]);
            string secondEncryption = desInstance.Encrypt(firstDecryption, key[1]);
            string finalDecryption = desInstance.Decrypt(secondEncryption, key[0]);

            return finalDecryption;
        }

        public string Encrypt(string plainText, List<string> key)
        {
            DES desInstance = new DES();

            string firstEncryption = desInstance.Encrypt(plainText, key[0]);
            string secondDecryption = desInstance.Decrypt(firstEncryption, key[1]);
            string finalEncryption = desInstance.Encrypt(secondDecryption, key[0]);

            return finalEncryption;
        }

        public List<string> Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }

    }
}