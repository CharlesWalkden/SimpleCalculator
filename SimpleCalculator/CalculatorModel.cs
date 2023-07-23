using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    public class CalculatorModel
    {
        private string? currentExpressionDisplay;
        private string currentTotal = "";
        private string currentInput = "0";
        private char? nextOperator;
        private bool replaceCurrentInput;

        /// <summary>
        /// Appends a new input(number) to the current input. 
        /// </summary>
        /// <param name="input">A number from 1-9 depending on the button pressed.</param>
        /// <returns></returns>
        public string AddInput(string input)
        {
            // If the current input is 0, replace it, otherwise, append to it.
            currentInput = currentInput == "0" || replaceCurrentInput ? currentInput = input : currentInput += input;
            replaceCurrentInput = false;
            return currentInput;
        }

        /// <summary>
        /// Removes the most recently added digit from the current input.
        /// </summary>
        /// <returns>The amended input.</returns>
        public string RemoveInput()
        {
            // If input is greater that 1 in length, remove a digit. Else replace it with a 0.
            return currentInput = currentInput.Length > 1 ? currentInput.Remove(currentInput.Length - 1) : "0";
        }

        /// <summary>
        /// Updates the current input value with the current total value.
        /// </summary>
        /// <returns>The current input value.</returns>
        public string UpdateCurrentInput()
        { 
            return currentInput = currentTotal;
        }

        /// <summary>
        /// Performs one of the operations (+ / * - =)
        /// </summary>
        /// <param name="newOperator">The operations you want to perform.</param>
        /// <returns>The display of the current expression</returns>
        public string PerformOperation(char newOperator) 
        {
            string newTotal = "0";

            if (!string.IsNullOrEmpty(currentTotal))
            {
                newTotal = ValidateExpression().ToString(); 
            }
            else
            {
                newTotal = currentInput;
            }

            if (newOperator == '=')
            {
                currentExpressionDisplay = $"{currentTotal} {nextOperator} {currentInput} =";
                currentInput = newTotal;
                newOperator = ' ';
            }
            else
            {
                nextOperator = newOperator;
                currentExpressionDisplay = $"{newTotal} {nextOperator}";
            }

            currentTotal = newTotal;
            replaceCurrentInput = true;

            return currentExpressionDisplay;
        }
        
        /// <summary>
        /// Validates the expression and performs the basic operations.
        /// </summary>
        /// <returns>The result of the operation performed.</returns>
        private string ValidateExpression()
        {
            double result;
            if (string.IsNullOrEmpty(currentTotal))
                return currentInput;

            switch (nextOperator)
            {
                case '+':
                    {
                        result = double.Parse(currentTotal) + double.Parse(currentInput);
                        break;
                    }
                case '-':
                    {
                        result = double.Parse(currentTotal) - double.Parse(currentInput);
                        break;
                    }
                case '/':
                    {
                        result = double.Parse(currentTotal) / double.Parse(currentInput);
                        break;
                    }
                case '*':
                    {
                        result = double.Parse(currentTotal) * double.Parse(currentInput);
                        break;
                    }
                default:
                    return "Error";

            }

            return result.ToString();
        }
    }



}
