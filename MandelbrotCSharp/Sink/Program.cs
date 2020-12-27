using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using NetMQ;
using NetMQ.Sockets;
namespace Sink
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Task Sink
            // Bindd PULL socket to tcp://localhost:5558
            // Collects results from workers via that socket
            Console.WriteLine("====== SINK ======");

            //socket to receive messages on
            using (var receiver = new PullSocket("@tcp://localhost:400"))
            {
                //wait for start of batch (see Ventilator.csproj Program.cs)
             //   var startOfBatchTrigger = receiver.ReceiveFrameString();
                Console.WriteLine("Seen start of batch");

                //Start our clock now
                var watch = Stopwatch.StartNew();

                for (int taskNumber = 0; taskNumber < 400; taskNumber= taskNumber+10)
                {
                    var workerDoneTrigger = receiver.ReceiveFrameBytes();
                    List<(int, int, int)> gameField = null;
                    BinaryFormatter binaryFormatter2 = new BinaryFormatter();

                    using (var memoryStream2 = new MemoryStream(workerDoneTrigger))
                    {
                        gameField = (List<(int, int, int)>)binaryFormatter2.Deserialize(memoryStream2);
                    }

                }
                watch.Stop();
                //Calculate and report duration of batch
                Console.WriteLine();
                Console.WriteLine("Total elapsed time {0} msec", watch.ElapsedMilliseconds);
                Console.ReadLine();
            }
        }
    }
}
