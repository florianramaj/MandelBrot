using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class Calculator
    {
        private object lockerTemp;

        public Calculator()
        {
            this.lockerTemp = new object();
            this.ComplexCalculator = new ComplexCalculator();
        }

        public ComplexCalculator ComplexCalculator
        {
            get;
            set;
        }

        public async Task<Bitmap> Calculate()
        {
            Bitmap bitmap = new Bitmap(400, 400);

            await Task.Run(() =>
            {
                double maxIt = 100;

                Parallel.For(0, 400, index =>
                {
                    Parallel.For(0, 400, index2 =>
                    {
                        double amount = 0.0;
                        ComplexNumber number = new ComplexNumber();
                        int counter = 0;
                        double cReal = (index - (400 / 2.0)) / (400 / 4.0);
                        double cImag = (index2 - (400 / 2.0)) / (400 / 4.0);
                        ComplexNumber c = new ComplexNumber(cReal, cImag);

                        while (counter < maxIt)
                        {
                            ComplexNumber numberHelp = new ComplexNumber();
                            numberHelp = this.ComplexCalculator.Sqaure(number);

                            number = this.ComplexCalculator.Add(numberHelp, c);
                            number = c + numberHelp;
                            amount = this.ComplexCalculator.Amount(number);

                            if (amount > 4)
                            {
                                break;
                            }

                            counter++;
                        }

                        lock (this.lockerTemp)
                        {
                            bitmap.SetPixel(index, index2, System.Drawing.Color.Black);
                        }

                        if (counter == maxIt)
                        {
                            lock (this.lockerTemp)
                            {
                                bitmap.SetPixel(index, index2, System.Drawing.Color.Red);
                            }
                        }
                    });


                });

            });

            return bitmap;
        }

        public List<(int, int, int)> Calculate(int width, int height)
        {
            //Bitmap bitmap = new Bitmap(width, height);
            var valueList = new List<(int, int, int)>();

            
                double maxIt = 100;

                Parallel.For(0, width, index =>
                {
                    Parallel.For(0, height, index2 =>
                    {
                        double amount = 0.0;
                        ComplexNumber number = new ComplexNumber();
                        int counter = 0;
                        double cReal = (index - (width / 2.0)) / (width / 4.0);
                        double cImag = (index2 - (height / 2.0)) / (height / 4.0);
                        ComplexNumber c = new ComplexNumber(cReal, cImag);

                        while (counter < maxIt)
                        {
                            ComplexNumber numberHelp = new ComplexNumber();
                            numberHelp = this.ComplexCalculator.Sqaure(number);

                            number = this.ComplexCalculator.Add(numberHelp, c);
                            number = c + numberHelp;
                            amount = this.ComplexCalculator.Amount(number);

                            if (amount > 4)
                            {
                                break;
                            }

                            counter++;
                        }

                        lock (this.lockerTemp)
                        {
                            valueList.Add((index, index2, counter));
                           // bitmap.SetPixel(index, index2, System.Drawing.Color.Black);
                        }

                        /*
                        if (counter == maxIt)
                        {
                            lock (this.lockerTemp)
                            {
                                bitmap.SetPixel(index, index2, System.Drawing.Color.Red);
                            }
                        }*/
                    });


                });

            return valueList;
        }
    }
}
