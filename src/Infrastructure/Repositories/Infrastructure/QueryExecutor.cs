using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.BaseModels;
using Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Infrastructure.Repositories.Infrastructure
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IChangeTracker _changeTracker;
        
        public QueryExecutor(IChangeTracker changeTracker)
        {
            _changeTracker = changeTracker;
        }

        public async Task<T> Execute<T>(T entity, Func<Task> method) where T : Entity
        {
            await method();
            _changeTracker.Track(entity);
            return entity;
        }

        public async Task<T> Execute<T>(Func<Task<T>> method) where T : Entity
        {
            var result = await method();
            _changeTracker.Track(result);
            return result;
        }

        public async Task<IEnumerable<T>> Execute<T>(Func<Task<IEnumerable<T>>> method) where T : Entity
        {
            var result = (await method()).ToList();
            foreach (var entity in result)
            {
                _changeTracker.Track(entity);
            }

            return result;
        }
    }
}