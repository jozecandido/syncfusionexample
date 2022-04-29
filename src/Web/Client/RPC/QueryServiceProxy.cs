using Aqua.Dynamic;
using MessagePack;
using Remote.Linq.Expressions;

namespace Web.Client.RPC
{
    /// <summary>
    /// Proxy to call the Query Service via SignalR
    /// </summary>
    internal class QueryServiceProxy : SignalrServiceProxyBase, IQueryService
    {
        public QueryServiceProxy(IConfiguration configuration)
            : base(new Uri(configuration.GetValue<Uri>("ServiceUrls:QueryService"), "/query"))
        {
        }

        /// <summary>
        /// Executes a remote query
        /// </summary>
        /// <param name="queryExpression"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<DynamicObject> ExecuteQuery(Expression queryExpression, CancellationToken cancellation)
        {
            // send expression to service and get back results
            // NOTE: we handle the serialization of the expression manually for this method because the SignalR type checking has a problem.  Other methods shouldn't need to do this.
            var result = await this.InvokeAsync<DynamicObject>(nameof(ExecuteQuery), cancellation, MessagePackSerializer.Typeless.Serialize(queryExpression));
            return result;
        }
    }
}
