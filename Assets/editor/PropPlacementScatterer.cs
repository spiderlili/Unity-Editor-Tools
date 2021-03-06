using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public struct RandomPtAngleInstData
{
    public Vector2 pointInDisc;
    public float randomAngleDeg;

    public void SetRandomValues()
    {
        pointInDisc = Random.insideUnitCircle;
        randomAngleDeg = Random.value * 360;
    }
}

public class PropPlacementScatterer : EditorWindow
{
    [MenuItem("Tools/Prop Placement Scatterer")]
    public static void OpenPropPlacermentTool() => GetWindow<PropPlacementScatterer>();

    public float radius = 2f;
    public float radiusIncrementer = 0.1f;
    public int spawnCount = 10;

    public GameObject spawnPrefab;
    public Material previewMaterial;
    RandomPtAngleInstData[] randomPoints;
    //generate random points in disc
    //Vector2[] randomPoints; //just want 2D coordinates within the disk's coordinate system

    //boilerplate for undo/redo system
    SerializedObject serializedObject;
    SerializedProperty propRadius;
    SerializedProperty propRadiusIncrementer;
    SerializedProperty propSpawnCount;
    SerializedProperty propSpawnPrefab;
    SerializedProperty propPreviewMaterial;

    //populate available prefabs to spawn into scene prefab select button
    GameObject[] spawnablePrefabs;

    void GenerateRandomPoints()
    {
        randomPoints = new RandomPtAngleInstData[spawnCount];
        for (int i = 0; i < spawnCount; i++)
        {
            randomPoints[i].SetRandomValues(); //random points of spawnCount inside a unit circle
        }
    }

    void OnGUI() //gui loop for editor window
    {
        serializedObject.Update(); //make serialized property update when parameters changed

        EditorGUILayout.HelpBox("Use scroll wheel to decrease / increase radius, hold Alt + scroll to zoom in the scene view, press Space to spawn prefab objects", MessageType.Info); //helper text

        EditorGUILayout.PropertyField(propRadius);
        propRadius.floatValue = Mathf.Max(1f, propRadius.floatValue); //limit range and prevent negative value

        EditorGUILayout.PropertyField(propRadiusIncrementer);
        propRadiusIncrementer.floatValue = Mathf.Max(0.1f, propRadiusIncrementer.floatValue);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(propSpawnCount);
        propSpawnCount.intValue = propSpawnCount.intValue.AtLeast(1);
        if (EditorGUI.EndChangeCheck())
        {
            GenerateRandomPoints(); //update all points every time you change spawncount
        }

        EditorGUILayout.PropertyField(propSpawnPrefab);

        //EditorGUILayout.PropertyField(propPreviewMaterial);

        //serializedObject.ApplyModifiedProperties();
        if (serializedObject.ApplyModifiedProperties()) //force repaint of the sceneview to make framerate smoother
        {
            SceneView.RepaintAll();
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0) //if left clicked in the editor window
        {
            GUI.FocusControl(null); //remove focus from ui
            Repaint(); //removes delay on the editorwindow ui
        }
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        propRadius = serializedObject.FindProperty("radius");
        propRadiusIncrementer = serializedObject.FindProperty("radiusIncrementer");
        propSpawnCount = serializedObject.FindProperty("spawnCount");
        propSpawnPrefab = serializedObject.FindProperty("spawnPrefab");
        propPreviewMaterial = serializedObject.FindProperty("propPreviewMaterial");
        GenerateRandomPoints();

        //sign up to an event called in every scene's onGUI event when the window is opened
        SceneView.duringSceneGui += DuringSceneGUI;

        //load prefabs
        string[] guids = AssetDatabase.FindAssets("t:prefabs", new []
        {
            "Assets/Prefabs"
        });
        IEnumerable<string> paths = guids.Select(AssetDatabase.GUIDToAssetPath);
        spawnablePrefabs = paths.Select(AssetDatabase.LoadAssetAtPath<GameObject>).ToArray();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI; //unsubscribe from event
    }

    private void DrawSphere(Vector3 pos) //requries a world space position but
    {
        Handles.SphereHandleCap(-1, pos, Quaternion.identity, 0.1f, EventType.Repaint); //1 repaint event is sent every frame
    }

