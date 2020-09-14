using System;
using System.Windows.Forms;

namespace SimpleCalc
{
    // Basic rundown: This calculator should be intuitive to use. Max screensize is 30 characters but you can easily perform 
    // an overflow test in that. Just know that screen space becomesan issue with large numbers. Return values are ints so division is wonky
    // Basically the form updates a single string and then when the equals button is clicked it calculates the answer.
    // Calculator.cs contains the form, the buttons are subscribed up to the backing code events in the designer.
    // CalculatorModel.cs may seem oddly named, I couldn't really think of what to call it as it's not really a presenter/controller.
    // it's just the backing logic that exposes events so that the form can react after calculation.
    // ShuntingYard.cs you can completely ignore if you want. It's just a naive implementation of an old calculator algorithm,
    // basically it handles all of the logic behind conversion of the form string to the result answer.
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Calculator(new CalculatorModel()));
        }
    }
}
