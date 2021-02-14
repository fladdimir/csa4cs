using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace Csa4cs.visualization
{
    public class Visualizer
    {
        public Model Model { get; }
        public ModelInfo ModelInfo { get; }
        public ICanvas Canvas { get; }
        public double FlowSpeedPxPerSec { get; set; }

        public Visualizer(Model model, ModelInfo modelInfo, ICanvas canvas, double flowSpeedPxPerSec = 100)
        {
            Model = model;
            ModelInfo = modelInfo;
            Canvas = canvas;
            FlowSpeedPxPerSec = flowSpeedPxPerSec;

            RegisterListener();
        }
        private void RegisterListener()
        {
            foreach (var block in Model.ModelGraph.Keys)
                block.AddListener(AnimateEntityMovement);
        }
        private void AnimateEntityMovement(Entity entity, Block from, Block to)
        {
            if (to is null) { Canvas.RemoveEntity(entity); return; }
            var visualizableBlock = ModelInfo.VisualizerInfo[from.Name];
            var wayFromBlock = visualizableBlock.Ways[to.Name];
            List<Vector2> wayPoints = new();
            wayPoints.Add(visualizableBlock.Position); // always start at from
            wayPoints.AddRange(wayFromBlock);
            wayPoints.Add(ModelInfo.VisualizerInfo[to.Name].Position); // always end at to

            // animation-loop
            int breakTimeMs = (int)(1000d / 40);
            Vector2 currentPosition = wayPoints[0];
            foreach (Vector2 target in wayPoints.GetRange(1, wayPoints.Count - 1))
            {
                Vector2 start = currentPosition;
                Vector2 direction = target - start;
                double distance = Vector2.Distance(start, target);
                double timeMs = distance / FlowSpeedPxPerSec * 1000;
                for (double t = 0; t <= timeMs; t += breakTimeMs)
                {
                    var startTspMs = NowTspMs();
                    float progress = (float)(t / timeMs);
                    currentPosition = start + Vector2.Multiply(direction, progress);
                    Canvas.DrawEntity(entity, currentPosition);
                    Thread.Sleep(RemainingBreakTimeMs(breakTimeMs, startTspMs));
                }
                currentPosition = target;
                Canvas.DrawEntity(entity, currentPosition);
            }
        }

        private static int RemainingBreakTimeMs(int breakTimeMs, long startTspMs)
        {
            return Math.Max(breakTimeMs - (int)(NowTspMs() - startTspMs), 0);
        }

        private static long NowTspMs()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

    }
}