using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator.Business
{
    public class ExpressionEvaluation
    {
        /// <summary>
        /// Validates input string
        /// </summary>
        /// <param name="input">string</param>
        /// <returns>bool</returns>
        public static bool Validate(string input)
        {
            try
            {
                input = input.Trim();

                if (Regex.IsMatch(input, @"[^0-9\/\-\)\(\*\+ \s]"))//If any character not in patter, then invalid
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error validating string", ex);
            }
        }

        /// <summary>
        /// Evaluates the input mathematical expression
        /// </summary>
        /// <param name="input">string</param>
        /// <returns>double</returns>
        public static double Evaluate(string input)
        {
            try
            {
                Stack<Double> stackValues = new Stack<Double>();
                Stack<string> stackOperators = new Stack<string>();
                //Stack<double> stackOutput = new Stack<double>();
                string buildOperand = string.Empty;

                char[] inputCharArray = input.Trim().ToCharArray();

                for (int i = 0; i < inputCharArray.Length; i++)
                {
                    if (inputCharArray[i] == ' ')
                        continue;
                    if (Char.IsNumber(inputCharArray[i]))
                    {
                        buildOperand = inputCharArray[i].ToString();

                        for (int j = i + 1; j < inputCharArray.Length; j++)
                        {
                            if ((Char.IsNumber(inputCharArray[j])) && inputCharArray[j] != ' ')
                            {
                                buildOperand = string.Concat(buildOperand, inputCharArray[j]);
                                if (j == inputCharArray.Length - 1)// EOL
                                {
                                    i = j;
                                }
                            }
                            else
                            {
                                i = j;
                                break;
                            }
                        }
                        stackValues.Push(Double.Parse(buildOperand));
                    }
                    // Hold all operations till we encounter a bracket close
                    if (Regex.IsMatch(inputCharArray[i].ToString(), "[(+/*-]"))
                    {
                        // Check Precedence, If current has lower precedence then execute last operations of greater precendence and proceed with stacking
                        while (stackOperators.Count > 0 && IsCurrentOperatorLowerPrecedence(inputCharArray[i].ToString(), stackOperators.Peek()) && (stackValues.Count >= 2))
                        {
                            double op2 = stackValues.Pop();
                            double op1 = stackValues.Pop();
                            string operation = stackOperators.Pop().ToString();
                            stackValues.Push(Math.Round(MathOperation(op1, op2, operation), 3));
                        }

                        stackOperators.Push(inputCharArray[i].ToString());

                    }

                    // If Right bracket, pop operations holder stack and move to main postFixStack
                    if (inputCharArray[i].ToString() == ")")
                    {
                        while (stackOperators.Count > 0)
                        {
                            string operation = stackOperators.Pop();
                            if (operation != "(")
                            {
                                if (stackValues.Count >= 2)
                                {
                                    double op2 = stackValues.Pop();
                                    double op1 = stackValues.Pop();
                                    stackValues.Push(Math.Round(MathOperation(op1, op2, operation), 3));
                                }
                            }
                        }
                    }
                }

                // End of input string
                while (stackOperators.Count > 0)
                {
                    string operation = stackOperators.Pop();
                    if (operation != "(")
                    {
                        if (stackValues.Count >= 2)
                        {
                            double op2 = stackValues.Pop();
                            double op1 = stackValues.Pop();
                            stackValues.Push(Math.Round(MathOperation(op1, op2, operation), 3));
                        }
                    }
                }

                if (stackValues.Count == 1)
                    return stackValues.Pop();
                else
                    return 0;
            }
            catch(Exception ex)
            {
                throw new Exception("Error evaluating string", ex);
            }
        }

        /// <summary>
        /// Math operations
        /// </summary>
        /// <param name="operand1">double</param>
        /// <param name="operand2">double</param>
        /// <param name="operation">string</param>
        /// <returns>double</returns>
        private static double MathOperation(double operand1, double operand2, string operation)
        {
            double result = 0;
            switch (operation)
            {
                case "/":
                    if (operand2 != 0)
                    {
                        result = operand1 / operand2;
                        return result;
                    }
                    else throw new DivideByZeroException("Division by 0 not allowed");

                case "*":
                    result = operand1 * operand2;
                    return result;

                case "+":
                    result = operand1 + operand2;
                    return result;

                case "-":
                    result = operand1 - operand2;
                    return result;

                default:
                    return result;
            }
        }

        /// <summary>
        /// Compares two operators and returns their precedence per BODMAS rules
        /// </summary>
        /// <param name="opCurrent">string</param>
        /// <param name="opLast">string</param>
        /// <returns>bool</returns>
        private static bool IsCurrentOperatorLowerPrecedence(string opCurrent, string opLast)
        {
            if (opCurrent == "(" || opCurrent == ")" || opLast == "(") return false;
            Dictionary<string, int> opPrecedenceDefinition = new Dictionary<string, int>() { { "-", 1 }, { "+", 1 }, { "*", 3 }, { "/", 4 } };
            return opPrecedenceDefinition[opCurrent] < opPrecedenceDefinition[opLast];
        }
    }
}
