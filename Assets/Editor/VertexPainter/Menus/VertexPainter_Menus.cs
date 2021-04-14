using System.Collections;
using UnityEngine;
using UnityEditor;

public class VertexPainter_Menus : MonoBehaviour
{
    [MenuItem("Tools/Art/Vertex Painter", false)]
    static void LaunchVertexPainter()
    {
        VertexPainter_Window.LaunchVertexPainter();
    }
}
