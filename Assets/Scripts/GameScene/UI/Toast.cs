using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameScene.UI
{
    public class Toast : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _toastText;
        
        private List<TextMeshProUGUI> _toasts = new();

        public void ShowToast(string msg)
        {
            TextMeshProUGUI toast = null;
            for (int i = 0; i < _toasts.Count; i++)
            {
                if (_toasts[i].gameObject.activeInHierarchy)
                {
                    continue;
                }
                
                toast = _toasts[i];
                toast.color = Color.white;
            }

            if (toast == null)
            {
                toast = Instantiate(_toastText, transform);
                _toasts.Add(toast);
            }
            
            toast.SetText(msg);
            toast.rectTransform.localPosition = Vector3.zero;
            
            toast.gameObject.SetActive(true);
            Sequence seq = DOTween.Sequence();
            seq.Append(toast.rectTransform.DOAnchorPosY(100, 2f));
            seq.Join(toast.DOColor(Color.clear, 2f));
            
            seq.OnComplete(() => toast.gameObject.SetActive(false));
        }
    }
}