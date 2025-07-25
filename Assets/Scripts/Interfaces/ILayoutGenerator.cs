using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILayoutGenerator
{
    LayoutConfig Config { get; }
    bool[,] Layout { get; }
    bool[,] Generate();  
    void Deconstruct();
    bool ChangeLevel(int levelIndex);
}