using Microsoft.JSInterop;

namespace Sommelier.Services;

public class SommelierJsInterop
{
    private readonly IJSRuntime _jsRuntime;

    public SommelierJsInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask SaveTastingMenuAsync(string json)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("sommelier.saveTastingMenu", json);
        }
        catch (JSDisconnectedException)
        {
            // Circuit disconnected or WebAssembly disposing
        }
    }

    public async ValueTask<string> LoadTastingMenuAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string>("sommelier.loadTastingMenu");
        }
        catch
        {
            return string.Empty;
        }
    }

    public async ValueTask ScrollToElementAsync(string elementId)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("sommelier.scrollToElement", elementId);
        }
        catch
        {
        }
    }

    public async ValueTask ScrollToTopAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("sommelier.scrollToTop");
        }
        catch
        {
        }
    }
}
