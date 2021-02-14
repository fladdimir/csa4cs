using SimSharp;

namespace Csa4cs
{
    public class RoundRobinGateway : Block
    {
        public RoundRobinGateway(Simulation environment, string name) : base(environment, name, 1)
        { }

        protected override Block FindSuccessor()
        {
            int nextIndex = (int)((InCounter - 1) % Successors.Count);
            return Successors[nextIndex];
        }
    }
}