using System;

namespace Framework.Domain
{
    public abstract class Entity<TKey> : IEntity
    {
        public TKey Id { get; set; }

        public DateTime InsertDateTime { get; private set; }
        public DateTime ModifiedDateTime { get; private set; }
    }
}