using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AXLibrary
{
    internal class AXTokenizer
    {
        private Dictionary<string,Regex> _patterns = null;

        #region CONSTRUCTOR

        public AXTokenizer()
        {
            _patterns = new Dictionary<string, Regex>();
        }

        #endregion

        #region PUBLIC METHODS

        public void AddPattern(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) 
                throw new AXException("The pattern cannot be null or an empty string.");
            if (_patterns.ContainsKey(pattern))
                throw new AXException("The pattern has already been added to the tokenizer.");

            try
            {
                _patterns.Add(pattern, new Regex(pattern));
            }
            catch
            {
                throw new AXException("The pattern must be a valid regular expression.");
            }
        }

        public bool RemovePattern(string pattern)
        {
            return _patterns.Remove(pattern);
        }

        public List<string> Tokenize(string text)
        {
            List<string> tokens = new List<string>();

            for (int i = 0; i < text.Length; i++)
            {
                bool matched = false;
                for (int j = text.Length - i; j > 0 && !matched; j--)
                {
                    foreach(KeyValuePair<string,Regex> pair in _patterns)
                    {
                        if (pair.Value.IsMatch(text.Substring(i, j)))
                        {
                            matched = true;
                            tokens.Add(text.Substring(i, j));
                            i += j - 1;
                            break;
                        }
                    }
                }

                if (!matched)
                {
                    throw new AXTokenException(i, "Unrecognized character sequence starting at index " + i.ToString() + ".");
                }
            }

            return tokens;
        }

        #endregion
    }
}
