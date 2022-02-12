using System;

namespace Entity.Structures
{
    public class DungeonTile
    {
        private readonly Guid _id;
        private bool _visited;
        private bool _walkable;
        private DungeonRoom _room;

        public DungeonTile()
        {
            this._id = Guid.NewGuid();
        }
        public Guid GetID()
        {
            return _id;
        }
        public bool IsVisited()
        {
            return _visited;
        }
        public void SetVisited(bool visited)
        {
            this._visited = visited;
        }
        public bool IsWalkable()
        {
            return _walkable;
        }
        public void SetWalkable(bool walkable)
        {
            this._walkable = walkable;
        }
        public DungeonRoom GetRoom()
        {
            return _room;
        }
        public void SetRoom(DungeonRoom room)
        {
            this._room = room;
        }
        
        public override string ToString()
        {
            return $"id: {_id}\nwalkable: {_walkable}\nvisited: {_visited}";
        }
    }
}