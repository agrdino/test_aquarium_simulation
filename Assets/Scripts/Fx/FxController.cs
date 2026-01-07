using Scripts.Helper;
using UnityEngine;

namespace Fx
{
    public class FxController : Singleton<FxController>
    {
        public void ShowCollectCoinFx(Vector3 position)
        {
            Fx fx = FxPooling.Instance.Get((int)EFxType.Coin);
            fx.transform.position = position + - 0.1f * Vector3.forward;
            fx.gameObject.SetActive(true);
        }
    }
}