using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;
using Template.API.Middlewares;
using Template.Domain.Interfaces.Identity;
using Xunit;

namespace Template.Unit.Tests.Middlewares
{
    public class LogMiddlewareTest
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly Mock<IIdentityService> _identityServiceMock;

        public LogMiddlewareTest()
        {
            _telemetryClient = new TelemetryClient(new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration());
            _identityServiceMock = new Mock<IIdentityService>();
        }

        [Fact]
        public async Task InvokeLogHandler_Test()
        {
            var httpContext = new DefaultHttpContext().Request.HttpContext;

            var logMiddleware = new LogMiddleware(async (innerHttpContext) =>
            {
                await innerHttpContext.Response.WriteAsync("Response body mock");
            }, _telemetryClient);

            await logMiddleware.Invoke(httpContext, _identityServiceMock.Object);

            Assert.NotNull(logMiddleware);
        }
    }
}
