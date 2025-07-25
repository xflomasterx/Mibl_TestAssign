using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelRenderer : MonoBehaviour, ILevelRenderer
{
    [SerializeField] TileCollection _tileCollection;
    [SerializeField] Tilemap _tilemap;
    [SerializeField] GameObject _lvlMask;
    bool[,] _cachedLayout;

    public void Render(bool[,] layout)
    {
        _cachedLayout = layout;
        for (int i = 0; i < _cachedLayout.GetLength(0); i++)
            for (int j = 0; j < _cachedLayout.GetLength(1); j++)
                if (layout[i, j])
                {
                    Vector3Int position = new Vector3Int(i, j, 0);
                    AdjacencyMatrix localAreaMatrix = snapLocalGrid(i,j);
                    TileBase tile = _tileCollection.FindTile(localAreaMatrix);
                    _tilemap.SetTile(position, tile);
                }
        _lvlMask.transform.localScale = new Vector3((float)layout.GetLength(0)-0.1f, (float)layout.GetLength(1)-0.1f, 0f);
        _lvlMask.transform.position = new Vector3((float)layout.GetLength(0)/2f, (float)layout.GetLength(1)/2f, 0f);
    }
    AdjacencyMatrix snapLocalGrid(int i, int j)
    {
        if (i < 0 || j < 0 || i >= _cachedLayout.GetLength(0) || j >= _cachedLayout.GetLength(1))
            return null;
        bool[,] localArea = new bool[3, 3];
        for (int ilocal = 0; ilocal < 3; ilocal++)
            for (int jlocal = 0; jlocal < 3; jlocal++)
                if ((ilocal + i - 1) < 0 || (jlocal + j - 1) < 0 || (ilocal + i - 1) >= _cachedLayout.GetLength(0) || (jlocal + j - 1) >= _cachedLayout.GetLength(1))
                    localArea[2 - jlocal, ilocal] = true;
                else

                    localArea[2 - jlocal, ilocal] = _cachedLayout[i + ilocal - 1, j + jlocal - 1];
        AdjacencyMatrix localMatrix = new AdjacencyMatrix(localArea);
        return localMatrix;
    }
    public void ClearRender()
    {
        for (int i = 0; i < _cachedLayout.GetLength(0); i++)
            for (int j = 0; j < _cachedLayout.GetLength(1); j++)
            {
                Vector3Int position = new Vector3Int(i, j, 0);
                _tilemap.SetTile(position, null);
            }
    }
}
