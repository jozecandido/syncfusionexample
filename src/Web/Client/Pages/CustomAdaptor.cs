using System;
using System.Collections;
using System.Linq;
using QueryModel;
using Remote.Linq;
using Remote.Linq.Async;
using Remote.Linq.DynamicQuery;
using Remote.Linq.ExpressionExecution;
using Remote.Linq.Expressions;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;

namespace Web.Client.Pages;
public class CustomAdaptor : DataAdaptor
{
    private IQueryDb _queryDb;

    public CustomAdaptor(IQueryDb queryDb)
    {
        this._queryDb = queryDb;
    }
    public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
    {
        Console.WriteLine("Read1");
        IRemoteQueryable<QueryModel.Entity> DataSource = (IRemoteQueryable<Entity>)_queryDb.Entities;
        Console.WriteLine("Read2");
        int count = await (DataSource as IQueryable<QueryModel.Entity>).CountAsync();
        try
        {
            if (dm.Search != null && dm.Search.Count > 0)
            {
                // Searching
                DataSource = (IRemoteQueryable<QueryModel.Entity>)DataOperations.PerformSearching(DataSource, dm.Search);
            }
            Console.WriteLine("Read3");

            if (dm.Sorted != null && dm.Sorted.Count > 0)
            {
                // Sorting
                var result = DataOperations.PerformSorting((IEnumerable)DataSource, dm.Sorted);
                Console.WriteLine(result);
            }

            Console.WriteLine("Read4");

            if (dm.Where != null && dm.Where.Count > 0)
            {
                // Filtering
                DataSource = (IRemoteQueryable<QueryModel.Entity>)DataOperations.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            Console.WriteLine("Read5");

            Console.WriteLine(dm.Skip);
            if (dm.Skip != 0)
            {
                //Paging
                DataSource = (IRemoteQueryable<QueryModel.Entity>)DataOperations.PerformSkip(DataSource, dm.Skip);
            }
            Console.WriteLine("Read6");
            Console.WriteLine(dm.Take);

            if (dm.Take != 0)
            {
                DataSource = (IRemoteQueryable<QueryModel.Entity>)DataOperations.PerformTake(DataSource, dm.Take);
            }
            Console.WriteLine("Read7");

            DataResult DataObject = new DataResult();
            if (dm.Group != null)
            {
                var data1 = await (DataSource  as IQueryable<QueryModel.Entity>).ToListAsync();

                IEnumerable ResultData = data1;
                // Grouping
                foreach (var group in dm.Group)
                {
                    ResultData = DataUtil.Group<QueryModel.Entity>(ResultData, group, dm.Aggregates, 0, dm.GroupByFormatter,dm.LazyLoad, dm.LazyExpandAllGroup);
                }
                DataObject.Result = ResultData;
                DataObject.Count = count;
                return dm.RequiresCounts ? DataObject : (object)ResultData;
            }

            if (dm.Aggregates != null) // Aggregation
            {
                var data1 = await (DataSource  as IQueryable<QueryModel.Entity>).ToListAsync();
                DataObject.Result = data1;
                DataObject.Count = count;
                DataObject.Aggregates = DataUtil.PerformAggregation(data1, dm.Aggregates);
                Console.WriteLine("Read8");

                return dm.RequiresCounts ? DataObject : (object)data1;
            }
            Console.WriteLine("Read9");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
        //var data = DataSource;
        var data = await (DataSource as IQueryable<QueryModel.Entity>).ToListAsync();
        return dm.RequiresCounts ? new DataResult() { Result = data, Count = count } : (object)data;

    }
}
