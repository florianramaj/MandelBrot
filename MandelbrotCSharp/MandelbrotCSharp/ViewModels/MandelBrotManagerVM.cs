using MandelbrotCSharp.Commands;
using MandelbrotCSharp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MandelbrotCSharp.ViewModels
{
    public class MandelBrotManagerVM 
    {
        private ComplexCalculator calculator;
        private Canvas canvas;

        public MandelBrotManagerVM(Canvas canvas)
        {
            this.canvas = canvas;
            this.Calculator = new ComplexCalculator();
          
        }

        public ComplexCalculator Calculator
        {
            get
            {
                return this.calculator;
            }

            set
            {
                this.calculator = value;
            }
        }

        private static object locker = new object();


        public void Calculate(int beginIndex, int endIndex)
        {
            double maxIt = 100;
            this.Points = new List<Point>();


            Application.Current.Dispatcher.Invoke(() =>
            {
                         

                for (int index = beginIndex; index < endIndex; index++)
                {
                    for (int index2 = 0; index2 < this.canvas.Height; index2++)
                    {
                        double amount = 0.0;
                        ComplexNumber number = new ComplexNumber();

                        int counter = 0;
                        double cReal = (index - (this.canvas.Width / 2.0)) / (this.canvas.Width / 4.0);
                        double cImag = (index2 - (this.canvas.Height / 2.0)) / (this.canvas.Height / 4.0);
                        ComplexNumber c = new ComplexNumber(cReal, cImag);

                        while (counter < maxIt)
                        {
                            ComplexNumber numberHelp = new ComplexNumber();
                            numberHelp = this.Calculator.Sqaure(number);

                            number = this.Calculator.Add(numberHelp, c);
                            amount = this.Calculator.Amount(number);

                            if (amount > 4)
                            {
                                break;
                            }

                            counter++;
                        }

                        Rectangle rec = new Rectangle();
                        rec.Width = 1;
                        rec.Height = 1;

                        //rec.Fill = Brushes.Black;

                        if (counter == maxIt)
                        {
                            rec.Fill = System.Windows.Media.Brushes.Red;
                            // this.Points.Add(new Point(index, index2));
                           // Debug.WriteLine("X " + index + " Y " + index2);
                        }

                        Canvas.SetLeft(rec, index);
                        Canvas.SetTop(rec, index2);
                        canvas.Children.Add(rec);
                    }
                }
            });


        }

        public List<Point> Points
        {
            get;
            set;
        }

        public ICommand CalculateCommand
        {
            get
            {
                return new Command(obj =>
                {

                    int intervall = 10;
                    int canvasHelpBegin = 0;
                    int canvasHelpEnd = (int)this.canvas.Width / intervall;
                    int different = (int)this.canvas.Width / intervall;


                    for (int i = 0; i < intervall; i++)
                    {
                        Thread thread = new Thread(delegate () { this.Calculate(canvasHelpBegin, canvasHelpEnd); });
                        if(i == intervall)
                        {
                            break;
                        }
                        canvasHelpBegin = canvasHelpEnd;
                        canvasHelpEnd += different;
                        thread.Start();
                        Thread.Sleep(50);
                    }

                 

                });
            }
        }
    }
}
