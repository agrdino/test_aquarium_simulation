using System;
using System.Collections.Generic;
using Config;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.UI
{
    public class FishMarketScroller : MonoBehaviour, IEnhancedScrollerDelegate
    {
        [SerializeField] private Button _btnClose;
        
        [Space]
        [SerializeField] private Button _btnRandom;
        
        [Space]
        [SerializeField] private EnhancedScroller _scroller;
        [SerializeField] private MarketItem _marketItemPrefab;
        
        private Action _onRandomFish;
        private Action<int> _onSelectFish;
        private List<(FishConfig fishConfig, int unlockLevel)> _fishData;
        
        private void Awake()
        {
            _scroller.Delegate = this;
        }

        private void Start()
        {
            _btnRandom.onClick.AddListener(OnClickRandomFish);
            _btnClose.onClick.AddListener(OnClickClose);
        }

        public void ShowItem(List<(FishConfig fishConfig, int unlockLevel)> fishData, Action onRandomFish,
            Action<int> onSelectFish)
        {
            _fishData = fishData;
            _onRandomFish = onRandomFish;
            _onSelectFish = onSelectFish;
            
            _scroller.ReloadData();
        }

        private void UpdateData(List<(FishConfig fishConfig, int unlockLevel)> fishData)
        {
            _fishData = fishData;

            for (var i = 0; i < _fishData.Count; i++)
            {
                (_scroller.GetCellViewAtDataIndex(i) as MarketItem).ShowItem(_fishData[i].fishConfig, _fishData[i].unlockLevel, _onSelectFish);
            }
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _fishData.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 160;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            MarketItem item = _scroller.GetCellView(_marketItemPrefab) as MarketItem;
            item.ShowItem(_fishData[dataIndex].fishConfig, _fishData[dataIndex].unlockLevel, _onSelectFish);
            return item;
        }

        private void OnClickRandomFish()
        {
            _onRandomFish?.Invoke();
        }

        private void OnClickClose()
        {
            gameObject.SetActive(false);
        }
    }
}