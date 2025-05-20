using SecurityLibrary.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        // Encrypt a message M using RSA public key (e, n)
        public int Encrypt(int p, int q, int M, int e)
        {
            // Calculate n = p * q (modulus for RSA)
            int n = p * q;
            // Compute ciphertext C = M^e mod n
            return (int)PowerMod(M, e, n);
        }

        // Decrypt a ciphertext C using RSA private key (d, n)
        public int Decrypt(int p, int q, int C, int e)
        {
            // Calculate n = p * q (modulus for RSA)
            int n = p * q;
            // Calculate phi = (p-1) * (q-1) for key generation
            int phi = (p - 1) * (q - 1);

            // Create object to find multiplicative inverse
            ExtendedEuclid extendedEuclid = new ExtendedEuclid();
            // Find d, the private key, where d * e = 1 mod phi
            int d = extendedEuclid.GetMultiplicativeInverse(e, phi);

            // Compute plaintext M = C^d mod n
            return (int)PowerMod(C, d, n);
        }

        // Helper method to calculate (num^power) mod mod efficiently
        private long PowerMod(long num, long power, long mod)
        {
            // Start with result = 1
            long result = 1;
            // Ensure num is within modulus range
            num = num % mod;

            // Loop until power is 0
            while (power > 0)
            {
                // If power is odd, multiply result with num and take mod
                if (power % 2 == 1)
                {
                    result = (result * num) % mod;
                }
                // Divide power by 2
                power = power / 2;
                // Square num and take mod to keep numbers small
                num = (num * num) % mod;
            }

            // Return the final result
            return result;
        }
    }
}
