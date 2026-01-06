using System;
using System.Linq;
using Config;
using Scripts.Helper;

namespace Data
{
    public class UserData : Singleton<UserData>
    {
        private int _coin;
        
        private int _userLevel = 1;
        private int _userExp = 0;
        private LevelConfig _currentLevelConfig;

        private int[] _unlockFish;
        
        public event Action<int> OnUserLevelUp;
        public event Action<int, int> OnReceiveExp;
        public event Action<int> OnCoinChanged;
        
        public int[] UnlockFish => _unlockFish;

        public int Coin
        {
            get => _coin;
            set
            {
                _coin = value;
                OnCoinChanged?.Invoke(_coin);
            }
        }
        
        public int UserLevel
        {
            get => _userLevel;
            set 
            {
                _userLevel = value;
                _currentLevelConfig = GameConfig.Instance.GetLevelConfig(_userLevel);
                _unlockFish = GameConfig.Instance.GetFishConfigs().Where(x => x.unlockLevel <= _userLevel).Select(x => x.id).ToArray();
                OnUserLevelUp?.Invoke(_userLevel);
            }
        }

        public int UserExp
        {
            get => _userExp;
            set
            {
                _userExp = value;

                if (_userExp >= _currentLevelConfig.targetExp)
                {
                    LevelConfig nextLevelConfig = GameConfig.Instance.GetLevelConfig(_userLevel + 1);
                    if (nextLevelConfig != null)
                    {
                        _userExp -= _currentLevelConfig.targetExp;
                        _currentLevelConfig = nextLevelConfig;
                        UserLevel += 1;
                    }
                }
                
                OnReceiveExp?.Invoke(_userExp, _currentLevelConfig.targetExp);
            }
        }

        private void Start()
        {
            Coin = GameConfig.Instance.StartCoin;
            UserLevel = 1;
        }
    }
}