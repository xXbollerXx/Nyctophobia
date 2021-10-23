using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DunGen
{
    [CustomEditor(typeof(MapGenerator)), CanEditMultipleObjects]
    public class MapGenEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            MapGenerator MapGen = (MapGenerator)target;

            if (DrawDefaultInspector() && MapGen.AutoUpdate)
            {
                MapGen.GenerateMap();

            }

            if (GUILayout.Button("Generate"))
            {
                MapGen.GenerateMap();
            }

            if (GUILayout.Button("Clear Tiles"))
            {
                MapGen.ClearTiles();
            }
            if (GUILayout.Button("Destroy Dungeon"))
            {
                DungenRenderer.DestroyDungeon();
            }

        }
    }
}
    
