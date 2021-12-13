using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class ZnRing : RingBase<long>, IEnumerable<long>
    {
        private long FMode;
        private List<long> FValues;

        public ZnRing(long aMode)
        {
            this.FMode = aMode;
            this.FValues = new List<long>();
            for(long i = 0; i < aMode; ++i)
            {
                this.FValues.Add(i);
            }
        }

        public override long One
        {
            get
            {
                return 1;
            }
        }

        public override long Zero
        {
            get
            {
                return 0;
            }
        }

        public override long Opposite(long a)
        {
            if (a == 0)
                return 0;

            return this.FMode - a % this.FMode;
        }

        public override long Prod(long a, long b)
        {
            return (a * b) % this.FMode;
        }

        public override long InnerReverse(long a)
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

        public override long Sum(long a, long b)
        {
            return (a + b) % this.FMode;
        }

        public override bool Equals(long a, long b)
        {
            return a % this.FMode == b % this.FMode;
        }

        public override string GetTexString(long a)
        {
            return a.ToString();
        }

        public override string ToString()
        {
            return String.Format("Z/{0}", this.FMode);
        }

        public override long RightReverse(long a)
        {
            return this.Reverse(a);
        }

        public override long LeftReverse(long a)
        {
            return this.Reverse(a);
        }

        public override bool IsNaN(long a)
        {
            return false;
        }

        public IEnumerator<long> GetEnumerator()
        {
            return this.FValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.FValues.GetEnumerator();
        }

        private int FIsField = 0;
        public override bool IsField
        {
            get
            {
                if (this.FIsField == 0)
                {
                    this.FIsField = 1;
                    var _sqr = Math.Sqrt(this.FMode);
                    for (var i = 2; i < _sqr + 1; i++)
                    {
                        if(this.FMode % i == 0)
                        {
                            this.FIsField = -1;
                            break;
                        }
                    }
                }

                return this.FIsField > 0;
            }
        }

        public override long Size
        {
            get { return this.FMode; }
        }
    }
}
