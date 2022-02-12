using System;
using Entity;
using Enum;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace MapGenerator
{
    public class RecursiveMapAlgorithm : BasicMapGenerator
    {
        public RecursiveMapAlgorithm(int rows, int columns) : base(rows, columns) { }

        public override void GenerateMap()
        {
            VisitRoom(0, 0, Direction.Start);
        }

        private void VisitRoom(int x, int z, Direction prevDirection)
        {
            var movesAvailable = new Direction[4];
            int movesAvailableCount;
            var room = GetRoom(x, z);
            
            do
            {
                movesAvailableCount = 0;
            
                //check move right
                var rightRoom = GetRoom(x, z + 1);
                if (z + 1 < ColumnCount && !rightRoom.IsVisited())
                {
                    movesAvailable[movesAvailableCount] = Direction.Right;
                    movesAvailableCount++;
                }
                else if (!room.IsVisited() && prevDirection != Direction.Left && !RoomType.Corridor.Equals(rightRoom?.GetRoomType()))
                {
                    //add wall
                    room.SetStructureRight(1);
                }

                //check move forward
                var forwardRoom = GetRoom(x + 1, z);
                if (x + 1 < RowCount && !forwardRoom.IsVisited())
                {
                    movesAvailable[movesAvailableCount] = Direction.Up;
                    movesAvailableCount++;
                }
                else if (!room.IsVisited() && prevDirection != Direction.Down && !RoomType.Corridor.Equals(forwardRoom?.GetRoomType()))
                {
                    //add wall
                    room.SetStructureUp(1);
                }

                //check move left
                var leftRoom = GetRoom(x, z - 1);
                if (z > 0 && z - 1 >= 0 && !leftRoom.IsVisited())
                {
                    movesAvailable[movesAvailableCount] = Direction.Left;
                    movesAvailableCount++;
                }
                else if (!room.IsVisited() && prevDirection != Direction.Right && !RoomType.Corridor.Equals(leftRoom?.GetRoomType()))
                {
                    //add wall
                    room.SetStructureLeft(1);
                }

                //check move backward
                var backRoom = GetRoom(x - 1, z);
                if (x > 0 && x - 1 >= 0 && !backRoom.IsVisited())
                {
                    movesAvailable[movesAvailableCount] = Direction.Down;
                    movesAvailableCount++;
                }
                else if (!room.IsVisited() && prevDirection != Direction.Up && !RoomType.Corridor.Equals(backRoom?.GetRoomType()))
                {
                    //add wall
                    room.SetStructureDown(1);
                }
                
                //if is empty room
                if (!room.IsVisited() && room.GetStructure().Equals(new Vector4(0, 0, 0, 0)))
                {
                    room.SetRoomType(RoomType.Empty);
                    //roll the dice for chest
                    if (GameUtils.Drop(25))
                    {
                        room.SetStructure(new Vector4(1, 1, 1, 1));
                    }
                    //roll the dice for enemy
                    else if(GameUtils.Drop(35))
                    {
                        room.SetStructure(new Vector4(2, 2, 2, 2));
                    }
                }
                else if (!room.IsVisited())
                {
                    room.SetRoomType(RoomType.Corridor);
                }
                room.SetVisited(true);

                if (movesAvailableCount > 0)
                {
                    switch (movesAvailable[Random.Range(0, movesAvailableCount)])
                    {
                        case Direction.Start:
                            break;
                        case Direction.Right:
                            VisitRoom(x, z + 1, Direction.Right);
                            break;
                        case Direction.Up:
                            VisitRoom(x + 1, z, Direction.Up);
                            break;
                        case Direction.Left:
                            VisitRoom(x, z - 1, Direction.Left);
                            break;
                        case Direction.Down:
                            VisitRoom(x - 1, z, Direction.Down);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            
            } while (movesAvailableCount > 0);
        }
    }
}