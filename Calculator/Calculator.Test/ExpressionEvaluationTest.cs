using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.Business;

namespace Calculator.Test
{
    [TestClass]
    public class ExpressionEvaluationTest
    {
        #region String Validation
        [TestMethod]
        public void testinputstring_alphabet_returns_invalid()
        {
            string input = "a+1-3*2";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void testinputstring_specialcharthatisnotoperator_returns_invalid()
        {
            string input = "1^1-3*2";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void testinputstring_expressionstartswithpositive_returns_valid()
        {
            string input = "+1+1-3*2";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void testinputstring_expressionstartswithspace_trimsspace_returns_valid()
        {
            string input = " 1/1-3*2";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void testinputstring_expressionstartswithbracket_returns_valid()
        {
            string input = " (1/1-3*2";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void testinputstring_expressionswithbracket_returns_valid()
        {
            string input = " 1/1-3*2/(3+4)";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void testinputstring_expressionswithspace_returns_valid()
        {
            string input = " 1 / 1 - 3 * 2 / ( 3 + 4 ) ";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);
        }
        #endregion


        #region Expression Evaluation
        [TestMethod]
        public void testevaluation_expressionwithsimpleaddition_returns_valid()
        {
            string input = "1+2";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);

            double outputValue = ExpressionEvaluation.Evaluate(input);

            Assert.AreEqual(3, outputValue);
        }

        [TestMethod]
        public void testevaluation_expressionwithsimpleprecedence_returns_valid()
        {
            string input = "6+2-3";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);

            double outputValue = ExpressionEvaluation.Evaluate(input);

            Assert.AreEqual(5, outputValue);
        }

        [TestMethod]
        public void testevaluation_expressionwithsimpleprecedencemultoplication_returns_valid()
        {
            string input = "6+2*3";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);

            double outputValue = ExpressionEvaluation.Evaluate(input);

            Assert.AreEqual(12, outputValue);
        }


        [TestMethod]
        public void testevaluation_expressionwithbrackets_returns_valid()
        {
            string input = "(4+(50-5)-10)+40/4";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);

            double outputValue = ExpressionEvaluation.Evaluate(input);

            Assert.AreEqual(49, outputValue);
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException), "Division by 0 not allowed")]
        public void testevaluation_dividebyzero_throwsexception()
        {
            string input = "(4+(50-5)-10)+40/0";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);

            ExpressionEvaluation.Evaluate(input);
        }

        [TestMethod]
        public void testevaluation_expressionwithsubtraction_returns_validnegative()
        {
            string input = "3-50";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);

            double outputValue = ExpressionEvaluation.Evaluate(input);

            Assert.AreEqual(-47, outputValue);
        }


        [TestMethod]
        public void testevaluation_expressionwithbrackets_returns_validdecimal()
        {
            string input = "(3/5)-(1/4)";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);



            double outputValue = ExpressionEvaluation.Evaluate(input);

            Assert.AreEqual(0.35, outputValue);
        }

        #endregion

        [TestMethod]
        public void testevaluation_expressionlargenumbers_returns_validnegative()
        {
            string input = "25+150+400*500/1000-25";
            bool result = ExpressionEvaluation.Validate(input);
            Assert.IsTrue(result);

            double outputValue = ExpressionEvaluation.Evaluate(input);
            Assert.AreEqual(350, outputValue);

        }

    }
}