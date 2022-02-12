using Entity.Structures;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Entity
{
    public enum Direction
    {
        Start,
        Right,
        Up,
        Left,
        Down,
    };
    
    public enum GameStatus
    {
        Pause,
        Play,
        Interact,
        Pickup,
        Inventory,
        GameOver
    }

    public enum NpcState
    {
        Idle,
        Chase,
        Attack,
        Wander,
        Death
    }

    public class Tile
    {
        public DungeonRoom ParentDungeonRoom;
        public bool Walkable;

        public Tile(DungeonRoom dungeonRoom)
        {
            ParentDungeonRoom = dungeonRoom;
            Walkable = true;
        }
    }
}