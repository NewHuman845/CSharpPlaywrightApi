using Microsoft.Playwright;
using Newtonsoft.Json;

namespace CSharpPlaywrightApi.Context;

public class ResponseContext<T> where T : class
{
    private IAPIResponse _apiResponse;

    public IAPIResponse ApiResponse
    {
        get
        {
            return _apiResponse;
        }
        set
        {
            _apiResponse = value;
            if(value is not null)
            {
                var responseText = value.TextAsync().GetAwaiter().GetResult();
                Response = JsonConvert.DeserializeObject<T>(responseText);
            }
        }
    }

    public T Response{ get; private set; }
}