using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static module_21_exercise_2_WPF.UserDataApi;

namespace module_21_exercise_2_WPF
{
    public static class ObservableCollectionLogic
    {
        public static double ToDouble(this string e) => Convert.ToDouble(e);

        public static ObservableCollection<Subscriber> ToObservableCollection(this IEnumerable<Subscriber> e)
        {
            ObservableCollection<Subscriber> t = new ObservableCollection<Subscriber>();
            foreach (var item in e)
            {
                t.Add(item);
            }
            return t;
        }
        public static ObservableCollection<User> ToObservableCollection(this IEnumerable<User> e)
        {
            ObservableCollection<User> t = new ObservableCollection<User>();
            foreach (var item in e)
            {
                t.Add(item);
            }
            return t;
        }

    }
}
