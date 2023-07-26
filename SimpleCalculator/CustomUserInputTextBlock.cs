using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

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

        private const double DefaultFontSize = 52;

        private void OnCustomTextChanged(object sender, TextChangedEventArgs e)
        {
            // Remove the text changed event so it is not imediatly triggered causing an infinate loop when we reset the text with the new value.
            this.TextChanged -= OnCustomTextChanged;
            AddCommasToNumber();
            // Re-add the text changed event once we are done.
            this.TextChanged += OnCustomTextChanged;
        }

        /// <summary>
        /// Add commas to the number added to the textbox
        /// </summary>
        private void AddCommasToNumber()
        {
            // Updated to take into account decimal numbers.
            // Updated to take into account negative numbers.
            string text = this.Text;
            bool isNegative = false;
            if (text.StartsWith('-'))
            {
                isNegative = true;
                text = text.TrimStart('-');
            }

            string[] decimalSplit = text.Split('.');

            string integerNumbers = decimalSplit[0];
            string decimalNumbers = decimalSplit.Length > 1 ? "." + decimalSplit[1] : "";

            // Figure out how many commas we will need
            int numberOfCommas = (integerNumbers.Length - 1) / 3;
            char[] result = new char[integerNumbers.Length + numberOfCommas + decimalNumbers.Length];

            if (numberOfCommas == 0) return;

            int numberIndex = integerNumbers.Length - 1;
            int newNumberIndex = result.Length - 1 - decimalNumbers.Length;

            for (int i = 0; i < integerNumbers.Length; i++)
            {
                if (i > 0 && i%3 == 0)
                {
                    result[newNumberIndex] = ',';
                    newNumberIndex--;
                }

                result[newNumberIndex] = integerNumbers[numberIndex];

                numberIndex--;
                newNumberIndex--;
            }

            // Add the decimals back in.
            for (int i=0; i < decimalNumbers.Length; i++)
            {
                result[result.Length - 1 - i] = decimalNumbers[decimalNumbers.Length - 1 - i];
            }

            string stringResult = new string(result);

            if (isNegative)
            {
                stringResult = stringResult.Insert(0, "-");
            }
            FormatText(stringResult);
            this.Text = stringResult;
        }

        /// <summary>
        /// Formats the text and changes the font size to fix all the text in the textbox.
        /// </summary>
        private void FormatText(string text)
        {
            double fontSize = FontSize;
            double textBoxWidth = ActualWidth == 0 ? Width : ActualWidth;
            string formatText = text;
            double textWidth = MeasureTextWidth(formatText);

            // If the text does not fit.
            if (textWidth > textBoxWidth)
            {
                // Loop through making the text smaller untill it does fit.
                while (textWidth > textBoxWidth && fontSize > 1)
                {
                    fontSize--;
                    SetFontSize(fontSize);
                    textWidth = MeasureTextWidth(formatText);
                }
            }
            // If the text does fit.
            else
            {
                // Loop through and see if we can start increasing the size again.
                while (textWidth < textBoxWidth && fontSize < DefaultFontSize)
                {
                    fontSize++;
                    SetFontSize(fontSize);
                    textWidth = MeasureTextWidth(formatText) + 5;
                }
            }
        }

        /// <summary>
        /// Measures a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns>The width of the measured string</returns>
        private double MeasureTextWidth(string text)
        {
            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Foreground,
                new NumberSubstitution(),
                VisualTreeHelper.GetDpi(this).PixelsPerDip
            );

            return formattedText.Width;
        }

        /// <summary>
        /// Sets the font size for this TextBox
        /// </summary>
        /// <param name="fontSize">The size to set the font</param>
        private void SetFontSize(double fontSize)
        {
            FontSize = fontSize;
        }
    }
}
