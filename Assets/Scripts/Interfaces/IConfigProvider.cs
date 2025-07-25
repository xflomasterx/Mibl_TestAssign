using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConfigProvider 
{
    LayoutConfig GetConfig(int levelIndex);
}
