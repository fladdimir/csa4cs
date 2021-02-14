using System.Numerics;

namespace Csa4cs.visualization
{
    public interface ICanvas
    {
        void DrawEntity(Entity entity, Vector2 currentPosition);
        void RemoveEntity(Entity entity);
    }
}