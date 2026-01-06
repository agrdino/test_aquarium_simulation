using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "LevelConfigs", menuName = "Config/LevelConfigs")]
    public class LevelConfigs : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> _levelConfigs = new();
        
        public LevelConfig this[int level] => _levelConfigs.Find(x => x.level == level);
    }

    [Serializable]
    public class LevelConfig
    {
        [SerializeField] private int _level;
        [SerializeField] private int _targetExp;
        
        public int level => _level;
        public int targetExp => _targetExp;
    }
}