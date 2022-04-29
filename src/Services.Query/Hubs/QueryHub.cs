using Aqua.Dynamic;
using MessagePack;
using Microsoft.AspNetCore.SignalR;
using Remote.Linq.Expressions;

namespace Services.Query.Hubs;

public class QueryHub : Hub
{
    private readonly RemoteQueryExecutor _queryExecutor;

    public QueryHub(RemoteQueryExecutor remoteQueryExecutor)
    {
        _queryExecutor = remoteQueryExecutor;
    }

    public async Task<DynamicObject> ExecuteQuery(byte[] serializedQueryExpression)
    {
        var queryExpression = (Expression)MessagePackSerializer.Typeless.Deserialize(serializedQueryExpression);
        var queryResult = await _queryExecutor.ExecuteQuery(queryExpression);
        return queryResult;
    }
}

