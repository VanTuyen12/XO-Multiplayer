using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] GameObject _squarePrefab;
    [SerializeField] GameObject _linePrefab;
    [SerializeField] int _rows = 3;
    [SerializeField] int _cols = 5;
    [SerializeField] float _baseTileSize = 1.5f;
    
    [Header("Scale Settings")]
    [SerializeField] float _maxGridWidth = 10f; 
    [SerializeField] float _maxGridHeight = 6f;
    [SerializeField] float _padding = 0.5f;     
    
    [SerializeField] float _lineGap = 0.1f; 
    [SerializeField] private float _actualTileSize;
    private float _scaleRatio;
    public float ScaleRatio => _scaleRatio;
    
    Dictionary<Vector2Int,Transform> _cellCache = new Dictionary<Vector2Int,Transform>();
    protected override void Start()
    {
        base.Start();
        CreateGrid();
        GameManager.Instance.PlayerTypesArray = new EnumPlayerType[_rows, _cols];
    }

    protected virtual void CreateGrid()
    {
        CalculateOptimalTileSize();
        
        CreateCells();
        
        if (_linePrefab != null)
        {
            CreateGridLines();
        }

        CenterGrid();
    }
    
    protected virtual void CreateCells()
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                
                GameObject cell = Instantiate(_squarePrefab, transform);

                float posX = col * _actualTileSize;
                float posY = row * -_actualTileSize;

                cell.transform.localPosition = new Vector2(posX, posY);
                cell.name = $"square {row}_{col}";
                _cellCache[new Vector2Int(row, col)] = cell.transform;
                
                SetGrid(cell, row, col);
                
                cell.transform.localScale = GetSquareSize();
            }
        }
    }

    private void SetGrid(GameObject obj, int row, int col)
    {
        var gridPos = obj.GetComponent<GridPosition>();
        
            gridPos.SetPosX(row);
            gridPos.SetPosY(col);
    }

    protected virtual void CreateGridLines()
    {

        GameObject linesParent = new GameObject("GridLines");
        linesParent.transform.SetParent(transform);
        linesParent.transform.localPosition = Vector3.zero;

        float scaledLineGap = _lineGap * (_actualTileSize / _baseTileSize);
        float gridWidth = _cols * _actualTileSize;
        float gridHeight = _rows * _actualTileSize;

        for (int i = 1; i < _cols; i++)
        {
            float x = (i - 1) * _actualTileSize + _actualTileSize / 2;

            GameObject vLine = Instantiate(_linePrefab, linesParent.transform);
            vLine.transform.localPosition = new Vector3(x, -gridHeight / 2 + _actualTileSize / 2, 0);
            vLine.transform.localRotation = Quaternion.Euler(0, 0, 90);

            float lineHeight = gridHeight - (scaledLineGap * 2);
            _scaleRatio = _actualTileSize / _baseTileSize;
            vLine.transform.localScale = new Vector3(lineHeight / _baseTileSize, _scaleRatio, 1);
        }

        for (int i = 1; i < _rows; i++)
        {
            float y = (i - 1) * -_actualTileSize - _actualTileSize / 2;

            GameObject hLine = Instantiate(_linePrefab, linesParent.transform);
            hLine.transform.localPosition = new Vector3(gridWidth / 2 - _actualTileSize / 2, y, 0);
            hLine.transform.localRotation = Quaternion.identity;

            float lineWidth = gridWidth - (scaledLineGap * 2);
            _scaleRatio = _actualTileSize / _baseTileSize;
            hLine.transform.localScale = new Vector3(lineWidth / _baseTileSize, _scaleRatio, 1);
        }
    }
    
    protected virtual void CalculateOptimalTileSize()
    {
        float baseGridWidth = _cols * _baseTileSize;
        float baseGridHeight = _rows * _baseTileSize;
        
        float availableWidth = _maxGridWidth - (_padding * 2);
        float availableHeight = _maxGridHeight - (_padding * 2);
        
        float scaleX = availableWidth / baseGridWidth;
        float scaleY = availableHeight / baseGridHeight;
        
        float finalScale = Mathf.Min(scaleX, scaleY, 1.5f);
        
        _actualTileSize = _baseTileSize * finalScale;
    }
    
    protected virtual void CenterGrid()
    {
        float gridWidth = _cols * _actualTileSize;
        float gridHeight = _rows * _actualTileSize;
        
        Vector2 centerOffset = new Vector2(
            -gridWidth / 2 + _actualTileSize / 2,
            gridHeight / 2 - _actualTileSize / 2 - 0.5f
        );
        
        transform.localPosition = centerOffset;
    }
    public Transform FindCell(int row, int col)
    {
        var key = new Vector2Int(row, col);
        return _cellCache.TryGetValue(key, out var cell) ? cell : null;
    }
    public Vector2 GetSquareSize()
    {
        return Vector3.one * (_actualTileSize/_baseTileSize);
    }
    
}