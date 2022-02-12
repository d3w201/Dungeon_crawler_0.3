using System;
using Entity;
using UnityEngine;

namespace Controller.Game
{
    public class GameController : MonoBehaviour
    {
        //Private-props
        private GameStatus _gameStatus;
        private Tile[,] _tiles;
        
        //Getter & Setter
        public void SetStatus(GameStatus status)
        {
            _gameStatus = status;
        }

        public GameStatus GetStatus()
        {
            return _gameStatus;
        }

        public Tile GetTile(Vector2Int pos)
        {
            return _tiles[pos.x, pos.y];
        }
        
        public void SetTile(Vector2Int pos, Tile tile)
        {
            _tiles[pos.x, pos.y] = tile;
        }
        
        public Tile[,] GetTiles()
        {
            return _tiles;
        }
        
        //Init_Start
        private void Awake()
        {
            SetStatus(GameStatus.Pause);
        }

        public void InitTiles(Vector2Int size)
        {
            _tiles = new Tile[size.x, size.y];
        }

        
        
        
    }
}
