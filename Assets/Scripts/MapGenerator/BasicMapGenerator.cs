using Entity;
using Entity.Structures;
using UnityEngine;

namespace MapGenerator
{
    public abstract class BasicMapGenerator
    {
        protected int RowCount { get; }
        protected int ColumnCount { get; }
        private readonly DungeonRoom[,] _rooms;

        protected BasicMapGenerator(int rows, int columns){
            RowCount = Mathf.Abs(rows);
            ColumnCount = Mathf.Abs(columns);
            if (RowCount == 0) {
                RowCount = 1;
            }
            if (ColumnCount == 0) {
                ColumnCount = 1;
            }
            _rooms = new DungeonRoom[rows,columns];
            var id = 0;
            for (var row = 0; row < rows; row++) {
                for(var column = 0; column < columns; column++)
                {
                    _rooms[row,column] = new DungeonRoom();
                }
            }
        }
    
        public abstract void GenerateMap();
    
        public DungeonRoom GetRoom(int row, int column){
            if (row >= 0 && column >= 0 && row < RowCount && column < ColumnCount) {
                return _rooms[row,column];
            }else{
                //Debug.Log(row+" "+column);
                return null;
            }
        }
    }
}
