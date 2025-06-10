using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Reversi.Models;

namespace Reversi.ViewModels;
public class CellViewModel : ObservableObject
{
    private int _row;
    private int _col;
    private MainWindowViewModel _parent;
    
    private CellState _state;
    public CellState State
    {
        get => _state;
        set => SetProperty(ref _state, value);
    }
    
    public IRelayCommand CellClickCommand { get; }
    
    public IBrush CellBrush
    {
        get
        {
            return State switch
            {
                CellState.Black => Brushes.Black,
                CellState.White => Brushes.White,
                CellState.Valid => Brushes.SlateGray,
                _ => Brushes.Transparent
            };
        }
    }
    
    public CellViewModel(int row, int col, MainWindowViewModel parent)
    {
        _row = row;
        _col = col;
        _parent = parent;
        
        CellClickCommand = new RelayCommand(OnCellClicked);
    }
    
    private void OnCellClicked()
    {
        _parent.CellClicked(_row, _col);
    }
    
    public void UpdateState(CellState newState)
    {
        State = newState;
        OnPropertyChanged(nameof(CellBrush));
    }
}