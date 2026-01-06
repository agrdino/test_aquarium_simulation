using System;
using Scripts.Helper.Pooling;
using UnityEngine;

namespace GameScene.Game
{
    public class Fish : ObjectPooling
    {
        private int _id;

        public int ID => _id;
        
        #region ----- Public Function -----

        public void InitStat(int id)
        {
            _id = id;
        }

        public void SetSide(Vector3 targetPosition)
        {
            transform.right = targetPosition.x >= transform.position.x ? Vector3.right : Vector3.left;
        }
        
        public void UpdatePosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        #endregion
    }
}