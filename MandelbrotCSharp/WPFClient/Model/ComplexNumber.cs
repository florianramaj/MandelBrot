using System;
using System.Collections.Generic;
using System.Text;

namespace WPFClient.Model
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
    }
}
