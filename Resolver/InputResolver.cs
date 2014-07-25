using System;
using AXLibrary;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Resolver
{

    public class InputResolver
    {
        private AXParser parser;
        private Expression exp;
	    public InputResolver()
	    {
            parser = new AXParser();
            exp = null;
	    }

        public string getResult(string input)
        {
            this.exp = parser.Parse(input);
            return this.exp.Evaluate().ToString();
        }
    }
}