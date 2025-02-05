using MediatR;

namespace Tambola.Api.src.Application.Common;

public interface ICommandHandler<TCommand,TResult> : IRequestHandler<TCommand,TResult> where TCommand : ICommand<TResult>
{}
