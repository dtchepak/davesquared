using System.ComponentModel;
using System.Windows.Input;

namespace WpfCalculatorKata
{
    public class AdderViewModel : INotifyPropertyChanged
    {
        private readonly Adder _adder;
        private int _result;

        public AdderViewModel(Adder adder)
        {
            _adder = adder;
        }

        public int FirstNumber { get; set; }

        public int SecondNumber { get; set; }

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

        public void AddNumbers()
        {
            Result = _adder.Add(FirstNumber, SecondNumber);
        }

        public ICommand AddNumbersCommand
        {
            get { return new DelegateCommand(AddNumbers); }
        }
    }
}