using System;
using System.Collections.Generic;
using Csa4cs;
using SimSharp;

namespace Benchmark
{
    public class SimpleModel : Model
    {

        // !properties
        public Source Source { get; set; }
        public RoundRobinGateway Gateway1 { get; set; }
        public Delay ParallelProcessing { get; set; }
        public Delay Buffer { get; set; }
        public Delay SequentialProcessing { get; set; }
        public RoundRobinGateway Gateway2 { get; set; }
        public Sink Sink { get; set; }

        public SimpleModel(Simulation environment)
        {
            // !assignments
            Source = new Source(environment, "source", 1, TimeSpan.FromSeconds(1));
            Gateway1 = new RoundRobinGateway(environment, "gateway1");
            ParallelProcessing = new Delay(environment, "delay1", int.MaxValue, TimeSpan.FromSeconds(1));
            Buffer = new Delay(environment, "buffer", int.MaxValue, TimeSpan.FromSeconds(0));
            SequentialProcessing = new Delay(environment, "delay2", 1, TimeSpan.FromSeconds(1));
            Gateway2 = new RoundRobinGateway(environment, "gateway2");
            Sink = new Sink(environment, "sink");

            // !modelGraph
            ModelGraph[Source] = new List<Block> { Gateway1 };
            ModelGraph[Gateway1] = new List<Block> { ParallelProcessing, Buffer };
            ModelGraph[ParallelProcessing] = new List<Block> { Gateway2 };
            ModelGraph[Buffer] = new List<Block> { SequentialProcessing };
            ModelGraph[SequentialProcessing] = new List<Block> { Gateway2 };
            ModelGraph[Gateway2] = new List<Block> { Sink };
            ModelGraph[Sink] = new List<Block> { };

            // init
            Init();
        }
    }
}