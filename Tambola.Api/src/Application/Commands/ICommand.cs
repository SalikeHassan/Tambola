using MediatR;

namespace Tambola.Api.src.Application.Commands;

public interface ICommand<TResult> : IRequest<TResult>
{}
