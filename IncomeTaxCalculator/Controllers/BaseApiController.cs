﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IncomeTaxCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??=
            HttpContext.RequestServices.GetService<IMediator>();
    }
}
