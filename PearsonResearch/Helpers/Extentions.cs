using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearsonResearch.Helpers
{
    public static class Extentions
    {
        public static T Find<T> (this ObservableCollection<T> tofindfrom , Predicate<T> pred)
        {
            T final = default(T);
            for (int i = 0; i < tofindfrom.Count; i++)
            {
                var item = tofindfrom[i];
                if (pred(item))
                {
                    final = item;
                    break;
                }
            }
            return final;
        }

    }
}
