using UnityEngine;
using DG.Tweening;
using UI.LoadScene;
using System.Collections;
using UnityEngine.Rendering;


namespace UI.LoadScene
{
    public class LogoAnimator : MonoBehaviour
    {
        [SerializeField] private TextAnimator _textAnimator;
        [SerializeField] private PopAnimation _popAnimation;

        [SerializeField] private float _waitForSeconds = 1f;
        [SerializeField] private float _popDuration = 1.2f;
        [SerializeField] private float _textAnimationDelay = 0.6f;

        void Start()
        {
            AnimateLogo();
        }

        public void AnimateLogo()
        {
            StartCoroutine(RunAnimation(_waitForSeconds));
        }

        private IEnumerator RunAnimation(float waitForSeconds)
        {
            yield return new WaitForSeconds(waitForSeconds);

            _popAnimation.gameObject.SetActive(true);
            _popAnimation.GetPopTween(_popDuration).Play();
            StartCoroutine(_textAnimator.RunAnimation(_textAnimationDelay));
        }

    }

}