using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;

namespace NewUI
{
    class NumericTextBox:TextBox
    {
        public NumericTextBox()
            : base()
        {
            base.Validating += new System.ComponentModel.CancelEventHandler(this.numValidating);
        }

        private bool CheckRange(double val)
        {
            return (val >= _Min && val <= _Max);
        }

        private bool CheckSteps()
        {
            if (_Steps > 0)
                return (_Value % _Steps == 0);
            else
                return true;
        }
        #region events
        private void numValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string s = Text;
            int l = s.Length;
            
            while (s.Length > 0)
            {
                try
                {
                    _Value = (ToValidate == ValidateType.Integer) ? int.Parse(s, NumStyles) : float.Parse(s, NumStyles);
                    //todo: There are some cases where double is not stored as exact value (try .4, .2) and results in a double value close to but not exactly the entered value.  Handle this? hn 5.5.2015
                    break;
                }
                catch
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (s.Length != 1)
                {
                    SelectionStart = s.Length;
                    SelectionLength = l - s.Length;
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                if (!CheckRange(_Value))
                {
                    MessageBox.Show(String.Format("The number must be between {0} and {1}", _Min.ToString(), _Max.ToString()));
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                if (!CheckSteps())
                {
                    MessageBox.Show(String.Format("The number must be incremented in steps of {0}", _Steps.ToString()));
                    e.Cancel = true;
                }
            }
            Text = _Value.ToString(NumberFormatAsString);
        }
        #endregion
        
        public enum ValidateType
        {
            Integer,
            Float,
            Double,
            Percent
        }
        public enum Formatter
        {
            NONE,
            E3,
            E6,
            F1,
            F2,
            F3,
            F4,
            F6,
            N3,
            P1
        }
        private Formatter _NumberFormat = Formatter.E6;
        public Formatter NumberFormat
        {
            get { return _NumberFormat; }
            set { _NumberFormat = value; }
        }
        public string NumberFormatAsString
        {
            get
            {
                switch (_NumberFormat)
                {
                    case (Formatter.E3):
                        return "E3";
                    case (Formatter.E6):
                        return "E6";
                    case (Formatter.F1):
                        return "F1";
                    case (Formatter.F2):
                        return "F2";
                    case (Formatter.F3):
                        return "F3";
                    case (Formatter.F4):
                        return "F4";
                    case (Formatter.F6):
                        return "F6";
                    case(Formatter.N3):
                        return "N3";
                    case (Formatter.P1):
                        return "P1";
                    case Formatter.NONE:
                    default:
                        return "";
                }
            }
        }
        private ValidateType _ToValidate = ValidateType.Double;
        public ValidateType ToValidate
        {
            get { return _ToValidate; }
            set
            {
                _ToValidate = value;
                if (_ToValidate == ValidateType.Percent)
                {
                    Min = 0;
                    Max = 1000;
                    NumberFormat = Formatter.P1;
                }
            }
        }
        private double _Value = 0;
        public double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                if (ToValidate == ValidateType.Integer)
                    Text = System.Convert.ToInt32(_Value).ToString();
                else if (ToValidate == ValidateType.Percent)
                    Text = (_Value / 100).ToString(NumberFormatAsString);
                else
                    Text = _Value.ToString(NumberFormatAsString);
            }
        }

        private NumberStyles _NumStyles = NumberStyles.Number | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent;
        public NumberStyles NumStyles
        {
            get { return _NumStyles; }
            set { _NumStyles = value; }
        }
        private double _Min = 0;
        public double Min
        {
            get { return _Min; }
            set { _Min = value; }
        }
        private double _Max = Double.MaxValue;
        public double Max
        {
            get { return _Max; }
            set { _Max = value; }
        }
        private double _Steps = -1;
        public double Steps
        {
            get { return _Steps; }
            set { _Steps = value; }
        }

    }
}
