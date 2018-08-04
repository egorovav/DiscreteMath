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
            long _size = 5;
            long l = (_size - 1) * (_size - 1);
            var _addOpp = new long[_size, _size];
            var _multOpp = new long[_size, _size];
            var _z3 = new FiniteRing(_addOpp, _multOpp);
            long _lenght = (long)Math.Pow(_size, l);
            var _fileName = "output.txt";
            var _ringCount = 0;

            Console.WriteLine(_lenght);

            for (var i = 0; i < _size; ++i)
            {
                _addOpp[i, 0] = i;
                _addOpp[0, i] = i;
            }

            for (long i = 0; i < _lenght; ++i)
            {
                var _temp = new long[l];
                long _i = i;
                for (var j = 0; j < l; ++j)
                {
                    _temp[j] = _i % _size;

                    _addOpp[j / (_size - 1) + 1, j % (_size - 1) + 1] = _i % _size;

                    _i /= _size;
                }

                string _res = null;

                try
                {
                    _res = _z3.CheckAdd();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (_res == null)
                {
                    var _str = String.Format("Add:{1}{0}{1}Mult:{1}", _z3.AddTable, Environment.NewLine);
                    Console.WriteLine(_str);
                    File.AppendAllText(_fileName, _str);

                    for (long s = 0; s < _lenght; ++s)
                    {
                        var _temp1 = new long[l];
                        var _s = s;
                        for (var j = 0; j < l; ++j)
                        {
                            _temp1[j] = _s % _size;

                            _multOpp[j / (_size - 1) + 1, j % (_size - 1) + 1] = _s % _size;

                            _s /= _size;
                        }

                        var _multRes = _z3.CheckMult();

                        if (_multRes == null 
                        //    _z3.CheckMultCom() == null &&
                        //    _z3.CheckOne() == null)
                        // _z3.CheckReverse() == null
                        )
                        {
                            //Console.WriteLine(String.Format("Add: {0}", String.Join(", ", _temp)));
                            _str = String.Format("{0}{1}", _z3.MultTable, Environment.NewLine);
                            Console.WriteLine(_str);
                            File.AppendAllText(_fileName, _str);
                            _ringCount++;
                        }
                    }

                }

                if (i % 1000000 == 0)
                    Console.Write(String.Format("{0}\r", i));         
            }
            var _str1 = String.Format("Count: {0}", _ringCount);
            Console.WriteLine(_str1);
            File.AppendAllText(_fileName, _str1);

            Console.WriteLine("Finish.");

            Console.ReadLine();
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
