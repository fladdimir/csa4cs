using Csa4cs;
using SimSharp;
using System;
using System.Collections.Generic;

namespace Tests.simplemodel
{
    public class SimpleModel : Model
    {

        // !properties
        public Source Source;
        public RoundRobinGateway Gateway1;
        public Delay Delay1;
        public Delay Buffer;
        public Delay Delay2;
        public RoundRobinGateway Gateway2;
        public Sink Sink;

        public SimpleModel(Simulation environment)
        {
            // !assignments
            Source = new Source(environment, "source", 1, TimeSpan.FromSeconds(1));
            Gateway1 = new RoundRobinGateway(environment, "gateway1");
            Delay1 = new Delay(environment, "delay1", int.MaxValue, TimeSpan.FromSeconds(1));
            Buffer = new Delay(environment, "buffer", int.MaxValue, TimeSpan.FromSeconds(0));
            Delay2 = new Delay(environment, "delay2", 1, TimeSpan.FromSeconds(1));
            Gateway2 = new RoundRobinGateway(environment, "gateway2");
            Sink = new Sink(environment, "sink");

            // !modelGraph
            ModelGraph[Source] = new List<Block> { Gateway1 };
            ModelGraph[Gateway1] = new List<Block> { Delay1, Buffer };
            ModelGraph[Delay1] = new List<Block> { Gateway2 };
            ModelGraph[Buffer] = new List<Block> { Delay2 };
            ModelGraph[Delay2] = new List<Block> { Gateway2 };
            ModelGraph[Gateway2] = new List<Block> { Sink };
            ModelGraph[Sink] = new List<Block> { };

            // init
            Init();
        }
    }
}