using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Integration
{
    public interface IStockApiIntegration
    {
        Task<bool> RequestGiveOut(IEnumerable<long> skuCollection, CancellationToken cancellationToken);
    }
}