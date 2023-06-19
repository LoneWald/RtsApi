using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Contracts;

namespace RtsApi;

internal class ResponseEnvelopeResultExecutor : ObjectResultExecutor
{
    public ResponseEnvelopeResultExecutor(OutputFormatterSelector formatterSelector, IHttpResponseStreamWriterFactory writerFactory, ILoggerFactory loggerFactory, IOptions<MvcOptions> mvcOptions) : base(formatterSelector, writerFactory, loggerFactory, mvcOptions)
    {
    }
    public override Task ExecuteAsync(ActionContext context, ObjectResult result)
    {
        if (result.Value is ContentResult)
            return base.ExecuteAsync(context, result);
        result.Value = new Answer<object>(result.Value);
        return base.ExecuteAsync(context, result);
    }
}