    private void TrySpawnObjects(List<Pose> poseHitPoints)
    {
        if (spawnPrefab == null)
        {
            return;
        }

        foreach (Pose poseHit in poseHitPoints) //spawn prefab
        {
            GameObject spawnedPf = (GameObject) PrefabUtility.InstantiatePrefab(spawnPrefab);
            Undo.RegisterCreatedObjectUndo(spawnedPf, "Spawn objects");
            spawnedPf.transform.position = poseHit.position;
            spawnedPf.transform.rotation = poseHit.rotation; //use world up vector as a reference vector
        }
        GenerateRandomPoints(); //update points after spawning to avoid stamping the same scene pattern again
    }

    private void DuringSceneGUI(SceneView sceneView) //gui for sceneview window: called per scene view you have open: can have multiple scenes open
    {
        Handles.BeginGUI();
        Rect rect = new Rect(8, 8, 64, 64);

        foreach (var pf in spawnablePrefabs) //try GameObject
        {
            Texture icon = AssetPreview.GetAssetPreview(pf); //get reference texture for the preview icon
            //check whether the prefab is selected
            if (GUI.Toggle(rect, spawnPrefab == pf, new GUIContent(icon))) // new GUIContent(pf.name, icon) to display name
            {
                spawnPrefab = pf;
            }
            rect.y += rect.height + 2; //add margin between the buttons
        }

        Handles.EndGUI();

        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

        //scene view raycasting: assumes camera centre = where to place things, cache camera: place objects to where the camera is pointing
        Transform cameraTransform = sceneView.camera.transform;

        //Ray ray = new Ray(cameraTransform.position, cameraTransform.forward); //help visualise where to scatter all the items

        if (Event.current.type == EventType.MouseMove) //repaints on mouse move
        {
            sceneView.Repaint();
        }

        //only change radius when holding Alt key
        bool holdingAlt = (Event.current.modifiers & EventModifiers.Alt) != 0;

        //change the radius with mouse scrollwheel
        if (Event.current.type == EventType.ScrollWheel && holdingAlt == false)
        {
            //find what direction the user scrolled in but not how much - control that separately
            float scrollDirection = Mathf.Sign(Event.current.delta.y);
            serializedObject.Update(); //update the serialized properties in the editor window
            propRadius.floatValue *= 1f + scrollDirection * radiusIncrementer; //change scroll increment to be a smaller value for percentual increase/decrease
            serializedObject.ApplyModifiedProperties();
            Repaint(); //updates editor window
            Event.current.Use(); //consume the event, don't let it fall through: any other events after this will be event.none
        }

        //check if you have any valid position that the cursor is poseHitting. Shoudn't spawn anything if it's poseHitting outside the surface
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition); //wants the UI mouse position, NOT input.mouseposition

