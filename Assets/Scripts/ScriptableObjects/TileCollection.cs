using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "MapGen/TileCollection")]
public class TileCollection : ScriptableObject
{
    [SerializeField] List<TileAdjacencyVariant> variants;
    [SerializeField] TileAdjacencyVariant defaultTile;
    public TileBase FindTile(AdjacencyMatrix neighborsMatrix)
    {
        foreach (TileAdjacencyVariant tileCandidate in variants)
        {
            if (tileCandidate.AdjacencyMatrix() == neighborsMatrix)
                return tileCandidate.TileToPlace;
        }
       return defaultTile.TileToPlace;
    }
}
