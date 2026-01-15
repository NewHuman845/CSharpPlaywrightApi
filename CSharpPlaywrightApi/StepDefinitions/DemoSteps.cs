using CSharpPlaywrightApi.Context;
using CSharpPlaywrightApi.Models;
using FluentAssertions;
using Reqnroll;

namespace CSharpPlaywrightApi.StepDefinitions;

[Binding]
public sealed class DemoSteps(ResponseContext<GetCats[]> responseContext)
{
    [When(@"I send a GET request")]
    public async Task WhenISendAGETRequest()
    {
        responseContext.ApiResponse = await Hooks.Hooks.ApiContext.GetAsync("v1/images/search?limit=10");
    }

    [Then(@"the response status should be (.*)")]
    public void ThenTheResponseStatusShouldBe(int statusCode)
    {
        responseContext.ApiResponse.Status.Should().Be(statusCode);
    }

    [Then(@"the response should contain ""(.*)""")]
    public async Task ThenTheResponseShouldContain(string key)
    {
        responseContext.Response.Count().Should().BeGreaterThan(1);
    }
}
