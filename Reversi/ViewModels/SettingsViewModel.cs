using System;
using System.Collections.Generic;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using Reversi.Models;

namespace Reversi.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    public Action OnSettingsConfirmed { get; set; }

    private bool _settingsSelected;
    public bool SettingsSelected
    {
        get => _settingsSelected;
        private set
        {
            SetProperty(ref _settingsSelected, value);
            OnSettingsConfirmed?.Invoke();
        }
    }

    private bool _enableBot;
    public bool EnableBot
    {
        get => _enableBot;
        set => SetProperty(ref _enableBot, value);
    }

    public Dictionary<string, (string Color, bool IsBlinking)> Themes { get; } = new Dictionary<string, (string, bool)>
    {
        { "Light", ("#5f5f5f", false) },
        { "Dark", ("#959595", false) },
        { "Blinking", ("#ff0000", true) }
    };

    private string _theme;
    public string Theme
    {
        get => _theme;
        set => SetProperty(ref _theme, value);
    }

    public (string Color, bool IsBlinking) GetCurrentThemeAttributes()
    {
        return Themes.TryGetValue(Theme, out var attributes) ? attributes : default;
    }

    public IBrush DynamicThemeHandling()
    {
        if (Themes.TryGetValue(Theme, out var theme) && theme.IsBlinking)
        {
            Theme = Theme == "Light" ? "Dark" : "Light";
        }

        return new SolidColorBrush(Color.Parse(theme.Color));
    }

    public SettingsViewModel()
    {
        EnableBot = false;
        Theme = "Blinking";
    }
}