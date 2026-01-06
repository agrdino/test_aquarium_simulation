using EnhancedUI.EnhancedScroller;
using UnityEngine;

namespace GameScene.UI
{
    public class MarketScroller : MonoBehaviour, IEnhancedScrollerDelegate 
    {
        [SerializeField] private EnhancedScroller _scroller;
        private void Awake()
        {
            _scroller.Delegate = this;
        }
        
        public void ShowItem(){}

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            throw new System.NotImplementedException();
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            throw new System.NotImplementedException();
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}