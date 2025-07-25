using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Configs/LayoutConfig")]
public class LayoutConfig : ScriptableObject
{
    public static int minHeight = 4;
    public static int maxHeightStep = 2;
    [Header("Basic")]
    public int height;
    public int width;
    [Header("Bottom layer")]
    public int floorSectionLendthMin;
    public int floorSectionLendthMax;
    [Header("Mid-air tiles")]
    public int platformLendthMin;
    public int platformLendthMax;
    public int platformGapMin;
    public int platformGapMax;
    [Header("Enemies")]
    public List<EnemyPopulation> populations;
}
