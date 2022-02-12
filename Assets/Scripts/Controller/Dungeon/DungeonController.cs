using Controller.Room;
using Entity;
using Entity.Structures;
using MapGenerator;
using UnityEngine;
using Utils;

namespace Controller.Dungeon
{
    public class DungeonController : RootController
    {
        public int rowNumber;
        public int colNumber;

        [SerializeField][ReadOnly] private int randomSeed = 12345;
        public GameObject floor;
        public GameObject top;
        public GameObject cube;
        public GameObject chest;
        public GameObject enemy;

        private const float FixedHeight = .55f;

        private RecursiveMapAlgorithm _recursiveMapAlgorithm;
        private GameObject _spawnObj;
        private GameObject _spawnObjInstance;

        [SerializeField] private int inspectX;
        [SerializeField] private int inspectZ;
        
        private void Start()
        {
            GenerateMap();
            PopulateDungeon();
            GameController.SetStatus(GameStatus.Play);
        }

        private void GenerateMap()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            //Random.InitState(randomSeed);
            
            _recursiveMapAlgorithm = new RecursiveMapAlgorithm(rowNumber, colNumber);
            _recursiveMapAlgorithm.GenerateMap();
        }

        private void PopulateDungeon()
        {
            Reset();
            if (_spawnObj == null) _spawnObj = new GameObject("Dungeon");
            _spawnObjInstance = Instantiate(_spawnObj, Vector3.zero, Quaternion.identity);
            GameController.InitTiles(new Vector2Int(rowNumber * 3, colNumber * 3));

            for (var row = 0; row < rowNumber; row++)
            {
                for (var column = 0; column < colNumber; column++)
                {
                    var localScale = floor.transform.localScale;
                    var x = column * localScale.x;
                    var z = row * localScale.z;
                    var room = _recursiveMapAlgorithm.GetRoom(row, column);
                    GenerateSquares(room, new Vector3(x, 0, z));
                    PopolateRoom(room, new Vector3(x + 1, 0, z + 1), _spawnObjInstance);
                }
            }
        }

