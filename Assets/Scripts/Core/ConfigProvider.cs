using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigProvider : MonoBehaviour, IConfigProvider
{
    [SerializeField] List<LayoutConfig> _levelConfigs;
    public LayoutConfig GetConfig(int levelIndex)
    {
        return _levelConfigs[levelIndex];
    }
}
