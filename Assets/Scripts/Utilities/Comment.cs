using System;
using UnityEditor;
using UnityEngine;


// This component is auto-erased at building time on this class EraseComponentsAfterSceneBuild.
public class Comment : MonoBehaviour
{
#if UNITY_EDITOR
    [Serializable]
    public enum CommentStates
    {
        Edit,
        Info,
        Alert,
        Error
    }

    public CommentStates State;
    public string Text;
#endif
}