﻿using Blog.Domain.Interfaces.Repositories;
using Blog.Test.Util.Commons;
using Blog.Test.Util.Factories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Blog.Test.Util
{
    [CollectionDefinition(nameof(TestBaseCollection))]
    public class TestBaseCollection : IClassFixture<TestBaseIntegrationFixture> { }

    public abstract class TestBaseIntegrationFixture : IClassFixture<WebApplicationFactoryTest<Program>> 
    {
        private IServiceScope _scope;
        private readonly WebApplicationFactoryTest<Program> _webApplicationFactory;
        protected HttpClient _client { get; private set; }

        protected IServiceProvider ServiceProvider { get; private set; }

        protected TestBaseIntegrationFixture(WebApplicationFactoryTest<Program> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
            _client = _webApplicationFactory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DefaultDataMock.GetBearerToken());

            _scope = _webApplicationFactory.Services.CreateScope();
            ServiceProvider = _scope.ServiceProvider;
        }
    }
}