using System;
using System.Collections.Generic;
using SimSharp;

namespace Csa4cs
{
    public class Source : Block
    {
        public int NumberOfEntities { get; set; }
        public TimeSpan InterArrivalTime { get; set; }

        public Source(Simulation environment, string name, int numberOfEntities, TimeSpan interArrivalTime) : base(environment, name, int.MaxValue)
        {
            NumberOfEntities = numberOfEntities;
            InterArrivalTime = interArrivalTime;
            Environment.Process(CreationLoop());
        }

        private IEnumerable<Event> CreationLoop()
        {
            while (InCounter < NumberOfEntities)
            {
                Entity entity = new(Name + "_Entity_" + (InCounter + 1))
                {
                    Request = Resource.Request()
                };
                yield return entity.Request;
                ReceiveEntity(entity);
                yield return Environment.Timeout(InterArrivalTime);
            }
        }
    }
}