using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public static class Algorithms
    {
        // const string _dllPath = "DiscreteMathAlgorithms.dll";

        const string _dllPath = @"\..\..\..\Debug\DiscreteMathAlgorithms.dll";

        [DllImport(_dllPath)]
        public static extern int get_gcd(int a, int b);

        // Gets greatest commod divisor.
        public static long GetGcd(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            long r1 = Math.Max(a, b);
            long r2 = a + b - r1;
            long temp = 0;

            while (r2 != 0)
            {
                temp = r2;
                r2 = r1 % r2;
                r1 = temp;
            }

            return r1;
        }

        public static Polynom<T, R> GetGcd<T, R>(Polynom<T, R> a, Polynom<T, R> b) where R : RingBase<T>
        {
            var temp = new List<Polynom<T, R>>();
            var r1 = a;
            var r2 = b;

            while (r2.Degree >= 0)
            {
                r1.Div(r2, temp);
                r1 = r2;
                r2 = temp.Last();
            }

            return r1;
        }

        // Gets greatest common divisor and u, v such that gcd(a,b) = a * u + b * v
        public static long GetGcdEx(long a, long b, out long u, out long v)
        {
            long gcd = 0;

            if (a == 0)
            {
                u = 0;
                v = (b == 0 ? 0 : 1);
                gcd = b;
            }
            else if (b == 0)
            {
                v = 0;
                u = (a == 0 ? 0 : 1);
                gcd = a;
            }
            else
            {
                long r1 = b;
                long r2 = a % b;

                long u1 = 1;
                long u2 = 0;
                long v1 = 1;
                long v2 = 1;
                long q = 0;

                if (r2 == 0)
                    u1 = 0;
                else
                    v1 = -a / b;

                long temp = 0;

                while (r2 != 0)
                {
                    q = r1 / r2;
                    temp = r2;
                    r2 = r1 % r2;
                    r1 = temp;
                    temp = u1;
                    u1 = u2 - u1 * q;
                    u2 = temp;
                    temp = v1;
                    v1 = v2 - v1 * q;
                    v2 = temp;
                }

                u = u2;
                v = v2;
                gcd = r1;
            }

            if (gcd < 0)
            {
                gcd = -gcd;
                u = -u;
                v = -v;
            }

            return gcd;
        }

        public static IEnumerable<int[]> GetPermutations(int n)
        {
            var _permutation = new List<List<int>>();
            _permutation.Add(new List<int>());
            for (int i = 0; i < n; i++)
            {
                var _permutationTemp = new List<List<int>>();
                for (int j = 0; j < _permutation.Count; j++)
                {
                    for (int k = 0; k <= i; k++)
                    {
                        var _perm = new List<int>(_permutation[j]);
                        _perm.Insert(k, i);
                        _permutationTemp.Add(_perm);
                    }
                }
                _permutation = _permutationTemp;
            }

            return _permutation.Select(x => x.ToArray());
        }

        public static int GetInversionsCount(int[] aPermutation)
        {
            var _permCount = 0;
            for(var i = 0; i < aPermutation.Length - 1; ++i)
            {
                for(var j = i + 1; j < aPermutation.Length; ++j)
                {
                    if (aPermutation[j] < aPermutation[i])
                        _permCount++;
                }
            }
            return _permCount;
        }
    }
}
