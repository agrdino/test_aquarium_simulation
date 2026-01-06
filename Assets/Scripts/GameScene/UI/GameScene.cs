using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Data;
using GameScene.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.UI
{
    public class GameScene : MonoBehaviour
    {
        #region ----- Component Config -----

        //pnl controller
        [SerializeField] private Button _btnFish;
        [SerializeField] private Button _btnDecor;
        [SerializeField] private Button _btnTank;
        [SerializeField] private TextMeshProUGUI _txtTankLevel;
        [SerializeField] private TextMeshProUGUI _txtLevelUpTankPrice;

        [SerializeField] private FishMarketScroller _fishMarketScroller;
        
        //pnl tank infor
        [Space]
        [SerializeField] private TextMeshProUGUI _txtCoin;
        [SerializeField] private TextMeshProUGUI _txtUserLevel;
        [SerializeField] private Slider _sldLevelProgress;
        [SerializeField] private TextMeshProUGUI _txtTankCapacity;
        
        //Scroller

        private TankController _tankController;
        
        #endregion

        #region ----- Unity Event -----

        private void Awake()
        {
            _btnFish.onClick.AddListener(OnClickButtonFish);
            _btnDecor.onClick.AddListener(OnClickButtonDecor);
            _btnTank.onClick.AddListener(OnClickButtonTank);
        }

        private void Start()
        {
            UserData.Instance.OnCoinChanged += OnCoinChanged;
            UserData.Instance.OnReceiveExp += OnReceiveExp;
            UserData.Instance.OnUserLevelUp += OnUserLevelUp;
            
            //Create tank
            int tankId = 1;
            TankConfig tankConfig = GameConfig.Instance.GetTankConfig(tankId);
            _tankController = Instantiate(GameConfig.Instance.GetTankConfig(tankId).prefab).GetComponent<TankController>();
            _tankController.OnTankLevelUp += OnTankLevelUp;
            _tankController.OnNewFishSpawned += OnFishChanged;
            
            _tankController.InitStat(GameConfig.Instance.StartFish, tankConfig, 1);
        }

        #endregion

        #region ----- Event -----

        private void OnClickButtonTank()
        {
            _tankController.LevelUpTank();
        }

        private void OnClickButtonDecor()
        {
            //show decor market scroller
            throw new NotImplementedException();
        }

        private void OnClickButtonFish()
        {
            //show fish market scroller
            _fishMarketScroller.gameObject.SetActive(true);

            int[] unlockFish = UserData.Instance.UnlockFish;
            List<FishConfig> fishConfigs = GameConfig.Instance.GetFishConfigs();

            List<(FishConfig fishConfig, int unlockLevel)> fishData = fishConfigs.Select(x =>
            {
                return (x, unlockFish.Contains(x.id) ? 0 : x.unlockLevel);
            }).ToList();
            
            _fishMarketScroller.ShowItem(fishData, RandomFish, BuyFish);
        }

        private void OnCoinChanged(int coin)
        {
            _txtCoin.SetText(coin.ToString());
        }

        private void OnTankLevelUp(int level)
        {
            _txtTankCapacity.SetText($"{_tankController.FishCount}/{_tankController.TankCapacity}");
            if (_tankController.TankLevel < _tankController.TankMaxLevel)
            {
                _txtTankLevel.SetText($"Tank level {_tankController.TankLevel}");
            }
            else
            {
                _txtTankLevel.SetText($"Tank level MAX");
            }
            _txtLevelUpTankPrice.SetText($"<size=50>Level up</size>\n${_tankController.TankLevelUpPrice}");
        }

        private void OnFishChanged(int fishCount, int tankCapacity)
        {
            _txtTankCapacity.SetText($"{fishCount}/{tankCapacity}");
        }

        private void OnReceiveExp(int exp, int targetExp)
        {
            _sldLevelProgress.maxValue = targetExp;
            _sldLevelProgress.value = exp;
        }

        private void OnUserLevelUp(int level)
        {
            _txtUserLevel.SetText($"Level {level}");

            if (_fishMarketScroller.gameObject.activeInHierarchy)
            {
                int[] unlockFish = UserData.Instance.UnlockFish;
                List<FishConfig> fishConfigs = GameConfig.Instance.GetFishConfigs();

                List<(FishConfig fishConfig, int unlockLevel)> fishData = fishConfigs.Select(x =>
                {
                    return (x, unlockFish.Contains(x.id) ? 0 : x.unlockLevel);
                }).ToList();
            
                _fishMarketScroller.ShowItem(fishData, RandomFish, BuyFish);
            }
        }

        #endregion

        #region ----- Callback -----

        private void RandomFish()
        {
            _tankController.RandomFish();
        }

        private void BuyFish(int fishID)
        {
            _tankController.BuyFish(fishID);
        }

        #endregion
    }
}
