using POKA.Utils.ValueObjects;
using POKA.Utils.Interfaces;

namespace POKA.Utils.Extensions
{
    public static class IMediatorExtensions
    {
        public static async Task PublishAsync<TObjectId>(this IMediator mediator, IDomainEvent<TObjectId> domainEvent, CancellationToken cancellationToken = default)
            where TObjectId : BaseObjectId =>
            await mediator.Publish(domainEvent, cancellationToken);

        public static async Task PublishAsync<TObjectId>(this IMediator mediator, IEnumerable<IDomainEvent<TObjectId>> domainEvents, CancellationToken cancellationToken = default) 
            where TObjectId : BaseObjectId
        {
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent, cancellationToken);
            }
        }

        public static async Task PublishAsync<TObjectId>(this IMediator mediator, IAggregateRoot<TObjectId> aggregate, CancellationToken cancellationToken = default)
            where TObjectId : BaseObjectId
        {
            await mediator.PublishAsync(aggregate.GetUncommittedDomainEvents(), cancellationToken);
            aggregate.CommitDomainEvents();
        }

        public static async Task<TResult> RunCommand<TCommand, TResult>(this IMediator mediator, CancellationToken cancellationToken = default)
            where TCommand : class, IRequest<TResult>, new()
        {
            var command = (TCommand)Activator.CreateInstance(typeof(TCommand));
            var result = await mediator.Send(command, cancellationToken);

            return result;
        }

        public static async Task PublishAndCommitDomainEventAsync<TObjectId>(this IMediator mediator, IAggregateRoot<TObjectId> aggregate, CancellationToken cancellationToken = default)
            where TObjectId : BaseObjectId
        {
            await mediator.PublishAsync(aggregate.GetUncommittedDomainEvents(), cancellationToken);
            aggregate.CommitDomainEvents();
        }

        public static Task<TResult> RunQuery<TQuery, TResult>(this IMediator mediator, CancellationToken cancellationToken = default)
            where TQuery : class, IRequest<TResult>, new() => mediator.RunCommand<TQuery, TResult>(cancellationToken);
    }
}
