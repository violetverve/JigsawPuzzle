using UnityEngine;
using TMPro;
using System.Collections;
using Utilities;


namespace UI.LoadScene
{
    [ExecuteInEditMode]
    public class TextAnimator : MonoBehaviour
    {
        [SerializeField] private string _message;
        [SerializeField] private TextMeshProUGUI _animatedText;
        [SerializeField] private AnimationCurve _yPositionCurve;

        [SerializeField][Range(0.0001f, 5)] private float _stringAnimationDuration = 1;
        [SerializeField][Range(0.0001f, 1)] private float _charAnimationDuration = 0.1f;
        [SerializeField][Range(0, 1)] private float _editorTValue;
        
        private float _timeElapsed;


        //private void Start()
        //{
        //    StartCoroutine(RunAnimation(2));
        //}

        private void Update()
        {
            EvaluateRichText(_editorTValue);
        }

        public IEnumerator RunAnimation(float waitForSeconds)
        {
            yield return new WaitForSeconds(waitForSeconds);

            float t = 0;
            while (t <= 1f)
            {
                EvaluateRichText(t);
                t = _timeElapsed / _stringAnimationDuration;
                t = Mathf.Clamp01(t);
                _timeElapsed += Time.deltaTime;

                yield return null;
            }

        }

        private void EvaluateRichText(float t)
        {
            _animatedText.text = "";

            for (int i = 0; i < _message.Length; i++)
            {
                _animatedText.text += EvaluateCharRichText(_message[i], _message.Length, i, t);
            }
        }

        private string EvaluateCharRichText(char c, int sLength, int cPosition, float t)
        {
            float startPoint = ((1 - _charAnimationDuration) / (sLength - 1)) * cPosition;
            float endPoint = startPoint + _charAnimationDuration;
            float subT = t.Map(startPoint, endPoint, 0, 1);

            string yStart = $"<voffset={_yPositionCurve.Evaluate(subT)}em>";
            string yEnd = "</voffset>";

            var alpha = Mathf.Clamp01(subT) > 0 ? 1 : 0;
            string alphaHex = Mathf.RoundToInt(alpha * 255).ToString("X2");
            string alphaStart = $"<alpha=#{alphaHex}>";

            return yStart + alphaStart + c + yEnd;
        }

    }
}