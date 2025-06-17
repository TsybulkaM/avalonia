using System.Collections.ObjectModel;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using Reversi.Models;

namespace Reversi.ViewModels;

public class MainWindowViewModel : ViewModelBase
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
    
    public IRelayCommand NewGameCommand { get; }
    
    public SettingsViewModel Settings { get; }
    
    public MainWindowViewModel()
    {
        Settings = new SettingsViewModel();
        Settings.OnSettingsConfirmed = () => 
        {
            NewGameCommand.NotifyCanExecuteChanged();
            SelectedTabIndex = 0;
        };
        
        _gameBoard = new GameBoard();
        _cells = new ObservableCollection<ObservableCollection<CellViewModel>>();
        
        NewGameCommand = new RelayCommand(NewGame, CanStartGame);
        _bot = new EvaluationBot();
        
        SelectedTabIndex = 1;
        InitializeCells();
        UpdateGameStatus();
    }
    
    private bool CanStartGame()
    {
        return Settings.SettingsSelected;
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
    
    public async void CellClicked(int row, int col)
    {
        if (!_gameBoard.MakeMove(row, col)) return;

        for (var i = 0; i < GameBoard.BoardSize; i++)
        {
            for (var j = 0; j < GameBoard.BoardSize; j++)
            {
                _cells[i][j].UpdateState(_gameBoard.GetCell(i, j));
            }
        }

        UpdateGameStatus();

        if (Settings.EnableBot)
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
    }
    
    private void UpdateGameStatus()
    {
        bool isBlackPlayer = _gameBoard.CurrentPlayer == CellState.Black;
        ScoreText = $"Black: {_gameBoard.CountPieces(CellState.Black)} - White: {_gameBoard.CountPieces(CellState.White)}";
        
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

        SystemBackgroundColor = Settings.DynamicThemeHandling();
    }
    
    private void NewGame()
    {
        _gameBoard = new GameBoard();
        InitializeCells();
        UpdateGameStatus();
    }
}


