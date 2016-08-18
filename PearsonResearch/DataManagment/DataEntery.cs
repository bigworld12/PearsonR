using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCalc;
using System.ComponentModel;
using System.Diagnostics;
using LiveCharts.Defaults;

namespace PearsonResearch.DataManagment
{
    /// <summary>
    /// the object that gets generated each experiment the user makes
    /// </summary>
    public class DataEntery : INotifyPropertyChanged
    {
        private PParameter m_Left_Parameter;
        public PParameter Left_Parameter
        {
            get { return m_Left_Parameter; }
            set
            {
                m_Left_Parameter = value;
                RaisePropertyChanged(nameof(Left_Parameter));
            }
        }



        private PParameter m_Right_Parameter;
        public PParameter Right_Parameter
        {
            get { return m_Right_Parameter; }
            set
            {
                m_Right_Parameter = value;
                RaisePropertyChanged(nameof(Right_Parameter));
            }
        }


        private ViewModel.MainWindowViewModel m_ViewModel;

        public ViewModel.MainWindowViewModel ViewModel
        {
            get { return m_ViewModel; }
            set
            {
                m_ViewModel = value;
                RaisePropertyChanged(nameof(ViewModel));
            }
        }




        public DataEntery()
        {
            Parameters = new ValuedParametersList();
            Right_Parameter = new PParameter(this);
            Left_Parameter = new PParameter(this);

            Parameters.ItemPropertyChanged += Parameters_ItemPropertyChanged;
        }

        private void Parameters_ItemPropertyChanged(object sender , PropertyChangedEventArgs e)
        {
            Left_Parameter.RaisePropertyChanged(nameof(Left_Parameter.Value));
            Right_Parameter.RaisePropertyChanged(nameof(Right_Parameter.Value));
        }

        //gets called when the object is created
        public void UpdateFromAllParameters()
        {
            var curparams = RequestAllParams();
            if (curparams == null) return;
            //add items that exist in curparams and not Parameters
            //remove items that exist in Parameters and not in curparams
            foreach (var item in curparams)
            {
                if (!Parameters.Any(x => x.BaseParameter == item))
                {
                    Parameters.Add(new ValuedParameter(item));
                }
            }
            foreach (var item in Parameters.ToList())
            {
                if (!curparams.Any(x => item.BaseParameter == x))
                {
                    Parameters.Remove(item);
                }
            }
        }

        private ValuedParametersList m_Parameters;

        public ValuedParametersList Parameters
        {
            get { return m_Parameters; }
            set
            {
                m_Parameters = value;
                RaisePropertyChanged(nameof(Parameters));
            }
        }


        public event EventHandler<RequestAllParametersListEventArgs> RequestParameters;

        //will get all parameters
        public ParametersList RequestAllParams()
        {
            RequestAllParametersListEventArgs finalargs = new RequestAllParametersListEventArgs();

            RequestParameters?.Invoke(this , finalargs);
            return finalargs.Result;
        }

        public void RaisePropertyChanged(string propname)
        {
            PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propname));
        }

        private ObservablePoint basepoint;
        public ObservablePoint LeftRightPoint
        {

            get
            {
                if (basepoint == null)
                    basepoint = new ObservablePoint(Right_Parameter.Value , Left_Parameter.Value);                
                else
                {
                    basepoint.X = Right_Parameter.Value;
                    basepoint.X = Left_Parameter.Value;                  
                }
                return basepoint;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class RequestAllParametersListEventArgs
    {
        public ParametersList Result { get; set; }
    }
}
