using DG.Tweening;
using UnityEngine;

namespace Fx
{
    public class FxCoin : Fx
    {
        [SerializeField] private Renderer[] _renderers;
        
        private void OnEnable()
        {
            if (_seqAnim != null && _seqAnim.IsActive())
            {
                _seqAnim.Kill();
            }
            
            _seqAnim = DOTween.Sequence();
            _seqAnim.Append(transform.DOMove(transform.position + 0.75f * Vector3.up, 1.5f));
            for (var i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].material.color = Color.white;
                _seqAnim.Join(_renderers[i].material.DOColor(Color.clear, 1.5f));
            }

            _seqAnim.OnComplete(() =>
            {
                FxPooling.Instance.Release(this);
                gameObject.SetActive(false);
            });
        }

    }
}