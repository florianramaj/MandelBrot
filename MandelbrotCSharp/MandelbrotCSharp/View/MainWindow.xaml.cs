using MandelbrotCSharp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MandelbrotCSharp.View
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MandelBrotManagerVM managerVM;

        public MainWindow()
        {
            this.InitializeComponent();
            this.ManagerVM = new MandelBrotManagerVM(canvas);
            this.DataContext = this.ManagerVM;

        }

        public MandelBrotManagerVM ManagerVM
        {
            get { return this.managerVM; }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(this.ManagerVM), "Must not be empty");
                }

                this.managerVM = value;
            }
        }
    }
}
