using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
The Level class follows a Singleton design pattern: the instantiation of the class is restricted to just 1 object. 
you can have access to this object from any other class using the Level.Instance method.
The Level class is a partial class, this means that its content is divided in several files: the Level.cs and Level.Logic.cs scripts. 
This is just to make its manipulation easier
*/
public partial class Level : MonoBehaviour
{
    private static Level instance; //implement singleton
    [SerializeField] private Sprite background;
    [SerializeField] private AudioClip bgm;
    [SerializeField] private float gravity;
    //[LevelCreator.Time]
    [SerializeField] public int totalTime;
    [SerializeField] public Array[] Pieces;
    // indicate the total number of level piece prefabs supported in the x and y axis.
    [SerializeField] private int totalColumns = 25;
    [SerializeField] private int totalRows = 25;

    public const float GridSize = 1.28f; // specifies the size of each cell in the grid.
    private readonly Color _normalColor = Color.grey;
    private readonly Color _selectedColor = Color.yellow;
    public int TotalTime
    {
        get
        {
            return totalTime;
        }
        set
        {
            totalTime = value;
        }
    }

    public float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }

    public AudioClip Bgm
    {
        get { return bgm; }
        set { bgm = value; }
    }

    public Sprite Background
    {
        get { return background; }
        set { background = value; }
    }

    public int TotalColumns
    {
        get { return totalColumns; }
        set { totalColumns = value; }
    }
    public int TotalRows
    {
        get { return totalRows; }
        set { totalRows = value; }
    }
    void LevelPiece()
    {

    }

    //help create the gizmo grid: creates a rectangle with a width of (cols * GridSize) and a height of (rows * GridSize)
    private void GridFrameGizmo(int cols, int rows)
    {
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(0, rows * GridSize, 0));
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(cols * GridSize, 0, 0));
        Gizmos.DrawLine(new Vector3(cols * GridSize, 0, 0),
                        new Vector3(cols * GridSize, rows * GridSize, 0));
        Gizmos.DrawLine(new Vector3(0, rows * GridSize, 0),
                        new Vector3(cols * GridSize, rows * GridSize, 0));
    }

    //using two for loops, creates the vertical and horizontal lines for the grid using Gizmos.DrawLine()
    private void GridGizmo(int cols, int rows)
    {
        for (int i = 1; i < cols; i++)
        {
            Gizmos.DrawLine(new Vector3(i * GridSize, 0, 0), new Vector3(i * GridSize, rows * GridSize, 0));
        }
        for (int j = 1; j < rows; j++)
        {
            Gizmos.DrawLine(new Vector3(0, j * GridSize, 0), new Vector3(cols * GridSize, j * GridSize, 0));
        }
    }

    /*
    If you select the Level game object and change its position: the grid remains in the same place. 
    This is because the points passed to the Gizmo's class methods are actually transformed using the Gizmo's matrix 
    before the grid is painted on the Scene View, but now the identity matrix is used by default.
    solution: change the value of the Gizmos.matrix variable to make the gizmo transform with the game object
    */
    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Matrix4x4 oldMatrix = Gizmos.matrix;

        //Gizmos.matrix is a static variable part of the Gizmos class: 
        //as good practice, always save the original matrix and restore it when you're done using it.
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = _normalColor;
        GridGizmo(totalColumns, totalRows);
        GridFrameGizmo(totalColumns, totalRows);
        Gizmos.color = oldColor;
        Gizmos.matrix = oldMatrix; //replaced the default identity matrix 
    }

    //add a visual feedback when the Level game object is selected: changing the color of the grid frame when that happens
    private void OnDrawGizmosSelected()
    {
        Color oldColor = Gizmos.color;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = _selectedColor;
        GridFrameGizmo(totalColumns, totalRows);
        Gizmos.color = oldColor;
        Gizmos.matrix = oldMatrix;
    }

    //snap to grid behaviour: convert 3D coordinates to 2D grid coordinates and vice versa 
    public Vector3 WorldToGridCoordinates(Vector3 point)
    {
        Vector3 gridPoint = new Vector3
        (
            (int)((point.x - transform.position.x) / GridSize),
            (int)((point.y - transform.position.y) / GridSize),
            0.0f
        );
        return gridPoint;
    }

    public Vector3 GridToWorldCoordinates(int col, int row)
    {
        Vector3 worldPoint = new Vector3
        (
            transform.position.x + (col * GridSize + GridSize / 2.0f),
            transform.position.y + (row * GridSize + GridSize / 2.0f),
            0.0f
        );
        return worldPoint; //returns a Vector3 corresponding to the world coordinates (assuming z = 0)
    }

    //a way to know when if the coordinates are outside the boundaries of the grid
    public bool IsInsideGridBounds(Vector3 point)
    {
        float minX = transform.position.x;
        float maxX = minX + totalColumns * GridSize;
        float minY = transform.position.y;
        float maxY = minY + totalRows * GridSize;
        bool isInsideGridBounds = (point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY);
        return isInsideGridBounds;
    }

    //alternative way: instead of a vector, receives a grid coordinate (col, row)
    public bool IsInsideGridBounds(int col, int row)
    {
        return (col >= 0 && col < totalColumns && row >= 0 && row < totalRows);
    }

    //implement singleton Instance
    public static Level Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Level>();
            }

            return instance;
        }
    }
}
