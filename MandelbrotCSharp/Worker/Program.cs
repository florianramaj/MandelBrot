using System;
using System.Threading;
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


                    Console.WriteLine("Sending to Sink");

                    sender.SendFrame(string.Empty);
                }
            }
        }
    }
}
