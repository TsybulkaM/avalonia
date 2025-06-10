using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using Reversi.Models;

namespace Reversi.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    private GameBoard _gameBoard;
    
    private ObservableCollection<ObservableCollection<CellViewModel>> _cells;
    public ObservableCollection<ObservableCollection<CellViewModel>> Cells => _cells;
    
    private Color _backgroundColor = Colors.LightGray;
    public Color BackgroundColor
    {
        get => _backgroundColor;
        set => SetProperty(ref _backgroundColor, value);
    }
    
    private Color _playerTextColor = Colors.Black;
    public Color PlayerTextColor
    {
        get => _playerTextColor;
        set => SetProperty(ref _playerTextColor, value);
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
    
    public MainWindowViewModel()
    {
        _gameBoard = new GameBoard();
        _cells = new ObservableCollection<ObservableCollection<CellViewModel>>();
        
        NewGameCommand = new RelayCommand(NewGame);
        
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
    
    public void CellClicked(int row, int col)
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
    }
    
    private void UpdateGameStatus()
    {
        bool isBlackPlayer = _gameBoard.CurrentPlayer == CellState.Black;
        CurrentPlayerText = $"Current player: {(isBlackPlayer ? "Black" : "White")}";
        ScoreText = $"Black: {_gameBoard.CountPieces(CellState.Black)} - White: {_gameBoard.CountPieces(CellState.White)}";
        
        BackgroundColor = isBlackPlayer ? 
            Color.Parse("#303030") :  // Light gray for Black's turn
            Color.Parse("#E0E0E0");   // Dark gray for White's turn
    
        PlayerTextColor = isBlackPlayer ? Colors.Black : Colors.White;
    }
    
    private void NewGame()
    {
        _gameBoard = new GameBoard();
        InitializeCells();
        UpdateGameStatus();
    }
}


