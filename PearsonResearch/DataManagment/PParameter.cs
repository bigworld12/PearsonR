using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCalc;
using System.ComponentModel;
using System.Diagnostics;

namespace PearsonResearch.DataManagment
{
    /// <summary>
    /// represents the parameter that will be used in Pearson's formula
    /// i.e. either x or y
    /// </summary>
    public class PParameter : INotifyPropertyChanged
    {

        public PParameter(DataEntery toassign)
        {
            AssignedObject = toassign;
        }
        private DataEntery m_AssignedObject;

        public DataEntery AssignedObject
        {
            get { return m_AssignedObject; }
            set
            {
                m_AssignedObject = value;
                RaisePropertyChanged(nameof(AssignedObject));
                RaisePropertyChanged(nameof(MathematicalRepresentation));
                RaisePropertyChanged(nameof(Value));
            }
        }


              

        /// <summary>
        /// the string that will be evaluated using ncalc library
        /// </summary>
        public string MathematicalRepresentation
        {
            get
            {
                if (AssignedObject.Left_Parameter == this)
                {
                    return AssignedObject.ViewModel.LeftExpression;
                }
                else
                {
                    //right parameter
                    return AssignedObject.ViewModel.RightExpression;
                }
            }
        }

        public decimal Value
        {
            get
            {
                try
                {
                    if (AssignedObject == null || string.IsNullOrWhiteSpace(MathematicalRepresentation) || AssignedObject.Parameters.Count == 0) return 0m;

                    Expression exp = new Expression(MathematicalRepresentation , EvaluateOptions.IgnoreCase | EvaluateOptions.NoCache);
                    foreach (var item in AssignedObject.Parameters)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Name))
                            exp.Parameters[item.Name] = item.Value;
                    }

                    var ev = exp.Evaluate();
                    if (ev != null)
                    {
                        decimal final = 0;
                        decimal.TryParse(ev.ToString() , out final);
                        return final;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch /*(Exception e)*/
                {
                    return 0;
                }
            }
        }


        public void RaisePropertyChanged(string propname)
        {
            PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propname));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
