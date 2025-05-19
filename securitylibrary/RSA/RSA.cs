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
        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q;
            return (int)PowerMod(M, e, n);
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int n = p * q;
            int phi = (p - 1) * (q - 1);

            ExtendedEuclid extendedEuclid = new ExtendedEuclid();
            int d = extendedEuclid.GetMultiplicativeInverse(e, phi);

            return (int)PowerMod(C, d, n);
        }

        private long PowerMod(long num, long power, long mod)
        {
            long result = 1;
            num = num % mod;

            while (power > 0)
            {
                if (power % 2 == 1)
                {
                    result = (result * num) % mod;
                }

                power = power / 2;
                num = (num * num) % mod;
            }

            return result;
        }
    }
}
