using PearsonResearch.DataManagment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace PearsonResearch.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            AllParametersList = new ParametersList();
            DataEnteries = new DataEntryList();


            AllParametersList.CollectionChanged += AllParametersList_CollectionChanged;
            AllParametersList.ItemPropertyChanged += AllParametersList_ItemPropertyChanged;

            DataEnteries.CollectionChanged += DataEnteries_CollectionChanged;
            DataEnteries.ItemPropertyChanged += DataEnteries_ItemPropertyChanged;
            
            InitiateColumns();
        }

        public void InitiateColumns()
        {
            columnsinstance = new ObservableCollection<DataGridColumn>();
            //===================================================================
            var leftcol = new DataGridTextColumn();
            leftcol.IsReadOnly = true;
            leftcol.Header = "Left";

            var leftbind = new Binding();
            leftbind.Path = new System.Windows.PropertyPath("Left_Parameter.Value");
            leftbind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            leftbind.Mode = BindingMode.OneWay;

            leftcol.Binding = leftbind;
            columnsinstance.Add(leftcol);
            //===================================================================
            var righttcol = new DataGridTextColumn();
            righttcol.IsReadOnly = true;
            righttcol.Header = "Right";

            var rightbind = new Binding();
            rightbind.Path = new System.Windows.PropertyPath("Right_Parameter.Value");
            rightbind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            rightbind.Mode = BindingMode.OneWay;

            righttcol.Binding = rightbind;
            columnsinstance.Add(righttcol);
        }

        #region DataEnteries
        private DataEntryList m_DataEnteries;
        public DataEntryList DataEnteries
        {
            get { return m_DataEnteries; }
            set
            {
                m_DataEnteries = value;
                RaisePropertyChanged(nameof(DataEnteries));
            }
        }

        /// <summary>
        /// not really important
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataEnteries_ItemPropertyChanged(object sender , PropertyChangedEventArgs e)
        {
            /*
             Data Entery has properties :
             Left_Parameter
             Right_Parameter
             Parameters
            =================
            Connection between viewModel and Data entery properties is done here            
             */
            DataEntery de = sender as DataEntery;

            de.Left_Parameter.RaisePropertyChanged(nameof(de.Left_Parameter.Value));
            de.Right_Parameter.RaisePropertyChanged(nameof(de.Right_Parameter.Value));
        }

        private void DataEnteries_CollectionChanged(object sender , System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add || e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems.Cast<DataEntery>())
                    {
                        item.Right_Parameter.PropertyChanged += Right_Parameter_PropertyChanged;
                        item.Left_Parameter.PropertyChanged += Left_Parameter_PropertyChanged;
                        //when adding/removing items from DataEntries list
                        item.Left_Parameter.MathematicalRepresentation = LeftExpression;
                        item.Right_Parameter.MathematicalRepresentation = RightExpression;

                        item.RequestParameters += Item_RequestParameters;
                        item.UpdateFromAllParameters();
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems.Cast<DataEntery>())
                    {
                        item.RequestParameters -= Item_RequestParameters;
                    }
                }
            }

        }

        private void Left_Parameter_PropertyChanged(object sender , PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                RaisePropertyChanged(nameof(LeftSum));
                RaisePropertyChanged(nameof(LeftRightSum));
                RaisePropertyChanged(nameof(LeftSquaredSum));

                RaisePropertyChanged(nameof(PearsonR));
            }         
        }

        private void Right_Parameter_PropertyChanged(object sender , PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                RaisePropertyChanged(nameof(RightSum));
                RaisePropertyChanged(nameof(LeftRightSum));
                RaisePropertyChanged(nameof(RightSquaredSum));

                RaisePropertyChanged(nameof(PearsonR));
            }
        }

        private void Item_RequestParameters(object sender , RequestAllParametersListEventArgs e)
        {
            e.Result = AllParametersList;
        }


        #endregion

        #region Parameters
        private ParametersList m_ParametersList;
        /// <summary>
        /// contains all the parameters used in the experiment
        /// </summary>
        public ParametersList AllParametersList
        {
            get { return m_ParametersList; }
            set
            {
                m_ParametersList = value;
                RaisePropertyChanged(nameof(AllParametersList));
                RaisePropertyChanged(nameof(DataEntryColumns));
            }
        }


        private ObservableCollection<DataGridColumn> columnsinstance;
        /// <summary>
        /// changes based on the values of AllParametersList property of the view model
        /// </summary>
        public ObservableCollection<DataGridColumn> DataEntryColumns
        {
            get
            {
                if (columnsinstance == null)
                {
                    columnsinstance = new ObservableCollection<DataGridColumn>();
                }

                /*
                 Column headers are bound to the names of ALLParameters
                 */
                //2 or more (has the left and right)
                if (columnsinstance.Count == AllParametersList.Count + 2)
                {
                    return columnsinstance;
                }
                else
                {
                    //add new columns based on header name ?
                    for (int i = 0; i < AllParametersList.Count; i++)
                    {
                        Parameter item = AllParametersList[i];
                        if (!CheckColumnByHeader(item.Name))
                        {
                            //in the DataEntry object
                            //use set binding for the data context
                            var newcol = new DataGridTextColumn();
                            //header will get from source item
                            //header will get from path Name of source item

                            var headerbinding = new Binding();
                            headerbinding.Source = item;
                            headerbinding.Path = new System.Windows.PropertyPath("Name");
                            headerbinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                            headerbinding.Mode = BindingMode.OneWay;

                            BindingOperations.SetBinding(newcol , DataGridColumn.HeaderProperty , headerbinding);

                            var bind = new Binding();
                            //Parameters property of the DataEntry object
                            bind.Path = new System.Windows.PropertyPath($"Parameters[{i}].Value");
                            bind.Mode = BindingMode.TwoWay;
                            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                            newcol.Binding = bind;

                            columnsinstance.Insert(i , newcol);
                        }
                    }
                }





                return columnsinstance;
            }


        }

        public bool CheckColumnByHeader(string Header)
        {
            return columnsinstance.Any(x => x.GetValue(DataGridColumn.HeaderProperty).ToString() == Header);
        }


        private void AllParametersList_CollectionChanged(object sender , System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {/*
            When Parameters get added/removed
            1 - Update columns
            2 - Add/Remove it to all DataEntery Objects (using UpdateFromAllParameters)            
             */
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var param = item as Parameter;
                    foreach (var toupdate in DataEnteries)
                    {

                        toupdate.UpdateFromAllParameters();
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var param = item as Parameter;
                    foreach (var toupdate in DataEnteries)
                    {
                        toupdate.UpdateFromAllParameters();
                    }
                }
            }
            RaisePropertyChanged(nameof(DataEntryColumns));
        }

        private void AllParametersList_ItemPropertyChanged(object sender , PropertyChangedEventArgs e)
        {
            //shouldn't affect updating the list
            //RaisePropertyChanged(nameof(DataEntryColumns));
            Parameter changeditem = sender as Parameter;

            foreach (var item in DataEnteries)
            {
                var changedparam = item.Parameters.First(x => x.BaseParameter == changeditem);
                changedparam.RaisePropertyChanged(e.PropertyName);
            }
        }

        #endregion

        //handle left parameter and right parameter




        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propname)
        {
            PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propname));
        }

        #region Pearson's Values
        public decimal LeftSum
        {
            get
            {
                decimal finalsum = 0;
                foreach (var item in DataEnteries)
                {
                    finalsum += item.Left_Parameter.Value;
                }
                return finalsum;                
            }
        }
        public decimal RightSum
        {
            get
            {
                decimal finalsum = 0;
                foreach (var item in DataEnteries)
                {
                    finalsum += item.Right_Parameter.Value;
                }
                return finalsum;
            }
        }
        public decimal LeftRightSum
        {
            get
            {
                decimal finalsum = 0;
                foreach (var item in DataEnteries)
                {
                    finalsum += (item.Left_Parameter.Value * item.Right_Parameter.Value);
                }
                return finalsum;
            }
        }
        public double LeftSquaredSum
        {
            get
            {
                double finalsum = 0;
                foreach (var item in DataEnteries)
                {
                    finalsum += Pow(item.Left_Parameter.Value,2);
                }
                return finalsum;
            }
        }
        public double RightSquaredSum
        {
            get
            {
                double finalsum = 0;
                foreach (var item in DataEnteries)
                {
                    finalsum += Pow(item.Right_Parameter.Value , 2);
                }
                return finalsum;
            }
        }

        public double PearsonR
        {
            get
            {
                /*
                 (c * sum[x * y]) - (sum[x]*sum[y])
          /
          sqrt(c*sum[ x^2 ] - sum[x] ^ 2) * sqrt(c * sum[ y^2 ] - sum[y] ^ 2 )
                  */
                int c = DataEnteries.Count;
                double TOP = 1;
                double.TryParse(((c * LeftRightSum) - (LeftSum * RightSum)).ToString(),out TOP);

                double BOTTOM = 
                    Math.Sqrt((c*LeftSquaredSum)-(Pow(LeftSum,2))) 
                    *
                    Math.Sqrt((c * RightSquaredSum) - (Pow(RightSum , 2)));


                return TOP / BOTTOM;
            }
        }
        #endregion




        public double Pow(decimal first,decimal second)
        {
            double dfirst = 1d;
            double dsec = 1d;

            double.TryParse(first.ToString() , out dfirst);
            double.TryParse(second.ToString() , out dsec);

            return Math.Pow(dfirst , dsec);

        }


        private string m_LeftExpression;
        public string LeftExpression
        {
            get { return m_LeftExpression; }
            set
            {
                m_LeftExpression = value;
                RaisePropertyChanged(nameof(LeftExpression));
            }
        }



        private string m_RightExpression;
        public string RightExpression
        {
            get { return m_RightExpression; }
            set
            {
                m_RightExpression = value;
                RaisePropertyChanged(nameof(RightExpression));
            }
        }


        //now we work on the maths part
    }
}
