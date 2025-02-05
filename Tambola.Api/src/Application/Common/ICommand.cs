using MediatR;

namespace Tambola.Api.src.Application.Common;

public interface ICommand<TResult> : IRequest<TResult>
{}
