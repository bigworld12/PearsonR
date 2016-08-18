using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PearsonResearch.DataManagment
{
    /// <summary>
    /// the small bits that form the big x/y variable
    /// </summary>
    public class Parameter : INotifyPropertyChanged
    {



        private string m_name;
        /// <summary>
        /// can contain complex names or simple characters like
        /// $lengh ,  $width , $a
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }





        private string m_Desc;
        /// <summary>
        /// what it represents
        /// </summary>
        public string Description
        {
            get { return m_Desc; }
            set { m_Desc = value; RaisePropertyChanged(nameof(Description)); }
        }



        public void RaisePropertyChanged(string propname)
        {
            PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propname));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ValuedParameter : INotifyPropertyChanged
    {
        public Parameter BaseParameter;


        public string Name
        {
            get { return BaseParameter.Name; }
            set
            {
                BaseParameter.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
        public string Description
        {
            get { return BaseParameter.Description; }
            set
            {
                BaseParameter.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }


        public ValuedParameter(Parameter tocopy)
        {
            BaseParameter = tocopy;
            Value = 0;
        }

        private decimal m_Value;
        public void RaisePropertyChanged(string propname)
        {
            PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propname));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public decimal Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                RaisePropertyChanged(nameof(Value));
            }
        }
    }
}
