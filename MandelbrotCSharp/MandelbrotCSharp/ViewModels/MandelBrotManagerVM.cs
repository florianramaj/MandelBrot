using MandelbrotCSharp.Commands;
using MandelbrotCSharp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        private static readonly HttpClient httpClient = new HttpClient();
        private ComplexCalculator calculator;
        private Canvas canvas;
        private BitmapImage image;
        

        public MandelBrotManagerVM()
        {
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

                    var json = JsonConvert.SerializeObject(new CalculationRequest() { Height = 400, Width = 400 });
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("http://localhost:62602/api/client", data);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var valueList = JsonConvert.DeserializeObject<List<TripleResult>>(responseString);

                    /*
                     * var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                     //dto
                     foreach (var value in valueList)
                    {
                        bitmap.SetPixel(value.X, value.Y, value.Iteration < 100 ? Color.Black : Color.White);
                    }*/


                    Bitmap map = await this.Calculate();
                    this.Image = this.ToBitmapImage(map);
                });
            }
        }


    }
}
