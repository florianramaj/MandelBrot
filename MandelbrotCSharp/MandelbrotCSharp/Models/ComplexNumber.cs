using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotCSharp.Models
{
    public class ComplexNumber
    {
        private double real;
        private double imag;

        public ComplexNumber(double real, double imag)
        {
            this.real = real;
            this.imag = imag;
        }

        public ComplexNumber()
        {
            this.real = 0;
            this.imag = 0;
        }


        public double Real
        {
            get
            {
                return this.real;
            }

            set
            {
                this.real = value;
            }
        }

        public double Imag
        {
            get
            {
                return this.imag;
            }

            set
            {
                this.imag = value;
            }
        }

        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b) => new ComplexNumber(a.Real + b.Real, a.Imag + b.Imag);

        public static ComplexNumber operator -(ComplexNumber number) => new ComplexNumber(number.real * number.real - number.imag * number.imag, 2.0 * number.Real * number.Imag);
    }
}
