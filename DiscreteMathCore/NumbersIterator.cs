using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class NumbersIterator : IEnumerator<List<int>>
    {

        private int radix;
        private int numberLength;

        public NumbersIterator(int radix, int numberLength)
        {
            this.radix = radix;
            this.numberLength = numberLength;
        }

        private List<int> current;

        public List<int> Current
        {
            get { return this.current.GetRange(1, numberLength); }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            if (this.current == null)
            {
                this.current = new int[this.numberLength + 1].ToList();
                return true;
            }
            else
            {
                for (int i = this.numberLength; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        return false;
                    }

                    if (this.current[i] == radix - 1)
                    {
                        this.current[i] = 0;
                    }
                    else
                    {
                        this.current[i]++;
                        break;
                    }
                }
                return true;
            }
        }

        public void Reset()
        {
            this.current = null;
        }
    }
}
