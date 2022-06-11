﻿using MandelbrotCSharp.Commands;
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
        private string stopwatch;
        private int height = 800;
        private int width = 800;


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
            Bitmap bitmap = new Bitmap(this.Width, this.Height);

            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Height; j++)
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
            Bitmap bitmap = new Bitmap(this.Width, this.Height);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            await Task.Run(() =>
            {
                double maxIt = 100;

                Parallel.For(0, this.Width, index =>
                {
                    Parallel.For(0, this.Height, index2 =>
                     {
                         double amount = 0.0;
                         ComplexNumber number = new ComplexNumber();
                         int counter = 0;
                         double cReal = (index - (this.Width / 2.0)) / (this.Height / 4.0);
                         double cImag = (index2 - (this.Width / 2.0)) / (this.Height / 4.0);
                         ComplexNumber c = new ComplexNumber(cReal, cImag);

                         while (counter < maxIt)
                         {
                             ComplexNumber numberHelp = new ComplexNumber();
                             numberHelp = this.Calculator.Sqaure(number);

                             number = this.Calculator.Add(numberHelp, c);
                             number = c + numberHelp;
                             amount = this.Calculator.Amount(number);

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

            sw.Stop();

            var ts = sw.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            this.Stopwatch = elapsedTime;

            return bitmap;

        }

        public string Stopwatch
        {
            get => this.stopwatch;
            set
            {
                this.stopwatch = value ?? throw new ArgumentNullException(nameof(this.Stopwatch));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Stopwatch)));
            }
        }

        public int Width
        {
            get => this.width;
            set
            {
                this.width = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Width)));
            }
        }

        public int Height
        {
            get => this.height;
            set
            {
                this.height = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Height)));
            }
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
