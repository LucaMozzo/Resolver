using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace AXLibrary
{
    public class AXException : ApplicationException 
    {
        public AXException() { }
        public AXException(string message)
            : base(message) { }
        public AXException(string message, Exception innerException)
            : base(message, innerException) { }
        public AXException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    public class AXTokenException : AXException
    {
        private int _index;

        public AXTokenException(int index) 
        {
            _index = index;
        }
        public AXTokenException(int index, string message)
            : base(message) 
        {
            _index = index;
        }
        public AXTokenException(int index, string message, Exception innerException)
            : base(message, innerException) 
        {
            _index = index;
        }
        public AXTokenException(int index, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _index = index;
        }

        public int Index
        {
            get { return _index; }
        }
    }

    public class AXParenException : AXException
    {
        public AXParenException() { }
        public AXParenException(string message)
            : base(message) { }
        public AXParenException(string message, Exception innerException)
            : base(message, innerException) { }
        public AXParenException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    public class AXSequenceException : AXException
    {
        private Expression _first;
        private Expression _second;

        public AXSequenceException(Expression first, Expression second) 
        {
            _first = first;
            _second = second;
        }

        public AXSequenceException(Expression first, Expression second, string message)
            : base(message) 
        {
            _first = first;
            _second = second;
        }

        public AXSequenceException(Expression first, Expression second, string message, Exception innerException)
            : base(message, innerException) 
        {
            _first = first;
            _second = second;
        }

        public AXSequenceException(Expression first, Expression second, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _first = first;
            _second = second;
        }

        public Expression FirstExpression
        {
            get { return _first; }
        }

        public Expression SecondExpression
        {
            get { return _second; }
        }
    }
}
