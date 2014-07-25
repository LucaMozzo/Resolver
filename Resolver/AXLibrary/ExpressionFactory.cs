using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace AXLibrary
{
    internal class ExpressionFactory
    {
        private Dictionary<string, KeyValuePair<Regex, Type>> _associations = null;

        #region CONSTRUCTOR

        public ExpressionFactory()
        {
            _associations = new Dictionary<string, KeyValuePair<Regex, Type>>();
        }

        #endregion

        #region PUBLIC METHODS

        public void AddAssociation(string pattern, Type type)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new AXException("The pattern cannot be null or an empty string.");
            if (type == null)
                throw new AXException("The type cannot be null.");
            if (_associations.ContainsKey(pattern))
                throw new AXException("The pattern has already been associated with a type.");

            try
            {
                _associations.Add(pattern, new KeyValuePair<Regex,Type>(new Regex(pattern),type));
            }
            catch
            {
                throw new AXException("The pattern must be a valid regular expression.");
            }
        }

        public bool RemoveAssociation(string pattern)
        {
            return _associations.Remove(pattern);
        }

        public Expression Create(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                foreach (KeyValuePair<string, KeyValuePair<Regex,Type>> pair in _associations)
                {
                    if (pair.Value.Key.IsMatch(token))
                    {
                        ConstructorInfo info = pair.Value.Value.GetConstructor(Type.EmptyTypes);
                        return (Expression)info.Invoke(null);
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
