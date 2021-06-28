﻿using System;
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
    [SerializeField] private int totalColumns;
    [SerializeField] private int totalRows;

    public const float GridSize = 1.28f; // specifies the size of each cell in the grid.
    private readonly Color _normalColor = Color.grey; private readonly Color _selectedColor = Color.yellow;
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LevelPiece()
    {

    }
}
