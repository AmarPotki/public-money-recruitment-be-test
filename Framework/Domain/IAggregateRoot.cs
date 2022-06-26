using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Domain
{
    public interface IAggregateRoot : IEntity
    {
        void ClearDomainEvents();
       
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
    }
}
