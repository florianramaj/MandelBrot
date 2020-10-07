using MandelbrotCSharp.Commands;
using MandelbrotCSharp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MandelbrotCSharp.ViewModels
{
    public class MandelBrotManagerVM : INotifyPropertyChanged
    {
        private ComplexCalculator calculator;
        private Canvas canvas;
        private BitmapImage image;
        private object lockerTemp;

        public MandelBrotManagerVM()
        {
            this.canvas = canvas;
            this.Calculator = new ComplexCalculator();
            this.lockerTemp = new object();

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

        public event PropertyChangedEventHandler PropertyChanged;

        public Bitmap ToBitmap()
        {
            Bitmap bitmap = new Bitmap(400, 400);

            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    bitmap.SetPixel(i, j, System.Drawing.Color.Red);
                }
            }

            return bitmap;
        }


        public BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;

            }
        }


        public async Task<Bitmap> Calculate()
        {
            Bitmap bitmap = new Bitmap(400, 400);

            await Task.Run(() =>
            {
                double maxIt = 100;
                // this.Points = new List<Point>();
                MandelbrotCSharp.Models.Image image = new MandelbrotCSharp.Models.Image(400, 400);

                

                Parallel.For(0, 400, index =>
                {
                    Parallel.For(0,400, index2 =>
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
                            numberHelp = this.Calculator.Sqaure(number);

                            number = this.Calculator.Add(numberHelp, c);
                            number = c + numberHelp;
                            amount = number.Real * number.Real + number.Imag * number.Imag;

                            if (amount > 4)
                            {
                                break;
                            }

                            counter++;
                        }

                        //Rectangle rec = new Rectangle();
                        //rec.Width = 1;
                        //rec.Height = 1;

                        //rec.Fill = Brushes.Black;

                        if (counter == maxIt)
                        {
                            //rec.Fill = System.Windows.Media.Brushes.Red;
                            //this.Points.Add(new Point(index, index2));
                            // Debug.WriteLine("X " + index + " Y " + index2);
                            lock (this.lockerTemp)
                            {
                                bitmap.SetPixel(index, index2, System.Drawing.Color.Red);
                            }
                            //this.Image = this.ToBitmapImage(bitmap);
                        }

                        //Canvas.SetLeft(rec, index);
                        //Canvas.SetTop(rec, index2);
                        //canvas.Children.Add(rec);
                    });


                });

            });

            return bitmap;

        }




        public BitmapImage Image
        {
            get => this.image;
            set
            {
                this.image = value ?? throw new ArgumentNullException(nameof(this.Image));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Image)));
            }
        }

        public ICommand CalculateCommand
        {
            get
            {
                return new Command(async obj =>
                {

                    Bitmap map = await this.Calculate();
                    this.Image = this.ToBitmapImage(map);

                    /*
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
                    }*/



                });
            }
        }


    }
}
