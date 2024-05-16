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

    private void HandleItemPickedUp(Transform piece)
    {
        _scrollRect.enabled = false;

        TryRemovePieceFromScrollView(piece);
    }

    private void TryRemovePieceFromScrollView(Transform piece)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition, Camera.main))
        {
            RemovePieceFromScrollView(piece);
        }
    }

    private void RemovePieceFromScrollView(Transform piece)
    {
        piece.SetParent(_gridParent, true);
        
        piece.localScale = Vector3.one * _originalPieceSize;

        RectTransform rectTransform = piece.GetComponent<RectTransform>();
        
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    private void HandleItemDropped(Transform piece)
    {
        TryAddPieceToScrollView(piece);

        _scrollRect.enabled = true;
    }

    private void PopulateScrollView()
    {
        var puzzleList = _gridGenerator.GeneratedPuzzles;

        foreach (var puzzle in puzzleList)
        {
            AddPieceToScrollView(puzzle.transform);
        }
    }

    public void AddPieceToScrollView(Transform piece)
    {
        if (_originalPieceSize == 0)
        {
            _originalPieceSize = piece.transform.localScale.x;
        }

        Vector3 position = piece.position;
 
        piece.SetParent(_content, true);
        InsertPieceAtIndex(piece as RectTransform, GetDropIndex(position));

        piece.localPosition = Vector3.zero;
        piece.localRotation = Quaternion.identity;
        piece.localScale = Vector3.one * _pieceSize;
    }

    private RectTransform CreateUIWrapper()
    {
        GameObject uiWrapper = new GameObject("UIWrapper");
        RectTransform rectTransform = uiWrapper.AddComponent<RectTransform>();
        
        rectTransform.SetParent(_content, false);
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = Vector3.zero;

        return rectTransform;
    }

    public void TryAddPieceToScrollView(Transform piece)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition, Camera.main))
        {
            AddPieceToScrollView(piece);
        }
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

}
