using System;

namespace Framework.Domain
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}