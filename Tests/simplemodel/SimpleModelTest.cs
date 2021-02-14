using System;
using Csa4cs;
using SimSharp;
using Xunit;

namespace Tests.simplemodel
{
    public class SimpleModelTest
    {

        [Fact]
        public void TestSimpleModelRun()
        {
            var environment = new Simulation();
            var model = new SimpleModel(environment);
            model.Source.NumberOfEntities = 5;
            model.Source.InterArrivalTime = TimeSpan.FromSeconds(1);
            model.Delay1.DelayTime = TimeSpan.FromSeconds(20); // parallel
            model.Delay2.DelayTime = TimeSpan.FromSeconds(15); // in a row

            environment.Run();

            Assert.True(environment.NowD == 31); // Entity_2 after 1 sec. goes to Delay2, Entity_4 processes from 16-31
            Assert.True(model.Sink.InCounter == 5);
            Assert.True(model.Delay1.InCounter == 3);
            Assert.True(model.Delay2.InCounter == 2);
        }

        [Fact]
        public void TestSimpleModelListenerNotifications()
        {
            var environment = new Simulation();
            var model = new SimpleModel(environment);
            model.Source.NumberOfEntities = 5;
            model.Source.InterArrivalTime = TimeSpan.FromSeconds(0);
            model.Delay1.DelayTime = TimeSpan.FromSeconds(0);
            model.Delay2.DelayTime = TimeSpan.FromSeconds(0);
            int blockStatusChanges = 0;
            int entityMovements = 0;
            int entityMovements2 = 0;
            model.Gateway1.AddListener((Block block) => blockStatusChanges++);
            model.Gateway1.AddListener((Entity entity, Block from, Block to) => entityMovements++);
            model.Gateway1.AddListener((Entity entity, Block from, Block to) => entityMovements2++);
            model.Gateway1.AddListener((Entity entity, Block from, Block to) => entityMovements2++);

            environment.Run();

            Assert.True(environment.NowD == 0);
            Assert.True(model.Sink.InCounter == 5);
            Assert.True(blockStatusChanges == 10);
            Assert.True(entityMovements == 5);
            Assert.True(entityMovements2 == 10);
        }
    }
}