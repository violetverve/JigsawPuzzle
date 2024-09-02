using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;
using DG.Tweening;
using Player;
using UI;

namespace UI.GameScene.Win
{
    public class RewardPopUp : MonoBehaviour
    {
        [SerializeField] private Transform _rewardPopUpWindow;
        [SerializeField] private TextMeshProUGUI _prizeText;
        [SerializeField] private CoinsCollectAnimator _coinsCollectAnimator;
        [SerializeField] private RewardTag _rewardTag;
        private int _reward;
    
        private void Start()
        {
            AnimateRewardPopUp();
        }

        private void AnimateRewardPopUp()
        {
            _rewardPopUpWindow.localScale = Vector3.zero;
            _rewardPopUpWindow.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }

        public void SetReward(int reward)
        {
            _rewardTag.SetRewardText(reward);
            _reward = reward;
        }

        public void ClaimPrize()
        {
            if (PlayerData.Instance != null)
            {
                PlayerData.Instance.UpdateCoins(_reward);
            }
            else
            {
              _reward = 100;
            }

            var sequence = _coinsCollectAnimator.GetCoinsCollectSequence(_reward)
                                                .OnComplete(LoadUIScene);

            sequence.Play();
        }

        private void LoadUIScene()
        {
            SceneManager.LoadScene("UIScene");
        }

    }
}