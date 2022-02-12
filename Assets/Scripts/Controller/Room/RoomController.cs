using System;
using Entity.Structures;
using UnityEngine;
using Utils;

namespace Controller.Room
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private Guid id;
        [SerializeField] private Vector4 structure;
        
        private DungeonRoom _room;

        public void SetRoom(DungeonRoom room)
        {
            _room = room;
            structure = room.GetStructure();
            id = room.GetID();
        }
    }
}