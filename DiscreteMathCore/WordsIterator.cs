using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class WordsIterator<T> : IEnumerator<List<T>>
    {
        private T[] alphabet;
        private int wordsLength;
        public WordsIterator(T[] alphabet, int wordsLength)
        {
            this.alphabet = alphabet;
            this.wordsLength = wordsLength;      
        }

        private List<T> currentWord;

        public List<T> Current
        {
            get
            {
                return currentWord.GetRange(1, wordsLength);
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            if(this.currentWord == null)
            {
                this.currentWord = new List<T>();
                for(int i = 0; i <= wordsLength; i++)
                {
                    currentWord.Add(alphabet[0]);
                }
                return true;
            } 
            else
            {
                for (int i = this.wordsLength; i >= 0; i--)
                {
                    if(i == 0)
                    {
                        return false;
                    }

                    if (this.currentWord[i].Equals(this.alphabet.Last()))
                    {
                        this.currentWord[i] = this.alphabet.First();
                    }
                    else
                    {
                        int index = Array.IndexOf(this.alphabet, this.currentWord[i]);
                        this.currentWord[i] = this.alphabet[index + 1];
                        break;
                    }
                }
                return true;
            }
        }

        public void Reset()
        {
            this.currentWord = null;
        }
    }
}
