using QueryModel;
using LinqToDB.Data;

namespace QueryData
{
    public class QueryDbDataContext : DataConnection, IQueryDb
    {
        public IQueryable<Entity> Entities => this.GetTable<Entity>();

        public QueryDbDataContext(string connectionString, string provider) : base(provider, connectionString)
        {
        }

        public IQueryable GetQueryableForEntityType(Type type)
        {
            // use reflection to call GetTable using the passed-in Type
            var method = this.GetType().GetMethod(nameof(GetTable), Array.Empty<Type>());
            var generic = method!.MakeGenericMethod(type);
            var queryable = generic.Invoke(this, null);

            if (queryable != null)
                return (IQueryable)queryable;
            else
                throw new Exception("Unknown entity type");
        }
    }
}
