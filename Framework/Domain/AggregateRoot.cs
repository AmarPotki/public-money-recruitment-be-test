using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Domain
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        protected AggregateRoot()
        {
            _domainEvents = new List<IDomainEvent>();
        }

        // **********
        private readonly List<IDomainEvent> _domainEvents;

        public void SetId(int id)
        {
            Id=id;
        }

        public IReadOnlyList<IDomainEvent> DomainEvents
        {
            get
            {
                return _domainEvents;
            }
        }
        // **********

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is null)
            {
                return;
            }

            _domainEvents?.Add(domainEvent);
        }

        protected void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is null)
            {
                return;
            }

            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
