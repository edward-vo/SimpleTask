using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace SimpleTask.Test.TaskTests
{
    public class TaskTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public TaskTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _server.Dispose();
            _client.Dispose();
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test2")]
        [InlineData("test3")]
        public async Task EnsureCanAddItemToTask(string taskName)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, $"v1/Task/add?taskName={taskName}");
            var resp = await _client.SendAsync(req);
            
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            var respStream = await resp.Content.ReadAsStreamAsync();
            var respDoc = await JsonDocument.ParseAsync(respStream);
            var respDocRoot = respDoc.RootElement;
            
            Assert.True(respDocRoot.TryGetProperty("taskName", out var taskNameDoc));
            Assert.Equal(taskName, taskNameDoc.GetString());
            Assert.True(respDocRoot.TryGetProperty("taskId", out var taskIdDoc));
            Assert.True(taskIdDoc.TryGetGuid(out var taskGuid));
            Assert.NotEqual(Guid.Empty, taskGuid);;
        }
    }
}