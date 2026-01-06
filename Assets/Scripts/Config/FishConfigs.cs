using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "FishConfig", menuName = "Config/FishConfig")]
    public class FishConfigs : ScriptableObject
    {
        [SerializeField] private List<FishConfig> _fishConfigs = new List<FishConfig>();
        
        public FishConfig this[int fishID] => _fishConfigs.Find(x => x.id == fishID);
        public List<FishConfig> GetFishConfigs() => _fishConfigs;
    }

    [Serializable]
    public class FishConfig : ItemConfig
    {
        [SerializeField] private int _exp;
        [SerializeField] private float _incomeInterval;
        [SerializeField] private int _income;

        public int exp => _exp;
        public int income => _income;
        public float incomeInterval => _incomeInterval;
    } 
}