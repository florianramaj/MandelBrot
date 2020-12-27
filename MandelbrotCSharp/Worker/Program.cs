using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using MandelbrotLib;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using WPFClient.Model;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task Worker
            // Connects PULL socket to tcp://localhost:5557
            // collects workload for socket from Ventilator via that socket
            // Connects PUSH socket to tcp://localhost:5558
            // Sends results to Sink via that socket
            Console.WriteLine("====== WORKER ======");
            using (var receiver = new PullSocket(">tcp://localhost:80"))
            using (var sender = new PushSocket(">tcp://localhost:400"))
            {
                //process tasks forever
                while (true)
                {
                    //workload from the vetilator is a simple delay
                    //to simulate some work being done, see
                    //Ventilator.csproj Proram.cs for the workload sent
                    //In real life some more meaningful work would be done
                    string workload = receiver.ReceiveFrameString();

                    //0 ... lower, 1... upper... 2..height
                    string[] workLoadArray = workload.Split(',');

                    var calculator = new Calculator();

                    var result = calculator.Calculate(Convert.ToInt32(workLoadArray[0]), Convert.ToInt32(workLoadArray[1]), Convert.ToInt32(workLoadArray[2]), 400);
                 

                    byte[] dataGame;
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    using (var memoryStream = new MemoryStream())
                    {
                        binaryFormatter.Serialize(memoryStream, result);
                        dataGame = memoryStream.ToArray();
                    }

                    Console.WriteLine("Sending to Sink");
                    sender.SendFrame(dataGame);

                    

                    

                    
                }
            }
        }
    }
}
