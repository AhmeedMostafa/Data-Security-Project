using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        /// 
        static long Fast_P(long bas, long exp, long mod)
        {
            // Start with result = 1
            long ans = 1;
            // Loop until exponent is 0
            while (exp > 0)
            {
                // If exponent is odd, multiply result with base and take mod
                if ((exp & 1) == 1)
                    ans = (ans * bas) % mod;
                // Divide exponent by 2
                exp /= 2;
                // Square base and take mod to keep numbers small
                bas = (bas * bas) % mod;
            }
            // Return the final result
            return ans;
        }

        // Calculate modular inverse of x under mod using Fermat's Little Theorem
        static long mod_inverse(long x, long mod)
        {
            // Modular inverse is x^(mod-2) mod mod for prime mod
            return Fast_P(x, mod - 2, mod);
        }

        // Encrypt a message m using ElGamal
        public List<long> Encrypt(int q, int alpha, int y, int k, int m)   // k => random number < q
        {
            // Calculate C1 = alpha^k mod q (part of ciphertext)
            long c1 = Fast_P(alpha, k, q);
            // Calculate C2 = m * y^k mod q (message encrypted with public key)
            long c2 = (m * Fast_P(y, k, q)) % q;
            // Create list to store C1 and C2
            List<long> ans = new List<long>();
            // Add C1 and C2 to the list
            ans.Add(c1);
            ans.Add(c2);
            // Print C1 and C2 for debugging
            Console.WriteLine(c1 + " " + c2);
            // Return the ciphertext as a list [C1, C2]
            return ans;
        }

        // Decrypt the ciphertext (c1, c2) to get the original message
        public int Decrypt(int c1, int c2, int x, int q)
        {
            // Calculate s = c1^x mod q (shared secret)
            long s = Fast_P(c1, x, q);
            // Calculate m = c2 * (s^-1) mod q to recover the message
            long m = (c2 * mod_inverse(s, q)) % q;
            // Return the decrypted message as an integer
            return (int)m;
        }
    }
}