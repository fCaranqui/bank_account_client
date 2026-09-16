namespace Client.Application.Common;

public interface IUseCase<TIn, TOut>
{
    Task<TOut> Execute(TIn input);
}
