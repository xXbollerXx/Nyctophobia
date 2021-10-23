using System;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public GameObject Floor;
    public GameObject Corner_BottomLeft, Corner_BottomRight, Corner_TopLeft, Corner_TopRight;
    public GameObject Wall_Left, Wall_Right, Wall_Top, Wall_Bottom;
    public GameObject Block;
    public static TileMap Instance;

    private void OnValidate()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
    private void Start()
    {

        if (!Instance)
        {
            Instance = this;
        }
    }
}
