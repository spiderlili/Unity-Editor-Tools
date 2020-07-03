using UnityEngine;

public class PropertyDrawersExample : MonoBehaviour
{
    [Range(0, 100)]
    public int intValue = 50;

    [Range(0, 1)]
    public float floatRange = 0.5f;

    [Multiline(2)]
    public string stringMultiline = "This text is using a multiline \nproperty drawer";

    [TextArea(2, 4)] //editable string within a height-flexible & scrollable text area: min / max value for scrollbar
    public string stringTextArea = "This text \nis using \na text area property drawer";

    //makes a method accessible in the context menu of component script: public ContextMenuItemAttribute(string name);
    [ContextMenu("Execute a custom method")]
    public void ExampleCustomFunction()
    {
        Debug.Log("ExampleCustomFunction called");
    }

    [ContextMenuItem("Reset this value", "Reset")]
    public int intReset = 100;

    public void Reset()
    {
        intReset = 0;
    }
}
