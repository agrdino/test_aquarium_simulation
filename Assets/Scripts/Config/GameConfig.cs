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
        
        public int StartCoin => _startCoin;
        
        public FishConfig GetFishConfig(int fishID)
        {
            return _fishConfigs[fishID];
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