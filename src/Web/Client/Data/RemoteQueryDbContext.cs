using Aqua.Dynamic;
using QueryModel;
using Web.Client.RPC;
using Remote.Linq;
using Remote.Linq.Expressions;

namespace Web.Client.Data
{
    public class RemoteQueryDbContext : IQueryDb
    {
        private readonly IQueryService _queryServiceProxy;


        public IQueryable<Entity> Entities => RemoteQueryable.Factory.CreateAsyncQueryable<Entity>(this.ExecuteRemoteQuery);

        public RemoteQueryDbContext(IQueryService queryServiceProxy)
        {
            _queryServiceProxy = queryServiceProxy;
        }

        private async ValueTask<DynamicObject> ExecuteRemoteQuery(Expression queryExpression, CancellationToken cancellation)
        {
            return await _queryServiceProxy.ExecuteQuery(queryExpression, cancellation);
        }

    }
}
