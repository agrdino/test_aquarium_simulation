using System.Collections.Generic;
using Scripts.Helper;
using UnityEngine;

namespace Config
{
    public class GameConfig : Singleton<GameConfig>
    {
        [SerializeField] private FishConfigs _fishConfigs;
        [SerializeField] private DecorConfigs _decorConfigs;
        [SerializeField] private TankConfigs _tankConfigs;
        [SerializeField] private LevelConfigs _levelConfigs;

        [Space] 
        [SerializeField] private int _startCoin;
        [SerializeField] private int _startFish;
        
        public int StartCoin => _startCoin;
        public int StartFish => _startFish;
        
        public FishConfig GetFishConfig(int fishID)
        {
            return _fishConfigs[fishID];
        }

        public List<FishConfig> GetFishConfigs()
        {
            return _fishConfigs.GetFishConfigs();
        }

        public DecorConfig GetDecorConfig(int decorID)
        {
            return _decorConfigs[decorID];
        }

        public TankConfig GetTankConfig(int tankID)
        {
            return _tankConfigs[tankID];
        }

        public LevelConfig GetLevelConfig(int level)
        {
            return _levelConfigs[level];
        }
    }
}