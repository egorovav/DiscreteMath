using Approximation;
using DiscreteMathCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PolynomR = DiscreteMathCore.Polynom<DiscreteMathCore.Q, DiscreteMathCore.Rational>;

namespace DiscreteMathConsole
{
    class Program
    {
        const string _dllPath =@"\..\..\..\Debug\DiscreteMathAlgorithms.dll";
        static void Main(string[] args)
        {
            HamiltonCayleyTest();

            Console.WriteLine("Finish.");

            Console.ReadLine();
        }

        private static void HamiltonCayleyTest()
        {
            var _msize = 4;
            var _rational = new Rational();
            var _mring = new MRing<Q, Rational>(_rational, _msize);
            var _pring = new Rx<Q, Rational>(_rational);
            var _mp = new MRing<Polynom<Q, Rational>, Rx<Q, Rational>>(_pring, _msize);
            //var _pm = new Rx<Matrix<Q, Rational>, MRing<Q, Rational>>(_mring);

            var _a_vals = new Q[,]
                {
                    { 4, 8, 3, 6 },
                    { -1, -2, -1, -2 },
                    { 3, 6, 4, 8 },
                    { -2, -4, -2, -4 }
                };

            var A = new Matrix<Q, Rational>(_rational, _a_vals);


            var _ax_vals = new Polynom<Q, Rational>[_msize, _msize];
            var _ex_vals = new Polynom<Q, Rational>[_msize, _msize];
            for (var i = 0; i < _msize; ++i)
                for (var j = 0; j < _msize; ++j)
                {
                    _ax_vals[i, j] = _pring.GetPolynomByElement(_a_vals[i, j]);
                    if (i == j)
                        _ex_vals[i, j] = new Polynom<Q, Rational>(_rational, new Q[] { 0, 1 });
                    else
                        _ex_vals[i, j] = _pring.GetPolynomByElement(0);
                }

            var _Ax = new Matrix<Polynom<Q, Rational>, Rx<Q, Rational>>(_pring, _ax_vals);
            var _EX = new Matrix<Polynom<Q, Rational>, Rx<Q, Rational>>(_pring, _ex_vals);
            var _xi = (_EX + _mp.Opposite(_Ax)).Determinant;

            var _xi_coeffs = _xi.Coeffs;

            var _mcoeffs = new Matrix<Q, Rational>[_xi_coeffs.Length];
            for (var i = 0; i < _xi_coeffs.Length; ++i)
            {
                _mcoeffs[i] = _mring.One;
                _mcoeffs[i].Mult(_xi_coeffs[i]);
            }

            var _m_xi = new Polynom<Matrix<Q, Rational>, MRing<Q, Rational>>(_mring, _mcoeffs);
            var _res = _m_xi.GetValue(A);

            Console.WriteLine(_res);
        }

        private static string ChequeVectorSpace(IRing<long> aRing)
        {
            var _sb = new StringBuilder();
            string _res = null;

            var _colCount = 2;
            var _rowCount = 4;

            var _size = _rowCount * _colCount;

            var _max = Math.Pow(_rowCount, _size);

            //for(var i = 0; i < _rowCount; ++i)
            //    _outerMult[i, 1] = i;

            var _temp = 0;
            var _outerMult = new int[_rowCount, _colCount];

            for (var i = 0; i < _max; ++i)
            {              
                _temp = i;
                for(var j = 0; j < _size; ++j)
                {
                    var _row = j / _colCount;
                    var _col = j % _colCount;
                    var _val = _temp % _rowCount;

                    if(_col == 1 && _row != _val)
                    {
                        _outerMult = null;
                        break;
                    }

                    _outerMult[_row, _col] = _val;
                    _temp /= _rowCount;
                }

                if (_outerMult != null)
                {
                    _res = ChequeAx(_outerMult, aRing);
                    if (_res != null)
                    {
                        _sb.AppendLine(_res);
                        _res = null;
                    }
                }
                else
                    _outerMult = new int[_rowCount, _colCount];
            }

            return _sb.ToString();
        }

        private static string ChequeAx(int[,] OuterMult, IRing<long> aRing)
        {
            var _sb = new StringBuilder(OuterMultToString(OuterMult));
            _sb.AppendLine();

            var _res = ChequeAx1(OuterMult);
            if (_res != null)
            {
                _sb.AppendFormat("Axiom 1 is not correct: {0}\n", _res);
                _res = null;
                return null;

            }

            _res = ChequeAx2(OuterMult, aRing);
            if (_res != null)
            {
                _sb.AppendFormat("Axiom 2 is not correct: {0}\n", _res);
                _res = null;
                return null;
            }

            _res = ChequeAx3(OuterMult, aRing);
            if (_res != null)
            {
                _sb.AppendFormat("Axiom 3 is not correct: {0}\n", _res);
                
            }

            return _sb.ToString();
        }

