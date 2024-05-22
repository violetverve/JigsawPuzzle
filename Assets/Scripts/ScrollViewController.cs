using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Grid;
using PuzzlePiece;


public class ScrollViewController : MonoBehaviour
{
    [SerializeField] private Transform _gridParent;
    [SerializeField] private RectTransform _content;
    [SerializeField] private GridGenerator _gridGenerator;
    [SerializeField] private ScrollRect _scrollRect;
    private float _pieceSize = 40;
    private float _originalPieceSize;
    private bool _isOriginalPieceSizeSet;
    
    private void Start()
    {
        PopulateScrollView();
    }

    private void OnEnable()
    {
        Draggable.OnItemDropped += HandleItemDropped;
        Draggable.OnItemPickedUp += HandleItemPickedUp;
    }

    private void OnDisable()
    {
        Draggable.OnItemDropped -= HandleItemDropped;
        Draggable.OnItemPickedUp -= HandleItemPickedUp;
    }

    private void HandleItemPickedUp(ISnappable snappable)
    {
        if (snappable is PuzzleGroup) return;
        _scrollRect.enabled = false;

        if (MouseOnScrollView())
        {
            RemovePieceFromScrollView(snappable.Transform);
        }
    }

    private void RemovePieceFromScrollView(Transform piece)
    {
        piece.SetParent(_gridParent, true);
        
        piece.localScale = Vector3.one * _originalPieceSize;

        RectTransform rectTransform = piece as RectTransform;
        
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    private void HandleItemDropped(ISnappable snappable)
    {
        if (snappable is PuzzleGroup) return;

        if (MouseOnScrollView())
        {
            AddPieceToScrollView(snappable.Transform);
        }

        _scrollRect.enabled = true;
    }

    private void PopulateScrollView()
    {
        foreach (var piece in _gridGenerator.GeneratedPieces)
        {
            AddPieceToScrollView(piece.transform);
        }
    }

    public void AddPieceToScrollView(Transform piece)
    {
        SetOriginalPieceSize(piece);

        Vector3 position = piece.position;
 
        piece.SetParent(_content, true);
        InsertPieceAtIndex(piece as RectTransform, GetDropIndex(position));

        piece.localPosition = Vector3.zero;
        piece.localRotation = Quaternion.identity;
        piece.localScale = Vector3.one * _pieceSize;
    }

    private void SetOriginalPieceSize(Transform piece)
    {
        if (!_isOriginalPieceSizeSet)
        {
            _originalPieceSize = piece.transform.localScale.x;
            _isOriginalPieceSizeSet = true;
        }
    }

    public bool MouseOnScrollView()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition, Camera.main);
    }

    private int GetDropIndex(Vector3 dropScreenPosition)
    {
        Vector2 localDropPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_content, dropScreenPosition, null, out localDropPosition);
        
        for (int i = 0; i < _content.childCount-1; i++)
        {
            Vector2 childLocalPosition = (_content.GetChild(i) as RectTransform).anchoredPosition;
            if (localDropPosition.x < childLocalPosition.x)
            {
                return i;
            }
        }
        return _content.childCount-1;
    }

    private void InsertPieceAtIndex(RectTransform piece, int index)
    {
        if (index >= 0 && index < _content.childCount)
        {
            piece.SetSiblingIndex(index);
        }
        else
        {
            piece.SetAsLastSibling();
        }
    }

    public bool IsInScrollView(Transform piece)
    {
        return piece.parent == _content;
    }

}
