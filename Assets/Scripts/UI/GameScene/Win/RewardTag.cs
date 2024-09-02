using UnityEngine;
using TMPro;
using DG.Tweening;

namespace UI.GameScene.Win
{
    public class RewardTag : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private CanvasGroup _rewardCanvasGroup;

        public void SetRewardText(int reward)
        {
            _rewardText.text = "+ " + reward.ToString();
        }

        public void FadeOut(float duration)
        {
            _rewardCanvasGroup.DOFade(0, duration).OnComplete(() => gameObject.SetActive(false));
        }

        public Tween GetFadeOutTween(float duration)
        {
            return _rewardCanvasGroup.DOFade(0, duration);
        }

    }
}