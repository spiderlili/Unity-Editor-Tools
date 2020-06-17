using UnityEngine;
using System.Collections;

public class SkinnedToggleGUI : MonoBehaviour
{
    public GUISkin testGUIskin;

    private bool firstToggle = false;
    private bool secondToggle = false;
    private bool thirdToggle = false;

    void Start()
    {
        if (this.testGUIskin == null)
        {
            Debug.LogError("Please assign a GUIskin on the editor!");
            this.enabled = false;
            return;
        }
    }

    void OnGUI()
    {
        this.firstToggle = GUI.Toggle(new Rect(32, 16, 64, 64), this.firstToggle, "Simple Toggle", this.testGUIskin.customStyles[0]);
        this.secondToggle = GUI.Toggle(new Rect(32, 96, 64, 64), this.secondToggle, "Toggle With Hover", this.testGUIskin.customStyles[1]);
        this.thirdToggle = GUI.Toggle(new Rect(32, 176, 64, 64), this.thirdToggle, "Complete Toggle", this.testGUIskin.customStyles[2]);
    }
}