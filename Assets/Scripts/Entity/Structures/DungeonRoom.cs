using System;
using Enum;
using UnityEngine;

namespace Entity.Structures
{
    public class DungeonRoom
    {
        private readonly Guid _id;
        private bool _visited;
        private Vector4 _structure;
        private RoomType _type;
        
        public DungeonRoom()
        {
            this._id = Guid.NewGuid();
            _structure = new Vector4(0,0,0,0);
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
        public Vector4 GetStructure()
        {
            return _structure;
        }
        public void SetStructure(Vector4 structure)
        {
            this._structure=structure;
        }
        public void SetStructureLeft(int type)
        {
            this._structure.x=type;
        }
        public int GetStructureLeft()
        {
            return (int)this._structure.x;
        }
        public void SetStructureRight(int type)
        {
            this._structure.y=type;
        }
        public int GetStructureRight()
        {
            return (int)this._structure.y;
        }
        public void SetStructureUp(int type)
        {
            this._structure.z=type;
        }
        public int GetStructureUp()
        {
            return (int)this._structure.z;
        }
        public void SetStructureDown(int type)
        {
            this._structure.w=type;
        }
        public int GetStructureDown()
        {
            return (int)this._structure.w;
        }
        public RoomType GetRoomType()
        {
            return _type;
        }
        public void SetRoomType(RoomType type)
        {
            this._type=type;
        }
        

        public override string ToString()
        {
            return $"id: {_id}\nstructure: {_structure.ToString()}\ntype: {_type.ToString()}\nvisited: {_visited}";
        }
    }
}