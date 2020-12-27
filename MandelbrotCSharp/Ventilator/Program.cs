using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

namespace Ventilator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task Ventilator
            // Binds PUSH socket to tcp://localhost:5557
            // Sends batch of tasks to workers via that socket
            Console.WriteLine("====== VENTILATOR ======");
            using (var sender = new PushSocket("@tcp://*:80"))
            using (var sink = new PullSocket(">tcp://localhost:8080"))
            {
                Thread.Sleep(1000);
                //the first message it "0" and signals start of batch
                //see the Sink.csproj Program.cs file for where this is used
                Console.WriteLine("Sending start of batch to Sink");
                //sink.SendFrame("0");
                Console.WriteLine("Sending tasks to workers");
                //initialise random number generator
                Random rand = new Random(0);
                //expected costs in Ms
                int totalMs = 0;
                //send 100 tasks (workload for tasks, is just some random sleep time that
                //the workers can perform, in real life each work would do more than sleep
                int upper = 10;
                int height = 400;
                for (int lower = 0; lower < height; lower += 10)
                {
                    
                    sender.SendFrame(lower + "," + upper + "," + height);
                    upper += 10;
                    
                }
                Console.WriteLine("Total expected cost : {0} msec", totalMs);
                Console.WriteLine("Press Enter to quit");
                Console.ReadLine();
            }
        }
    }
}
