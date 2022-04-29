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
        var init = DateTime.Now;
        Console.WriteLine("Init: " + DateTime.Now);

        IRemoteQueryable<QueryModel.Entity> DataSource = (IRemoteQueryable<Entity>)_queryDb.Entities;
        int count = await (DataSource as IQueryable<QueryModel.Entity>).CountAsync();
        try
        {
            if (dm.Skip != 0)
            {
                //Paging
                DataSource = (IRemoteQueryable<QueryModel.Entity>)DataOperations.PerformSkip(DataSource, dm.Skip);
            }

            if (dm.Take != 0)
            {
                DataSource = (IRemoteQueryable<QueryModel.Entity>)DataOperations.PerformTake(DataSource, dm.Take);
            }

            DataResult DataObject = new DataResult();
            if (dm.Group != null)
            {
                var data = await (DataSource  as IQueryable<QueryModel.Entity>).ToListAsync();

                IEnumerable ResultData = data;
                // Grouping
                foreach (var group in dm.Group)
                {
                    ResultData = DataUtil.Group<QueryModel.Entity>(ResultData, group, dm.Aggregates, 0, dm.GroupByFormatter,dm.LazyLoad, dm.LazyExpandAllGroup);
                }
                DataObject.Result = ResultData;
                DataObject.Count = count;

                var end = DateTime.Now;
                Console.WriteLine("end: " + DateTime.Now);
                Console.WriteLine("total: " + (end-init));

                return dm.RequiresCounts ? DataObject : (object)ResultData;
            }

            if (dm.Aggregates != null) // Aggregation
            {
                var data = await (DataSource  as IQueryable<QueryModel.Entity>).ToListAsync();
                DataObject.Result = data;
                DataObject.Count = count;
                DataObject.Aggregates = DataUtil.PerformAggregation(data, dm.Aggregates);
                
                var end = DateTime.Now;
                Console.WriteLine("end: " + DateTime.Now);
                Console.WriteLine("total: " + (end-init));

                return dm.RequiresCounts ? DataObject : (object)data;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
        //var data = DataSource;
        var result = await (DataSource as IQueryable<QueryModel.Entity>).ToListAsync();
        
        var mainEnd = DateTime.Now;
        Console.WriteLine("end: " + DateTime.Now);
        Console.WriteLine("total: " + (mainEnd-init));
        return dm.RequiresCounts ? new DataResult() { Result = result, Count = count } : (object)result;

    }
}
