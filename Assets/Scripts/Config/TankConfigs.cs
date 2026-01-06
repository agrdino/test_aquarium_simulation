using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "TankConfig", menuName = "Config/TankConfig")]
    public class TankConfigs : ScriptableObject
    {
        [SerializeField] private List<TankConfig>  _tankConfigs = new List<TankConfig>();
        public TankConfig this[int decorID] => _tankConfigs.Find(x => x.id == decorID);
    }

    [Serializable]
    public class TankConfig : ItemConfig
    {
        public int width;
        public int tankCapacity;
        public int levelUpCost;
        public int maxLevel;
    }
}