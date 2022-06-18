using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class Qext
    {
        private Q p;
        private Q q;
        private int r;

        public Qext(Q p, Q q, int r)
        {
            this.p = p;
            this.q = q;
            this.r = r;
        }

        public static Qext operator +(Qext qe1, Qext qe2)
        {
            if (qe1.r != qe2.r)
            {
                throw new ArgumentException("Operands fields extentions are defferent.");
            }

            return new Qext(qe1.p + qe2.p, qe1.q + qe2.q, qe1.r);
        }

        public static Qext operator -(Qext qe1, Qext qe2)
        {
            if (qe1.r != qe2.r)
            {
                throw new ArgumentException("Operands fields extentions are defferent.");
            }

            return new Qext(qe1.p - qe2.p, qe1.q - qe2.q, qe1.r);
        }

        public static Qext operator *(Qext qe1, Qext qe2)
        {
            if (qe1.r != qe2.r)
            {
                throw new ArgumentException("Operands fields extentions are defferent.");
            }

            return new Qext(
                qe1.p * qe2.p + qe1.q * qe2.q * qe1.r,
                qe1.p * qe2.q + qe1.q * qe2.p, qe1.r
                );
        }

        public static Qext operator /(Qext qe1, Qext qe2)
        {
            if (qe1.r != qe2.r)
            {
                throw new ArgumentException("Operands fields extentions are defferent.");
            }

            Q _denominator = qe2.p * qe2.p - qe2.q * qe2.q * qe1.r;

            if (_denominator == 0)
            {
                throw new DivideByZeroException();
            }

            Qext _reverseQe2 = new Qext(qe2.p / _denominator, -qe2.q / _denominator, qe1.r);
            return qe1 * _reverseQe2;
        }

        public static bool operator ==(Qext qe1, Qext qe2)
        {
            if (qe1.r != qe2.r)
            {
                throw new ArgumentException("Operands fields extentions are defferent.");
            }

            return qe1.p == qe2.p && qe1.q == qe2.q;
        }

        public static bool operator !=(Qext qe1, Qext qe2)
        {
            if (qe1.r != qe2.r)
            {
                throw new ArgumentException("Operands fields extentions are defferent.");
            }

            return qe1.p != qe2.p || qe1.q != qe2.p;
        }

        public static Qext operator -(Qext qe)
        {
            return new Qext(-qe.p, -qe.q, qe.r);
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            if(obj is Qext)
            {
                Qext qe = (Qext)obj;
                return this.p == qe.p && this.q == qe.q;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (int)(this.q.GetHashCode() ^ this.q.GetHashCode() ^ this.r);
        }

        public override string ToString()
        {
            if (this.q == 0)
            {
                return p.ToString();
            }

            if (this.p == 0 && this.q != 0)
            {
                return String.Format("{1}[{2}]", this.q, this.r);
            }

            return String.Format("({0},{1}[{2}])", this.p, this.q, this.r);
        }

        public string TexString
        {
            get
            {

                if (this.q == 0)
                {
                    return p.ToString();
                }

                if (this.p == 0 && this.q != 0)
                {
                    return String.Format("{0}\\sqrt{{{1}}}", this.q, this.r);
                }

                String _sign = "";
                if(q.Real >= 0)
                {
                    _sign = "+";
                }
                return String.Format("{0}{1}{2}\\sqrt{{{3}}}", 
                    this.p.TexString, _sign, this.q.TexString, this.r);
            }
        }
    }

    public class QextendedByNsqrt : RingBase<Qext>
    {
        private int r;

        public QextendedByNsqrt(int ext) 
        {
            this.r = ext;
        }

        public Qext getVal(Q p, Q q)
        {
            return new Qext(p, q, r);
        }

        public override Qext One
        {
            get { return new Qext(1, 0, r); }
        }

        public override Qext Zero
        {
            get { return new Qext(0, 0, r); }
        }

        public override bool Equals(Qext a, Qext b)
        {
            return a == b;
        }

        public override string GetTexString(Qext a)
        {
            return a.TexString;
        }

        public override Qext InnerReverse(Qext a)
        {
            return this.One / a;
        }

        public override bool IsNaN(Qext a)
        {
            throw new NotImplementedException();
        }

        public override Qext LeftReverse(Qext a)
        {
            return this.InnerReverse(a);
        }

        public override Qext Opposite(Qext a)
        {
            return -a;
        }

        public override Qext Prod(Qext a, Qext b)
        {
            return a * b;
        }

        public override Qext RightReverse(Qext a)
        {
            return this.InnerReverse(a);
        }

        public override Qext Sum(Qext a, Qext b)
        {
            return a + b;
        }
    }
}
