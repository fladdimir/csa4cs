using System.Collections.Generic;

namespace Csa4cs
{
    public class Model
    {
        public Dictionary<Block, List<Block>> ModelGraph { get; } = new();

        protected void Init()
        {
            foreach (var entry in ModelGraph)
            {
                entry.Key.Successors = entry.Value;
            }
        }

    }
}