using System;
using System.Collections.Generic;
using Scripts.Helper.Pooling;
using UnityEngine;

namespace Fx
{
    public class FxPooling : SystemPooling<Fx>
    {
        [SerializeField] private List<FxAsset> _fxAssets;
        private FxAsset this[EFxType itemID] => _fxAssets.Find(x => x.fxType == itemID);
        
        public override Fx Get(int itemID)
        {
            if (!_pool.ContainsKey(itemID))
            {
                _pool.Add(itemID, new ());
            }
            
            List<Fx> pool = _pool[itemID];
            for (var i = 0; i < pool.Count; i++)
            {
                if (!pool[i].IsRelease())
                {
                    continue;
                }

                pool[i].Take();
                return pool[i];
            }

            Fx newObject = Instantiate(this[(EFxType)itemID].prefab, transform).GetComponent<Fx>();
            pool.Add(newObject);
            newObject.Take();
            
            return newObject;
        }
    }

    [Serializable]
    public class FxAsset
    {
        [SerializeField] private EFxType _fxType;
        [SerializeField] private GameObject _prefab;
        
        public EFxType fxType =>  _fxType;
        public GameObject prefab => _prefab;
    }
}