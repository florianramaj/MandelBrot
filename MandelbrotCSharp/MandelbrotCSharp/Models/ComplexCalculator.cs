using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MandelbrotCSharp.Models
{
    public class ComplexCalculator
    {
        public ComplexNumber Add(ComplexNumber complexNumber1, ComplexNumber complexNumber2)
        {
            ComplexNumber result = new ComplexNumber();

            result.Imag = complexNumber1.Imag + complexNumber2.Imag;
            result.Real = complexNumber1.Real + complexNumber2.Real;

            return result;
        }

        public ComplexNumber Sqaure(ComplexNumber complexNumber1)
        {
            ComplexNumber result = new ComplexNumber();

            result.Real = complexNumber1.Real * complexNumber1.Real - complexNumber1.Imag * complexNumber1.Imag;
            result.Imag = 2.0 * complexNumber1.Real * complexNumber1.Imag;

            return result;
        }

        public double Amount(ComplexNumber complexNumber)
        {
            double result = 0;

            result = complexNumber.Real * complexNumber.Real + complexNumber.Imag * complexNumber.Imag;

            return result;
        }
    }
}
