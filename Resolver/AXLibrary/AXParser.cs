using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AXLibrary
{
    public sealed class AXParser
    {
        private ExpressionFactory _factory = null;
        private AXTokenizer _tokenizer = null;

        #region CONSTRUCTOR

        public AXParser()
        {
            _factory = new ExpressionFactory();
            _tokenizer = new AXTokenizer();

            /* canned support */

            //numbers and variables
            AddAssociationInternal(@"^[a-z]$", typeof(VariableExpression));
            AddAssociationInternal(@"^\d+(\.\d+)?$", typeof(NumericExpression));

            //constants
            AddAssociationInternal(@"^PI$", typeof(PIExpression));
            AddAssociationInternal(@"^E$", typeof(EExpression));

            //standard unary operators
            //AddAssociationInternal(@"^!$", typeof(FactorialExpression));
            //

            //standard binary operators
            AddAssociationInternal(@"^\+$", typeof(AddExpression));
            AddAssociationInternal(@"^-$", typeof(SubtractExpression));
            AddAssociationInternal(@"^\*$", typeof(MulitplyExpression));
            AddAssociationInternal(@"^/$", typeof(DivideExpression));
            AddAssociationInternal(@"^\^$", typeof(PowerExpression));

            //semantic binary operators
            //AddAssociationInternal(@"^(?i)choose$", typeof(ChooseExpression));

            //unary functions
            AddAssociationInternal(@"^(?i)abs$", typeof(AbsExpression));
            AddAssociationInternal(@"^(?i)ln$", typeof(LogExpression));
            AddAssociationInternal(@"^(?i)log$", typeof(LogExpression));
            AddAssociationInternal(@"^(?i)log2$", typeof(Log2Expression));
            AddAssociationInternal(@"^(?i)log10$", typeof(Log10Expression));
            AddAssociationInternal(@"^(?i)exp$", typeof(ExpExpression));
            AddAssociationInternal(@"^(?i)exp2$", typeof(Exp2Expression));
            AddAssociationInternal(@"^(?i)exp10$", typeof(Exp10Expression));
            AddAssociationInternal(@"^(?i)sin$", typeof(SinExpression));
            AddAssociationInternal(@"^(?i)cos$", typeof(CosExpression));
            AddAssociationInternal(@"^(?i)tan$", typeof(TanExpression));
            AddAssociationInternal(@"^(?i)arcsin$", typeof(ArcSinExpression));
            AddAssociationInternal(@"^(?i)arccos$", typeof(ArcCosExpression));
            AddAssociationInternal(@"^(?i)arctan$", typeof(ArcTanExpression));
            AddAssociationInternal(@"^(?i)sinh$", typeof(SinhExpression));
            AddAssociationInternal(@"^(?i)cosh$", typeof(CoshExpression));
            AddAssociationInternal(@"^(?i)tanh$", typeof(TanhExpression));

            //parens
            AddAssociationInternal(@"^\($", typeof(LeftParenExpression));
            AddAssociationInternal(@"^\)$", typeof(RightParenExpression));
        }

        #endregion

        #region PUBLIC METHODS

        public Expression Parse(string text)
        {
            string copy = text.Replace(" ", string.Empty).Trim();

            List<string> tokens = _tokenizer.Tokenize(copy);
            List<Expression> expressions = TokensToExpressions(tokens);

            AXValidator.Validate(expressions); //throws

            RemoveExcessParens(expressions);
            while (expressions.Count > 1)
            {
                int i = DetermineHighestPrecedence(expressions);
                CollapseExpression(expressions, i);
                RemoveExcessParens(expressions);
            }

            return expressions[0];
        }

        public void AddAssociation(string pattern, Type type)
        {
            if (type.BaseType.Name != "ConstantExpression" && type.BaseType.Name != "FunctionExpression")
                throw new AXException("The type must directly inherit from either ConstantExpression or FunctionExpression.");

            _factory.AddAssociation(pattern, type);
            _tokenizer.AddPattern(pattern);
        }

        #endregion

        #region PRIVATE METHODS

        private void AddAssociationInternal(string pattern, Type type)
        {
            _factory.AddAssociation(pattern, type);
            _tokenizer.AddPattern(pattern);
        }

        private void CollapseExpression(List<Expression> expressions, int i)
        {
            Expression current = expressions[i];
            Expression previous = new NullExpression();
            Expression next = new NullExpression();
            if (i - 1 >= 0)
                previous = expressions[i - 1];
            if (i + 1 < expressions.Count)
                next = expressions[i + 1];

            if (current is SubtractExpression && !previous.IsBound() && !(previous is RightParenExpression))
            {
                SubtractExpression expression = (SubtractExpression)current;
                NumericExpression zero = new NumericExpression();
                zero.Bind(0.0);
                expression.Bind(zero, next);
                expressions.RemoveAt(i + 1);
            }
            else if (current is FunctionExpression)
            {
                FunctionExpression expression = (FunctionExpression)current;
                expression.Bind(next);
                expressions.RemoveAt(i + 1);
            }
            else if (current is BinaryExpression)
            {
                BinaryExpression expression = (BinaryExpression)current;
                expression.Bind(previous, next);
                expressions.RemoveAt(i + 1);
                expressions.RemoveAt(i - 1);
            }
        }

        private int DetermineHighestPrecedence(List<Expression> expressions)
        {
            int highest = int.MinValue;
            int precedence = int.MinValue;
            int maxPrecedence = int.MinValue;
            for (int i = 0; i < expressions.Count; i++)
            {
                if (expressions[i] is LeftParenExpression)
                {
                    highest = int.MinValue;
                    precedence = int.MinValue;
                    maxPrecedence = int.MinValue;
                }
                else if (expressions[i] is RightParenExpression)
                {
                    break;
                }
                else
                {
                    precedence = DeterminePrecendence(expressions, i);
                    if (precedence > maxPrecedence)
                    {
                        highest = i;
                        maxPrecedence = precedence;
                    }
                }
            }

            return highest;
        }

        private int DeterminePrecendence(List<Expression> expressions, int i)
        {
            /* immediate negation=4, function=3, power=2, multiply/divide=1, add/subtract=0 */

            Expression current = expressions[i];
            Expression previous = new NullExpression();
            Expression next = new NullExpression();
            if (i - 1 >= 0)
                previous = expressions[i - 1];
            if (i + 1 < expressions.Count)
                next = expressions[i + 1];

            int precendence = int.MinValue;
            if (!current.IsBound())
            {
                if (current is SubtractExpression && next.IsBound() && !previous.IsBound() && !(previous is RightParenExpression))
                {
                    precendence = 4;
                }
                else if (current is FunctionExpression)
                {
                    precendence = 3;
                }
                else if (current is PowerExpression)
                {
                    precendence = 2;
                }
                else if (current is MulitplyExpression || current is DivideExpression)
                {
                    precendence = 1;
                }
                else if (current is AddExpression || current is SubtractExpression)
                {
                    precendence = 0;
                }
            }

            return precendence;
        }

        private void RemoveExcessParens(List<Expression> expressions)
        {
            bool flag = true;
            while (flag)
            {
                flag = false;
                for (int i = expressions.Count - 1; i >= 0; i--)
                {
                    if (expressions[i] is RightParenExpression)
                    {
                        if (i > 0 && expressions[i - 1] is LeftParenExpression)
                        {
                            flag = true;
                            expressions.RemoveAt(i);
                            expressions.RemoveAt(i - 1);
                            i -= 1;
                        }
                        else if (i > 1 && expressions[i - 2] is LeftParenExpression)
                        {
                            flag = true;
                            expressions.RemoveAt(i);
                            expressions.RemoveAt(i - 2);
                            i -= 2;
                        }
                    }
                }
            }
        }

        private List<Expression> TokensToExpressions(List<string> tokens)
        {
            List<Expression> expressions = new List<Expression>();

            for (int i = 0; i < tokens.Count; i++)
            {
                expressions.Add(_factory.Create(tokens[i]));

                if (expressions[i] is NumericExpression)
                    ((NumericExpression)expressions[i]).Bind(double.Parse(tokens[i]));
                else if (expressions[i] is VariableExpression)
                    ((VariableExpression)expressions[i]).Bind((Variable)Enum.Parse(typeof(Variable), tokens[i]));
            }

            return expressions;
        }

        #endregion
    }
}
