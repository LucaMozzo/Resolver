using System;
using System.Collections.Generic;
using System.Text;

namespace AXLibrary
{
    internal class AXValidator
    {
        private static int[,] legalSequences = 
                {
                    { 0, 1, 1, 0, 0, 1 },   //follow numeric, variable, constant
                    { 1, 0, 1, 1, 1, 0 },   //follow binary operators except subtract
                    { 1, 0, 1, 1, 1, 0 },   //follow subtract
                    { 1, 0, 1, 1, 1, 0 },   //follow function
                    { 1, 0, 1, 1, 1, 0 },   //follow opening parenthesis
                    { 0, 1, 1, 0, 0, 1 }    //follow closing parenthesis
                };        

        public static void Validate(List<Expression> expressions)
        {
            SequenceCheck(expressions);
            ParenCheck(expressions);
        }

        private static void SequenceCheck(List<Expression> expressions)
        {
            BeginCheck(expressions[0]);
            EndCheck(expressions[expressions.Count-1]);

            int first, second;
            for (int i = 0; i < expressions.Count - 1; i++)
            {
                first = GetTypeIndex(expressions[i]);
                second = GetTypeIndex(expressions[i + 1]);

                if (legalSequences[first, second] != 1)
                    throw new AXSequenceException(expressions[i], expressions[i + 1], "An invalid character sequence exists.");
            }
        }

        private static int GetTypeIndex(Expression expression)
        {
            if (expression is NumericExpression || expression is VariableExpression || expression is ConstantExpression)
                return 0;
            else if (expression is BinaryExpression && !(expression is SubtractExpression))
                return 1;
            else if (expression is SubtractExpression)
                return 2;
            else if (expression is FunctionExpression)
                return 3;
            else if (expression is LeftParenExpression)
                return 4;
            else
                return 5;
        }

        private static void BeginCheck(Expression expression)
        {
            if (expression is RightParenExpression)
                throw new AXSequenceException(null, expression, "The expression cannot begin with a closing parenthesis.");
            if (expression is BinaryExpression && !(expression is SubtractExpression))
                throw new AXSequenceException(null, expression, " The expression cannot being with a binary operator.");
        }

        private static void EndCheck(Expression expression)
        {
            if (expression is LeftParenExpression)
                throw new AXSequenceException(expression, null, "The expression cannot end with an opening parenthesis.");
            if (expression is BinaryExpression || expression is FunctionExpression)
                throw new AXSequenceException(expression, null, "The expression cannot end with an operator or function of any kind.");
        }

        private static void ParenCheck(List<Expression> expressions)
        {
            int i = 0;
            int counter = 0;
            for (i = 0; i < expressions.Count; i++)
            {
                if (expressions[i] is LeftParenExpression)
                {
                    counter++;
                }
                else if (expressions[i] is RightParenExpression)
                {
                    counter--;
                    if (counter < 0)
                    {
                        throw new AXParenException("A closing parenthesis does not match an opening parenthesis.");
                    }
                }
            }

            if (counter != 0)
            {
                throw new AXParenException("A closing parenthesis is missing.");
            }
        }
    }
}
