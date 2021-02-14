using System;
using Csa4cs.visualization;
using Csa4cs.visualization.skiasharp;
using SimSharp;
using Xunit;

namespace Tests.visualization.skiasharp
{
    public class SkiaSharpTest
    {
        [Fact]
        public void TestCanvas()
        {
            Uri bgImageUri = new("file:///home/s/def/csa4cs/Tests/visualization/skiasharp/diagram.png");
            Uri entityIconUri = new("file:///home/s/def/csa4cs/Tests/visualization/skiasharp/simple_entity_icon.png");
            ICanvas canvas = new SkiaSharpCanvas(bgImageUri, entityIconUri);

            var env = new Simulation();
            var model = new SampleModel(env);
            model.Source.NumberOfEntities = 2;
            model.Source.InterArrivalTime = TimeSpan.FromSeconds(2);
            model.Delay.DelayTime = TimeSpan.FromSeconds(1);
            var modelInfo = new SampleModelInfo();
            var visualizer = new Visualizer(model, modelInfo, canvas, flowSpeedPxPerSec: 2e20);
            Assert.True(visualizer is not null);

            env.Run();

            Assert.True(model.Sink.InCounter == model.Source.NumberOfEntities);
        }

        [Fact]
        public void TestSampleModel()
        {
            var env = new Simulation();
            var model = new SampleModel(env);
            model.Source.NumberOfEntities = 5;
            model.Source.InterArrivalTime = TimeSpan.FromSeconds(5);
            model.Delay.DelayTime = TimeSpan.FromSeconds(10);

            env.Run();

            Assert.True(env.NowD == 30);
            Assert.True(model.Sink.InCounter == model.Source.NumberOfEntities);
        }
    }
}