        private static string ChequeAx1(int[,] OuterMult)
        {
            var _rowCount = OuterMult.GetLength(0);
            var _colCount = OuterMult.GetLength(1);

            var _GF2 = new ZnRing(_colCount);

            for(var i = 0; i < _rowCount; ++i)
                for(var j = 0; j < _colCount; ++j)
                    for(var k = 0; k < _colCount; ++k)
                    {
                        var _left = OuterMult[OuterMult[i, j], k];
                        var _right = OuterMult[i, _GF2.Prod(j, k)];
                        if ( _left != _right)
                            return String.Format("({0}o{1})o{2}={3} != {0}o({1}{2})={4}", i, j, k, _left, _right);
                    }
            return null;
        }

        private static string ChequeAx2(int[,] OuterMult, IRing<long> aRing)
        {
            var _rowCount = OuterMult.GetLength(0);
            var _colCount = OuterMult.GetLength(1);

            var _GF2 = new ZnRing(_colCount);
            //var _z3 = new ZnRing(_rowCount);

            for (var i = 0; i < _rowCount; ++i)
                for(var j = 0; j < _colCount; ++j)
                    for(var k = 0; k < _colCount; ++k)
                    {
                        var _left = OuterMult[i, _GF2.Sum(j, k)];
                        //var _right = _z3.Sum(OuterMult[i, j], OuterMult[i, k]);
                        var _right = aRing.Sum(OuterMult[i, j], OuterMult[i, k]);

                        if ( _left != _right)
                            return String.Format("{0}o({1}+{2})={3} != ({0}o{1}+{0}o{2}))={4}", i, j, k, _left, _right);
                    }
            return null;
        }

        private static string ChequeAx3(int[,] OuterMult, IRing<long> aRing)
        {
            var _rowCount = OuterMult.GetLength(0);
            var _colCount = OuterMult.GetLength(1);

            var _GF2 = new ZnRing(_colCount);
            //var _z3 = new ZnRing(_rowCount);

            for (var i = 0; i < _rowCount; ++i)
                for(var j = 0; j < _rowCount; ++j)
                    for(var k = 0; k < _colCount; ++k)
                    {
                        //var _left = OuterMult[_z3.Sum(i, j), k];
                        var _left = OuterMult[aRing.Sum(i, j), k];
                        //var _right = _z3.Sum(OuterMult[i, k], OuterMult[j, k]);
                        var _right = aRing.Sum(OuterMult[i, k], OuterMult[j, k]);
                        if ( _left != _right)
                            return String.Format("({0}+{1})o{2}={3} != ({0}o{2}+{1}o{2})={4}", i, j, k, _left, _right);
                    }
            return null;
        }

        private static string OuterMultToString(int[,] OuterMult)
        {
            var _rowCount = OuterMult.GetLength(0);
            var _colCount = OuterMult.GetLength(1);
            var _sb = new StringBuilder();
            for (var i = 0; i < _rowCount; ++i)
            {
                for (var j = 0; j < _colCount; ++j)
                {
                    _sb.Append(OuterMult[i, j]);
                }

                _sb.AppendLine();
            }

            return _sb.ToString();
        }

        private static string OuterMultToString(long[,] OuterMult)
        {
            var _rowCount = OuterMult.GetLength(0);
            var _colCount = OuterMult.GetLength(1);
            var _sb = new StringBuilder();
            for (var i = 0; i < _rowCount; ++i)
            {
                for (var j = 0; j < _colCount; ++j)
                {
                    _sb.Append(OuterMult[i, j]);
                }

                _sb.AppendLine();
            }

            return _sb.ToString();
        }

        private static void Test1()
        {
            var _rational = new Rational();
            var _real = new Real();

            var _qVal = new Q[,]
                {
                    { 12, 9, 3, 10, 13 },
                    { 4, 3, 1, 2, 3 },
                    { 8, 6, 2, 5, 7 }
                };

            var _qMatrix = new Matrix<Q, Rational>(_rational, _qVal);

            _qMatrix.ConvertToE();
            Console.WriteLine(_qMatrix);

            var _rVal = new double[,]
                {
                    { 12, 9, 3, 10, 13 },
                    { 4, 3, 1, 2, 3 },
                    { 8, 6, 2, 5, 7 }
                };

            var _rMatrix = new Matrix<double, Real>(_real, _rVal);

            _rMatrix.ConvertToE();
            Console.WriteLine(_rMatrix);
        }

        private static void Test2()
        {
            var m = new Q[3, 6]
                {
                    { 1, 2, -1, 3, -2, -1 },
                    { 2, 4, -1, 0, 1, 2 },
                    { 3, 6, -2, 3, -1, 1}
                };

            var _matrix = new Matrix<Q, Rational>(new Rational(), m);
            var s = Matrix<Q, Rational>.SolveLs(_matrix);
            Console.WriteLine(s);
        }

        [DllImport(_dllPath)]
        static extern int get_gcd(int a, int b);
    }
}
