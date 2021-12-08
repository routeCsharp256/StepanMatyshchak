using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ValueTask StartTransaction(CancellationToken token);
        
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}