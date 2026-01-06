using System.Collections.Generic;
using Config;
using Scripts.Helper.Pooling;

namespace GameScene.Game
{
    public class DecorPooling : SystemPooling<Decor>
    {
        public override Decor Get(int itemID)
        {
            if (!_pool.ContainsKey(itemID))
            {
                _pool.Add(itemID, new ());
            }
            List<Decor> pool = _pool[itemID];
            for (var i = 0; i < pool.Count; i++)
            {
                if (!pool[i].IsRelease())
                {
                    continue;
                }

                pool[i].Take();
                return pool[i];
            }

            Decor newObject = Instantiate(GameConfig.Instance.GetDecorConfig(itemID).prefab, transform).GetComponent<Decor>();
            pool.Add(newObject);
            newObject.Take();
            
            return newObject;
        }
    }
}