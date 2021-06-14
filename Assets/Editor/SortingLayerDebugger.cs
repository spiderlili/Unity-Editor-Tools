using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Sorting Layer Debugger.
/// https://diegogiacomelli.com.br/a-sorting-layer-debugger-for-unity/
/// </summary>
public class SortingLayerDebugger : EditorWindow
{
    SceneView _currentSceneView;
    Dictionary<LayerInfo, RendererInfo[]> _layers;

    private bool Enabled
    {
        get
        {
            return _layers != null;
        }

        set
        {
            if (value)
            {
                LoadLayers();
                SceneView.duringSceneGui += HandleDuringSceneGui;
                EditorApplication.hierarchyChanged += HandleHierarchyChanged;
                EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
                UpdateSceneView();
            }
            else
            {
                // Called to put the renderers in original state (before debugger became enabled).
                HideShowRenderers(true);
                _layers = null;
                SceneView.duringSceneGui -= HandleDuringSceneGui;
                EditorApplication.hierarchyChanged -= HandleHierarchyChanged;
                EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;
                UpdateSceneView();
            }
        }
    }

    [MenuItem("Tools/UI/Sorting Layer Debugger")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<SortingLayerDebugger>("Sorting Layer Debugger");
    }

    void HandleDuringSceneGui(SceneView sceneView)
    {
        if (sceneView != null)
            _currentSceneView = sceneView;

        HideShowRenderers();
    }

    void HandleHierarchyChanged()
    {
        LoadLayers();
    }

    void HandlePlayModeStateChanged(PlayModeStateChange obj)
    {
        Enabled = false;
    }

    void OnDestroy()
    {
        Enabled = false;
    }

    /// <summary>
    /// Updates the scene view.
    /// </summary>
    void UpdateSceneView()
    {
        _currentSceneView?.Repaint();
        HideShowRenderers();
    }

    /// <summary>
    /// Hides the show renderers based on user selections.
    /// </summary>
    /// <param name="useOriginalValues">If set to <c>true</c> use original values.</param>
    void HideShowRenderers(bool useOriginalValues = false)
    {
        if (Enabled)
        {
            foreach (var item in _layers)
            {
                foreach (var r in item.Value)
                {
                    if (r.Renderer == null)
                        continue;

                    r.Renderer.enabled = useOriginalValues ? r.OriginalEnabled : item.Key.Enabled;

                    if (item.Key.Enabled)
                    {
                        Handles.Label(
                            r.Renderer.transform.position,
                            SortingLayer.IDToName(r.Renderer.sortingLayerID));
                    }
                }
            }

            Repaint();
        }
    }

    /// <summary>
    /// Loads layers and renderers to an internal dictionary.
    /// </summary>
    void LoadLayers()
    {
        // Find all SpriteRenderer in current scene view.
        var all = Object.FindObjectsOfType<SpriteRenderer>();

        // Use already existent layers, otherwise starts a new one.
        _layers = _layers ?? new Dictionary<LayerInfo, RendererInfo[]>();

        foreach (var l in SortingLayer.layers)
        {
            var item = _layers.FirstOrDefault(x => x.Key.Equals(l));
            var layerRenderers = all.Where(r => r.sortingLayerID == l.id).Select(r => new RendererInfo(r)).ToArray();

            // If layer is new, add it.
            if (item.Key == null)
            {
                _layers.Add(new LayerInfo(l), layerRenderers);

            }
            else
            {
                // If layer is already in dictionary, just update the renderers.
                _layers[item.Key] = layerRenderers;
            }
        }
    }

    void OnGUI()
    {
        if (Enabled)
        {
            EditorGUILayout.LabelField("Layers", EditorStyles.boldLabel);

            foreach (var item in _layers)
            {
                if (item.Value.Length == 0)
                    continue;

                if (GUILayout.Toggle(item.Key.Enabled, $"{item.Key.Layer.name} ({item.Value.Length})") != item.Key.Enabled)
                {
                    item.Key.Enabled = !item.Key.Enabled;
                    UpdateSceneView();
                }
            }

            if (GUILayout.Button("Disable"))
                Enabled = false;
        }
        else
        {
            if (GUILayout.Button("Enable"))
                Enabled = true;

            EditorGUILayout.HelpBox("Click on 'Enable' button to list/show the Sorting Layers.", MessageType.Info);
        }
    }

    class LayerInfo : System.IEquatable<LayerInfo>, System.IEquatable<SortingLayer>
    {
        public LayerInfo(SortingLayer layer)
        {
            Layer = layer;
            Enabled = true;
        }

        public SortingLayer Layer { get; private set; }
        public bool Enabled { get; set; }

        public bool Equals(LayerInfo other)
        {
            if (other == null)
                return false;

            return Layer.id.Equals(other.Layer.id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LayerInfo);
        }

        public bool Equals(SortingLayer other)
        {
            return Layer.id.Equals(other.id);
        }

        public override int GetHashCode()
        {
            return Layer.GetHashCode();
        }
    }

    class RendererInfo
    {
        public RendererInfo(SpriteRenderer renderer)
        {
            Renderer = renderer;
            OriginalEnabled = renderer.enabled;
        }

        public SpriteRenderer Renderer { get; private set; }
        public bool OriginalEnabled { get; private set; }
    }
}