using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class ZnRing : IRing<long>
    {
        private long FMode;

        public ZnRing(long aMode)
        {
            this.FMode = aMode;
        }

        public long One
        {
            get
            {
                return 1;
            }
        }

        public long Zero
        {
            get
            {
                return 0;
            }
        }

        public long Opposite(long a)
        {
            return this.FMode - a % this.FMode;
        }

        public long Prod(long a, long b)
        {
            return (a * b) % this.FMode;
        }

        public long Reverse(long a)
        {
            if (a == this.Zero)
                throw new DivideByZeroException(
                    String.Format("The element {0} isn't invertible on the ring {1}.", a, this));

            long u, v;
            var _gcd = Algorithms.GetGcdEx(a, this.FMode, out u, out v);
            if (_gcd != this.One)
                throw new DivideByZeroException(
                    String.Format("The element {0} isn't invertible on the ring {1}.", a, this));
            return u > 0 ? u : this.FMode + u;
        }

        public long Sum(long a, long b)
        {
            return (a + b) % this.FMode;
        }

        public bool Equals(long a, long b)
        {
            return a % this.FMode == b % this.FMode;
        }

        public string GetTexString(long a)
        {
            return a.ToString();
        }

        public override string ToString()
        {
            return String.Format("Z/{0}", this.FMode);
        }

        public long RightReverse(long a)
        {
            return this.Reverse(a);
        }

        public long LeftReverse(long a)
        {
            return this.Reverse(a);
        }

        public bool IsNaN(long a)
        {
            return false;
        }
    }
}
