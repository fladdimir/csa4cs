using System.Collections.Generic;
using System.Numerics;

namespace Csa4cs.visualization
{
    public class ModelInfo
    {
        public Dictionary<string, VisualizableBlock> VisualizerInfo { get; set; } = new();
    }

    public class VisualizableBlock
    {
        public Vector2 Position { get; set; }
        public Dictionary<string, List<Vector2>> Ways { get; } = new();
    }
}