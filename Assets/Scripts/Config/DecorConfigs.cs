using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "DecorConfig", menuName = "Config/DecorConfig")]
    public class DecorConfigs : ScriptableObject
    {
        [SerializeField] private List<DecorConfig> _decorConfigs = new List<DecorConfig>();
        
        public DecorConfig this[int decorID] => _decorConfigs.Find(x => x.id == decorID);
    }

    [Serializable]
    public class DecorConfig : ItemConfig
    {
    } 
}