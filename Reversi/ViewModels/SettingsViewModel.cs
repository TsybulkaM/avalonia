using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
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
    
    private BotType _selectedBot;
    public BotType SelectedBot
    {
        get => _selectedBot;
        set => SetProperty(ref _selectedBot, value);
    }
    
    public BotType[] AvailableBots { get; }
    public Dictionary<string, string> Themes { get; } = new Dictionary<string, string>
    {
        { "Light", "#959595" },
        { "Dark", "#5f5f5f" }
    };
    
    private bool _enableThemeSwitching;
    public bool ThemeSwitching
    {
        get => _enableThemeSwitching;
        set => SetProperty(ref _enableThemeSwitching, value);
    }

    private string _theme;
    public string Theme
    {
        get => _theme;
        set => SetProperty(ref _theme, value);
    }

    public string GetCurrentThemeAttributes()
    {
        return Themes.TryGetValue(Theme, out var attributes) ? attributes : null;
    }

    public IBrush DynamicThemeHandling(CellState cellState)
    {
        if (_enableThemeSwitching)
        {
            Theme = cellState == CellState.White ? "Dark" : "Light";
        }

        return new SolidColorBrush(Color.Parse(GetCurrentThemeAttributes()));
    }

    public SettingsViewModel()
    {
        AvailableBots = (BotType[])Enum.GetValues(typeof(BotType));
        SelectedBot = BotType.Evaluation;
        
        EnableBot = true;
        Theme = "Dark";
        ThemeSwitching = true;
    }
}