using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class Matrix<T, R> where R: IRing<T>
    {
        private T[,] FValues;
        private R FRing;

        public Matrix(R aRing, T[,] aValues)
        {
            this.FRing = aRing;
            this.FValues = aValues;
        }

        public Matrix(R aRing, int aRowCount, int aColumnCount)
        {
            this.FRing = aRing;
            this.FValues = new T[aRowCount, aColumnCount];
            for (var i = 0; i < aRowCount; ++i)
                for (var j = 0; j < aColumnCount; ++j)
                    this.FValues[i, j] = aRing.Zero;
        }

        public Matrix(R aRing, int aSize) 
            : this(aRing, aSize, aSize)
        {

        }

        public Matrix(Matrix<T, R> aMatrix)
        {
            this.FRing = aMatrix.FRing;
            this.FValues = new T[aMatrix.RowCount, aMatrix.ColumnCount];
            for (var i = 0; i < aMatrix.RowCount; ++i)
                for (var j = 0; j < aMatrix.ColumnCount; ++j)
                    this.FValues[i, j] = aMatrix.FValues[i, j];
        }

        public static Matrix<T, R> GetOne(R aRing, int aSize)
        {
            var _one = new Matrix<T, R>(aRing, aSize);
            for (var i = 0; i < aSize; ++i)
                _one.FValues[i, i] = aRing.One;
            return _one;
        }

        public int RowCount
        {
            get { return this.FValues.GetLength(0); }
        }

        public int ColumnCount
        {
            get { return this.FValues.GetLength(1); }
        }

        public static Matrix<T, R> operator +(Matrix<T, R> A, Matrix<T, R> B)
        {
            if (A.RowCount != B.RowCount || A.ColumnCount != B.ColumnCount)
                throw new ArgumentException();

            var _res = new Matrix<T, R>(A.FRing, A.RowCount, B.RowCount);
            for(var i = 0; i < A.RowCount; ++i)
            {
                for(var j = 0; j < A.ColumnCount; ++j)
                {
                    _res.FValues[i, j] = A.FRing.Sum(A.FValues[i, j], B.FValues[i, j]); 
                }
            }
            return _res;
        }

        public static Matrix<T, R> operator *(Matrix<T, R> A, Matrix<T, R> B)
        {
            if (A.ColumnCount != B.RowCount)
                throw new ArgumentException();

            var _res = new Matrix<T, R>(A.FRing, A.RowCount, B.ColumnCount);
            for(var i = 0; i < A.RowCount; ++i)
            {
                for(var j = 0; j < B.ColumnCount; ++j)
                {
                    var _ij = A.FRing.Zero;
                    for (var k = 0; k < A.ColumnCount; ++k)
                    {
                        _ij = A.FRing.Sum(_ij, A.FRing.Prod(A.FValues[i, k], B.FValues[k, j]));
                    }
                    _res.FValues[i, j] = _ij;
                }
            }

            return _res;
        }

        //private T MultColumnByRow(Matrix<T, R> A, int aRowNum, Matrix<T, R> B, int aColNum)
        //{
        //    if (A.ColumnCount != B.RowCount)
        //        throw new ArgumentException();

        //    var _res = A.FRing.Zero;
        //    for (var i = 0; i < A.ColumnCount; ++i)
        //    {
        //        _res = A.FRing.Sum(_res, A.FRing.Prod(A.FValues[aRowNum, i], B.FValues[i, aColNum]));
        //    }
        //    return _res;
        //}

        public static bool operator ==(Matrix<T, R> A, Matrix<T, R> B)
        {
            if ((object)A == null || (object)B == null)
            {
                return (object)A == null && (object)B == null;
            }

            if (A.RowCount != B.RowCount || A.ColumnCount != B.ColumnCount)
                return false;

            for (var i = 0; i < A.RowCount; ++i)
                for (var j = 0; j < A.ColumnCount; ++j)
                    if (!A.FRing.Equals(A.FValues[i, j], B.FValues[i, j]))
                        return false;

            return true;
        }

        public static bool operator !=(Matrix<T, R> A, Matrix<T, R> B)
        {
            return !(A == B);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Matrix<T, R>)
            {
                var M = (Matrix<T, R>)obj;
                return this == M;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            var _res = 0;
            for (var i = 0; i < this.RowCount; ++i)
                for (var j = 0; j < this.ColumnCount; ++j)
                    _res ^= this.FValues[i, j].GetHashCode();

            return _res;
        }

        public override string ToString()
        {
            var _sb = new StringBuilder();
            for(var i = 0; i < this.RowCount; ++i)
            {
                for(var j = 0; j < this.ColumnCount; ++j)
                {
                    _sb.AppendFormat("{0}\t", this.FValues[i, j]);
                }
                _sb.AppendLine();
            }
            return _sb.ToString();
        }

        public string TexString
        {
            get
            {
                var _sb = new StringBuilder();
                _sb.AppendLine(@"\begin{pmatrix}");
                for (var i = 0; i < this.RowCount; ++i)
                {
                    for (var j = 0; j < this.ColumnCount - 1; ++j)
                    {
                        _sb.AppendFormat("{0}&", this.FRing.GetTexString(this.FValues[i, j]));
                    }
                    _sb.Append(this.FRing.GetTexString(this.FValues[i, this.ColumnCount - 1]));              
                    _sb.AppendLine(@"\\");
                }
                _sb.AppendLine(@"\end{pmatrix}");
                return _sb.ToString();
            }
        }

        public void Mult(T aVal)
        {
            for (var i = 0; i < this.RowCount; ++i)
                for (var j = 0; j < this.ColumnCount; ++j)
                    this.FValues[i, j] = this.FRing.Prod(this.FValues[i, j], aVal);

            this.FIsInvalidDeterminant = true;
            this.FIsInvalidAdjugate = true;
        }

        private List<int[]> FPermutations;

        private bool FIsInvalidDeterminant = true;
        private T FDeterminant;
        public T Determinant
        {
            get
            {
                if (this.RowCount != this.ColumnCount)
                    throw new ArgumentException();

                if(this.FIsInvalidDeterminant)
                {
                    var _minusOne = this.FRing.Opposite(this.FRing.One);
                    var _res = this.FRing.Zero;
                    if (this.FPermutations == null)
                        this.FPermutations = Algorithms.GetPermutations(this.RowCount).ToList(); ;

                    foreach(var _perm in this.FPermutations)
                    {
                        var m = this.FRing.One;
                        for(var j = 0; j < this.RowCount; ++j)
                        {
                            m = this.FRing.Prod(m, this.FValues[j, _perm[j]]);
                        }
                        var _ic = Algorithms.GetInversionsCount(_perm);
                        if(_ic % 2 == 1)
                        {
                            m = this.FRing.Prod(m, _minusOne);
                        }
                        _res = this.FRing.Sum(_res, m);
                    }
                    this.FDeterminant = _res;
                    this.FIsInvalidDeterminant = false;
                }

                return this.FDeterminant;
            }
        }

        private bool FIsInvalidAdjugate = true;
        private Matrix<T, R> FAdjugate;
        public Matrix<T, R> Adjucate
        {
            get
            {
                if (this.RowCount != this.ColumnCount)
                    throw new ArgumentException();

                if (this.FIsInvalidAdjugate)
                {
                    var _res = new Matrix<T, R>(this.FRing, this.RowCount);
                    for (var i = 0; i < this.RowCount; ++i)
                    {
                        for (var j = 0; j < this.ColumnCount; ++j)
                        {
                            var _subMatrix = new Matrix<T, R>(this);
                            for (var k = 0; k < this.RowCount; ++k)
                            {
                                _subMatrix.FValues[i, k] = this.FRing.Zero;
                                _subMatrix.FValues[k, j] = this.FRing.Zero;
                            }
                            _subMatrix.FValues[i, j] = this.FRing.One;
                            _res.FValues[j, i] = _subMatrix.Determinant;
                        }
                    }

                    this.FAdjugate = _res;
                    this.FIsInvalidAdjugate = false;
                }

                return this.FAdjugate;
            }
        }

        public Matrix<T, R> Reverse
        {
            get
            {
                var D = this.Determinant;
                //if (D.Equals(this.FRing.Zero))
                if(this.FRing.Equals(D, this.FRing.Zero))
                    throw new DivideByZeroException(
                        String.Format("The matrix\n{0}isn't invertible on the ring {1}.", this, this.FRing));

                var _dr = this.FRing.Zero;
                try
                {
                    _dr = this.FRing.Reverse(D);
                }
                catch (DivideByZeroException ex)
                {
                    var _sb = new StringBuilder();
                    _sb.AppendLine(ex.Message);
                    _sb.AppendFormat(
                        String.Format("The matrix\n{0}isn't invertible on the ring {1}.", this, this.FRing));
                    throw new DivideByZeroException(_sb.ToString());
                }
               


                var _copy = new Matrix<T, R>(this);
                var _adj = _copy.Adjucate;
                _adj.Mult(_dr);
                return _adj;

            }
        }

        private void ChangeRows(int aRowNum1, int aRowNum2)
        {
            var _temp = this.FRing.Zero;
            for (int i = 0; i < this.ColumnCount; ++i)
            {
                _temp = this.FValues[aRowNum1, i];
                this.FValues[aRowNum1, i] = this.FValues[aRowNum2, i];
                this.FValues[aRowNum2, i] = _temp;
            }
        }

        public void ConvertToDiag()
        {
            var _temp = this.FRing.Zero;
            var _colNum = -1;
            var _rowNum = 0;
            var _vars = new int[this.ColumnCount];
            for (int i = 0; i < this.ColumnCount; ++i)
                _vars[i] = i;

            while (_rowNum < this.RowCount && _colNum < this.ColumnCount - 1)
            {
                _colNum++;
                int _noZero = -1;
                for (int j = _rowNum; j < this.RowCount; ++j)
                {
                    if (!this.FRing.Equals(this.FValues[j, _colNum], this.FRing.Zero))
                    {
                        _noZero = j;
                        break;
                    }
                }
                if (_noZero < 0)
                    continue;

                if (_noZero > _rowNum)
                {
                    this.ChangeRows(_noZero, _rowNum);
                    var t = _vars[_noZero];
                    _vars[_noZero] = _vars[_rowNum];
                    _vars[_rowNum] = t;
                }

                var _ii = this.FValues[_rowNum, _colNum];
                var _iir = this.FRing.Reverse(_ii);
                this.FValues[_rowNum, _colNum] = this.FRing.One;
                for (int j = _colNum + 1; j < this.ColumnCount; ++j)
                {
                    this.FValues[_rowNum, j] = this.FRing.Prod(_iir, this.FValues[_rowNum, j]);
                }

                for (int j = _rowNum + 1; j < this.RowCount; ++j)
                {
                    var _ji = this.FValues[j, _colNum];
                    this.FValues[j, _colNum] = this.FRing.Zero;
                    for (int k = _colNum + 1; k < this.ColumnCount; ++k)
                    {
                        _temp = this.FRing.Opposite(this.FRing.Prod(this.FValues[_rowNum, k], _ji));
                        this.FValues[j, k] = this.FRing.Sum(this.FValues[j, k], _temp);
                    }
                }
                _rowNum++;
            }
        }

        public void ConvertToE()
        {
            this.ConvertToDiag();

            var _temp = this.FRing.Zero;
            for (int i = this.RowCount - 1; i > 0; --i)
            {
                var _colNum = -1;
                for (int j = 0; j < this.ColumnCount; ++j)
                {
                    if (!this.FRing.Equals(this.FValues[i, j], this.FRing.Zero))
                    {
                        _colNum = j;
                        break;
                    }
                }

                if (_colNum < 0)
                    continue;

                for (int j = 0; j < i; ++j)
                {
                    var _ji = this.FValues[j, _colNum];
                    this.FValues[j, _colNum] = this.FRing.Zero;
                    for (int k = _colNum + 1; k < this.ColumnCount; ++k)
                    {
                        _temp = this.FRing.Opposite(this.FRing.Prod(this.FValues[i, k], _ji));
                        this.FValues[j, k] = this.FRing.Sum(this.FValues[j, k], _temp);
                    }
                }
            }
        }

        // return the set of solvation as x(1)c(1)+...+x(n-1)c(n-1)+c_n, where
        // c(1),...,c(n) the columns of the returned matrix
        // x(1),...,x(n) arbitrary elements of the ring.  
        public static Matrix<T, R> SolveLs(Matrix<T, R> aLsMatrix)
        {
            aLsMatrix.ConvertToE();

            var _freeVarsCnt = aLsMatrix.ColumnCount - aLsMatrix.RowCount - 1;
            Matrix<T, R> _res = null;
            var _boundVarIndexes = new List<int>();
            for (int i = aLsMatrix.RowCount - 1; i >= 0; --i)
            {
                int _nonZeroIndex = -1;
                for (int j = 0; j < aLsMatrix.ColumnCount; ++j)
                {
                    if (!aLsMatrix.FRing.Equals(aLsMatrix.FValues[i, j], aLsMatrix.FRing.Zero))
                    {
                        _nonZeroIndex = j;
                        break;
                    }
                }

                if (_nonZeroIndex == aLsMatrix.ColumnCount - 1)
                    //return new MRing<T, R>(aLsMatrix.FRing, 1).Zero;
                    return null;

                if (_nonZeroIndex < 0)
                {
                    _freeVarsCnt++;
                    continue;
                }

                if (_res == null)
                    _res = new Matrix<T, R>(aLsMatrix.FRing, aLsMatrix.ColumnCount - 1, _freeVarsCnt + 1);

                var _cnt = 1;
                for (int j = aLsMatrix.ColumnCount - 1; j > _nonZeroIndex; --j)
                {
                    if (!_boundVarIndexes.Contains(j))
                    {
                        if (j == aLsMatrix.ColumnCount - 1)
                            _res.FValues[_nonZeroIndex, _res.ColumnCount - _cnt] = aLsMatrix.FValues[i, j];
                        else
                            _res.FValues[_nonZeroIndex, _res.ColumnCount - _cnt] =
                                aLsMatrix.FRing.Opposite(aLsMatrix.FValues[i, j]);
                        _cnt++;
                    }
                }

                _boundVarIndexes.Add(_nonZeroIndex);
            }

            var _colNum = 0;
            for (int i = 0; i < aLsMatrix.ColumnCount - 1; ++i)
            {
                if (!_boundVarIndexes.Contains(i))
                {
                    _res.FValues[i, _colNum] = aLsMatrix.FRing.One;
                    _colNum++;
                }
            }

            return _res;
        }

        public T[,] Values
        {
            get
            {
                var _res = new T[this.RowCount, this.ColumnCount];
                Array.Copy(this.FValues, _res, this.RowCount * this.ColumnCount);
                return _res; 
            }
        }
    }
}
