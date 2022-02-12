using UnityEngine;

namespace Collection
{
    public class Grid<T> where T : new()
    {
        private readonly int _width;
        private readonly int _height;
        private readonly T[,] _gridArray;
        private readonly int _cellSize;
        private readonly Vector3 _origin;
        

        public Grid(int xSize, int ySize, int cellSize, Vector3 position, Color color)
        {
            this._width = xSize;
            this._height = ySize;
            this._gridArray = new T[xSize, ySize];
            this._cellSize = cellSize;
            this._origin = position;
            
            for (var x = 0; x < xSize; x++)
            {
                for (var y = 0; y < ySize; y++)
                {
                    _gridArray[x, y] = new T();
                    var wPos = GetWorldPosition(x, y);
                    Debug.DrawLine(new Vector3(wPos.x-.5f,wPos.y,wPos.z-.5f), new Vector3(wPos.x-.5f,wPos.y,wPos.z+.5f), color, 100f);
                    Debug.DrawLine(new Vector3(wPos.x-.5f,wPos.y,wPos.z-.5f), new Vector3(wPos.x+.5f,wPos.y,wPos.z-.5f), color, 100f);
                }
            }
            var xWPos = GetWorldPosition(0, ySize);
            var yWPos = GetWorldPosition(xSize, 0);
            var lastGrid = GetWorldPosition(xSize, ySize);
            
            Debug.DrawLine(new Vector3(xWPos.x-.5f,xWPos.y,xWPos.z-.5f), new Vector3(lastGrid.x-.5f,lastGrid.y,lastGrid.z-.5f), Color.red, 100f);
            Debug.DrawLine(new Vector3(yWPos.x-.5f,yWPos.y,yWPos.z-.5f), new Vector3(lastGrid.x-.5f,lastGrid.y,lastGrid.z-.5f), Color.red, 100f);
        }
        
        public Grid(int xSize, int ySize, int cellSize, Vector3 position) : this(xSize,ySize,cellSize,position, Color.white){}
        
        public void SetValue(Vector3 pos, T val)
        {
            GetXY(pos, out var x, out var y);
            SetValue(x, y, val);
        }
        
        private void SetValue(int x, int y, T val)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height) _gridArray[x, y] = val;
        }
        
        public T GetValue(Vector3 pos)
        {
            GetXY(pos, out var x, out var y);
            return GetValue(x, y);
        }

        public T GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
                return _gridArray[x, y];
            return default(T);
        }

        private void GetXY(Vector3 pos, out int x, out int y)
        {
            x = Mathf.FloorToInt((pos - _origin).x / _cellSize);
            y = Mathf.FloorToInt((pos - _origin).y / _cellSize);
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * _cellSize + _origin;
        }
        
        public Vector3 GetFixedPosition(int x, int y)
        {
            var rWorldPosition = GetWorldPosition(x, y);
            var posX = rWorldPosition.x+1;
            var posY = rWorldPosition.z+1;
            return new Vector3(posX, 0, posY);
        }
    }
}