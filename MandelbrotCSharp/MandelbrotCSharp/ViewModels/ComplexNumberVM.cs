using MandelbrotCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotCSharp.ViewModels
{
    public class ComplexNumberVM
    {
        private ComplexNumber number;

        public ComplexNumberVM(ComplexNumber number)
        {
            this.number = number;
        }

        public double Imag
        {
            get
            {
                return this.number.Imag;
            }

            set
            {
                this.number.Imag = value;
            }
        }

        public double Real
        {
            get
            {
                return this.number.Real;
            }

            set
            {
                this.number.Real = value;
            }
        }
    }
}
