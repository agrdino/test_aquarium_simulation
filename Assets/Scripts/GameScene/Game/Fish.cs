using Config;
using Scripts.Helper.Pooling;
using UnityEngine;

namespace GameScene.Game
{
    public class Fish : ObjectPooling
    {
        private FishConfig _config;
        private float _nextTimeIncomeReady;

        public int ID => _config.id;
        public int Income => _config.income;
        public float InComeInterval => _config.incomeInterval;
        public float NextTimeIncomeReady => _nextTimeIncomeReady;
        
        #region ----- Public Function -----

        public void InitStat(FishConfig config)
        {
            _config = config;
        }

        public void SetSide(Vector3 targetPosition)
        {
            transform.right = targetPosition.x >= transform.position.x ? Vector3.right : Vector3.left;
        }
        
        public void UpdatePosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public void ResetIncomeTimer(float gameTimer)
        {
            _nextTimeIncomeReady = gameTimer + InComeInterval;
        }

        #endregion
    }
}