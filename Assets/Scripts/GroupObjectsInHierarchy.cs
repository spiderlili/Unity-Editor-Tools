#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

//group objects in the hierarchy using ctrl+g
public static class GroupObjectsInHierarchy
{
	[MenuItem("Edit/Group %g", true)]
	public static bool GroupTest()
	{
		return Selection.activeTransform != null;
	}
	[MenuItem("Edit/Group %g")]
	public static void Group()
	{
		var parents = GetParents(Selection.gameObjects[0].transform);
		for (int i = 1; i < Selection.gameObjects.Length; ++i)
		{
			var otherParents = GetParents(Selection.gameObjects[i].transform);
			parents = GetCommonParents(parents, otherParents);
			if (parents.Count == 0)
				break;
		}
		var group = new GameObject("Group");
		if (parents.Count > 0)
		{
			group.transform.parent = parents[0];
		}
		group.transform.localPosition = Vector3.zero;
		group.transform.localRotation = Quaternion.identity;
		group.transform.localScale = Vector3.one;
		Undo.RegisterCreatedObjectUndo(group, "Group");
		for (int i = 0; i < Selection.gameObjects.Length; ++i)
		{
			Undo.SetTransformParent(Selection.gameObjects[i].transform, group.transform, "");
		}
		Collapse(group, true);
	}
	public static void Collapse(GameObject gameObject, bool collapse)
	{
		// bail out immediately if the go doesn't have children
		if (gameObject.transform.childCount == 0) return;
		// get a reference to the hierarchy window
		var hierarchy = GetFocusedWindow("Hierarchy");
		// select game object
		SelectObject(gameObject);
		// create a new key event (RightArrow for collapsing, LeftArrow for folding)
		var key = new Event { keyCode = collapse ? KeyCode.RightArrow : KeyCode.LeftArrow, type = EventType.KeyDown };
		// finally, send the window the event
		hierarchy.SendEvent(key);
	}
	public static void SelectObject(Object obj)
	{
		Selection.activeObject = obj;
	}
	public static EditorWindow GetFocusedWindow(string window)
	{
		FocusOnWindow(window);
		return EditorWindow.focusedWindow;
	}
	public static void FocusOnWindow(string window)
	{
		EditorApplication.ExecuteMenuItem("Window/General/" + window);
	}
	static List<Transform> GetParents(Transform child)
	{
		var parents = new List<Transform>();
		var parent = child.parent;
		while (parent != null)
		{
			parents.Add(parent);
			parent = parent.parent;
		}
		return parents;
	}
	static List<Transform> GetCommonParents(List<Transform> parents, List<Transform> otherParents)
	{
		var commonParents = new List<Transform>();
		bool found = false;
		for (int i = 0; i < parents.Count; ++i)
		{
			if (found)
			{
				commonParents.Add(parents[i]);
			}
			else
			{
				for (int j = 0; j < otherParents.Count; ++j)
				{
					if (parents[i] == otherParents[j])
					{
						commonParents.Add(parents[i]);
						found = true;
						break;
					}
				}
			}
		}
		return commonParents;
	}
}
#endif