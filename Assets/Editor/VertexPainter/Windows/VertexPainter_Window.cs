using System.Collections;
using UnityEditor;
public class VertexPainter_Window : EditorWindow
{
    #region Variables
    #endregion

    #region MainMethod
    public static void LaunchVertexPainter() //init the editor window: normal window, title, focused on launch
    {
        var vertexPainterWindow = EditorWindow.GetWindow<VertexPainter_Window>(false, "Vertex Painter", true);
    }

    #endregion

    #region GUIMethods
    #endregion

    #region UtilityMethods
    #endregion
}
