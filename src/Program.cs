using NumSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Tensorflow;
using Tensorflow.Util;

namespace CalcEventsTFS
{
    internal class Program
    {
        private static string modelLocation = @"../../../../model/";

        private static void Main(string[] args)
        {
            var thcount = Process.GetCurrentProcess().Threads.Count;
            Console.WriteLine($"Threads: {thcount}");

            if (args.Length > 0)
                modelLocation = args[0];

            while (true)
            {
                Work();
                Console.WriteLine("End");

                thcount = Process.GetCurrentProcess().Threads.Count;
                Console.WriteLine($"Threads: {thcount}");

                Console.WriteLine("Press Q to break or any another to repeat");
                if (Console.ReadKey().Key == ConsoleKey.Q)
                    break;
            }
        }

        private static void Work()
        {
            var graph = c_api.TF_NewGraph();
            var status = new Status();
            var opt = new SessionOptions();

            var tags = new string[] { "serve" };

            var session = Session.LoadFromSavedModel(modelLocation).as_default();

            var inputs = new[] { "sp", "fuel" };

            var inp = inputs.Select(name => session.graph.OperationByName(name).output).ToArray();
            var outp = session.graph.OperationByName("softmax_tensor").output;

            for (var i = 0; i < 1; i++)
            {
                {
                    var data = new float[96];
                    FeedItem[] feeds = new FeedItem[2];

                    for (int f = 0; f < 2; f++)
                        feeds[f] = new FeedItem(inp[f], new NDArray(data));

                    try
                    {
                        session.run(outp, feeds);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            session.close();
        }
    }
}
