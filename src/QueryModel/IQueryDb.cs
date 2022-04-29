using System.Linq;

namespace QueryModel
{
    public interface IQueryDb
    {
        IQueryable<Entity> Entities { get; }

    }
}
