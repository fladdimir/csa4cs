using SimSharp;

namespace Csa4cs
{
    public class Entity
    {
        public string Name { get; private set; }

        public Request Request { get; set; }

        public Entity(string name)
        {
            Name = name;
        }

    }
}