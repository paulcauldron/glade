using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Calculator.Business;

namespace Calculator
{
    class Program
    { 
        const string Instruction = "Enter equation to compute, OR \nEnter \'q\' to quit.";
        const string ExitKeyword = "q";
        static char[] operators = { '*', '/', '+', '-' };
        public static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine(Instruction);
                    string input = Console.ReadLine();
                    input = input.Trim();

                    if (!(string.IsNullOrEmpty(input) || input.Length == 0))
                    {
                        if (input.ToLower() == ExitKeyword)
                        {
                            Console.WriteLine("Ending Calculator");
                            break;
                        }
                        if (ExpressionEvaluation.Validate(input))
                        {
                            double output = ExpressionEvaluation.Evaluate(input);
                            Console.WriteLine("output = " + output);
                        }
                        else
                        { 
                            Console.WriteLine("Input is invalid");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Encountered: " + ex.Message); // Ideally log, and not displayed
                Console.ReadLine();
            }
        }
    }
}

