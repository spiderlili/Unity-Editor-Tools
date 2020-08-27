using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PropPlacementScatterer : EditorWindow
{
    [MenuItem("Tools/Prop Placement Scatterer")]
    public static void OpenPropPlacermentTool() => GetWindow<PropPlacementScatterer>();

    public float radius = 2f;
    public float radiusIncrementer = 0.1f;
    public int spawnCount = 10;

    //boilerplate for undo/redo system
    SerializedProperty propRadius;
    SerializedProperty propRadiusIncrementer;
    SerializedProperty propSpawnCount;
    SerializedObject serializedObject;

    //generate random points in disc
    Vector2[] randomPoints; //just want 2D coordinates within the disk's coordinate system

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        propRadius = serializedObject.FindProperty("radius");
        propRadiusIncrementer = serializedObject.FindProperty("radiusIncrementer");
        propSpawnCount = serializedObject.FindProperty("spawnCount");

        //sign up to an event called in every scene's onGUI event when the window is opened
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI; //unsubscribe from event
        GenerateRandomPoints();
    }

    private void DrawSphere(Vector3 pos) //requries a world space position but
    {
        Handles.SphereHandleCap(-1, pos, Quaternion.identity, 0.1f, EventType.Repaint); //1 repaint event is sent every frame
    }

    private void DuringSceneGUI(SceneView sceneView) //gui for sceneview window: called per scene view you have open: can have multiple scenes open
    {
        //Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

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

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition); //wants the UI mouse position, NOT input.mouseposition

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //set up a full tangent space coordinate system for the point you hit to get full orientation
            //use cross product between up vector & normal rather than forward for predicatable rotation as it's aligned with the camera
            Vector3 hitNormalVectorZ = hit.normal;
            Vector3 hitTangentVectorY = Vector3.Cross(hitNormalVectorZ, cameraTransform.up).normalized; //not normalised unless both inputs are normalised & orthogonal
            Vector3 hitBitangentVectorX = Vector3.Cross(hitNormalVectorZ, hitTangentVectorY);

            //create ray for this point given its 2d position
            Ray GetTangentRay(Vector2 tangentSpacePos)
            {
                Vector3 ptWorldPosRayOrigin = hit.point + (hitTangentVectorY * tangentSpacePos.x + hitBitangentVectorX * tangentSpacePos.y) * radius; //scale the position to the radius
                ptWorldPosRayOrigin += hitNormalVectorZ * 2; //offset margin distance for the upper projection disc so points move up along a curved surface
                Vector3 rayDirection = -hitNormalVectorZ; //negated version of the normal: point in the opposite direction
                return new Ray(ptWorldPosRayOrigin, rayDirection);
            }

            foreach (Vector2 pt in randomPoints) //needs to be transformed into a world space position for DrawSphere() as it's in its own tangent space coordinate system
            {
                Ray ptRay = GetTangentRay(pt);
                //raycast to find points to surface
                if (Physics.Raycast(ptRay, out RaycastHit ptHit))
                {
                    DrawSphere(ptHit.point); //draw sphere and normal on surface, disc is around the blue vector on the xy plane
                    Handles.DrawAAPolyLine(ptHit.point, ptHit.point + ptHit.normal);
                }
            }

            //mark the area hit: draw normal, tangent, bitangent according to their colour convention
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(5, hit.point, hit.point + hitTangentVectorY);
            Handles.color = Color.blue;
            Handles.DrawAAPolyLine(5, hit.point, hit.point + hitNormalVectorZ);
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(5, hit.point, hit.point + hitBitangentVectorX);

            //visualise the radius to scatter objects in
            Handles.color = Color.white;
            //Handles.DrawWireDisc(hit.point, hit.normal, radius);

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
                if (Physics.Raycast(rayCircle, out RaycastHit circleHit))
                {
                    rayCirclePoints[i] = circleHit.point; //set to hit position
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

    void OnGUI() //gui loop for editor window
    {
        serializedObject.Update(); //make serialized property update when parameters changed

        EditorGUILayout.HelpBox("Use scroll wheel to decrease / increase radius, hold Alt + scroll to zoom in the scene view", MessageType.Info); //helper text

        EditorGUILayout.PropertyField(propRadius);
        EditorGUILayout.PropertyField(propRadiusIncrementer);

        propRadius.floatValue = Mathf.Max(1f, propRadius.floatValue); //limit range and prevent negative value
        propRadiusIncrementer.floatValue = Mathf.Max(0.1f, propRadiusIncrementer.floatValue);
        propSpawnCount.intValue = propSpawnCount.intValue.AtLeast(1);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(propSpawnCount);
        if (EditorGUI.EndChangeCheck())
        {
            GenerateRandomPoints(); //update all points every time you change spawncount
        }
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

    void GenerateRandomPoints()
    {
        randomPoints = new Vector2[spawnCount];
        for (int i = 0; i < spawnCount; i++)
        {
            randomPoints[i] = Random.insideUnitCircle; //random points of spawnCount inside a unit circle
        }
    }
}