using Unity.VisualScripting;
using UnityEngine;

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

    public static EnumPlayerType CheckWin(EnumPlayerType[,] board, int lastX, int lastY)
    {
      
        var playerType = board[lastX, lastY];
        if (playerType == EnumPlayerType.None) return EnumPlayerType.None;

        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        for (int i = 0; i < Directions.Length; i++)
        {
            if (CheckDirection(board, lastX, lastY, Directions[i], playerType, rows, cols))
                return playerType;
        }
       
        return EnumPlayerType.None;
    }

    private static bool CheckDirection(EnumPlayerType[,] board, int x, int y, Vector2Int dir,
        EnumPlayerType playerType, int rows, int cols)
    {
        int count = 1;

        int nX = x + dir.x;
        int nY = y + dir.y;
        
        while (IsValid(board, nX, nY, playerType, rows, cols))
        {
            count++;
            if (count >= WIN_CONDITION) return true;
            
            nX += dir.x;
            nY += dir.y;
        }
        
        nX = x - dir.x;
        nY = y - dir.y;
        while (IsValid(board, nX, nY, playerType, rows, cols))
        {
            count++;
            if (count >= WIN_CONDITION) return true;
            
            nX -= dir.x;
            nY -= dir.y;
        }
        
        return count >= WIN_CONDITION;
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