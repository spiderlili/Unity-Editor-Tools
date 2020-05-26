using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Comment))]
public class CommentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Comment commentScript = (Comment)target;

        // Space at the beginning
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();

        // Select the mode
        commentScript.State = (Comment.CommentStates)EditorGUILayout.EnumPopup(commentScript.State, GUILayout.Width(50));

        // Show the corresponding help box depending on the state we are
        EditorGUILayout.Space();
        if (commentScript.State == Comment.CommentStates.Edit)
        {
            EditorStyles.textArea.wordWrap = true;
            commentScript.Text = EditorGUILayout.TextArea(commentScript.Text, EditorStyles.textArea);
        }
        else if (commentScript.State == Comment.CommentStates.Info)
        {
            EditorGUILayout.HelpBox(commentScript.Text, MessageType.Info);
        }
        else if (commentScript.State == Comment.CommentStates.Alert)
        {
            EditorGUILayout.HelpBox(commentScript.Text, MessageType.Warning);
        }
        else if (commentScript.State == Comment.CommentStates.Error)
        {
            EditorGUILayout.HelpBox(commentScript.Text, MessageType.Error);
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(commentScript);
        }

        EditorGUILayout.Space();
    }
}