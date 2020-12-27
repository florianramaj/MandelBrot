using MandelbrotLib;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
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
                /*
                var json = JsonConvert.SerializeObject(new CalculationRequest() { Height = 400, Width = 400 });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://localhost:62602/api/client", data);
                var responseString = await response.Content.ReadAsStringAsync();
                var valueList = JsonConvert.DeserializeObject<List<TripleResult>>(responseString);
                */
                List<(int, int, int)> valueList = new List<(int, int, int)>();

                    //socket to receive messages on
                    using (var receiver = new PullSocket("@tcp://localhost:400"))
                    {


                        //Start our clock now
                        var watch = Stopwatch.StartNew();

                        for (int taskNumber = 0; taskNumber < 400; taskNumber = taskNumber + 10)
                        {
                            var workerDoneTrigger = receiver.ReceiveFrameBytes();
                            List<(int, int, int)> gameField = null;
                            BinaryFormatter binaryFormatter2 = new BinaryFormatter();

                            using (var memoryStream2 = new MemoryStream(workerDoneTrigger))
                            {
                                gameField = (List<(int, int, int)>)binaryFormatter2.Deserialize(memoryStream2);
                                valueList.AddRange(gameField);
                            }

                        }
                        watch.Stop();
                    }

                    var resultTripkeResult = valueList.Select(item => new TripleResult()
                    {
                        X = item.Item1,
                        Y = item.Item2,
                        Iteration = item.Item3
                    }).ToList();

                    var bitmap = new Bitmap(400, 400);

                    foreach (var value in resultTripkeResult)
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
