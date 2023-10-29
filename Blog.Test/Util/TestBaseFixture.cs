using Blog.Test.Util.Commons;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Blog.Test.Util
{
    [CollectionDefinition(nameof(TestBaseCollection))]
    public class TestBaseCollection : IClassFixture<TestBaseFixture> { }

    public abstract class TestBaseFixture : IDisposable
    {
        private IServiceScope _scope;
        private readonly WebApplicationFactory<Program> _webApplicationFactory;
        protected HttpClient _client { get; private set; }

        protected IServiceProvider ServiceProvider { get; private set; }

        protected readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        protected TestBaseFixture(WebApplicationFactory<Program> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
            _client = _webApplicationFactory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DefaultDataMock.GetBearerToken());

            _scope = _webApplicationFactory.Services.CreateScope();
            ServiceProvider = _scope.ServiceProvider;
        }

        public void Dispose()
        {
            _scope?.Dispose();
            _client?.Dispose();
            _webApplicationFactory?.Dispose();
        }
    }
}