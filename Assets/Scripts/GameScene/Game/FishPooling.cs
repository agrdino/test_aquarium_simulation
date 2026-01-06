using System.Collections.Generic;
using Config;
using Scripts.Helper.Pooling;

namespace GameScene.Game
{
    public class FishPooling : SystemPooling<Fish>
    {
        public override Fish Get(int itemID)
        {
            if (!_pool.ContainsKey(itemID))
            {
                _pool.Add(itemID, new ());
            }
            List<Fish> pool = _pool[itemID];
            for (var i = 0; i < pool.Count; i++)
            {
                if (!pool[i].IsRelease())
                {
                    continue;
                }

                pool[i].Take();
                return pool[i];
            }

            Fish newObject = Instantiate(GameConfig.Instance.GetFishConfig(itemID).prefab, transform).GetComponent<Fish>();
            pool.Add(newObject);
            newObject.Take();
            
            return newObject;
        }
    }
}