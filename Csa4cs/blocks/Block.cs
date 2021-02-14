using System.Collections.Generic;
using SimSharp;

namespace Csa4cs
{
    public class Block : ActiveObject<Simulation>
    {
        public string Name { get; }
        public Resource Resource { get; }
        public List<Block> Successors { get; set; }
        public List<Entity> Entities { get; } = new();
        public long InCounter { get; protected set; } = 0;
        public event BlockStatusChangeListener BlockStatusChangeEvent;
        public event EntityMovementListener EntityMovementEvent;

        public Block(Simulation environment, string name, int capacity) : base(environment)
        {
            Name = name;
            Resource = new Resource(environment, capacity);
        }

        /// <summary>
        /// Main entry-point for entities coming from predecessor-blocks.
        /// </summary>
        protected virtual void ReceiveEntity(Entity entity)
        {
            Environment.Process(Process(entity));
        }

        protected virtual IEnumerable<Event> Process(Entity entity)
        {
            // 1. accept incoming entity
            InCounter++;
            Entities.Add(entity);
            NotifyBlockStatusChangeListeners();

            // 2. process
            yield return Environment.Process(MainProcess(entity));

            // 3. wait for successor
            Block successor = FindSuccessor();
            Request ownResourceRequest = entity.Request;
            entity.Request = successor.Resource.Request();
            yield return entity.Request;
            Resource.Release(ownResourceRequest);

            // 4. forward
            Entities.Remove(entity);
            NotifyBlockStatusChangeListeners();
            NotifyEntityMovementListeners(entity, successor);
            successor.ReceiveEntity(entity);
        }

        /// <summary>
        /// Can be overridden for custom processing logic, defaults to 0-timeout.
        /// </summary>
        protected virtual IEnumerable<Event> MainProcess(Entity entity)
        {
            yield return Environment.TimeoutD(0);
        }

        /// <summary>
        /// Can be overridden for custom successor retrieval, defaults to first successor.
        /// </summary>
        protected virtual Block FindSuccessor()
        {
            return Successors[0];
        }

        protected void NotifyEntityMovementListeners(Entity entity, Block successor)
        {
            EntityMovementEvent?.Invoke(entity, this, successor);
        }

        protected void NotifyBlockStatusChangeListeners()
        {
            BlockStatusChangeEvent?.Invoke(this);
        }

        public void AddListener(EntityMovementListener listener)
        {
            EntityMovementEvent += listener;
        }

        public void AddListener(BlockStatusChangeListener listener)
        {
            BlockStatusChangeEvent += listener;
        }

        public delegate void BlockStatusChangeListener(Block block);
        public delegate void EntityMovementListener(Entity entity, Block from, Block to);
    }


}