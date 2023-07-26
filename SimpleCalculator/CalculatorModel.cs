using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    public class CalculatorModel : INotifyPropertyChanged
    {
        private string? currentExpressionDisplay;
        private string currentTotal = "";
        private string currentInput = "0";
        private char? nextOperator;
        private bool replaceCurrentInput;
        private const int inputLimit = 16;

        public string? CurrentExpressionDisplay
        {
            get => currentExpressionDisplay;
            set
            {
                if (currentExpressionDisplay == value)
                    return;

                currentExpressionDisplay = value;
                OnPropertyChanged();
            }
        }

        public string CurrentInput
        {
            get => currentInput;
            set
            {
                if (currentInput == value)
                    return;

                currentInput = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Appends a new input(number) to the current input. 
        /// </summary>
        /// <param name="input">A number from 1-9 depending on the button pressed.</param>
        public void AddInput(string input)
        {
            // Stop the input addition if the limit has been hit.
            // Only want to do this if we are not replacing the current input.
            if (CurrentInput.Length == inputLimit && !replaceCurrentInput)
                return;

            // If we try to add more that one dot, don't add it.
            if (input == "." && CurrentInput.Contains('.'))
                return;

            // If the current input is 0, replace it, otherwise, append to it.
            CurrentInput = CurrentInput == "0" && input != "." || replaceCurrentInput ? CurrentInput = input : CurrentInput += input;
            replaceCurrentInput = false;
        }

        /// <summary>
        /// Removes the most recently added digit from the current input.
        /// </summary>
        public void RemoveInput()
        {
            // If input is greater that 1 in length, remove a digit. Else replace it with a 0.
            CurrentInput = CurrentInput.Length > 1 ? CurrentInput.Remove(CurrentInput.Length - 1) : "0";
        }

        /// <summary>
        /// Performs one of the operations (+ / * - =)
        /// </summary>
        /// <param name="newOperator">The operation you want to perform.</param>
        public void PerformOperation(char newOperator) 
        {
            // If we have not selected another input after performing an operation,
            // we just want to replace it instead of keep performing an operation.
            // and then update the display with the new operation.
            if (replaceCurrentInput)
            {
                if (newOperator != nextOperator)
                {
                    nextOperator = newOperator;
                    CurrentExpressionDisplay = $"{currentTotal} {nextOperator}";
                }
                return;
            }

            // If we have nothing following on from a . remove it.
            if (CurrentInput.EndsWith('.'))
            {
                CurrentInput = CurrentInput.TrimEnd('.');
            }

            string newTotal;
            if (!string.IsNullOrEmpty(currentTotal))
            {
                newTotal = ValidateExpression().ToString(); 
            }
            else
            {
                newTotal = currentInput;
            }

            // If the next operator is =, we need to change the format of the display and set the next operator to null as
            // we are at the end of the expression.
            if (newOperator == '=')
            {
                CurrentExpressionDisplay = $"{currentTotal} {nextOperator} {CurrentInput} =";
                nextOperator = null;
            }
            else
            {
                nextOperator = newOperator;
                CurrentExpressionDisplay = $"{newTotal} {nextOperator}";
            }

            currentTotal = newTotal;
            CurrentInput = newTotal;

            // Set this flag to true to allow the user to start inputting a new number.
            replaceCurrentInput = true;
        }
        
        /// <summary>
        /// Validates the expression and performs the basic operations.
        /// </summary>
        /// <returns>The result of the operation performed.</returns>
        private string ValidateExpression()
        {
            double result;
            if (string.IsNullOrEmpty(currentTotal) || nextOperator == null)
                return CurrentInput;

            switch (nextOperator)
            {
                case '+':
                    {
                        result = double.Parse(currentTotal) + double.Parse(CurrentInput);
                        break;
                    }
                case '-':
                    {
                        result = double.Parse(currentTotal) - double.Parse(CurrentInput);
                        break;
                    }
                case '÷':
                    {
                        result = double.Parse(currentTotal) / double.Parse(CurrentInput);
                        break;
                    }
                case 'x':
                    {
                        result = double.Parse(currentTotal) * double.Parse(CurrentInput);
                        break;
                    }
                default:
                    result = 0;
                    break;

            }

            return result.ToString();
        }

        /// <summary>
        /// Changes the input number from a negative number to a positive number or a positive number to a negative number.
        /// </summary>
        public void NegPosToggle()
        {
            if (CurrentInput.Contains('-'))
            {
                CurrentInput = CurrentInput.Substring(1, CurrentInput.Length -1);
            }
            else
            {
                CurrentInput = CurrentInput.Insert(0, "-");
            }
        }

        /// <summary>
        /// Clears and resets the calculator.
        /// </summary>
        public void ResetCalculator()
        {
            CurrentExpressionDisplay = string.Empty;
            currentTotal = string.Empty;
            CurrentInput = "0";
            nextOperator = null;
            replaceCurrentInput = false;
            AddInput("0");
        }

        /// <summary>
        /// Implementation of the INotifyPropertyChanged Interface.
        /// </summary>
        #region ProperyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
