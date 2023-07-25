using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleCalculator
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        #region Private Properties

        private CalculatorModel calculatorModel;
        
        private string? currentExpression { get; set; }
        private string? inputDisplay { get; set; }

        #endregion

        #region Public Properties

        public string? CurrentExpression
        {
            get => currentExpression;
            set
            {
                if (currentExpression == value)
                    return;

                currentExpression = value;
                OnPropertyChanged();
            }
        }
        public string? InputDisplay
        {
            get => inputDisplay;
            set
            {
                if (inputDisplay == value)
                    return;

                inputDisplay = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Constructor

        public CalculatorViewModel()
        {
            calculatorModel = new CalculatorModel();
            calculatorModel.PropertyChanged += CalculatorModel_PropertyChanged;

            RegisterCommands();
        }

        #endregion

        #region Commands

        public ICommand? EqualCommand { get; set; }
        public ICommand? AddCommand { get; set; }
        public ICommand? SubtractCommand { get; set; }
        public ICommand? DivideCommand { get; set; }
        public ICommand? MultiplyCommand { get; set; }
        public ICommand? NumbersCommand { get; set; }
        public ICommand? DeleteCommand { get; set; }
        public ICommand? NegPosCommand { get; set; }
        public ICommand? ClearCommand { get; set; }

        #endregion

        #region Command Methods

        private void RegisterCommands()
        {
            EqualCommand = new RelayCommand(Equal);
            AddCommand = new RelayCommand(Add);
            SubtractCommand = new RelayCommand(Subtract);
            DivideCommand = new RelayCommand(Divide);
            MultiplyCommand = new RelayCommand(Multiple);
            NumbersCommand = new RelayCommand<string>(Numbers);
            DeleteCommand = new RelayCommand(Delete);
            NegPosCommand = new RelayCommand(NegPos);
            ClearCommand = new RelayCommand(Clear);
        }

        private void Equal()
        {
            calculatorModel.PerformOperation('=');
            calculatorModel.SetCurrentInputAsTotal();
        }
        private void Add()
        {
            calculatorModel.PerformOperation('+');
        }
        private void Subtract()
        {
            calculatorModel.PerformOperation('-');
        }
        private void Divide()
        {
            calculatorModel.PerformOperation('/');
        }
        private void Multiple()
        {
            calculatorModel.PerformOperation('*');
        }
        private void Numbers(string number)
        {
            calculatorModel.AddInput(number);
        }
        private void Delete()
        {
            calculatorModel.RemoveInput();
        }
        private void NegPos()
        {
            calculatorModel.NegPosToggle();
        }
        private void Clear()
        {
            calculatorModel.ResetCalculator();
            calculatorModel.AddInput("0");
        }

        #endregion

        /// <summary>
        /// Implementation of the INotifyPropertyChanged Interface.
        /// </summary>
        #region ProperyChanged

        public void CalculatorModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            CurrentExpression = calculatorModel.CurrentExpressionDisplay;
            InputDisplay = calculatorModel.CurrentInput;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
