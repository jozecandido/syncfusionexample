using Aqua.Dynamic;
using Remote.Linq.Expressions;

namespace Web.Client.RPC;

public interface IQueryService
{
    Task<DynamicObject> ExecuteQuery(Expression queryExpression, CancellationToken cancellation);
}

