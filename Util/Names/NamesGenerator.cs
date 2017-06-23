using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hoTools.Utils.Names
{
    public class NamesGenerator
    {
        private char _numberProxy;
        private int _startValue;
        private string _format;

        private string _value;
        public NamesGenerator(string initial)
        {
            init(@"0", 0, @"REF_0.00.0_AA", initial);

        }

        private void init(string proxy, int start, string format, string initial)
        {
            _numberProxy = Convert.ToChar(proxy);
            _startValue = start;
            _format = format;
            _value = initial;
        }

        /// <summary>
        /// Check if value is according to format
        /// </summary>
        /// <returns></returns>
        private bool isValid()
        {
            bool valid = true;
            int pos = 0;
            foreach (char c in _format)
            {
                if (c == _numberProxy)
                {
                    if (! Char.IsNumber(_value[pos] )) return false;
                }
                else
                {
                    if (_value[pos] != c) return false;

                }
                pos = pos + 1;

            }
            return valid;
        }

        private int getNumber()
        {
            return 0;
        }
        private string getString()
        {
            return "";
        }
    }
}
