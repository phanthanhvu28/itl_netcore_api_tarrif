
namespace AppCore.Contracts.Mediators
{
    public interface ICommand<TResponse> : Mediator.ICommand<ResultModel<TResponse>> where TResponse : notnull
    {
    }
}
