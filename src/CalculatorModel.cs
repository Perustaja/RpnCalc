
using System;

namespace SimpleCalc
{
    public class CalculatorModel
    {
        // Events for the form to subscribe to (see args class below)
        public event EventHandler<ScreenUpdatedEventArgs> Calculation_Success;
        public event EventHandler<ScreenUpdatedEventArgs> Calculation_Failure;

        public void Calculate(string input)
        {
            try
            {
                input = input.ToPostFix().CalculateFromRPN().ToString();
                Calculation_Success?.Invoke(this, new ScreenUpdatedEventArgs(input));
            }
            catch (ArgumentException e) // ArgumentExceptions are thrown by the extension methods with formatted messages
            {
                Calculation_Failure?.Invoke(this, new ScreenUpdatedEventArgs(e.Message));

            }
            catch (DivideByZeroException)
            {
                Calculation_Failure?.Invoke(this, new ScreenUpdatedEventArgs("Error: division by zero"));
            }
            catch (OverflowException)
            {
                Calculation_Failure?.Invoke(this, new ScreenUpdatedEventArgs("Error: exceeded max/min value"));
            }
        }
    }
    public class ScreenUpdatedEventArgs : EventArgs
    {
        public ScreenUpdatedEventArgs(string displayText) => DisplayText = displayText;
        public readonly string DisplayText;
    }
}
