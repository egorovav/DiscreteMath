using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class MRing<T, R> : IRing<Matrix<T, R>> where R : IRing<T>
    {
        private R FRing;
        private int FSize;

        public MRing(R aRing, int aSize)
        {
            this.FRing = aRing;
            this.FSize = aSize;
        }

        public Matrix<T, R> One
        {
            get
            {
                return Matrix<T, R>.GetOne(this.FRing, this.FSize);
            }
        }

        public Matrix<T, R> Zero
        {
            get
            {
                return new Matrix<T, R>(this.FRing, this.FSize);
            }
        }

        public bool Equals(Matrix<T, R> a, Matrix<T, R> b)
        {
            return a == b;
        }

        public string GetTexString(Matrix<T, R> a)
        {
            return a.TexString;
        }

        public Matrix<T, R> Opposite(Matrix<T, R> a)
        {
            var _minusOne = this.FRing.Opposite(this.FRing.One);
            var _res = new Matrix<T, R>(a);
            _res.Mult(_minusOne);
            return _res;
        }

        public Matrix<T, R> Prod(Matrix<T, R> a, Matrix<T, R> b)
        {
            return a * b;
        }

        public Matrix<T, R> Reverse(Matrix<T, R> a)
        {
            return a.Reverse;
        }

        public Matrix<T, R> Sum(Matrix<T, R> a, Matrix<T, R> b)
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

        public Matrix<T, R> RightReverse(Matrix<T, R> a)
        {
            throw new NotImplementedException();
        }

        public Matrix<T, R> LeftReverse(Matrix<T, R> a)
        {
            throw new NotImplementedException();
        }

        public Matrix<T, R> CreateVandermond(Dictionary<T, T> aArgValues)
        {
            var _rowCount = aArgValues.Count;
            var _colCount = _rowCount + 1;
            var _length = _rowCount * _colCount;
            var _matrix = new Matrix<T, R>(this.FRing, _rowCount, _colCount);
            var _count = 0;
            var _values = new T[_rowCount, _colCount];
            foreach (var _pair in aArgValues)
            {
                var p = this.FRing.One;
                for (int i = 0; i < _rowCount; i++)
                {
                    _values[_count, i] = p;
                    p = this.FRing.Prod(p, _pair.Key);
                }
                _values[_count,  _colCount - 1] = _pair.Value;
                _count++;
            }

            return _matrix;
        }

        public Matrix<T, R> SolveLs(Matrix<T, R> aMatrix, T[] aValues)
        {
            return null; 
        }

        public bool IsNaN(Matrix<T, R> a)
        {
            return false;
        }
    }
}
