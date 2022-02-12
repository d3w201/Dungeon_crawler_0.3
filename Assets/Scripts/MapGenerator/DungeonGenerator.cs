using System;
using Collection;
using Controller;
using Controller.Room;
using Entity;
using Entity.Structures;
using Enum;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace MapGenerator
{
    public class DungeonGenerator : RootController
    {
        [SerializeField] private int width = 5;
        [SerializeField] private int height = 5;
        [SerializeField] private int roomsize = 3;
        [SerializeField] private int tilesize = 1;
        [SerializeField] private GameObject cube;
        [SerializeField] private Vector3 origin = new Vector3(1,0,1);

        private Grid<DungeonRoom> rooms;
        private Grid<DungeonTile> tiles;
        
        private GameObject _spawnObj;
        private GameObject _spawnObjInstance;

        private const float FixedHeight = .55f;

        private void OnEnable()
        {
            Random.InitState((int)DateTime.Now.Ticks);

            GenerateBase();
            GenerateMap();
        }

        private void Start()
        {
            BuildDungeon();
            PopolateDungeon();
        }

        private void PopolateDungeon()
        {
            //throw new NotImplementedException();
        }

        private void BuildDungeon()
        {
            if (_spawnObj == null) _spawnObj = new GameObject("Dungeon");
            _spawnObjInstance = Instantiate(_spawnObj, Vector3.zero, Quaternion.identity);
            
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    BuildRoom(x,y);
                }
            }
        }

        private void BuildRoom(int x, int y)
        {
            //get pos
            var rWorldPosition = rooms.GetFixedPosition(x, y);
            var posX = rWorldPosition.x;
            var posZ = rWorldPosition.z;

            Debug.Log($"[x:{x}][y:{y}] world_pos: {posX}|{posZ}");
            
            //test
            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.localScale = new Vector3(.3f, .2f, .3f);
            plane.transform.position = new Vector3(posX, 0, posZ);
            
            //get room
            var room = rooms.GetValue(x, y);
            
            //new prefab
            var roomPrefab = new GameObject($"Room_{room.GetID()}");
            
            //transform prefab
            roomPrefab.transform.SetParent(_spawnObjInstance.transform);
            roomPrefab.transform.position = new Vector3(rWorldPosition.x, 0, rWorldPosition.y);
            
            switch (room.GetStructure())
            {
                case var v when v.Equals(new Vector4(0, 0, 0, 0)):
                    //GenerateWall(room.transform,0,0, pos);
                    break;
                case var v when v.Equals(new Vector4(0, 0, 0, 1)):
                    SpawnWall_Z(roomPrefab.transform, -1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, -1, 1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 1, 1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(0, 0, 1, 0)):
                    SpawnWall_Z(roomPrefab.transform, 1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, -1, -1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 1, -1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(0, 0, 1, 1)):
                    SpawnWall_Z(roomPrefab.transform, -1, rWorldPosition);
                    SpawnWall_Z(roomPrefab.transform, 1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(0, 1, 0, 0)):
                    SpawnWall_X(roomPrefab.transform, 1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, -1, 1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, -1, -1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(0, 1, 0, 1)):
                    SpawnCube(roomPrefab.transform, -1, 1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 1, 1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 1, 0, rWorldPosition);
                    SpawnWall_Z(roomPrefab.transform, -1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(0, 1, 1, 0)):
                    SpawnCube(roomPrefab.transform, -1, -1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 1, 0, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 1, -1, rWorldPosition);
                    SpawnWall_Z(roomPrefab.transform, 1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(0, 1, 1, 1)):
                    SpawnWall_Z(roomPrefab.transform, -1, rWorldPosition);
                    SpawnWall_Z(roomPrefab.transform, 1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 1, 0, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(1, 0, 0, 0)):
                    SpawnWall_X(roomPrefab.transform, -1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 1, 1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 1, -1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(1, 0, 0, 1)):
                    SpawnCube(roomPrefab.transform, -1, 0, rWorldPosition);
                    SpawnCube(roomPrefab.transform, -1, 1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 1, 1, rWorldPosition);
                    SpawnWall_Z(roomPrefab.transform, -1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(1, 0, 1, 0)):
                    SpawnCube(roomPrefab.transform, 1, -1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, -1, -1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, -1, 0, rWorldPosition);
                    SpawnWall_Z(roomPrefab.transform, 1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(1, 0, 1, 1)):
                    SpawnWall_Z(roomPrefab.transform, 1, rWorldPosition);
                    SpawnWall_Z(roomPrefab.transform, -1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, -1, 0, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(1, 1, 0, 0)):
                    SpawnWall_X(roomPrefab.transform, -1, rWorldPosition);
                    SpawnWall_X(roomPrefab.transform, 1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(1, 1, 0, 1)):
                    SpawnWall_X(roomPrefab.transform, 1, rWorldPosition);
                    SpawnWall_X(roomPrefab.transform, -1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 0, -1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(1, 1, 1, 0)):
                    SpawnWall_X(roomPrefab.transform, 1, rWorldPosition);
                    SpawnWall_X(roomPrefab.transform, -1, rWorldPosition);
                    SpawnCube(roomPrefab.transform, 0, 1, rWorldPosition);
                    break;
                case var v when v.Equals(new Vector4(1, 1, 1, 1)):
                    /*GenerateWall(roomPrefab.transform, -1, 1, pos);
                    GenerateWall(roomPrefab.transform, 1, 1, pos);
                    GenerateWall(roomPrefab.transform, -1, -1, pos);
                    GenerateWall(roomPrefab.transform, 1, -1, pos);*/
                    //SpawnChest(roomPrefab.transform, 0, 0, pos);
                    break;
                case var v when v.Equals(new Vector4(2, 2, 2, 2)):
                    //SpawnEnemy(roomPrefab.transform, 0, 0, pos);
                    break;
                default:
                    Debug.Log("404_NOT_FOUND");
                    break;
            }
        }

        private void SpawnCube(Transform parent, int x, int z, Vector3 floorPos)
        {
            var fixedPosX = (int)Mathf.Abs(x + floorPos.x);
            var fixedPosZ = (int)Mathf.Abs(z + floorPos.z);

            //Generate prefab
            Instantiate(cube, new Vector3(fixedPosX, FixedHeight, fixedPosZ), Quaternion.identity);
            //Set game props
            //GameController.GetTile(new Vector2Int(fixedPosX, fixedPosZ)).Walkable = false;
        }

        private void SpawnWall_X(Transform parent, int x, Vector3 floorPos)
        {
            var fixedPosX = (int)Mathf.Abs(floorPos.x);
            var fixedPosZ = (int)Mathf.Abs(floorPos.z);

            //Generate prefab
            Instantiate(cube, new Vector3(x + fixedPosX, FixedHeight, -1 + fixedPosZ), Quaternion.identity);
            Instantiate(cube, new Vector3(x + fixedPosX, FixedHeight, 0 + fixedPosZ), Quaternion.identity);
            Instantiate(cube, new Vector3(x + fixedPosX, FixedHeight, 1 + fixedPosZ), Quaternion.identity);
            //Set game props
            //GameController.GetTile(new Vector2Int(x + fixedPosX, -1 + fixedPosZ)).Walkable = false;
            //GameController.GetTile(new Vector2Int(x + fixedPosX, 0 + fixedPosZ)).Walkable = false;
            //GameController.GetTile(new Vector2Int(x + fixedPosX, 1 + fixedPosZ)).Walkable = false;
        }

        private void SpawnWall_Z(Transform parent, int z, Vector3 floorPos)
        {
            var fixedPosX = (int)Mathf.Abs(floorPos.x);
            var fixedPosZ = (int)Mathf.Abs(floorPos.z);

            //Generate prefab
            Instantiate(cube, new Vector3(-1 + fixedPosX, FixedHeight, z + fixedPosZ), Quaternion.identity);
            Instantiate(cube, new Vector3(0 + fixedPosX, FixedHeight, z + fixedPosZ), Quaternion.identity);
            Instantiate(cube, new Vector3(1 + fixedPosX, FixedHeight, z + fixedPosZ), Quaternion.identity);
            //Set game props
            //GameController.GetTile(new Vector2Int(-1 + fixedPosX, z + fixedPosZ)).Walkable = false;
            //GameController.GetTile(new Vector2Int(0 + fixedPosX, z + fixedPosZ)).Walkable = false;
            //GameController.GetTile(new Vector2Int(1 + fixedPosX, z + fixedPosZ)).Walkable = false;
        }

        private void GenerateMap()
        {
            tiles = new Grid<DungeonTile>(width * roomsize, height * roomsize, tilesize, origin, Color.blue);
        }

        private void AddTilesRoom(int xRoomGridPosition, int yRoomGridPosition)
        {
            var rm = rooms.GetValue(xRoomGridPosition, yRoomGridPosition);
            for (var x = 0; x < roomsize; x++)
            {
                for (var y = 0; y < roomsize; y++)
                {
                    var tile = tiles.GetValue(x + xRoomGridPosition, y + yRoomGridPosition);
                    tile.SetRoom(rm);
                }
            }
        }

        private void GenerateBase()
        {
            rooms = new Grid<DungeonRoom>(width, height, roomsize, origin, Color.yellow);
            VisitRoom(0, 0, Direction.Start);
        }

        private void VisitRoom(int x, int y, Direction prevDirection)
        {
            var movesAvailable = new Direction[4];
            int movesAvailableCount;

            var room = rooms.GetValue(x, y);
            
            //test
            

            do
            {
                movesAvailableCount = 0;
                var rightRoom = rooms.GetValue(x+1, y);
                var leftRoom = rooms.GetValue(x - 1, y);
                var upRoom = rooms.GetValue(x, y + 1);
                var downRoom = rooms.GetValue(x, y - 1);

                //check move right
                if (x + 1 < width && !rightRoom.IsVisited())
                {
                    movesAvailable[movesAvailableCount] = Direction.Right;
                    movesAvailableCount++;
                }
                else if (!room.IsVisited() && prevDirection != Direction.Right &&
                         !RoomType.Corridor.Equals(rightRoom?.GetRoomType()))
                {
                    //add wall
                    room.SetStructureRight(1);
                }

                //check move forward
                if (y + 1 < height && !upRoom.IsVisited())
                {
                    movesAvailable[movesAvailableCount] = Direction.Up;
                    movesAvailableCount++;
                }
                else if (!room.IsVisited() && prevDirection != Direction.Up &&
                         !RoomType.Corridor.Equals(upRoom?.GetRoomType()))
                {
                    //add wall
                    room.SetStructureUp(1);
                }

                //check move left
                if (x > 0 && x - 1 >= 0 && !leftRoom.IsVisited())
                {
                    movesAvailable[movesAvailableCount] = Direction.Left;
                    movesAvailableCount++;
                }
                else if (!room.IsVisited() && prevDirection != Direction.Left &&
                         !RoomType.Corridor.Equals(leftRoom?.GetRoomType()))
                {
                    //add wall
                    room.SetStructureLeft(1);
                }

                //check move backward
                if (y > 0 && y - 1 >= 0 && !downRoom.IsVisited())
                {
                    movesAvailable[movesAvailableCount] = Direction.Down;
                    movesAvailableCount++;
                }
                else if (!room.IsVisited() && prevDirection != Direction.Down &&
                         !RoomType.Corridor.Equals(downRoom?.GetRoomType()))
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
                    else if (GameUtils.Drop(35))
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
                            VisitRoom(x+1, y, Direction.Left);
                            break;
                        case Direction.Up:
                            VisitRoom(x, y+1, Direction.Down);
                            break;
                        case Direction.Left:
                            VisitRoom(x-1, y , Direction.Right);
                            break;
                        case Direction.Down:
                            VisitRoom(x, y-1, Direction.Up);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            } while (movesAvailableCount > 0);
        }
        
        public void Reset()
        {
            if (_spawnObjInstance != null)
                DestroyImmediate(_spawnObjInstance);
        }
    }
}