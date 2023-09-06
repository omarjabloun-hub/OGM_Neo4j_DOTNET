using Neo4j_OGM.Entities;
using Neo4j_OGM.Exceptions;
using Neo4j_OGM.Mapper;
using Neo4j.Driver;

namespace Neo4j_OGM.Executor;

public class Neo4jQueryExecutor : INeo4jQueryExecutor
{
    private readonly IDriver _driver;
    private IAsyncSession? _session;

    public Neo4jQueryExecutor(IDriver driver)
    {
        _driver = driver;
    }


    public async Task<T> ExecuteWriteAsync<T>(Query query) where T : EntityBase<T>
    {
        var session = await GetSession();
        var result = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(query);
            if (await cursor.FetchAsync())
                return MapEntity.Map<T>(cursor.Current);
            throw new NotFoundNeo4jException(" Not Found");
        });
        return result;
    }

    public async Task ExecuteWriteAsync(Query query)
    {
        var session = await GetSession();
        await session.ExecuteWriteAsync(async tx =>
        {
            var result = await tx.RunAsync(query);
            if (!await result.FetchAsync())
                throw new QueryNotExecutedException("Query Not Executed");
        });
    }

    public async Task<T> ExecuteInsideTransactionAsync<T>(IAsyncTransaction tx, Query query) where T : EntityBase<T>
    {
        var cursor = await tx.RunAsync(query);
        if (await cursor.FetchAsync())
            return MapEntity.Map<T>(cursor.Current);
        throw new NotFoundNeo4jException(" Not Found");
    }

    public async Task<T> ExecuteInsideTransactionPrimitiveAsync<T>(IAsyncTransaction tx, Query query)
    {
        var cursor = await tx.RunAsync(query);
        if (await cursor.FetchAsync())
            return MapEntity.MapPrimitive<T>(cursor.Current);
        throw new NotFoundNeo4jException(" Not Found");
    }

    public async Task ExecuteInsideTransactionAsync(IAsyncTransaction tx, Query query)
    {
        var result = await tx.RunAsync(query);
        if (!await result.FetchAsync())
            throw new QueryNotExecutedException("Query Not Executed");
    }

    public async Task<IEnumerable<T>> ExecuteWriteAsyncToList<T>(Query query) where T : EntityBase<T>
    {
        var session = await GetSession();
        var result = await session.ExecuteWriteAsync(async tx =>
        {
            var cursors = await tx.RunAsync(query);
            var records = await cursors.ToListAsync();
            if (records.Count == 0)
                throw new NotFoundNeo4jException(" Not Found");
            return MapEntity.Map<T>(records);
        });
        return result;
    }

    public async Task<T?> ExecuteReadAsync<T>(Query query) where T : EntityBase<T>
    {
        var session = await GetSession();
        var result = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(query);
            if (await cursor.FetchAsync())
                return MapEntity.Map<T>(cursor.Current);
            return null;
        });
        return result;
    }

    public async Task<Pagination<T>> ExecuteReadPaginationAsync<T>(Query query) where T : EntityBase<T>
    {
        var session = await GetSession();

        var result = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(query);
            if (!await cursor.FetchAsync()) return new Pagination<T>();
            ;
            var record = cursor.Current;
            return MapEntity.MapPagination<T>(record);
        });
        return result;
    }


    public async Task<IEnumerable<T>> ExecuteReadAsyncToList<T>(Query query) where T : EntityBase<T>
    {
        var session = await GetSession();

        var result = await session.ExecuteReadAsync(async tx =>
        {
            var cursors = await tx.RunAsync(query);
            var records = await cursors.ToListAsync();
            return records.Count == 0 ? new List<T>() : MapEntity.Map<T>(records);
        });
        return result;
    }

    public async Task<long> ExecuteReadNumberAsync(Query query)
    {
        var session = await GetSession();
        var result = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(query);
            if (!await cursor.FetchAsync())
                throw new NotFoundNeo4jException("Number Not Found");
            var x = cursor.Current[0];
            return x.As<long>();
        });
        return result;
    }

    public async Task ExecuteTransactionAsync(Func<IAsyncTransaction, Task> transactionWork)
    {
        var session = await GetSession();
        var tx = await session.BeginTransactionAsync();
        try
        {
            await transactionWork(tx);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task<TResult> ExecuteTransactionAsync<TResult>(Func<IAsyncTransaction, Task<TResult>> transactionWork)
    {
        var session = await GetSession();
        var tx = await session.BeginTransactionAsync();
        try
        {
            var result = await transactionWork(tx);
            await tx.CommitAsync();
            return result;
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public void Dispose()
    {
        _session?.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        _session?.DisposeAsync();
        return ValueTask.CompletedTask;
    }

    private Task<IAsyncSession> GetSession()
    {
        return Task.FromResult(_session ??= _driver.AsyncSession());
    }
}