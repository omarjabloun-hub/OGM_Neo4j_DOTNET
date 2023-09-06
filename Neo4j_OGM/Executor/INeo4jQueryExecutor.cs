using Neo4j_OGM.Entities;
using Neo4j.Driver;

namespace Neo4j_OGM.Executor;

public interface INeo4jQueryExecutor : IDisposable, IAsyncDisposable
{
    Task<T> ExecuteWriteAsync<T>(Query query) where T : EntityBase<T>;
    Task ExecuteWriteAsync(Query query);
    Task<T> ExecuteInsideTransactionAsync<T>(IAsyncTransaction tx, Query query) where T : EntityBase<T>;
    Task<T> ExecuteInsideTransactionPrimitiveAsync<T>(IAsyncTransaction tx, Query query);
    Task ExecuteInsideTransactionAsync(IAsyncTransaction tx, Query query);
    Task<IEnumerable<T>> ExecuteWriteAsyncToList<T>(Query query) where T : EntityBase<T>;
    Task<T?> ExecuteReadAsync<T>(Query query) where T : EntityBase<T>;
    Task<Pagination<T>> ExecuteReadPaginationAsync<T>(Query query) where T : EntityBase<T>;
    Task<IEnumerable<T>> ExecuteReadAsyncToList<T>(Query query) where T : EntityBase<T>;
    Task<long> ExecuteReadNumberAsync(Query query);
    Task ExecuteTransactionAsync(Func<IAsyncTransaction, Task> transactionWork);
    Task<TResult> ExecuteTransactionAsync<TResult>(Func<IAsyncTransaction, Task<TResult>> transactionWork);
}