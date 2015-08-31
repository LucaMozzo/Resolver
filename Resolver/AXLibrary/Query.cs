using System;

namespace AXLibrary
{
    #region ABSTRACT QUERY BASE CLASS
    /*
     * The idea of Query as parent of expression is to allow
     * expressions to live inside assignments, and to handle scope.
     * There will be a workspace scope for variables, but when a variable
     * name is used as an argument, that will take precedence if that name
     * has been assigned in the global scope.
     * 
     * This will be useful for integration with respect to a variable
     * and assignments of a function to a variable.
     */
    public abstract class Query
    {
        public Query()
        {}
        /*
         * We will want a class that encapsulates output
         * for the web form, which will allow us to show
         * the user more than just a number
         */

        /*
         * public Output Resolve(QueryContext context) {}
         */

    }
    #endregion

    #region EQUATIONS

    public class Equation : Query
    {
        private Expression _leftExpr, _rightExpr;

        protected void InnerBind(params object[] arguments)
        {
            _leftExpr  = (Expression)arguments[0];
            _rightExpr = (Expression)arguments[1];
        }
    }

    #endregion

    #region FUNCTION NAMING

    //public class FunctionHandle {}

    #endregion

    #region ASSIGNMENT OPERATIONS


    #endregion
}