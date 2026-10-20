using System.Text.Json;
using Sommelier.Models;

namespace Sommelier.Services;

public class SommelierState
{
    private readonly WineCatalog _catalog;
    private readonly SommelierJsInterop _jsInterop;
    private CancellationTokenSource? _toastCts;

    public SommelierState(WineCatalog catalog, SommelierJsInterop jsInterop)
    {
        _catalog = catalog;
        _jsInterop = jsInterop;
        BuilderWine = _catalog.GetAllWines().FirstOrDefault() ?? new Wine();
    }

    public event Action? OnChange;

    public HashSet<string> SelectedFlavors { get; } = new() { "Spicy", "Umami" };

    public string DishName { get; set; } = string.Empty;

    public List<Wine> SavedTastingMenu { get; } = new();

    public Wine BuilderWine { get; set; }

    public Wine? ActiveDetailWine { get; private set; }

    public bool IsToastVisible { get; private set; }

    public string TitleTags => SelectedFlavors.Count > 0 
        ? string.Join(" · ", SelectedFlavors) 
        : "your meal";

    public string MenuSummary
    {
        get
        {
            var count = SavedTastingMenu.Count;
            var reds = SavedTastingMenu.Count(w => string.Equals(w.Type, WineType.Red, StringComparison.OrdinalIgnoreCase));
            var pairingStr = $"{count} pairing{(count == 1 ? "" : "s")}";
            if (reds > 0)
            {
                return $"{pairingStr} · {reds} red{(reds == 1 ? "" : "s")}";
            }
            return pairingStr;
        }
    }

    public async Task InitializeFromStorageAsync()
    {
        var json = await _jsInterop.LoadTastingMenuAsync();
        if (!string.IsNullOrWhiteSpace(json))
        {
            try
            {
                var loaded = JsonSerializer.Deserialize<List<Wine>>(json);
                if (loaded != null && loaded.Count > 0)
                {
                    SavedTastingMenu.Clear();
                    SavedTastingMenu.AddRange(loaded);
                    NotifyStateChanged();
                }
            }
            catch
            {
                // Ignore parse errors from stale/invalid storage
            }
        }
    }

    public void ToggleFlavor(string flavor)
    {
        if (SelectedFlavors.Contains(flavor))
        {
            SelectedFlavors.Remove(flavor);
        }
        else
        {
            SelectedFlavors.Add(flavor);
        }
        NotifyStateChanged();
    }

    public void ClearFlavors()
    {
        SelectedFlavors.Clear();
        NotifyStateChanged();
    }

    public void SetDish(string dish)
    {
        DishName = dish;
        NotifyStateChanged();
    }

    public void SetBuilderWine(Wine wine)
    {
        BuilderWine = wine;
        NotifyStateChanged();
    }

    public void AddWine(Wine wine)
    {
        if (!SavedTastingMenu.Any(w => string.Equals(w.Name, wine.Name, StringComparison.OrdinalIgnoreCase)))
        {
            SavedTastingMenu.Add(wine);
            _ = PersistTastingMenuAsync();
        }

        ShowToast();
        NotifyStateChanged();
    }

    public void RemoveWine(string wineName)
    {
        SavedTastingMenu.RemoveAll(w => string.Equals(w.Name, wineName, StringComparison.OrdinalIgnoreCase));
        _ = PersistTastingMenuAsync();
        NotifyStateChanged();
    }

    public void OpenDetail(Wine wine)
    {
        ActiveDetailWine = wine;
        NotifyStateChanged();
    }

    public void CloseDetail()
    {
        ActiveDetailWine = null;
        NotifyStateChanged();
    }

    private void ShowToast()
    {
        _toastCts?.Cancel();
        _toastCts = new CancellationTokenSource();
        IsToastVisible = true;

        var token = _toastCts.Token;
        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(2400, token);
                if (!token.IsCancellationRequested)
                {
                    IsToastVisible = false;
                    NotifyStateChanged();
                }
            }
            catch (TaskCanceledException)
            {
            }
        });
    }

    public void DismissToast()
    {
        _toastCts?.Cancel();
        IsToastVisible = false;
        NotifyStateChanged();
    }

    private async Task PersistTastingMenuAsync()
    {
        var json = JsonSerializer.Serialize(SavedTastingMenu);
        await _jsInterop.SaveTastingMenuAsync(json);
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
