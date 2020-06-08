using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;

public class TextToTMProConverter : ScriptableWizard
{
    public TMP_FontAsset fontAsset;
    public bool onlyBestFitTexts;

    //[MenuItem("Tools/Text/Convert All Text to TMPro")]
    public static void RunConvertAllWizard()
    {
        ScriptableWizard.DisplayWizard("Convert Text to TMPro", typeof(TextToTMProConverter), "Convert", string.Empty);
    }

    [MenuItem("Tools/Text/Convert Selected Texts to TMPro")]
    public static void ConvertSelected()
    {
        foreach (var go in Selection.gameObjects)
        {
            Text txt = go.GetComponent<Text>();
            if (txt != null)
            {
                ConvertToTmPro(txt, null);
            }
        }
    }

    void OnWizardCreate()
    {
        foreach (var txt in Resources.FindObjectsOfTypeAll<Text>())
        {
            if (!ParentHasComponent<InputField>(txt.gameObject) &&
                (!onlyBestFitTexts || txt.resizeTextForBestFit))
            {
                ConvertToTmPro(txt, fontAsset);
            }
        }
    }

    void OnWizardUpdate()
    {
        if (fontAsset == null)
        {
            errorString = "Select a TM Pro font asset to use on all converted components";
            isValid = false;
        }
        else
        {
            errorString = string.Empty;
            isValid = true;
        }
    }

    private static void ConvertToTmPro(Text txt, TMP_FontAsset font)
    {
        var origText = txt.text;
        var origAlignment = txt.alignment;
        var origStyle = txt.fontStyle;
        var origFontSize = txt.fontSize;
        var origColor = txt.color;
        var go = txt.gameObject;
        var origAutoSize = txt.resizeTextForBestFit;
        var origMinSize = txt.resizeTextMinSize;
        var origMaxSize = txt.resizeTextMaxSize;

        DestroyImmediate(txt, true);

        var tmPro = go.AddComponent<TextMeshProUGUI>();
        tmPro.text = origText;
        tmPro.fontSize = origFontSize;
        tmPro.enableAutoSizing = origAutoSize;
        if (tmPro.enableAutoSizing)
        {
            tmPro.fontSizeMin = origMinSize;
            tmPro.fontSizeMax = origMaxSize;
        }

        if (origStyle == FontStyle.Bold)
            tmPro.fontStyle = FontStyles.Bold;
        else if (origStyle == FontStyle.BoldAndItalic)
            tmPro.fontStyle = FontStyles.Bold | FontStyles.Italic;
        else if (origStyle == FontStyle.Italic)
            tmPro.fontStyle = FontStyles.Italic;
        else
            tmPro.fontStyle = FontStyles.Normal;

        if (origAlignment == TextAnchor.LowerCenter)
            tmPro.alignment = TextAlignmentOptions.Bottom;
        else if (origAlignment == TextAnchor.LowerLeft)
            tmPro.alignment = TextAlignmentOptions.BottomLeft;
        else if (origAlignment == TextAnchor.LowerRight)
            tmPro.alignment = TextAlignmentOptions.BottomRight;
        else if (origAlignment == TextAnchor.MiddleCenter)
            tmPro.alignment = TextAlignmentOptions.Center;
        else if (origAlignment == TextAnchor.MiddleLeft)
            tmPro.alignment = TextAlignmentOptions.Left;
        else if (origAlignment == TextAnchor.MiddleRight)
            tmPro.alignment = TextAlignmentOptions.Right;
        else if (origAlignment == TextAnchor.UpperCenter)
            tmPro.alignment = TextAlignmentOptions.Top;
        else if (origAlignment == TextAnchor.UpperLeft)
            tmPro.alignment = TextAlignmentOptions.TopLeft;
        else if (origAlignment == TextAnchor.UpperRight)
            tmPro.alignment = TextAlignmentOptions.TopRight;

        if (font != null)
        {
            tmPro.font = font;
        }
        tmPro.color = origColor;

        EditorSceneManager.MarkSceneDirty(go.scene);

        Debug.Log("Converted [" + origText + "]", tmPro);
    }

    // Can't simply use GetComponentInParent<> because we have to handle disabled hierarchies, too
    private bool ParentHasComponent<T>(GameObject child)
    {
        bool hasAny = false;
        var xform = child.transform;
        while (xform != null)
        {
            if (xform.GetComponent<T>() != null)
            {
                hasAny = true;
                break;
            }
            xform = xform.parent;
        }
        return hasAny;
    }
}