using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class LinearLayoutGenerator :ILayoutGenerator
{
    private LayoutConfig _config;
    private bool[,] _layout;
    private int currentLevelIndex = -1;
    private readonly IConfigProvider _configProvider;

    public LayoutConfig Config { get => _config; private set { _config = value; Deconstruct(); } }

    public bool[,] Layout { get => _layout; private set => _layout = value; }

    [Inject]
    public LinearLayoutGenerator(IConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }
    public bool ChangeLevel(int levelIndex)
    {
        if (levelIndex != currentLevelIndex)
        {
            LayoutConfig newConfig = _configProvider.GetConfig(levelIndex);
            if (newConfig == null)
            {
                Debug.LogError($"[LayoutGenerator] No config found for level {levelIndex}");
                return false;
            }
            Config = newConfig;
            currentLevelIndex = levelIndex;
        }
        return true;
    }
    public bool[,] Generate()
    {
        int[] _floorHeightMap = new int[_config.width];
        //fulfil layout frame
        for (int i = 0; i < _config.width; i++)
        {
            _layout[i, 0] = true;
            _layout[i, _config.height - 1] = true;
        }
        for (int j = 1; j < _config.height - 1; j++)
        {
            _layout[0, j] = true;
            _layout[_config.width - 1, j] = true;
        }
        //fulfil floor
        int lastSectionHeight = Random.Range(0, _config.height - LayoutConfig.minHeight);
        for (int i = 1; i < _config.width-1;)
        {
            int floorSectionLendth = Mathf.Min(Random.Range(_config.floorSectionLendthMin, _config.floorSectionLendthMax), _config.width - i-1);
            for (int iFloorSection = 0; iFloorSection < floorSectionLendth; iFloorSection++)
            {
                for (int j = 0; j < lastSectionHeight; j++)
                    _layout[i + iFloorSection, j] = true;
                _floorHeightMap[i + iFloorSection] = lastSectionHeight;
            }
            i += floorSectionLendth;
            lastSectionHeight = Random.Range(0, Mathf.Min(_config.height - LayoutConfig.minHeight, lastSectionHeight + LayoutConfig.maxHeightStep));
        }
        //place platforms
        for (int i = 4; i < _config.width-1;)// start from 4 to leave space for initial player placing
        {
            int platformLendth = Mathf.Min(Random.Range(_config.platformLendthMin, _config.platformLendthMax), _config.width - i-1);
            int platformElevation = Random.Range(_floorHeightMap[i] + 2, _config.height - 2);
            for (int iPlatformSection = 0; iPlatformSection < platformLendth && _floorHeightMap[i+iPlatformSection]>= _floorHeightMap[i + iPlatformSection+1]; iPlatformSection++)
            {
                    _layout[i + iPlatformSection, platformElevation] = true;
            }
            i += platformLendth;
            i += Random.Range(_config.platformGapMin, _config.platformGapMax);
        }
        return _layout;
    }
    public void Deconstruct()
    {
        _layout = new bool[_config.width, _config.height];
    }   
}
