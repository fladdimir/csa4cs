using SimSharp;


namespace Csa4cs
{
    public class Sink : Block
    {
        public Sink(Simulation environment, string name) : base(environment, name, int.MaxValue)
        {
        }

        protected override void ReceiveEntity(Entity entity)
        {
            InCounter++;
            Resource.Release(entity.Request);
            NotifyEntityMovementListeners(entity, null); // move into the void
        }
    }
}