using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman
    {
        // Method to generate shared secret keys for two parties using Diffie-Hellman
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            // Calculate public key for party A: alpha^xa mod q
            int ya = ModPow(alpha, xa, q);
            // Calculate public key for party B: alpha^xb mod q
            int yb = ModPow(alpha, xb, q);

            // Calculate shared secret key for A: yb^xa mod q
            int k1 = ModPow(yb, xa, q);
            // Calculate shared secret key for B: ya^xb mod q
            int k2 = ModPow(ya, xb, q);

            // Return both shared keys (they should be the same)
            return new List<int> { k1, k2 };
        }

        // Helper method to calculate (baseVal^exp) mod mod efficiently
        private int ModPow(int baseVal, int exp, int mod)
        {
            // Start with result = 1
            int result = 1;
            // Ensure baseVal is within modulus range
            baseVal = baseVal % mod;

            // Loop until exponent is 0
            while (exp > 0)
            {
                // If exponent is odd, multiply result with baseVal and take mod
                if (exp % 2 == 1)
                {
                    result = (result * baseVal) % mod;
                }

                // Divide exponent by 2 (right shift)
                exp = exp >> 1;
                // Square baseVal and take mod to keep numbers manageable
                baseVal = (baseVal * baseVal) % mod;
            }

            // Return the final result
            return result;
        }
    }
}