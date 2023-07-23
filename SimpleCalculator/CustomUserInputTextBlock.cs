using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SimpleCalculator
{
    public class CustomUserInputTextBlock : TextBox
    {
        private SolidColorBrush TransparentBrush = new SolidColorBrush(Colors.Transparent);
        public CustomUserInputTextBlock()
        {
            this.DefaultStyleKey = typeof(CustomUserInputTextBlock);
            this.TextChanged += OnCustomTextChanged;
        }
        

        private void OnCustomTextChanged(object sender, TextChangedEventArgs e)
        {
            // Remove the text changed event so it is not imediatly triggered causing an infinate loop when we reset the text with the new value.
            this.TextChanged -= OnCustomTextChanged;
            AddCommasToNumber();
            // Re-add the text changed event once we are done.
            this.TextChanged += OnCustomTextChanged;
        }

        private void AddCommasToNumber()
        {
            string text = this.Text;

            // Figure out how many commas we will need
            int numberOfCommas = (text.Length - 1) / 3;
            char[] result = new char[text.Length + numberOfCommas];

            if (numberOfCommas == 0) return;

            int numberIndex = text.Length - 1;
            int newNumberIndex = result.Length - 1;


            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0 && i%3 == 0)
                {
                    result[newNumberIndex] = ',';
                    newNumberIndex--;
                }

                result[newNumberIndex] = text[numberIndex];

                numberIndex--;
                newNumberIndex--;
            }

            this.Text = new string(result);


        }
    }
}
