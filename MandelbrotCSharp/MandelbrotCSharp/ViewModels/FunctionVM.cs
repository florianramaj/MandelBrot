using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MandelbrotCSharp.ViewModels
{
    public class FunctionVM
    {
        public FunctionVM()
        {
            this.Points = new ObservableCollection<Point>();
        }

        public ObservableCollection<Point> Points
        {
            get;
            set;
        }
    }
}
