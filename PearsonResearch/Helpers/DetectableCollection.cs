using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearsonResearch.Helpers
{
    public class DetectableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public DetectableCollection()
        {
            Handlers = new List<PropertyChangedEventHandler>();
            CollectionChanged += ValuedParametersList_CollectionChanged;
        }
        private List<PropertyChangedEventHandler> Handlers;
        public event PropertyChangedEventHandler ItemPropertyChanged
        {
            add
            {
                if (!Handlers.Contains(value)) Handlers.Add(value);                
                foreach (var item in this)
                {
                    item.PropertyChanged += value;
                }
            }
            remove
            {
                if (Handlers.Contains(value)) Handlers.Remove(value);
                foreach (var item in this)
                {
                    item.PropertyChanged -= value;
                }
            }
        }    
        private void ValuedParametersList_CollectionChanged(object sender , System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems.Cast<T>())
                    {
                        foreach (var h in Handlers)
                        {                            
                            item.PropertyChanged += h;
                        }

                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems.Cast<T>())
                    {
                        foreach (var h in Handlers)
                        {
                            item.PropertyChanged -= h;
                        }
                    }
                }
            }
        }
    }
}
