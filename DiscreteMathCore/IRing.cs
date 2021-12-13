using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public interface IRing<T>
    {
        T Zero { get; }
        T One { get; }

        T Sum(T a, T b);
        T Prod(T a, T b);

        T Opposite(T a);
        T Reverse(T a);
        T RightReverse(T a);
        T LeftReverse(T a);

        bool Equals(T a, T b);

        string GetTexString(T a);

        bool IsNaN(T a);
    }

    public abstract class RingBase<T> : IRing<T>
    {
        public abstract T One { get; }

        public abstract T Zero { get; }

        public abstract bool Equals(T a, T b);

        public abstract string GetTexString(T a);

        public abstract bool IsNaN(T a);

        public abstract T LeftReverse(T a);

        public abstract T Opposite(T a);

        public abstract T Prod(T a, T b);

        public abstract T InnerReverse(T a);

        public abstract T RightReverse(T a);

        public abstract T Sum(T a, T b);

        protected Dictionary<T, T> ReverseCache = new Dictionary<T, T>();

        public T Reverse(T a)
        {
            if (!this.ReverseCache.ContainsKey(a))
            {
                this.ReverseCache[a] = this.InnerReverse(a);
            }

            return this.ReverseCache[a];

            //return this.InnerReverse(a);
        }

        public T Mult(T t, long mult)
        {
            var _sum = this.Zero;
            for (var i = 0; i < mult; i++)
            {
                _sum = this.Sum(_sum, t);
            }
            return _sum;
        }

        public T Pow(T b, long p)
        {
            var _prod = this.One;
            for (var i = 0; i < p; i++)
            {
                _prod = this.Prod(_prod, b);
            }
            return _prod;
        }

        public virtual bool IsField
        {
            get { return false; }
        }

        public virtual long Size
        {
            get { return -1; }
        }

        public virtual long SimpleSubfieldSize
        {
            get { return this.Size; }
        }

        //public virtual T Pow(T b, int p)
        //{
        //    var _res = this.One;
        //    for (var i = 0; i < p; ++i)
        //        _res = this.Prod(_res, b);
        //    return _res;
        //}
    }
}
