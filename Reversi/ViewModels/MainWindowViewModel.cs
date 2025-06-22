using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using Reversi.Models;
using Avalonia.Controls;


namespace Reversi.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private GameBoard _gameBoard;
    private IBot _bot;
    
    private int _selectedTabIndex;
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetProperty(ref _selectedTabIndex, value);
    }
    
    private ObservableCollection<ObservableCollection<CellViewModel>> _cells;
    public ObservableCollection<ObservableCollection<CellViewModel>> Cells => _cells;
    
    private IBrush _systemBackgroundColor = new SolidColorBrush(Colors.LightGray);
    public IBrush SystemBackgroundColor
    {
        get => _systemBackgroundColor;
        set => SetProperty(ref _systemBackgroundColor, value);
    }
    
    private string _currentPlayerText;
    public string CurrentPlayerText
    {
        get => _currentPlayerText;
        set => SetProperty(ref _currentPlayerText, value);
    }

    private string _scoreText;
    public string ScoreText
    {
        get => _scoreText;
        set => SetProperty(ref _scoreText, value);
    }
    
    public SettingsViewModel Settings { get; }
    
    private bool _isInteractionBlocked;
    public MainWindowViewModel()
    {
        Settings = new SettingsViewModel();
        Settings.PropertyChanged += OnSettingsPropertyChanged;
        Settings.OnSettingsConfirmed = () => 
        {
            NewGameCommand.NotifyCanExecuteChanged();
            SelectedTabIndex = 0;
        };
        
        _gameBoard = new GameBoard();
        _cells = new ObservableCollection<ObservableCollection<CellViewModel>>();
        
        UpdateBotImplementation();
        
        SelectedTabIndex = 1;
        InitializeCells();
        UpdateGameStatus();
    }
    
    private void InitializeCells()
    {
        _cells.Clear();
        
        for (int i = 0; i < GameBoard.BoardSize; i++)
        {
            var row = new ObservableCollection<CellViewModel>();
            for (int j = 0; j < GameBoard.BoardSize; j++)
            {
                var cell = new CellViewModel(i, j, this);
                cell.UpdateState(_gameBoard.GetCell(i, j));
                row.Add(cell);
            }
            _cells.Add(row);
        }
    }
    
    private void OnSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SettingsViewModel.SelectedBot))
        {
            UpdateBotImplementation();
        }

        if (e.PropertyName == nameof(SettingsViewModel.Theme) || e.PropertyName == nameof(SettingsViewModel.ThemeSwitching))
        {
            SystemBackgroundColor = Settings.DynamicThemeHandling(_gameBoard.CurrentPlayer);
        }
        {
            SystemBackgroundColor = Settings.DynamicThemeHandling(_gameBoard.CurrentPlayer);
        }
    }
    
    private void UpdateBotImplementation()
    {
        
        switch (Settings.SelectedBot)
        {
            case BotType.Evaluation:
                _bot = new EvaluationBot();
                break;
        }
    }
    
    public async void CellClicked(int row, int col)
    {
        if (_isInteractionBlocked || !_gameBoard.MakeMove(row, col)) return;
        
        _isInteractionBlocked = true;

        for (var i = 0; i < GameBoard.BoardSize; i++)
        {
            for (var j = 0; j < GameBoard.BoardSize; j++)
            {
                _cells[i][j].UpdateState(_gameBoard.GetCell(i, j));
            }
        }

        UpdateGameStatus();

        if (Settings.EnableBot && !_gameBoard.GameOver && _gameBoard.CurrentPlayer == CellState.White)
        {
            await _bot.MakeBestMove(_gameBoard);
            for (var i = 0; i < GameBoard.BoardSize; i++)
            {
                for (var j = 0; j < GameBoard.BoardSize; j++)
                {
                    _cells[i][j].UpdateState(_gameBoard.GetCell(i, j));
                }
            }

            UpdateGameStatus();
        }
        
        _isInteractionBlocked = false;
    }

    private void UpdateGameStatus()
    {
        bool isBlackPlayer = _gameBoard.CurrentPlayer == CellState.Black;
        ScoreText =
            $"Black: {_gameBoard.CountPieces(CellState.Black)} - White: {_gameBoard.CountPieces(CellState.White)}";

        if (_gameBoard.GameOver)
        {
            if (_gameBoard.CountPieces(CellState.Black) > _gameBoard.CountPieces(CellState.White))
            {
                ScoreText += " - Black wins!";
            }
            else if (_gameBoard.CountPieces(CellState.White) > _gameBoard.CountPieces(CellState.Black))
            {
                ScoreText += " - White wins!";
            }
            else
            {
                ScoreText += " - It's a draw!";
            }
        }

        CurrentPlayerText = $"Current player: {(isBlackPlayer ? "Black" : "White")}";

        SystemBackgroundColor = Settings.DynamicThemeHandling(_gameBoard.CurrentPlayer);
    }
    
    [RelayCommand]
    private async Task NewGame(Window owner)
    {
        var dialog = new Views.ConfirmationDialog
        {
            DataContext = new ConfirmationDialogViewModel("Are you sure you want to start a new game?")
        };
        
        var result = await dialog.ShowDialog<bool>(owner);
        
        if (result == true)
        {
            _gameBoard = new GameBoard();
            InitializeCells();
            UpdateGameStatus();
        }
    }
}


