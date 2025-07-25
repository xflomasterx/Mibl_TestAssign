using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
[System.Serializable]
public class NeighborRow3
{
    public bool left;
    public bool center;
    public bool right;
    public ref bool this[int index]
    {
        get
        {
            if (index == 0) return ref left;
            if (index == 1) return ref center;
            if (index == 2) return ref right;
            throw new IndexOutOfRangeException();
        }
    }
    public NeighborRow3(bool [] values)
    {
        if(values == null)
            throw new SystemException($"NeighborRow3 instance constructor failed: init values array is null");
        if (values.Length != 3)
            throw new SystemException($"NeighborRow3 instance constructor failed: expected init values array length 3, recived: {values.Length}");
        for (int i = 0; i < 3; i++)
            this[i] = values[i];
    }
}

[System.Serializable]
public class AdjacencyMatrix
{
    public NeighborRow3 top;
    public NeighborRow3 middle;
    public NeighborRow3 bottom;
    public ref NeighborRow3 this[int index]
    {
        get
        {
            if (index == 0) return ref top;
            if (index == 1) return ref middle;
            if (index == 2) return ref bottom;
            throw new IndexOutOfRangeException();
        }
    }
    public static bool operator ==(AdjacencyMatrix a, AdjacencyMatrix b)
    {

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (a[i][j] != b[i][j])
                    return false;
        return true;
    }
    public static bool operator !=(AdjacencyMatrix a, AdjacencyMatrix b)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (a[i][j] == b[i][j])
                    return false;
        return true;
    }
    public AdjacencyMatrix(bool[,] values)
    {
        if (values == null)
            throw new SystemException($"AdjacencyMatrix instance constructor failed: init values array is null");
        if (values.GetLength(0) != 3 || values.GetLength(1)!= 3)
            throw new SystemException($"AdjacencyMatrix instance constructor failed: expected init values matrix with size 3x3, recived: {values.GetLength(0)}x{values.GetLength(1)}");

        for (int i = 0; i < 3; i++)
        {
            bool[] tmp = new bool[3];
            for (int j = 0; j < 3; j++)
                tmp[j] = values[i, j];
            this[i] = new NeighborRow3(tmp);
        }
    }
}

[CreateAssetMenu(menuName = "MapGen/TileAdjacencyVariant")]
public class TileAdjacencyVariant : ScriptableObject
{
    [SerializeField] private TileBase tileToPlace;
    [SerializeField] private AdjacencyMatrix adjacencyMatrix;
    public TileBase TileToPlace { get => tileToPlace; set => tileToPlace = value; }

    public AdjacencyMatrix AdjacencyMatrix()
    {
        return adjacencyMatrix;
    }
}
