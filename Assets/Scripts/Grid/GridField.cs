using UnityEngine;


namespace Grid
{
    public class GridField : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private float _width;
        public float Width => _width;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public float CalculateWidth()
        {
            Vector3[] worldCorners = new Vector3[4];
            _rectTransform.GetWorldCorners(worldCorners);

            _width = Vector3.Distance(worldCorners[0], worldCorners[3]);

            return _width;
        }
    }
}