        if (Physics.Raycast(ray, out RaycastHit poseHit))
        {
            //set up a full tangent space coordinate system for the point you poseHit to get full orientation
            //use cross product between up vector & normal rather than forward for predicatable rotation as it's aligned with the camera
            Vector3 poseHitNormalVectorZ = poseHit.normal;
            Vector3 poseHitTangentVectorY = Vector3.Cross(poseHitNormalVectorZ, cameraTransform.up).normalized; //not normalised unless both inputs are normalised & orthogonal
            Vector3 poseHitBitangentVectorX = Vector3.Cross(poseHitNormalVectorZ, poseHitTangentVectorY);

            //create ray for this point given its 2d position
            Ray GetTangentRay(Vector2 tangentSpacePos)
            {
                Vector3 ptWorldPosRayOrigin = poseHit.point + (poseHitTangentVectorY * tangentSpacePos.x + poseHitBitangentVectorX * tangentSpacePos.y) * radius; //scale the position to the radius
                ptWorldPosRayOrigin += poseHitNormalVectorZ * 2; //offset margin distance for the upper projection disc so points move up along a curved surface
                Vector3 rayDirection = -poseHitNormalVectorZ; //negated version of the normal: point in the opposite direction
                return new Ray(ptWorldPosRayOrigin, rayDirection);
            }

            List<Pose> poseHitPts = new List<Pose>();

            //drawing random points: needs to be transformed into a world space position for DrawSphere() as it's in its own tangent space coordinate system
            foreach (RandomPtAngleInstData pt in randomPoints)
            {
                Ray ptRay = GetTangentRay(pt.pointInDisc);
                //raycast to find points to surface
                if (Physics.Raycast(ptRay, out RaycastHit ptposeHit))
                {
                    //calculate rotation & assign to pose together with position
                    float randomAngleDeg = Random.value * 360;
                    Quaternion randomRotation = Quaternion.Euler(0f, 0f, pt.randomAngleDeg); //random rotation around the z axis
                    Quaternion rot = Quaternion.LookRotation(poseHit.normal) * (randomRotation * Quaternion.Euler(90f, 0f, 0f)); //rotate pf +90 degree around x so it has z up - point at the right direction
                    Pose pose = new Pose(ptposeHit.point, rot);
                    poseHitPts.Add(pose);

                    DrawSphere(ptposeHit.point); //draw sphere and normal on surface, disc is around the blue vector on the xy plane. set up the points to draw
                    Handles.DrawAAPolyLine(ptposeHit.point, ptposeHit.point + ptposeHit.normal);

                    //get all mesh in the hierarchy that contains a mesh filter, iterate through mesh filter
                    if (spawnPrefab != null)
                    {
                        Matrix4x4 poseToWorldMatrix = Matrix4x4.TRS(pose.position, pose.rotation, Vector3.one);
                        MeshFilter[] filters = spawnPrefab.GetComponentsInChildren<MeshFilter>();
                        foreach (var filter in filters)
                        {
                            Matrix4x4 childToPose = filter.transform.localToWorldMatrix;
                            Matrix4x4 childToWorldMatrix = poseToWorldMatrix * childToPose; //transform from child to world space
                            Mesh meshShared = filter.sharedMesh; //TODO: Add safety check to see if mesh is null
                            Material materialShared = spawnPrefab.GetComponent<MeshRenderer>().sharedMaterial;
                            materialShared.SetPass(0);
                            Graphics.DrawMeshNow(meshShared, childToWorldMatrix);
                            //mesh asset if the prefab is made of a single mesh
                            Mesh mesh = spawnPrefab.GetComponent<MeshFilter>().sharedMesh;
                            Material mat = spawnPrefab.GetComponent<MeshRenderer>().sharedMaterial;
                            mat.SetPass(0);
                            Graphics.DrawMeshNow(mesh, pose.position, pose.rotation);
                        }
                    }
                }
            }

            //spawn prefabs on press
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
            {
                TrySpawnObjects(poseHitPts);
                //Debug.Log(Event.current.type);
            }

            //mark the area poseHit: draw normal, tangent, bitangent according to their colour convention
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(5, poseHit.point, poseHit.point + poseHitTangentVectorY);
            Handles.color = Color.blue;
            Handles.DrawAAPolyLine(5, poseHit.point, poseHit.point + poseHitNormalVectorZ);
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(5, poseHit.point, poseHit.point + poseHitBitangentVectorX);

            //visualise the radius to scatter objects in
            Handles.color = Color.white;
            //Handles.DrawWireDisc(poseHit.point, poseHit.normal, radius);

            //draw points on the circle adapted to the terrain
            const int circleDetail = 64;
            Vector3[] rayCirclePoints = new Vector3[circleDetail]; //ray array - get all the rays around the perimeter of the circle 

            for (int i = 0; i < circleDetail; i++)
            {
                //-1 because Handles.DrawAAPolyLine will only draw from start point to end point BUT won't conncet them, go back to 0/1 position
                float t = i / ((float) circleDetail - 1);
                const float TAU = 6.28318530718f;
                float angRad = t * TAU;
                Vector3 dir = new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad)); //radius is automatically added by GetTangentRay(Vector2 tangentSpacePos)
                Ray rayCircle = GetTangentRay(dir);
                if (Physics.Raycast(rayCircle, out RaycastHit circleposeHit))
                {
                    rayCirclePoints[i] = circleposeHit.point + circleposeHit.normal * 0.02f; //set to poseHit position and add margin to avoid intersecting with objects
                }
                else //if the ray cast misses, set it to the origin of the ray rather than the previous point - as previous point could actually fail
                {
                    rayCirclePoints[i] = rayCircle.origin;
                }
            }
            Handles.DrawAAPolyLine(rayCirclePoints);
        }
        Handles.DrawAAPolyLine(Vector3.zero, Vector3.one);
    }
}