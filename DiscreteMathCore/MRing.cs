using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class MRing<T, R> : RingBase<Matrix<T, R>> where R : RingBase<T>
    {
        private R FRing;
        private int FSize;

        public MRing(R aRing, int aSize)
        {
            this.FRing = aRing;
            this.FSize = aSize;
        }

        public override Matrix<T, R> One
        {
            get
            {
                return Matrix<T, R>.GetOne(this.FRing, this.FSize);
            }
        }

        public override Matrix<T, R> Zero
        {
            get
            {
                return new Matrix<T, R>(this.FRing, this.FSize);
            }
        }

        public override bool Equals(Matrix<T, R> a, Matrix<T, R> b)
        {
            return a == b;
        }

        public override string GetTexString(Matrix<T, R> a)
        {
            return a.TexString;
        }

        public override Matrix<T, R> Opposite(Matrix<T, R> a)
        {
            var _minusOne = this.FRing.Opposite(this.FRing.One);
            var _res = new Matrix<T, R>(a);
            _res.Mult(_minusOne);
            return _res;
        }

        public override Matrix<T, R> Prod(Matrix<T, R> a, Matrix<T, R> b)
        {
            return a * b;
        }

        public override Matrix<T, R> InnerReverse(Matrix<T, R> a)
        {
            if (a.Equals(One))
                return One;

            return a.Reverse;
        }

        public override Matrix<T, R> Sum(Matrix<T, R> a, Matrix<T, R> b)
        {
            return a + b;
        }

        public override string ToString()
        {
            return String.Format("Matrix {0}x{1} on {2}", this.FSize, this.FSize, this.FRing);
        }

        public Matrix<T, R> GetMatrixByElement(T aVal)
        {
            var _one = this.One;
            _one.Mult(aVal);
            return _one;
        }

        public override Matrix<T, R> RightReverse(Matrix<T, R> a)
        {
            throw new NotImplementedException();
        }

        public override Matrix<T, R> LeftReverse(Matrix<T, R> a)
        {
            throw new NotImplementedException();
        }

        public Matrix<T, R> SolveLs(Matrix<T, R> aMatrix, T[] aValues)
        {
            return null; 
        }

        public override bool IsNaN(Matrix<T, R> a)
        {
            return false;
        }
    }
}
