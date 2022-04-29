using System.Security.Claims;
using Aqua.Dynamic;
using LinqToDB;
using QueryData;
using Remote.Linq.ExpressionExecution;
using Remote.Linq.Expressions;

namespace Services.Query
{
    public class RemoteQueryExecutor
    {
        private IConfiguration _configuration;
        public RemoteQueryExecutor(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task<DynamicObject> ExecuteQuery(Expression queryExpression)
        {
            using (var db = new QueryDbDataContext(_configuration["ConnectionStrings:Read"], ProviderName.SqlServer2017))
            {
                var result = queryExpression.Execute(db.GetQueryableForEntityType);
                return result;
            }
        }
    }
}
