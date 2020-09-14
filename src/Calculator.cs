using System;
using System.Windows.Forms;

namespace SimpleCalc
{
    public partial class Calculator : Form
    {
        CalculatorModel _model;
        bool _isInErrorState = false;
        public Calculator(CalculatorModel model)
        {
            InitializeComponent();
            _model = model;
            // Subscribe to the backing model's events so we can react after calculation
            _model.Calculation_Success += Calculation_Succeeded;
            _model.Calculation_Failure += Calculation_Failed;
        }
        // First two methods are eventhandlers for when the backing calculator is done
        void Calculation_Succeeded(object sender, ScreenUpdatedEventArgs e)
        {
            lblScreen.Text = e.DisplayText;
        }
        void Calculation_Failed(object sender, ScreenUpdatedEventArgs e)
        {
            lblScreen.Text = e.DisplayText;
            _isInErrorState = true;
        }
        // Eventhandlers for the form controls
        void Equal_Pressed(object sender, EventArgs e) => _model.Calculate(lblScreen.Text);
        void Clear_Pressed(object sender, EventArgs e)
        {
            lblScreen.ResetText();
            _isInErrorState = false;
        }
        void Bspace_Pressed(object sender, EventArgs e)
        {
            if (_isInErrorState == true)
                return;
            var len = lblScreen.Text.Length;
            if (len < 1)
                return;
            else
                lblScreen.Text = lblScreen.Text.Substring(0, len - 1);
        }

        void Key_Pressed(object sender, EventArgs e)
        {
            var b = sender as Button;
            Append_To_Screen(b.Text);
        }
        // Helper methods
        void Append_To_Screen(string text)
        {
            if (_isInErrorState)
                return;
            else if (lblScreen.Text.Length < 30)
                lblScreen.Text += text;
        }
    }
}
