using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.Playwright;
using Reqnroll;
using System.Xml.Linq;

namespace CSharpPlaywrightApi.Hooks;

[Binding]
public class Hooks
{
    private static IPlaywright _playwright; private static IAPIRequestContext _apiContext; 
    public static IAPIRequestContext ApiContext => _apiContext;
    private static ExtentReports _extent;
    private static ExtentTest _test;

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        _playwright = await Playwright.CreateAsync();
        _apiContext = await _playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = "https://api.thecatapi.com/",
            ExtraHTTPHeaders = new Dictionary<string, string> { { "content-type", "application/json" }
            }
        });

        var reportPath = Environment.GetEnvironmentVariable("REPORT_PATH") ?? Path.Combine(Directory.GetCurrentDirectory(), "Reports", "ExtentReport.html");
        Directory.CreateDirectory(Path.GetDirectoryName(reportPath)!); 
        Console.WriteLine("Reort path = "+reportPath);
        var sparkReporter = new ExtentSparkReporter(reportPath); _extent = new ExtentReports();
        sparkReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Dark;
        sparkReporter.Config.DocumentTitle = "API Automation Report"; 
        sparkReporter.Config.ReportName = "Playwright API Test Results";

        _extent.AddSystemInfo("Environment", "QA");
        _extent.AddSystemInfo("Tester", "Damn");
        _extent.AddSystemInfo("Framework", "Playwright + xUnit + Reqnroll");
        _extent.AttachReporter(sparkReporter);
    }

    [BeforeScenario]
    public void BeforeScenario(ScenarioContext scenarioContext)
    {
        _test = _extent.CreateTest(scenarioContext.ScenarioInfo.Title);
    }

    [AfterStep]
    public void AfterStep(ScenarioContext scenarioContext)
    {
        if (scenarioContext.TestError == null)
            _test.Pass(scenarioContext.StepContext.StepInfo.Text);
        else
            _test.Fail(scenarioContext.StepContext.StepInfo.Text +
                       " - Error: " + scenarioContext.TestError.Message);
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        await _apiContext.DisposeAsync(); _playwright.Dispose();
        _extent.Flush();
    }
}
