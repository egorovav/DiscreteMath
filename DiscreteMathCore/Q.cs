using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class Q
    {
        private static long MaxMult;
        private static long MaxSum;

        private long FNumerator;
        private long FDenumerator;

        static Q()
        {
            MaxMult = (long)Math.Sqrt(Int64.MaxValue);
            MaxSum = (long)Math.Sqrt(Int64.MaxValue / 2);
        }

        public Q(long aNumerator)
        {
            this.FNumerator = aNumerator;
            this.FDenumerator = 1;
        }

        public Q(long aNumerator, long aDenumerator)
        {
            if (aDenumerator == 0)
                throw new DivideByZeroException();

            long _numerator = Math.Abs(aNumerator);
            long _denumerator = Math.Abs(aDenumerator);
            long _sign = 1;
            if (Math.Sign(aNumerator) > 0 && Math.Sign(aDenumerator) < 0)
                _sign = -1;
            if (Math.Sign(aNumerator) < 0 && Math.Sign(aDenumerator) > 0)
                _sign = -1;

            var _gcd = Algorithms.GetGcd(_numerator, _denumerator);

            this.FNumerator = _sign * _numerator / _gcd;
            this.FDenumerator = _denumerator / _gcd;
        }

        public long IntegerPart
        {
            get { return this.FNumerator / this.FDenumerator; }
        }

        public Q FractionalPart
        {
            get
            {
                var _numerator = Math.Abs(this.FNumerator % this.FDenumerator);
                return new Q(_numerator, this.FDenumerator); 
            }
        }

        public static Q operator *(Q q1, Q q2)
        {
            if (q1 == null || q2 == null)
                return null;

            if (q1.FNumerator > Q.MaxMult && q2.FNumerator > Q.MaxMult)
                throw new ArgumentException(
                    String.Format("The numerator of a multiplier must be less than {0}", Q.MaxMult));
            if (q1.FDenumerator > Q.MaxMult && q2.FDenumerator > Q.MaxMult)
                throw new ArgumentException(
                    String.Format("The denumerator of a multiplier must be less than {0}", Q.MaxMult));

            var _numerator = q1.FNumerator * q2.FNumerator;
            var _denumerator = q1.FDenumerator * q2.FDenumerator;
            return new Q(_numerator, _denumerator);
        }

        public static Q operator *(Q q1, int m) 
        {
            return new Q(q1.FNumerator * m, q1.FDenumerator);
        }

        public static Q operator /(Q q1, Q q2)
        {
            if (q1 == null || q2 == null)
                return null;

            var _numerator = q1.FNumerator * q2.FDenumerator;
            var _denumerator = q1.FDenumerator * q2.FNumerator;
            return new Q(_numerator, _denumerator);
        }

        public static Q operator +(Q q1, Q q2)
        {
            if (q1 == null || q2 == null)
                return null;


            if (q1.FNumerator > Q.MaxMult && q2.FDenumerator > Q.MaxMult)
                throw new ArgumentException(
                    String.Format("The numerator of a multiplier must be less than {0}", Q.MaxMult));
            if (q1.FDenumerator > Q.MaxMult && q2.FNumerator > Q.MaxMult)
                throw new ArgumentException(
                    String.Format("The denumerator of a multiplier must be less than {0}", Q.MaxMult));

            if (q1.FDenumerator > Q.MaxMult && q2.FDenumerator > Q.MaxMult)
                throw new ArgumentException(
                    String.Format("The denumerator of a multiplier must be less than {0}", Q.MaxMult));

            var _s1 = q1.FNumerator * q2.FDenumerator;
            var _s2 = q2.FNumerator * q1.FDenumerator;

            if (_s1 > Q.MaxSum && _s2 > Q.MaxSum)
                throw new ArgumentException(
                    String.Format("The numerator of a summand must be less than {0}", Q.MaxSum));
            if (_s1 > Q.MaxSum && _s2 > Q.MaxSum)
                throw new ArgumentException(
                    String.Format("The denumerator of a summand must be less than {0}", Q.MaxSum));

            var _numerator = _s1 + _s2; 
            var _denumerator = q1.FDenumerator * q2.FDenumerator;
            return new Q(_numerator, _denumerator);
        }

        public static Q operator -(Q q)
        {
            return new Q(-q.FNumerator, q.FDenumerator);
        }

        public static Q operator -(Q q1, Q q2)
        {
            return q1 + (-q2);
        }

        public static implicit operator Q(long integer)
        {
            return new Q(integer);
        }

        public double Real
        {
            get
            {
                return (double)this.FNumerator / (double)this.FDenumerator;
            }
        }


        public static bool operator ==(Q a, Q b)
        {
            if ((object)a == null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Q a, Q b)
        {
            return !(a == b);
        }


        public static implicit operator Q(int x)
        {
            return new Q(x);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Q)
            {
                var q = (Q)obj;
                return this.FNumerator == q.FNumerator && this.FDenumerator == q.FDenumerator;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return (int)(this.FNumerator ^ this.FDenumerator);
        }

        public override string ToString()
        {
            var _ip = this.IntegerPart;
            if (_ip != 0)
            {
                var _res = this.IntegerPart.ToString();
                if(this.FractionalPart != 0)
                    _res = String.Format("{0}.{1}", this.IntegerPart, this.FractionalPart);
                return _res;
            }
            else
                if (this.FNumerator == 0)
                return "0";
            else
                return String.Format("{0}:{1}", this.FNumerator, this.FDenumerator);
        }

        public string TexString
        {
            get
            {
                var _ip = this.IntegerPart;
                if (_ip != 0)
                {
                    var _frac = this.FractionalPart == 0 ? String.Empty : this.FractionalPart.TexString;
                    return String.Format("{0}{1}", this.IntegerPart, _frac);
                }
                else
                    if (this.FNumerator == 0)
                    return "0";
                else
                {
                    var _sign = String.Empty;
                    if (this.FNumerator < 0)
                        _sign = "-";

                    return String.Format("{0}\\frac{{{1}}}{{{2}}}", _sign, Math.Abs(this.FNumerator), this.FDenumerator);
                }
            }
        }
    }

    public class Rational : RingBase<Q>
    {
        public override Q One
        {
            get
            {
                return 1;
            }
        }

        public override Q Zero
        {
            get
            {
                return 0;
            }
        }

        public override Q Opposite(Q a)
        {
            return -a;
        }

        public override Q Prod(Q a, Q b)
        {
            return a * b;
        }

        public override Q InnerReverse(Q a)
        {
            if (a == 0)
                throw new DivideByZeroException();

            return 1 / a;
        }

        public override Q Sum(Q a, Q b)
        {
            return a + b;
        }

        public override bool Equals(Q a, Q b)
        {
            return a == b;
        }

        public override string GetTexString(Q a)
        {
            return a.TexString;
        }

        public override string ToString()
        {
            return "Rational numbers";
        }

        public override Q RightReverse(Q a)
        {
            return this.Reverse(a);
        }

        public override Q LeftReverse(Q a)
        {
            return this.Reverse(a);
        }

        public override bool IsNaN(Q a)
        {
            return false;
        }

        public override bool IsField
        {
            get { return true; }
        }
    }
}
