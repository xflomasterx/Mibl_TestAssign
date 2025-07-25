using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelRenderer 
{
    void Render(bool[,] layout);
    void ClearRender();
}
