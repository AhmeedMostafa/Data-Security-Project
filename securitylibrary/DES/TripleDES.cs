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
        // Decrypt a ciphertext using Triple DES with three keys
        public string Decrypt(string cipherText, List<string> key)
        {
            // Create a DES object to use single DES encryption/decryption
            DES desInstance = new DES();

            // Step 1: Decrypt ciphertext with first key (reverse of encryption)
            string firstDecryption = desInstance.Decrypt(cipherText, key[0]);
            // Step 2: Encrypt result with second key (middle step is encryption)
            string secondEncryption = desInstance.Encrypt(firstDecryption, key[1]);
            // Step 3: Decrypt result with first key again to get plaintext
            string finalDecryption = desInstance.Decrypt(secondEncryption, key[0]);

            // Return the final plaintext
            return finalDecryption;
        }

        // Encrypt a plaintext using Triple DES with three keys
        public string Encrypt(string plainText, List<string> key)
        {
            // Create a DES object to use single DES encryption/decryption
            DES desInstance = new DES();

            // Step 1: Encrypt plaintext with first key
            string firstEncryption = desInstance.Encrypt(plainText, key[0]);
            // Step 2: Decrypt result with second key (middle step is decryption)
            string secondDecryption = desInstance.Decrypt(firstEncryption, key[1]);
            // Step 3: Encrypt result with first key again to get ciphertext
            string finalEncryption = desInstance.Encrypt(secondDecryption, key[0]);

            // Return the final ciphertext
            return finalEncryption;
        }

        // Analyse method to find keys (not implemented)
        public List<string> Analyse(string plainText, string cipherText)
        {
            // Throw an error because key analysis is not supported
            throw new NotSupportedException();
        }
    }
}