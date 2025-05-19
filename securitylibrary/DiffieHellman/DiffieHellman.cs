using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman
    {
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            // Calculate public keys
            int ya = ModPow(alpha, xa, q);
            int yb = ModPow(alpha, xb, q);

            // Calculate shared secret keys
            int k1 = ModPow(yb, xa, q);
            int k2 = ModPow(ya, xb, q);

            return new List<int> { k1, k2 };
        }

        private int ModPow(int baseVal, int exp, int mod)
        {
            int result = 1;
            baseVal = baseVal % mod;

            while (exp > 0)
            {
                if (exp % 2 == 1)
                {
                    result = (result * baseVal) % mod;
                }

                exp = exp >> 1;
                baseVal = (baseVal * baseVal) % mod;
            }

            return result;
        }
    }
}