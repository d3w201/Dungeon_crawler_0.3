using Controller.Dungeon;
using UnityEditor;
using UnityEngine;

//TODO: DELETE THIS CLASS. in next release other editor inspector will be implemented
namespace MapGenerator
{
    [CustomEditor(typeof(DungeonController))]
    public class DungeonInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DungeonController dungeonController = (DungeonController)target;
            /*
            if (GUILayout.Button("Pop Dungeon"))
            {
                dungeonController.PopulateDungeon();
            }
            if (GUILayout.Button("Reset"))
            {
                dungeonController.Reset();
            }
            if (GUILayout.Button("DEBUG:Pop Room"))
            {
                dungeonController.PopolateRoom(dungeonController.structure, dungeonController.nextPos,new GameObject());
            }*/
            /*if (GUILayout.Button("DEBUG:Generate Map"))
            {
                dungeonController.GenerateMap();
            }*/
            if (GUILayout.Button("DEBUG:Inspect Room"))
            {
                dungeonController.InspectRoom();
            }
            if (GUILayout.Button("DEBUG:Inspect Square"))
            {
                dungeonController.InspectSquare();
            }
            
        }
    }
}
