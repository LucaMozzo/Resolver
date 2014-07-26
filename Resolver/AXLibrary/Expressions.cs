using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AXLibrary
{
    #region ABSTRACT EXPRESSION BASE CLASSES

    public abstract class Expression
    {
        protected readonly double ZERO_THRESHOLD = 1.0 * Math.Pow(10.0, -14.0);

        protected bool _isBound;

        public Expression()
        {
            _isBound = false;
        }

        public bool IsBound()
        {
            return _isBound;
        }

        public void Bind(params object[] arguments)
        {
            InnerBind(arguments);
            _isBound = true;
        }

        public double Evaluate(ExpressionContext context)
        {
            double result = InnerEvaluate(context);

            if (Math.Abs(result) <= ZERO_THRESHOLD)
                return 0;
            else
                return result;
        }

        public double Evaluate()
        {
            ExpressionContext emptyContext = new ExpressionContext();
            double result = InnerEvaluate(emptyContext);

            if (Math.Abs(result) <= ZERO_THRESHOLD)
                return 0;
            else
                return result;
        }

        protected abstract void InnerBind(params object[] arguments);
        protected abstract double InnerEvaluate(ExpressionContext context);
    }

    public abstract class ConstantExpression : Expression
    {
        public ConstantExpression()
        {
            _isBound = true;
        }

        protected sealed override void InnerBind(params object[] arguments) { }
    }

    public abstract class BinaryExpression : Expression
    {
        protected Expression _operand1;
        protected Expression _operand2;

        protected sealed override void InnerBind(params object[] arguments)
        {
            _operand1 = (Expression)arguments[0];
            _operand2 = (Expression)arguments[1];
        }
    }

    public abstract class FunctionExpression : Expression
    {
        protected Expression _operand;

        protected sealed override void InnerBind(params object[] arguments)
        {
            _operand = (Expression)arguments[0];
        }
    }

    #endregion

    #region VARIABLE AND NUMERIC EXPRESSIONS

    public sealed class VariableExpression : Expression
    {
        private Variable _variable;

        protected override void InnerBind(params object[] arguments)
        {
            _variable = (Variable)arguments[0];
        }

        protected override double InnerEvaluate(ExpressionContext context)
        {
            return context.Lookup(_variable);
        }
    }

    public sealed class NumericExpression : Expression
    {
        private double _value;

        protected override void InnerBind(params object[] arguments)
        {
            _value = (double)arguments[0];
        }

        protected override double InnerEvaluate(ExpressionContext context)
        {
            return _value;
        }
    }

    #endregion

    #region CONSTANT EXPRESSIONS

    public sealed class PIExpression : ConstantExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.PI;
        }
    }

    public sealed class EExpression : ConstantExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.E;
        }
    }

    #endregion

    #region BINARY EXPRESSIONS

    public sealed class AddExpression : BinaryExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return _operand1.Evaluate(context) + _operand2.Evaluate(context);
        }
    }

    public sealed class SubtractExpression : BinaryExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return _operand1.Evaluate(context) - _operand2.Evaluate(context);
        }
    }

    public sealed class MulitplyExpression : BinaryExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return _operand1.Evaluate(context) * _operand2.Evaluate(context);
        }
    }

    public sealed class DivideExpression : BinaryExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return _operand1.Evaluate(context) / _operand2.Evaluate(context);
        }
    }

    public sealed class PowerExpression : BinaryExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Pow(_operand1.Evaluate(context), _operand2.Evaluate(context));
        }
    }

 /*   public sealed class ChooseExpression : BinaryExpression
    {
        //NOTICE: this only works for integers
        protected override double InnerEvaluate(ExpressionContext context)
        {
            
        }
    }
 */
    #endregion

    #region FUNCTION EXPRESSIONS

    public sealed class AbsExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Abs(_operand.Evaluate(context));
        }
    }

    public sealed class LogExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Log(_operand.Evaluate(context), Math.E);
        }
    }

    public sealed class Log2Expression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Log(_operand.Evaluate(context), 2);
        }
    }

    public sealed class Log10Expression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Log10(_operand.Evaluate(context));
        }
    }

    public sealed class ExpExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Exp(_operand.Evaluate(context));
        }
    }

    public sealed class Exp2Expression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Pow(_operand.Evaluate(context), Math.E);
        }
    }

    public sealed class Exp10Expression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Pow(_operand.Evaluate(context), 10.0);
        }
    }

    public sealed class SinExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Sin(_operand.Evaluate(context));
        }
    }

    public sealed class CosExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Cos(_operand.Evaluate(context));
        }
    }

    public sealed class TanExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Tan(_operand.Evaluate(context));
        }
    }

    public sealed class ArcSinExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Asin(_operand.Evaluate(context));
        }
    }

    public sealed class ArcCosExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Acos(_operand.Evaluate(context));
        }
    }

    public sealed class ArcTanExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Atan(_operand.Evaluate(context));
        }
    }

    public sealed class SinhExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Sinh(_operand.Evaluate(context));
        }
    }

    public sealed class CoshExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Cosh(_operand.Evaluate(context));
        }
    }

    public sealed class TanhExpression : FunctionExpression
    {
        protected override double InnerEvaluate(ExpressionContext context)
        {
            return Math.Tanh(_operand.Evaluate(context));
        }
    }

    #endregion

    #region PAREN AND NULL EXPRESSIONS

    public sealed class LeftParenExpression : Expression
    {
        protected override void InnerBind(params object[] arguments) { }

        protected override double InnerEvaluate(ExpressionContext context)
        {
            return double.NaN;
        }
    }

    public sealed class RightParenExpression : Expression
    {
        protected override void InnerBind(params object[] arguments) { }

        protected override double InnerEvaluate(ExpressionContext context)
        {
            return double.NaN;
        }
    }

    public sealed class NullExpression : Expression
    {
        protected override void InnerBind(params object[] arguments) { }

        protected override double InnerEvaluate(ExpressionContext context)
        {
            return double.NaN;
        }
    }

    #endregion
}
