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
using DiscreteMathCoreTests;

namespace DiscreteMathConsole
{
    class Program
    {
        const string _dllPath =@"\..\..\..\Debug\DiscreteMathAlgorithms.dll";
        static void Main(string[] args)
        {
             int[] _inputData = new int[] { 3, 3, 7, 7 };
            //int[] _inputData = new int[] { 1, 3, 4, 5 };
            // int[] _inputData = new int[] { 6, 8, 8, 9 };
            // int[] _inputData = new int[] { 1, 5, 5, 5 };
            // int[] _inputData = new int[] { 1, 3, 4, 6 };
            //int[] _inputData = new int[] { 2, 3, 6, 6 };
            //int[] _inputData = new int[] { 1, 4, 5, 6 };
            ResultCalculator _resultCalculator = new ResultCalculator(_inputData, 24);
            _resultCalculator.calculate();
            Console.WriteLine(_resultCalculator.ResultExpressions);


            Console.WriteLine();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static MatrixUnitTest Test = new MatrixUnitTest();

        private static void EnumerableTest()
        {
            var _ring = new ZnRing(3);
            //foreach (var i in _ring)
            //    Console.WriteLine(i); 

            var _pfring = new PxFx<long, ZnRing>(_ring, new Polynom<long, ZnRing>(_ring, 3));

            //foreach (var _pol in _pfring)
            //    Console.WriteLine(_pol);
        }

        private static void FactorisationTest1()
        {
            var _GF2 = new ZnRing(2); 
            var _pring = new Rx<long, ZnRing>(_GF2);
            var p = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1, 1, 1, 0, 1, 1 });

            Console.WriteLine(p);

            var _res = p.Factorisation();
            var _prod = new Polynom<long, ZnRing>(_GF2, new long[] { 1 });
            foreach (var _pp in _res)
            {
                _prod = _pring.Prod(_prod, _pp);
                Console.WriteLine(_pp);
            }

            Console.WriteLine(_prod);
        }

        private static void FactorisationTest()
        {
            var p = 3;
            var _degree = 3;
            var _ring = new ZnRing(p);
            var _mod = new Polynom<long, ZnRing>(_ring, _degree);
            var _total = Math.Pow(p, _degree);
            Console.WriteLine("Total - {0}", _total);
            var _pfx = new PxFx<long, ZnRing>(_ring, _mod);

            var _count = 0;
            foreach (var _pol in _pfx)
            {
                var _mults = _pol.Factorisation();
                var _prod = new Polynom<long, ZnRing>(_ring, new long[] { 1 });
                foreach (var _pp in _mults)
                    _prod *= _pp;

                //if (!_pol.Equals(_prod))
                if(_mults.Count == 1)
                    Console.WriteLine("{0}:{1} - {2}", 
                        _count, _pol.TexString("y", false), _prod.TexString("y", false));

                _count++;
                if (_count % 1000 == 0)
                    Console.Write("\r{0}", _count);
                
            }

            Console.WriteLine();
        }

        private static void FactorisationTest2()
        {
            var p = 3;
            var _GF3 = new ZnRing(p);
            var _ring = new PxFx<long, ZnRing>(
                _GF3, new Polynom<long, ZnRing>(_GF3, new long[] { 2, 2, 1 }));
            var _mod = new Polynom<Polynom<long, ZnRing>, PxFx<long, ZnRing>>(_ring, 
                new Polynom<long, ZnRing>[] {
                    _ring.One, _ring.One, _ring.Zero, _ring.One,
                    new Polynom<long, ZnRing>(_GF3, new long[] { 0, 1 })
                });
            var _pfx = new PxFx<Polynom<long, ZnRing>, PxFx<long, ZnRing>>(_ring, _mod);
            Console.WriteLine("Total - {0}", _pfx.Size);

            var _count = 0;
            foreach (var _pol in _pfx)
            {
                var _mults = _pol.Factorisation();
                var _prod = new Polynom<Polynom<long, ZnRing>, PxFx<long, ZnRing>>(
                    _ring, new Polynom<long, ZnRing>[] { _ring.One });
                foreach (var _mult in _mults)
                {
                    _prod *= _mult;
                }

                if (!_pol.Equals(_prod))
                    Console.WriteLine("{0}:{1} - {2}", 
                        _count, _pol.TexString("y", false), _prod.TexString("y", false));

                _count++;
                if (_count % 1000 == 0)
                    Console.Write("\r{0}", _count);
            }

            Console.WriteLine();
        }

        private static string ChequeVectorSpace(RingBase<long> aRing)
        {
            var _sb = new StringBuilder();
            string _res = null;

            var _colCount = 2;
            var _rowCount = 4;

            var _size = _rowCount * _colCount;

            var _max = Math.Pow(_rowCount, _size);

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

        private static string ChequeAx(int[,] OuterMult, RingBase<long> aRing)
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

        private static string ChequeAx2(int[,] OuterMult, RingBase<long> aRing)
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

        private static string ChequeAx3(int[,] OuterMult, RingBase<long> aRing)
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

        //private List<Polynom<long, ZnRing>> BatlerTest(Polynom<long, ZnRing> fx, long p)
        //{
        //    var _res = new List<Polynom<long, ZnRing>>();
        //    var _ring = new ZnRing(p);
        //    var _pring = new Rx<long, ZnRing>(_ring);
        //    var _derivative = fx.Derivative;
        //    var _gcd = Polynom<long, ZnRing>.GetGcd(fx, _derivative);

        //    if (_gcd.Degree > 0)
        //    {
        //        if (_derivative != _pring.Zero)
        //        {
        //            _res.Add(_gcd);
        //            _res.Add(fx.Div(_gcd, null));
        //            return _res;
        //        }
        //        else
        //        {
        //            var c = fx.Degree / p;
        //            var _pol = new Polynom<long, ZnRing>(_ring, (int)c);
        //            var _coeff = new List<long>();
        //            for(var i = 0; i < c; i++)
        //            {
                        
        //            }
        //        }
        //    }

        //    return _res;
        //}

        [DllImport(_dllPath)]
        static extern int get_gcd(int a, int b);
    }
}
