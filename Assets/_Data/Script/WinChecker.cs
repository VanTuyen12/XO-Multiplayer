using Unity.VisualScripting;
using UnityEngine;

public enum EnumDirection
{
    None,
    Horizontal,
    Vertical,
    DiagonalDown,
    DiagonalUp
}
public struct WinResult
{
    public EnumPlayerType winner;
    public Vector2Int posStart;
    public Vector2Int posEnd;
    public EnumDirection winDirection;
}

public static class WinChecker
{
    private const int WIN_CONDITION = 3;

    private static readonly Vector2Int[] Directions = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1)
    };

    public static WinResult CheckWin(EnumPlayerType[,] board, int lastX, int lastY)
    {
      
        var playerType = board[lastX, lastY];
        if (playerType == EnumPlayerType.None) return new WinResult { winner = EnumPlayerType.None };

        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        for (int i = 0; i < Directions.Length; i++)
        {
            var result = CheckDirection(board, lastX, lastY, Directions[i], playerType, rows, cols);
            if (result.winner != EnumPlayerType.None)
                return result;
        }
       
        return new WinResult { winner = EnumPlayerType.None };
    }

    private static WinResult CheckDirection(EnumPlayerType[,] board, int x, int y, Vector2Int dir,
        EnumPlayerType playerType, int rows, int cols)
    {
        int count = 1;
        Vector2Int start = new Vector2Int(x, y);
        Vector2Int end = new Vector2Int(x, y);
        
        int nX = x + dir.x;
        int nY = y + dir.y;
        
        while (IsValid(board, nX, nY, playerType, rows, cols))
        {
            count++;
            start = new Vector2Int(nX, nY);
            if (count >= WIN_CONDITION)
            {
                var winDir = WinDirection(start, end);
                return new WinResult{ winner =playerType , posStart = start, posEnd = end ,winDirection = winDir};
            } 
                
            
            nX += dir.x;
            nY += dir.y;
        }
        
        nX = x - dir.x;
        nY = y - dir.y;
        while (IsValid(board, nX, nY, playerType, rows, cols))
        {
            count++;
            end = new Vector2Int(nX, nY);
            if (count >= WIN_CONDITION)
            {
                var winDir = WinDirection(start, end);
                return new WinResult{ winner =playerType , posStart = start, posEnd = end , winDirection = winDir};
            }
                
            
            nX -= dir.x;
            nY -= dir.y;
        }
        
        return new WinResult{ winner= EnumPlayerType.None , winDirection = EnumDirection.None};
    }

    private static EnumDirection WinDirection(Vector2Int start, Vector2Int end)
    {
        var dir = end - start;

        if (dir.x == 0) return EnumDirection.Horizontal;
        if (dir.y == 0) return EnumDirection.Vertical;
        
        return dir.x == dir.y ? EnumDirection.DiagonalDown : EnumDirection.DiagonalUp;
    }
    private static bool IsValid(EnumPlayerType[,] board, int x, int y, EnumPlayerType playerType, int rows, int cols)
    {
        return x >= 0 && x < rows && y >= 0 && y < cols && board[x, y] == playerType;
    }

    public static bool IsBoardFull(EnumPlayerType[,] board)
    {
        foreach (var playerType in board)
        {
            if(playerType == EnumPlayerType.None) return false;
        }
        return true;
    }
    
}