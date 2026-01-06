using System;
using Config;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.UI
{
    public class MarketItem : EnhancedScrollerCellView
    {
        [SerializeField] private Button _btnSelect;
        [SerializeField] private Image _imgAvatar;

        [SerializeField] private GameObject _pnlUnlockLevel;
        [SerializeField] private TextMeshProUGUI _txtUnlockLevel;
        [SerializeField] private TextMeshProUGUI _txtPrice;
        
        private int _itemID;
        private Action<int> _onSelectItem;

        private void Start()
        {
            _btnSelect.onClick.AddListener(OnSelectItem);
        }

        public void ShowItem(ItemConfig itemConfig, int unlockLevel, Action<int> onSelectItem)
        {
            _itemID = itemConfig.id;
            _onSelectItem = onSelectItem;
            
            if (unlockLevel != 0)
            {
                _pnlUnlockLevel.SetActive(true);
                _txtUnlockLevel.SetText($"Unlock at level {unlockLevel}");
                _btnSelect.enabled = false;
            }
            else
            {
                _btnSelect.enabled = true;
                _pnlUnlockLevel.SetActive(false);
            }
            
            _txtPrice.SetText($"${itemConfig.price}");
            _imgAvatar.sprite = itemConfig.avatar;
        }

        private void OnSelectItem()
        {
            _onSelectItem?.Invoke(_itemID);
        }
    }
}