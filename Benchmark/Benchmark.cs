using System;
using System.Collections.Generic;
using System.Diagnostics;
using SimSharp;

namespace Benchmark
{
    public static class Benchmark
    {
        const string RESULTS_PATH = "./results/results.csv";
        const string runtime = "Casymda@SimSharp(net5.0)";
        private static readonly List<int> n_entities = new() { 10, 100, 1000, 10_000, 50_000, 100_000, 200_000 };
        private static readonly List<double> inter_arrival_times = new() { 0, 10 };
        private static int c = 0;

        public static void RunBenchmark()
        {
            List<Dictionary<string, string>> results = new();
            foreach (var n_entity in n_entities)
            {
                foreach (var iat in inter_arrival_times)
                {
                    var sequential_proc_time = 10;
                    var overall_seq_time = iat + n_entity / 2 * sequential_proc_time;
                    var last_time = (n_entity - 1) * iat + sequential_proc_time;
                    var expected_end = Math.Max(last_time, overall_seq_time);
                    var t = Run(
                       max_entities: n_entity,
                       inter_arrival_time: iat,
                       sequential_proc_time: sequential_proc_time,
                       expected_end: expected_end
                   );
                    Dictionary<string, string> result = new()
                    {
                        { "runtime", runtime.ToString() },
                        { "n_entities", n_entity.ToString() },
                        { "inter_arrival_time", iat.ToString() },
                        { "time", t.ToString() }
                    };
                    results.Add(result);
                }
            }
            string content = string.Join(",", results[1].Keys);
            results.ForEach(result =>
            {
                content += System.Environment.NewLine;
                content += string.Join(",", result.Values);
            });

            System.IO.File.WriteAllText(RESULTS_PATH, content);
        }

        private static double Run(
            int max_entities = 10,
            double inter_arrival_time = 0,
            double parallel_proc_time = 10,
            double sequential_proc_time = 10,
            double expected_end = 50
            )
        {
            c++;
            var env = new Simulation();
            var model = new SimpleModel(env);

            model.Source.NumberOfEntities = max_entities;
            model.Source.InterArrivalTime = TimeSpan.FromSeconds(inter_arrival_time);

            model.ParallelProcessing.DelayTime = TimeSpan.FromSeconds(parallel_proc_time);
            model.SequentialProcessing.DelayTime = TimeSpan.FromSeconds(sequential_proc_time);

            var t0 = DateTime.Now;
            env.Run();
            var t = DateTime.Now - t0;

            Trace.Assert(model.Sink.InCounter == model.Source.InCounter);
            Trace.Assert(model.ParallelProcessing.InCounter == model.Source.NumberOfEntities / 2);
            Trace.Assert(model.SequentialProcessing.InCounter == model.Source.NumberOfEntities / 2);
            Trace.Assert(env.NowD == expected_end);

            Console.WriteLine("Finished run: " + c);
            return t.TotalSeconds;
        }
    }
}