using System;
using System.Collections.Generic;

namespace Reversi.Models;

public enum CellState
{
    Empty,
    Black,
    White,
    Valid
}

public class GameBoard
{
    public CellState ChangePlayer(CellState currentPlayer) => 
        (currentPlayer == CellState.Black) ? CellState.White : CellState.Black;
    
    public const int BoardSize = 8;
    private CellState[,] _board;
    public CellState CurrentPlayer { get; internal set; }
    public bool GameOver { get; set; }

    public GameBoard()
    {
        _board = new CellState[BoardSize, BoardSize];
        InitializeBoard();
        HasValidMoves(CurrentPlayer);
    }

    private void InitializeBoard()
    {
        GameOver = false;
        
        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize; j++)
            {
                _board[i, j] = CellState.Empty;
            }
        }
        
        CurrentPlayer = CellState.Black;
        
        var midle = BoardSize / 2;
        _board[midle - 1, midle] = CellState.Black;
        _board[midle, midle - 1] = CellState.Black;
        _board[midle - 1, midle - 1] = CellState.White;
        _board[midle, midle] = CellState.White;
    }

    public CellState GetCell(int row, int col)
    {
        if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
        {
            throw new IndexOutOfRangeException($"Invalid cell position: ({row}, {col}).");
        }

        return _board[row, col];
    }

    public bool MakeMove(int row, int col)
    {
        if (GetCell(row, col) != CellState.Valid || GameOver)
            return false;
        
        _board[row, col] = CurrentPlayer;
        FlipPieces(row, col, CurrentPlayer);
        
        CurrentPlayer = ChangePlayer(CurrentPlayer);
        
        if (!HasValidMoves(CurrentPlayer)) GameOver = true;
        
        return true;
    }

    private bool IsValidMove(int row, int col, CellState player)
    {
        // Basic checks
        if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize || _board[row, col] != CellState.Empty)
            return false;

        // Check in all 8 directions
        int[] directions = [-1, 0, 1];
        
        foreach (var drow in directions)
        {
            foreach (var dcol in directions)
            {
                // Skip the (0,0) direction
                if (drow == 0 && dcol == 0)
                    continue;
                    
                if (WouldFlipInDirection(row, col, drow, dcol, player))
                    return true;
            }
        }
        
        return false;
    }

    public bool WouldFlipInDirection(int row, int col, int drow, int dcol, CellState player)
    {
        var opponent = ChangePlayer(CurrentPlayer);
        var r = row + drow;
        var c = col + dcol;
        
        if (r < 0 || r >= BoardSize || c < 0 || c >= BoardSize || _board[r, c] != opponent)
            return false;
        
        while (true)
        {
            r += drow;
            c += dcol;
            
            if (r < 0 || r >= BoardSize || c < 0 || c >= BoardSize)
                return false;
                
            if (_board[r, c] == CellState.Empty)
                return false;
                
            if (_board[r, c] == player)
                return true;
        }
    }

    private void FlipPieces(int row, int col, CellState player)
    {
        int[] directions = { -1, 0, 1 };
        
        foreach (var drow in directions)
        {
            foreach (var dcol in directions)
            {
                // Skip the (0,0) direction
                if (drow == 0 && dcol == 0)
                    continue;
                    
                if (WouldFlipInDirection(row, col, drow, dcol, player))
                    FlipInDirection(row, col, drow, dcol, player);
            }
        }
    }

    private void FlipInDirection(int row, int col, int drow, int dcol, CellState player)
    {
        var opponent = ChangePlayer(CurrentPlayer);
        var r = row + drow;
        var c = col + dcol;
        
        while (_board[r, c] == opponent)
        {
            _board[r, c] = player;
            r += drow;
            c += dcol;
        }
    }

    private bool HasValidMoves(CellState player)
    {
        var hasValidMove = false;
        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize; j++)
            {
                if (IsValidMove(i, j, player))
                {
                    _board[i, j] = CellState.Valid;
                    hasValidMove = true;
                } else if (GetCell(i, j) != CellState.Black && GetCell(i, j) != CellState.White)
                {
                    _board[i, j] = CellState.Empty;
                } 
            }
        }
        return hasValidMove;
    }

    public int CountPieces(CellState player)
    {
        var count = 0;
        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize; j++)
            {
                if (_board[i, j] == player)
                    count++;
            }
        }
        return count;
    }

    public List<(int, int)> GetValidMoves(CellState player)
    {
        var validMoves = new List<(int, int)>();
        
        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize; j++)
            {
                if (IsValidMove(i, j, player))
                {
                    validMoves.Add((i, j));
                }
            }
        }
        
        return validMoves;
    }
    
    public GameBoard CloneBoard()
    {
        var newBoard = new GameBoard();
        newBoard.CurrentPlayer = CurrentPlayer;
        newBoard.GameOver = GameOver;
        
        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize; j++)
            {
                newBoard._board[i, j] = _board[i, j];
            }
        }
        
        return newBoard;
    }
}