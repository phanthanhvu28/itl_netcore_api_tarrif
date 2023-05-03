using Microsoft.AspNetCore.Mvc;
using Mediator;

namespace CoreAPI.Controllers
{
    public class AppControllerBase : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
    }
}
