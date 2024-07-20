namespace POKA.Utils.Interfaces
{
    public interface IBaseIQuery
    {

    }

    public interface IQuery : IBaseIQuery, IRequest<Unit>
    {

    }

    public interface IQuery<TResponse> : IBaseIQuery, IRequest<TResponse>
    {

    }
}
