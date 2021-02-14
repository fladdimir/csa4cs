using System;
using System.Collections.Generic;
using System.Numerics;
using Csa4cs;
using Csa4cs.visualization;
using SimSharp;

namespace Tests.visualization.skiasharp
{
    public class SampleModel : Model
    {

        // !properties
        public Source Source;
        public Delay Delay;
        public Sink Sink;

        public SampleModel(Simulation environment)
        {
            // !assignments
            Source = new Source(environment, "source", 1, TimeSpan.FromSeconds(1));
            Delay = new Delay(environment, "delay", int.MaxValue, TimeSpan.FromSeconds(1));
            Sink = new Sink(environment, "sink");

            // !modelGraph
            ModelGraph[Source] = new List<Block> { Delay };
            ModelGraph[Delay] = new List<Block> { Sink };
            ModelGraph[Sink] = new List<Block> { };

            Init();
        }
    }

    public class SampleModelInfo : ModelInfo
    {
        public SampleModelInfo()
        {
            VisualizerInfo["source"] = new VisualizableBlock
            {
                Position = new Vector2(6, 32)
            };
            VisualizerInfo["source"].Ways["delay"] = new List<Vector2> { new Vector2(6, 105) };

            VisualizerInfo["delay"] = new VisualizableBlock
            {
                Position = new Vector2(105, 105)
            };
            VisualizerInfo["delay"].Ways["sink"] = new List<Vector2> { };

            VisualizerInfo["sink"] = new VisualizableBlock
            {
                Position = new Vector2(212, 105)
            };
        }
    }
}