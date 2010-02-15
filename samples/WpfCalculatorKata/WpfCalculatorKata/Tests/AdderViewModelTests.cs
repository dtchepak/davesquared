using NUnit.Framework;
using Rhino.Mocks;

namespace WpfCalculatorKata.Tests
{
    public class AdderViewModelTests
    {
        [TestFixture]
        public class When_both_numbers_are_provided
        {
            private Adder _adder;
            private AdderViewModel _adderViewModel;
            private int _expectedResult;
            private string _propertyChanged;

            [SetUp]
            public void SetUp()
            {
                const int firstNumber = 10;
                const int secondNumber = 123;
                _expectedResult = 4545;

                _adder = MockRepository.GenerateStub<Adder>();
                _adder.Stub(x => x.Add(firstNumber, secondNumber)).Return(_expectedResult);
                _adderViewModel = new AdderViewModel(_adder);
                RecordPropertyChangeOnViewModel();

                _adderViewModel.FirstNumber = firstNumber;
                _adderViewModel.SecondNumber = secondNumber;
            }

            private void RecordPropertyChangeOnViewModel()
            {
                _adderViewModel.PropertyChanged += (sender, args) => _propertyChanged = args.PropertyName;
            }

            [Test]
            public void Should_set_result_from_adder()
            {
                Assert.That(_adderViewModel.Result, Is.EqualTo(_expectedResult));
            }

            [Test]
            public void Should_indicate_that_result_has_changed()
            {
                Assert.That(_propertyChanged, Is.EqualTo("Result"), "Should have raised property changed event for Result property");
            }
        }
    }
}