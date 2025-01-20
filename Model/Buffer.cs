using System.Collections;

namespace Multimetro1_0_2.Model
{
    public class Buffer2 : IEnumerator, IEnumerable
    {
        public readonly string data;
        private string newLine;
        private int position = -1;

        public Buffer2(string buffer)
        {
            this.data = buffer;
            newLine = string.Empty;
        }
        public Buffer2(char c)
        {
            data = string.Empty;
            data += c;
            newLine = string.Empty;
        }
        private string ReadLine()
        {
            int length;
            string newLine = "";
            char c;
            length = data.Length;
            while (position < length - 1)
            {
                c = data[position + 1];
                if (c == '\n')
                {
                    position++;
                    break;
                }
                else
                {
                    newLine += c;
                }
                position++;
            }
            return newLine;
        }
        public object Current
        {
            get
            {
                return newLine;
            }
        }


        public bool MoveNext()
        {
            if (position < data.Length - 1)
            {
                newLine = ReadLine();
                return true;
            }
            return false;
        }

        public void Reset()
        {
            position = -1;
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }
    }
}
