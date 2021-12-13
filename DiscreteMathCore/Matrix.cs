using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class Matrix<T, R> where R: RingBase<T>
    {
        private T[,] FValues;
        private R FRing;

        public Matrix(R aRing, T[,] aValues)
            :this(aRing, aValues.GetLength(0), aValues.GetLength(1))
        {
            for (var i = 0; i < aValues.GetLength(0); ++i)
            {
                for (var j = 0; j < aValues.GetLength(1); ++j)
                {
                    if (aValues[i, j] != null)
                        this.FValues[i, j] = aValues[i, j];
                    else
                        this.FValues[i, j] = aRing.Zero;
                }
            }
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

        public Matrix<T, R> Transponent
        {
            get
            {
                var _values = new T[this.ColumnCount, this.RowCount];
                for(var i = 0; i < this.ColumnCount; i++)
                {
                    for(var j = 0; j < this.RowCount; j++)
                    {
                        _values[i, j] = this.FValues[j, i];
                    }
                }

                return new Matrix<T, R>(this.FRing, _values);
            }
        }

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
                    if (this.FRing.IsField)
                    {
                        var _matrix = new Matrix<T, R>(this.FRing, this.FValues);
                        var _perm = _matrix.ConvertToDiag();
                        var _res1 = this.FRing.One;
                        for (var i = 0; i < _matrix.RowCount; i++)
                        {
                            _res1 = this.FRing.Prod(_res1, _matrix.FValues[i, i]);
                        }

                        var _ic = Algorithms.GetInversionsCount(_perm);
                        var _even = _ic % 2 == 0 ? 1 : -1;
                        if(_even < 0)
                        {
                            _res1 = this.FRing.Opposite(_res1);
                        }

                        this.FDeterminant = _res1;
                    }
                    else
                    {
                        var _matrixStack = new Stack<Matrix<T, R>>();
                        _matrixStack.Push(this);
                        var _multStack = new Stack<T>();
                        _multStack.Push(this.FRing.One);
                        var _signStack = new Stack<int>();
                        _signStack.Push(1);

                        var _res = this.FRing.Zero;

                        while (_matrixStack.Count > 0)
                        {
                            var _matrix = _matrixStack.Pop();
                            var _mult = _multStack.Pop();
                            var _sign = _signStack.Pop();

                            if (_matrix.RowCount == 1)
                            {
                                var _addition = _matrix.FValues[0, 0];
                                _addition = this.FRing.Prod(_addition, _mult);
                                if (_sign < 0)
                                {
                                    _addition = this.FRing.Opposite(_addition);
                                }

                                _res = this.FRing.Sum(_res, _addition);

                                continue;
                            }

                            var _maxZeroInRow = 0;
                            var _rowIndex = 0;
                            for (var i = 0; i < _matrix.RowCount; i++)
                            {
                                var _zeroCount = 0;
                                for (var j = 0; j < _matrix.ColumnCount; j++)
                                {
                                    if (_matrix.FRing.Equals(_matrix.FValues[i, j], _matrix.FRing.Zero))
                                    {
                                        _zeroCount++;
                                    }
                                }

                                if (_zeroCount > _maxZeroInRow)
                                {
                                    _maxZeroInRow = _zeroCount;
                                    _rowIndex = i;
                                }
                            }

                            var _maxZeroInColumn = 0;
                            var _colIndex = 0;
                            for (var i = 0; i < _matrix.ColumnCount; i++)
                            {
                                var _zeroCount = 0;
                                for (var j = 0; j < _matrix.RowCount; j++)
                                {
                                    if (_matrix.FRing.Equals(_matrix.FValues[j, i], _matrix.FRing.Zero))
                                    {
                                        _zeroCount++;
                                    }
                                }

                                if (_zeroCount > _maxZeroInColumn)
                                {
                                    _maxZeroInColumn = _zeroCount;
                                    _colIndex = i;
                                }
                            }

                            if (_maxZeroInRow + _maxZeroInColumn == 0)
                            {
                                var _addition = _matrix.GetDeterminant();
                                _addition = this.FRing.Prod(_addition, _mult);
                                if (_sign < 0)
                                {
                                    _addition = this.FRing.Opposite(_addition);
                                }

                                _res = this.FRing.Sum(_res, _addition);
                            }
                            else
                            {
                                if (_maxZeroInRow >= _maxZeroInColumn)
                                {
                                    for (var i = 0; i < _matrix.ColumnCount; i++)
                                    {
                                        var _item = _matrix.FValues[_rowIndex, i];
                                        if (!this.FRing.Equals(_item, this.FRing.Zero))
                                        {
                                            var _minor = _matrix.GetMinor(_rowIndex, i);
                                            var _even = (_rowIndex + i) % 2 == 0 ? 1 : -1;

                                            _matrixStack.Push(_minor);
                                            _multStack.Push(this.FRing.Prod(_item, _mult));
                                            _signStack.Push(_sign * _even);
                                        }
                                    }
                                }
                                else
                                {
                                    for (var i = 0; i < _matrix.RowCount; i++)
                                    {
                                        var _item = _matrix.FValues[i, _colIndex];
                                        if (!this.FRing.Equals(_item, this.FRing.Zero))
                                        {
                                            var _minor = _matrix.GetMinor(i, _colIndex);
                                            var _even = (_colIndex + i) % 2 == 0 ? 1 : -1;

                                            _matrixStack.Push(_minor);
                                            _multStack.Push(this.FRing.Prod(_item, _mult));
                                            _signStack.Push(_sign * _even);
                                        }
                                    }
                                }
                            }
                        }

                        this.FDeterminant = _res;
                    }
                    this.FIsInvalidDeterminant = false;
                }

                return this.FDeterminant;
            }
        }

        public Matrix<T, R> GetMinor(int aRowIndex, int aColumnIndex)
        {
            var _subValues = new T[this.RowCount - 1, this.ColumnCount - 1];
            var _listRows = new List<List<T>>();
            for (var j = 0; j < this.RowCount; j++)
            {
                if (j == aRowIndex)
                {
                    continue;
                }

                var _row = new List<T>();
                for (var k = 0; k < this.ColumnCount; k++)
                {
                    if (k == aColumnIndex)
                    {
                        continue;
                    }

                    _row.Add(this.FValues[j, k]);
                }
                _listRows.Add(_row);
            }

            for (var j = 0; j < _listRows.Count; j++)
            {
                var _row = _listRows[j];
                for (var k = 0; k < _row.Count; k++)
                {
                    _subValues[j, k] = _row[k];
                }
            }

            return new Matrix<T, R>(this.FRing, _subValues);
        }

        private T GetDeterminant()
        {
            var _minusOne = this.FRing.Opposite(this.FRing.One);
            var _res = this.FRing.Zero;
            var _permutationIterator = new PermutationIterator(this.RowCount);
            while (_permutationIterator.MoveNext())
            {
                var _perm = _permutationIterator.Current;
                var m = this.FRing.One;
                for (var j = 0; j < this.RowCount; ++j)
                {
                    m = this.FRing.Prod(m, this.FValues[j, _perm[j]]);
                }
                var _ic = Algorithms.GetInversionsCount(_perm.ToArray());
                if (_ic % 2 == 1)
                {
                    m = this.FRing.Prod(m, _minusOne);
                }
                _res = this.FRing.Sum(_res, m);
            }

            return _res;
        }

        private bool FIsInvalidAdjugate = true;
        private Matrix<T, R> FAdjugate;
        public Matrix<T, R> Adjugate
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
                var _adj = _copy.Adjugate;
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

        public int[] ConvertToDiag()
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
                var _row = new T[this.ColumnCount];
                for (int j = _colNum + 1; j < this.ColumnCount; ++j)
                {
                    _row[j] = this.FRing.Prod(_iir, this.FValues[_rowNum, j]);
                }

                for (int j = _rowNum + 1; j < this.RowCount; ++j)
                {
                    var _ji = this.FValues[j, _colNum];
                    this.FValues[j, _colNum] = this.FRing.Zero;
                    for (int k = _colNum + 1; k < this.ColumnCount; ++k)
                    {
                        _temp = this.FRing.Opposite(this.FRing.Prod(_row[k], _ji));
                        this.FValues[j, k] = this.FRing.Sum(this.FValues[j, k], _temp);
                    }
                }
                _rowNum++;
            }

            return _vars;
        }

        public void ConvertToE()
        {
            this.ConvertToDiag();

            var _temp = this.FRing.Zero;
            for (int i = this.RowCount - 1; i >= 0; --i)
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

                var _noZero = this.FValues[i, _colNum];
                var _iir = this.FRing.Reverse(_noZero);
                this.FValues[i, _colNum] = this.FRing.One;

                for (var k = _colNum + 1; k < this.ColumnCount; k++)
                {
                    this.FValues[i, k] = this.FRing.Prod(this.FValues[i, k], _iir);
                }

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

            if (_res == null)
                _res = new Matrix<T, R>(aLsMatrix.FRing, aLsMatrix.ColumnCount - 1, _freeVarsCnt + 1);
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

        public Polynom<T, R> CharPolynom
        {
            get
            {
                if (this.RowCount != this.ColumnCount)
                    throw new ArgumentException();

                var _msize = this.RowCount;
                var _pring = new Rx<T, R>(this.FRing);
                var _mp = new MRing<Polynom<T, R>, Rx<T, R>>(_pring, _msize);

                var _ax_vals = new Polynom<T, R>[_msize, _msize];
                var _ex_vals = new Polynom<T, R>[_msize, _msize];
                for (var i = 0; i < _msize; ++i)
                    for (var j = 0; j < _msize; ++j)
                    {
                        _ax_vals[i, j] = _pring.GetPolynomByElement(this.FValues[i, j]);
                        if (i == j)
                            _ex_vals[i, j] = new Polynom<T, R>(this.FRing, new T[] { this.FRing.Zero, this.FRing.One });
                        else
                            _ex_vals[i, j] = _pring.GetPolynomByElement(this.FRing.Zero);
                    }

                var _Ax = new Matrix<Polynom<T, R>, Rx<T, R>>(_pring, _ax_vals);
                var _EX = new Matrix<Polynom<T, R>, Rx<T, R>>(_pring, _ex_vals);
                var _xi = (_EX + _mp.Opposite(_Ax)).Determinant;
                return _xi;
            }
        }
    }
}
