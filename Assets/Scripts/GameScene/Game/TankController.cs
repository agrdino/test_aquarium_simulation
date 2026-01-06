using System;
using System.Collections.Generic;
using Config;
using Data;
using Scripts.Helper;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameScene.Game
{
    public partial class TankController : Singleton<TankController>
    {
        //Stat
        private int _fishCount;
        private int _tankCapacity;
        private int _tankLevel;
        
        private TankConfig _tankConfig;


        public int FishCount
        {
            get => _fishCount;
            set
            {
                OnNewFishSpawned?.Invoke(value, _tankCapacity);
                _fishCount = value;
            }
        }

        public int TankCapacity => _tankCapacity;

        public int TankLevelUpPrice => _tankConfig.levelUpCost;

        public int TankLevel
        {
            get  => _tankLevel;
            set
            {
                _tankLevel = value;
                _tankCapacity = (int)(_tankConfig.tankCapacity * Mathf.Pow(_tankLevel, 1.5f));
                OnTankLevelUp?.Invoke(value);
            }
        }
        
        public int TankMaxLevel => _tankConfig.maxLevel;

        private int _coin
        {
            get => UserData.Instance.Coin;
            set => UserData.Instance.Coin = value;
        }

        private int _userExp
        {
            get => UserData.Instance.UserExp;
            set => UserData.Instance.UserExp = value;
        }
        
        //Event
        public event Action<int, int> OnNewFishSpawned;
        public event Action<int> OnTankLevelUp;
        
        //Limit
        [SerializeField] private Transform _limitLeft, _limitRight, _limitUp, _limitDown;
        
        public void InitStat(int startFish, TankConfig tankConfig, int tankLevel)
        {
            _fishes = new List<Fish>();
            _fishMoveData = new(Allocator.Persistent);

            _tankConfig = tankConfig;
            TankLevel = tankLevel;
            
            int[] unlockFish = UserData.Instance.UnlockFish;
            for (int i = 0; i < startFish; i++)
            {
                int randomFish = unlockFish[Random.Range(0, unlockFish.Length)];
                FishConfig fishConfig = GameConfig.Instance.GetFishConfig(randomFish);
                SpawnFish(fishConfig);
            }
        }
    }
}