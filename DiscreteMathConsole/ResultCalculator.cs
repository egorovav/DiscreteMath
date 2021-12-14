using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscreteMathCore;

namespace DiscreteMathConsole
{
    class ResultCalculator
    {
        private const int operatorsCount = 4;
        private const double epsilon = 0.000001;

        private int[] inputData;
        private int result;

        public ResultCalculator(int[] inputData, int result)
        {
            this.inputData = inputData;
            this.result = result;

            operators[0] = (x, y) => x + y;
            operators[1] = (x, y) => x - y;
            operators[2] = (x, y) => x * y;
            operators[3] = (x, y) => x / y;
        }

        private Func<double, double, double>[] operators = 
            new Func<double, double, double>[operatorsCount];
        private String[] opertorSigns = new string[] { "+", "-", "*", "/" };

        private HashSet<String> resultExpressions = new HashSet<String>();

        public String ResultExpressions
        {
            get
            {
                StringBuilder _temp = new StringBuilder();
                foreach(String _expression in this.resultExpressions)
                {
                    _temp.AppendLine(_expression);
                }
                return _temp.ToString();
            }
        }

        public void calculate()
        {
            int _inputDataSize = this.inputData.Length;
            int _operatorCombinationCount = (int)Math.Round(Math.Pow(operatorsCount, _inputDataSize - 1));

            PermutationIterator _permutationInterator = new PermutationIterator(_inputDataSize);
            while (_permutationInterator.MoveNext())
            {
                for (int _opIndex = 0; _opIndex < _operatorCombinationCount; _opIndex++)
                {
                    int _temp = _opIndex;
                    double[] _sortedInputData = new double[_inputDataSize];
                    for (int i = 0; i < _inputDataSize; i++)
                    {
                        _sortedInputData[i] = this.inputData[_permutationInterator.Current[i]];
                    }
                    int[] _opList = new int[_inputDataSize - 1];                   
                    for (int j = 0; j < _inputDataSize - 1; j++)
                    {
                        int _op = _temp % operatorsCount;
                        _temp /= operatorsCount;
                        _opList[j] = _op;                     
                    }
                    compute1(_sortedInputData, _opList);
                    compute2(_sortedInputData, _opList);
                    compute3(_sortedInputData, _opList);
                    compute4(_sortedInputData, _opList);
                    compute5(_sortedInputData, _opList);
                }
            }
        }

        //((a.b).c).d
        private double compute1(double[] operands, int[] ops)
        {
            double _result1 = operators[ops[0]](operands[0], operands[1]);
            double _result2 = operators[ops[1]](_result1, operands[2]);
            double _result3 = operators[ops[2]](_result2, operands[3]);
            if (Math.Abs(_result3 - this.result) < epsilon)
            {
                String _text = String.Format("(({0}{1}{2}){3}{4}){5}{6} = {7}",
    operands[0], opertorSigns[ops[0]], operands[1], opertorSigns[ops[1]], operands[2],
    opertorSigns[ops[2]], operands[3], _result3);

                this.resultExpressions.Add(_text);

                //Console.Write(_text);

                //System.IO.File.AppendAllText("output.txt", _text);
            }

            return _result3;
        }

        //(a.b).(c.d)
        private double compute2(double[] operands, int[] ops)
        {
            double _result1 = operators[ops[0]](operands[0], operands[1]);
            double _result2 = operators[ops[1]](operands[2], operands[3]);
            double _result3 = operators[ops[2]](_result1, _result2);
            if (Math.Abs(_result3 - this.result) < epsilon)
            {
                String _text = String.Format("({0}{1}{2}){3}({4}{5}{6}) = {7}",
    operands[0], opertorSigns[ops[0]], operands[1], opertorSigns[ops[2]], operands[2],
    opertorSigns[ops[1]], operands[3], _result3);

                this.resultExpressions.Add(_text);

                //Console.Write(_text);

                //System.IO.File.AppendAllText("output.txt", _text);
            }

            return _result3;
        }

        //(a.(b.c)).d
        private double compute3(double[] operands, int[] ops)
        {
            double _result1 = operators[ops[0]](operands[1], operands[2]);
            double _result2 = operators[ops[1]](operands[0], _result1);
            double _result3 = operators[ops[2]](_result2, operands[3]);
            if (Math.Abs(_result3 - this.result) < epsilon)
            {
                String _text = String.Format("({0}{1}({2}{3}{4})){5}{6} = {7}",
    operands[0], opertorSigns[ops[1]], operands[1], opertorSigns[ops[0]], operands[2],
    opertorSigns[ops[2]], operands[3], _result3);

                this.resultExpressions.Add(_text);

                //Console.Write(_text);

                //System.IO.File.AppendAllText("output.txt", _text);
            }

            return _result3;
        }

        //a.((b.c).d)
        private double compute4(double[] operands, int[] ops)
        {
            double _result1 = operators[ops[0]](operands[1], operands[2]);
            double _result2 = operators[ops[1]](_result1, operands[3]);
            double _result3 = operators[ops[2]](operands[0], _result2);
            if (Math.Abs(_result3 - this.result) < epsilon)
            {
                String _text = String.Format("{0}{1}(({2}{3}{4}){5}{6}) = {7}",
    operands[0], opertorSigns[ops[2]], operands[1], opertorSigns[ops[0]], operands[2],
    opertorSigns[ops[1]], operands[3], _result3);

                this.resultExpressions.Add(_text);

                //Console.Write(_text);

                //System.IO.File.AppendAllText("output.txt", _text);
            }

            return _result3;
        }

        //a.(b.(c.d))
        private double compute5(double[] operands, int[] ops)
        {
            double _result1 = operators[ops[0]](operands[2], operands[3]);
            double _result2 = operators[ops[1]](operands[1], _result1);
            double _result3 = operators[ops[2]](operands[0], _result2);
            if (Math.Abs(_result3 - this.result) < epsilon)
            {
                String _text = String.Format("{0}{1}({2}{3}({4}{5}{6})) = {7}",
    operands[0], opertorSigns[ops[2]], operands[1], opertorSigns[ops[1]], operands[2],
    opertorSigns[ops[0]], operands[3], _result3);

                this.resultExpressions.Add(_text);

                //Console.Write(_text);

                //System.IO.File.AppendAllText("output.txt", _text);
            }

            return _result3;
        }
    }
}
