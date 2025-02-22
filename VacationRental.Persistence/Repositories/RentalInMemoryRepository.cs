﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Persistence.Repositories
{
    public class RentalInMemoryRepository: RepositoryInMemoryBase<Rental>, IRentalRepository
    {
        private readonly IDictionary<int, Rental> _db;
        public RentalInMemoryRepository(IDictionary<int, Rental> db,IMediator mediator) : base(db, mediator)
        {
            _db = db;
        }

        public override Task<Rental> AddAsync(Rental entity, CancellationToken cancellationToken = default)
        {
            var lastUnitId = _db.Values.SelectMany(c => c.Units).Any()
                ? _db.Values.SelectMany(c => c.Units).Max(x => x.Id)
                : 0;
            foreach (var unit in entity.Units)
            {
                lastUnitId++;
                unit.SetId(lastUnitId);
            }
            return base.AddAsync(entity, cancellationToken);
        }

        public override void Update(Rental entity)
        {
            var e = DB.First(x => x.Key == entity.Id);
            if (e.Value == null) throw new ArgumentNullException(paramName: nameof(entity));
            var lastUnitId = _db.Values.SelectMany(c => c.Units).Max(x => x.Id);
            var unitsWithoutId = _db.Values.SelectMany(c => c.Units).Where(x => x.Id == 0);

            foreach (var unit in unitsWithoutId)
            {
                lastUnitId++;
                unit.SetId(lastUnitId);
            }
            DB[entity.Id] = entity;
        }

        public Task<bool> IsExistAsync(int rentalId)
        {
            return Task.FromResult( DB.Keys.Contains(rentalId));
        }
    }
}