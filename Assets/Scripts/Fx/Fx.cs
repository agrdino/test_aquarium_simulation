using DG.Tweening;
using Scripts.Helper.Pooling;

namespace Fx
{
    public abstract class Fx : ObjectPooling
    {
        protected Sequence _seqAnim;
        

        private void OnDisable()
        {
            _seqAnim?.Kill();
        }
    }
}