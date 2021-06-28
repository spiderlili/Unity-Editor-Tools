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
    // Start is called before the first frame update
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

    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = _normalColor;
        GridGizmo(totalColumns, totalRows);
        GridFrameGizmo(totalColumns, totalRows);
        Gizmos.color = oldColor;
    }

    //add a visual feedback when the Level game object is selected: changing the color of the grid frame when that happens
    private void OnDrawGizmosSelected()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = _selectedColor;
        GridFrameGizmo(totalColumns, totalRows);
        Gizmos.color = oldColor;
    }
}
