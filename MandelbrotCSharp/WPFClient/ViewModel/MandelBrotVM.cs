using MandelbrotLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFClient.Commands;
using WPFClient.Model;

namespace WPFClient.ViewModel
{
    public class MandelBrotVM : INotifyPropertyChanged
    {
        private BitmapImage image;
        private static readonly HttpClient httpClient = new HttpClient();

        public event PropertyChangedEventHandler PropertyChanged;


        public MandelBrotVM()
        {
            this.Calculator = new Calculator();
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

        public Calculator Calculator
        {
            get;
            set;
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


                    var bitmap = new Bitmap(400, 400);

                    foreach (var value in valueList)
                    {
                        bitmap.SetPixel(value.X, value.Y, value.Iteration < 100 ? Color.Black : Color.White);
                    }


                    // Bitmap map = await this.Calculator.Calculate();
                    // this.Image = this.ToBitmapImage(map);#
                    this.Image = this.ToBitmapImage(bitmap);
                });
            }
        }
    }
}
