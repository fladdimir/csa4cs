using SimSharp;
using System;
using System.Collections.Generic;

namespace Csa4cs
{
    public class Delay : Block
    {
        public TimeSpan DelayTime { get; set; }

        public Delay(Simulation environment, string name, int capacity, TimeSpan delayTime) : base(environment, name, capacity)
        {
            DelayTime = delayTime;
        }

        protected override IEnumerable<Event> MainProcess(Entity entity)
        {
            yield return Environment.Timeout(DelayTime);
        }
    }
}