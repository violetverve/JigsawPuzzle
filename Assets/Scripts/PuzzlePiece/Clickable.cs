using UnityEngine;
using System;

namespace PuzzlePiece
{
    public class Clickable : MonoBehaviour
    {

        public static event Action<ISnappable, Vector3> OnItemClicked;

        private Vector3 _initialPosition;
        private float _tolerance = 0.01f;

        private ISnappable _snappable;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        
        private void Start()
        {
            _snappable = gameObject.GetComponent<ISnappable>();
        }

        private void OnMouseDown()
        {
            _initialPosition = transform.position;
        }

        private void OnMouseUp()
        {
            if (Vector3.Distance(_initialPosition, transform.position) < _tolerance)
            {
                OnItemClicked?.Invoke(_snappable, GetMouseWorldPos());
            }
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = _mainCamera.WorldToScreenPoint(transform.position).z;
            return _mainCamera.ScreenToWorldPoint(mousePoint);
        }
    }
}