        private void GenerateSquares(DungeonRoom dungeonRoom, Vector3 pos)
        {
            var localScale = floor.transform.localScale;
            var rows = localScale.x;
            var columns = localScale.z;

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var tile = new Tile(dungeonRoom);
                    var position = new Vector2Int(row + (int)Mathf.Abs(pos.x), column + (int)Mathf.Abs(pos.z));
                    GameController.SetTile(position, tile);
                }
            }
        }

        private void PopolateRoom(DungeonRoom room, Vector3 pos, GameObject parent)
        {
            if (parent == null) parent = new GameObject();
            var roomPrefab = new GameObject($"Room_{room.GetID()}");
            roomPrefab.AddComponent<RoomController>();
            var roomController = roomPrefab.GetComponent<RoomController>();
            roomController.SetRoom(room);
            roomPrefab.transform.SetParent(parent.transform);
            Instantiate(floor, pos, Quaternion.identity, roomPrefab.transform);
            Instantiate(top, new Vector3(pos.x, 1, pos.z), top.transform.rotation, roomPrefab.transform);
            switch (room.GetStructure())
            {
                case var v when v.Equals(new Vector4(0, 0, 0, 0)):
                    //GenerateWall(room.transform,0,0, pos);
                    break;
                case var v when v.Equals(new Vector4(0, 0, 0, 1)):
                    SpawnWall_Z(roomPrefab.transform, -1, pos);
                    SpawnCube(roomPrefab.transform, -1, 1, pos);
                    SpawnCube(roomPrefab.transform, 1, 1, pos);
                    break;
                case var v when v.Equals(new Vector4(0, 0, 1, 0)):
                    SpawnWall_Z(roomPrefab.transform, 1, pos);
                    SpawnCube(roomPrefab.transform, -1, -1, pos);
                    SpawnCube(roomPrefab.transform, 1, -1, pos);
                    break;
                case var v when v.Equals(new Vector4(0, 0, 1, 1)):
                    SpawnWall_Z(roomPrefab.transform, -1, pos);
                    SpawnWall_Z(roomPrefab.transform, 1, pos);
                    break;
                case var v when v.Equals(new Vector4(0, 1, 0, 0)):
                    SpawnWall_X(roomPrefab.transform, 1, pos);
                    SpawnCube(roomPrefab.transform, -1, 1, pos);
                    SpawnCube(roomPrefab.transform, -1, -1, pos);
                    break;
                case var v when v.Equals(new Vector4(0, 1, 0, 1)):
                    SpawnCube(roomPrefab.transform, -1, 1, pos);
                    SpawnCube(roomPrefab.transform, 1, 1, pos);
                    SpawnCube(roomPrefab.transform, 1, 0, pos);
                    SpawnWall_Z(roomPrefab.transform, -1, pos);
                    break;
                case var v when v.Equals(new Vector4(0, 1, 1, 0)):
                    SpawnCube(roomPrefab.transform, -1, -1, pos);
                    SpawnCube(roomPrefab.transform, 1, 0, pos);
                    SpawnCube(roomPrefab.transform, 1, -1, pos);
                    SpawnWall_Z(roomPrefab.transform, 1, pos);
                    break;
                case var v when v.Equals(new Vector4(0, 1, 1, 1)):
                    SpawnWall_Z(roomPrefab.transform, -1, pos);
                    SpawnWall_Z(roomPrefab.transform, 1, pos);
                    SpawnCube(roomPrefab.transform, 1, 0, pos);
                    break;
                case var v when v.Equals(new Vector4(1, 0, 0, 0)):
                    SpawnWall_X(roomPrefab.transform, -1, pos);
                    SpawnCube(roomPrefab.transform, 1, 1, pos);
                    SpawnCube(roomPrefab.transform, 1, -1, pos);
                    break;
                case var v when v.Equals(new Vector4(1, 0, 0, 1)):
                    SpawnCube(roomPrefab.transform, -1, 0, pos);
                    SpawnCube(roomPrefab.transform, -1, 1, pos);
                    SpawnCube(roomPrefab.transform, 1, 1, pos);
                    SpawnWall_Z(roomPrefab.transform, -1, pos);
                    break;
                case var v when v.Equals(new Vector4(1, 0, 1, 0)):
                    SpawnCube(roomPrefab.transform, 1, -1, pos);
                    SpawnCube(roomPrefab.transform, -1, -1, pos);
                    SpawnCube(roomPrefab.transform, -1, 0, pos);
                    SpawnWall_Z(roomPrefab.transform, 1, pos);
                    break;
                case var v when v.Equals(new Vector4(1, 0, 1, 1)):
                    SpawnWall_Z(roomPrefab.transform, 1, pos);
                    SpawnWall_Z(roomPrefab.transform, -1, pos);
                    SpawnCube(roomPrefab.transform, -1, 0, pos);
                    break;
                case var v when v.Equals(new Vector4(1, 1, 0, 0)):
                    SpawnWall_X(roomPrefab.transform, -1, pos);
                    SpawnWall_X(roomPrefab.transform, 1, pos);
                    break;
                case var v when v.Equals(new Vector4(1, 1, 0, 1)):
                    SpawnWall_X(roomPrefab.transform, 1, pos);
                    SpawnWall_X(roomPrefab.transform, -1, pos);
                    SpawnCube(roomPrefab.transform, 0, -1, pos);
                    break;
                case var v when v.Equals(new Vector4(1, 1, 1, 0)):
                    SpawnWall_X(roomPrefab.transform, 1, pos);
                    SpawnWall_X(roomPrefab.transform, -1, pos);
                    SpawnCube(roomPrefab.transform, 0, 1, pos);
                    break;
                case var v when v.Equals(new Vector4(1, 1, 1, 1)):
                    /*GenerateWall(roomPrefab.transform, -1, 1, pos);
                    GenerateWall(roomPrefab.transform, 1, 1, pos);
                    GenerateWall(roomPrefab.transform, -1, -1, pos);
                    GenerateWall(roomPrefab.transform, 1, -1, pos);*/
                    SpawnChest(roomPrefab.transform, 0, 0, pos);
                    break;
                case var v when v.Equals(new Vector4(2, 2, 2, 2)):
                    SpawnEnemy(roomPrefab.transform, 0, 0, pos);
                    break;
                default:
                    Debug.Log("404_NOT_FOUND");
                    break;
            }
        }

        private void SpawnEnemy(Transform parent, int x, int z, Vector3 floorPos)
        {
            var fixedPosX = (int)Mathf.Abs(x + floorPos.x);
            var fixedPosZ = (int)Mathf.Abs(z + floorPos.z);

            //Generate prefab
            Instantiate(enemy, new Vector3(fixedPosX, .1f, fixedPosZ), Quaternion.identity, parent);
            //Set game props
            GameController.GetTile(new Vector2Int(fixedPosX, fixedPosZ)).Walkable = false;
        }

        private void SpawnChest(Transform parent, int x, int z, Vector3 floorPos)
        {
            var fixedPosX = (int)Mathf.Abs(x + floorPos.x);
            var fixedPosZ = (int)Mathf.Abs(z + floorPos.z);

            //Generate prefab
            Instantiate(chest, new Vector3(fixedPosX, .2f, fixedPosZ), Quaternion.identity, parent);
            //Set game props
            GameController.GetTile(new Vector2Int(fixedPosX, fixedPosZ)).Walkable = false;
        }

        private void SpawnCube(Transform parent, int x, int z, Vector3 floorPos)
        {
            var fixedPosX = (int)Mathf.Abs(x + floorPos.x);
            var fixedPosZ = (int)Mathf.Abs(z + floorPos.z);

            //Generate prefab
            Instantiate(cube, new Vector3(fixedPosX, FixedHeight, fixedPosZ), Quaternion.identity, parent);
            //Set game props
            GameController.GetTile(new Vector2Int(fixedPosX, fixedPosZ)).Walkable = false;
        }

        private void SpawnWall_X(Transform parent, int x, Vector3 floorPos)
        {
            var fixedPosX = (int)Mathf.Abs(floorPos.x);
            var fixedPosZ = (int)Mathf.Abs(floorPos.z);

            //Generate prefab
            Instantiate(cube, new Vector3(x + fixedPosX, FixedHeight, -1 + fixedPosZ), Quaternion.identity, parent);
            Instantiate(cube, new Vector3(x + fixedPosX, FixedHeight, 0 + fixedPosZ), Quaternion.identity, parent);
            Instantiate(cube, new Vector3(x + fixedPosX, FixedHeight, 1 + fixedPosZ), Quaternion.identity, parent);
            //Set game props
            GameController.GetTile(new Vector2Int(x + fixedPosX, -1 + fixedPosZ)).Walkable = false;
            GameController.GetTile(new Vector2Int(x + fixedPosX, 0 + fixedPosZ)).Walkable = false;
            GameController.GetTile(new Vector2Int(x + fixedPosX, 1 + fixedPosZ)).Walkable = false;
        }

        private void SpawnWall_Z(Transform parent, int z, Vector3 floorPos)
        {
            var fixedPosX = (int)Mathf.Abs(floorPos.x);
            var fixedPosZ = (int)Mathf.Abs(floorPos.z);

            //Generate prefab
            Instantiate(cube, new Vector3(-1 + fixedPosX, FixedHeight, z + fixedPosZ), Quaternion.identity, parent);
            Instantiate(cube, new Vector3(0 + fixedPosX, FixedHeight, z + fixedPosZ), Quaternion.identity, parent);
            Instantiate(cube, new Vector3(1 + fixedPosX, FixedHeight, z + fixedPosZ), Quaternion.identity, parent);
            //Set game props
            GameController.GetTile(new Vector2Int(-1 + fixedPosX, z + fixedPosZ)).Walkable = false;
            GameController.GetTile(new Vector2Int(0 + fixedPosX, z + fixedPosZ)).Walkable = false;
            GameController.GetTile(new Vector2Int(1 + fixedPosX, z + fixedPosZ)).Walkable = false;
        }

        public void InspectRoom()
        {
            if (_recursiveMapAlgorithm != null)
            {
                var room = GameController.GetTile(new Vector2Int(inspectX, inspectZ)).ParentDungeonRoom;
                Debug.Log(room != null ? room.ToString() : "null");
            }
        }

        public void InspectSquare()
        {
            var tile = GameController.GetTile(new Vector2Int(inspectX, inspectZ));
            Debug.Log(tile != null ? tile.Walkable ? "walkable" : "not_walkable" : "null");
        }

        public void Reset()
        {
            if (_spawnObjInstance != null)
                DestroyImmediate(_spawnObjInstance);
        }
    }
}