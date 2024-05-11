using UnityEngine;
using System;
using UnityEngine.EventSystems;
using PuzzlePiece;

public class Draggable : MonoBehaviour
{
    public static event Action<Transform> OnItemDropped;
    public static event Action<Transform> OnItemPickedUp;
    private Camera _mainCamera;
    private bool _isDragging;
    private Vector3 _offset;
    
    private RectTransform _rectTransform;
    private Canvas _canvas;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        _isDragging = true;
        _offset = transform.position - GetMouseWorldPos();
        
        OnItemPickedUp?.Invoke(transform);
    }

    private void OnMouseUp()
    {
        _isDragging = false;

        OnItemDropped?.Invoke(transform);
    }

    private void OnMouseDrag()
    {
        if (_isDragging) 
        {
            transform.position = GetMouseWorldPos() + _offset;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = _mainCamera.WorldToScreenPoint(transform.position).z;
        return _mainCamera.ScreenToWorldPoint(mousePoint);
    }
}
