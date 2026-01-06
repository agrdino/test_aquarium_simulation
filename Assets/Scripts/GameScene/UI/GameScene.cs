using System;
using Config;
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
        
        //pnl tank infor
        [Space]
        [SerializeField] private TextMeshProUGUI _txtCoin;
        [SerializeField] private TextMeshProUGUI _txtTankLevel;
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
            //Create tank
            int tankId = 1;
            TankConfig tankConfig = GameConfig.Instance.GetTankConfig(tankId);
            _tankController = Instantiate(GameConfig.Instance.GetTankConfig(tankId).prefab).GetComponent<TankController>();
            _tankController.OnCoinChanged += OnCoinChanged;
            _tankController.OnTankLevelUp += OnTankLevelUp;
            _tankController.OnNewFishSpawned += OnFishChanged;
            _tankController.OnReceiveExp += OnReceiveExp;
            
            _tankController.InitStat(GameConfig.Instance.StartCoin, tankConfig.tankCapacity, 1);
        }

        #endregion

        #region ----- Event -----

        private void OnClickButtonTank()
        {
            //show upgrade
            throw new NotImplementedException();
        }

        private void OnClickButtonDecor()
        {
            //show decor market scroller
            throw new NotImplementedException();
        }

        private void OnClickButtonFish()
        {
            //show fish market scroller

            Debug.LogError(_tankController.SpawnFish(1));
        }

        private void OnCoinChanged(float coin)
        {
            _txtCoin.SetText(coin.ToString("N0"));
        }

        private void OnTankLevelUp(int level, int maxExp)
        {
            _txtTankLevel.SetText($"Level {level}");
            _sldLevelProgress.maxValue = maxExp;
        }

        private void OnFishChanged(int fishCount, int tankCapacity)
        {
            _txtTankCapacity.SetText($"{fishCount}/{tankCapacity}");
        }

        private void OnReceiveExp(int exp)
        {
            _sldLevelProgress.value = exp;
        }

        #endregion

        #region ----- Callback -----

        private void SpawnFish(int fishID)
        {
            
        }

        #endregion
    }
}
