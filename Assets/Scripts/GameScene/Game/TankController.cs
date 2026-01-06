using System;
using Config;
using Scripts.Helper;
using UnityEngine;

namespace GameScene.Game
{
    public partial class TankController : Singleton<TankController>
    {
        //Stat
        private float _coin;
        private int _fishCount;
        private int _tankCapacity;
        private int _level;
        private int _exp;
        
        private LevelConfig _currentLevelConfig;

        public float Coin
        {
            get => _coin;
            set 
            {
                OnCoinChanged?.Invoke(value);
                _coin = value;
            }
        }

        public int FishCount
        {
            get => _fishCount;
            set
            {
                OnNewFishSpawned?.Invoke(value, _tankCapacity);
                _fishCount = value;
            }
        }

        public int TankCapacity
        {
            get => _tankCapacity;
            set
            {
                _tankCapacity = value;
            }
        }

        public int Level
        {
            get  => _level;
            set
            {
                OnTankLevelUp?.Invoke(value, _currentLevelConfig.targetExp);
                _level = value;
            }
        }

        public int Exp
        {
            get => _exp;
            set
            {
                _exp = value;

                if (_exp >= _currentLevelConfig.targetExp)
                {
                    _exp -= _currentLevelConfig.targetExp;
                    
                    LevelConfig nextLevelConfig = GameConfig.Instance.GetLevelConfig(_level + 1);
                    if (nextLevelConfig != null)
                    {
                        _currentLevelConfig = nextLevelConfig;
                        Level += 1;
                    }
                }
                
                OnReceiveExp?.Invoke(_exp);
            }
        }
        
        //Event
        public event Action<float> OnCoinChanged;
        public event Action<int, int> OnNewFishSpawned;
        public event Action<int, int> OnTankLevelUp;
        public event Action<int> OnReceiveExp;
        
        //Limit
        [SerializeField] private Transform _limitLeft, _limitRight, _limitUp, _limitDown;
        
        public void InitStat(int coin, int tankCapacity, int level)
        {
            Coin = coin;
            TankCapacity = tankCapacity;
            _currentLevelConfig = GameConfig.Instance.GetLevelConfig(level);
            
            Level = level; 
            Exp = 0;
        }
    }
}