using MandelbrotCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MandelbrotCSharp.ViewModels
{
    public class ComplexCalculatorVM
    {
        private readonly ComplexCalculator complexCalculator;

        public ComplexCalculatorVM(ComplexCalculator calculator)
        {
            this.complexCalculator = calculator;
        }

    }
}
