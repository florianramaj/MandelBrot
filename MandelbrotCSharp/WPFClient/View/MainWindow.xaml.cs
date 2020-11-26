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
using WPFClient.ViewModel;

namespace WPFClient.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MandelBrotVM managerVM;

        public MainWindow()
        {
            InitializeComponent();
            this.ManagerVM = new MandelBrotVM();
            this.DataContext = this.ManagerVM;
        }

        public MandelBrotVM ManagerVM
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
