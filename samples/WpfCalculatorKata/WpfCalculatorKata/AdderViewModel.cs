using System.ComponentModel;

namespace WpfCalculatorKata
{
    public class AdderViewModel : INotifyPropertyChanged
    {
        private readonly Adder _adder;
        private int _firstNumber;
        private int _secondNumber;
        private int _result;

        public AdderViewModel(Adder adder)
        {
            _adder = adder;
        }

        public int FirstNumber
        {
            get { return _firstNumber; }
            set { _firstNumber = value; NumberChanged(); }
        }

        public int SecondNumber
        {
            get { return _secondNumber; }
            set { _secondNumber = value; NumberChanged(); }
        }

        private void NumberChanged()
        {
            Result = _adder.Add(_firstNumber, _secondNumber);
        }

        public int Result
        {
            get { return _result; }
            set { _result = value; OnResultChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnResultChanged()
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs("Result"));
        }
    }
}