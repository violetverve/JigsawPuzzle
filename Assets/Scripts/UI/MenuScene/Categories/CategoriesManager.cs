using System.Collections.Generic;
using UnityEngine;
using UI.MenuScene;
using UI.MenuScene.Puzzle;
using UnityEngine.Pool;
using TMPro;
using PuzzleData;

namespace JigsawPuzzles.UI.MenuScene.Categories
{
    public class CategoriesManager : MonoBehaviour
    {
        [Header("Category Settings")]
        [SerializeField] private List<PuzzleCategorySO> _categories;
        [SerializeField] private CategoryPanel _categoryPanelPrefab;
        [SerializeField] private Transform _categoriesParent;
        [SerializeField] private ColorPaletteSO _colorPalette;

        [Header("Puzzle Panels Settings")]
        [SerializeField] private CategoryWindow _categoryWindow;
        [SerializeField] private Transform _categoriesWindowParent;
        [SerializeField] private PuzzlePanelUI _puzzlePanelPrefab;
        [SerializeField] private PuzzleList _puzzleList;
        [SerializeField] private TextMeshProUGUI _categoryName;
        [SerializeField] private int _defaultPoolCapacity = 30;
        [SerializeField] private int _maxPoolSize = 50;


        private ObjectPool<PuzzlePanelUI> _puzzlePanelPool;
        private List<PuzzlePanelUI> _puzzlePanelUIs = new List<PuzzlePanelUI>();

        private void Awake()
        {
            InitializeCategories();
            InitializePuzzlePanelsPool();
        }

        private void OnEnable()
        {
            CategoryPanel.CategorySelected += HandleCategorySelected;
            UIManager.OnPuzzleUnlocked += HandlePuzzleUnlocked;
        }

        private void OnDisable()
        {
            CategoryPanel.CategorySelected -= HandleCategorySelected;
            UIManager.OnPuzzleUnlocked -= HandlePuzzleUnlocked;
        }

        private void InitializePuzzlePanelsPool()
        {
            _puzzlePanelPool = new ObjectPool<PuzzlePanelUI>(
            createFunc: () => Instantiate(_puzzlePanelPrefab, _categoriesWindowParent),
            actionOnGet: panel => panel.gameObject.SetActive(true),
            actionOnRelease: panel => panel.gameObject.SetActive(false),
            collectionCheck: false,
            defaultCapacity: _defaultPoolCapacity,
            maxSize: _maxPoolSize
            );
        }

        private void InitializeCategories()
        {
            for (int i = 0; i < _categories.Count; i++)
            {
                var category = _categories[i];
                Color color = _colorPalette.GetColor(i);
                CategoryPanel categoryPanel = Instantiate(_categoryPanelPrefab, _categoriesParent);
                categoryPanel.Initialize(category, color);
            }
        }

        #region Event Handlers
        private void HandlePuzzleUnlocked(int puzzleId)
        {
            if (_categoryWindow.gameObject.activeSelf)
            {
                var panel = _puzzlePanelUIs.Find(p => p.PuzzleID == puzzleId);

                if (panel != null)
                {
                    panel.SetLocked(false);
                }
            }
        }

        private void HandleCategorySelected(PuzzleCategory category)
        {
            ClearCategoryWindow();
            PopulateCategoryWindow(category);
            _categoryWindow.SetActive(true);;

            _categoryName.text = category.ToString();
        }
        #endregion


        #region Category Window Management
        private void ClearCategoryWindow()
        {
            if (_puzzlePanelPool == null)
            {
                return;
            }

            foreach (var puzzlePanel in _puzzlePanelUIs)
            {
                _puzzlePanelPool.Release(puzzlePanel);
            }

            _puzzlePanelUIs.Clear();
        }

        private void PopulateCategoryWindow(PuzzleCategory category)
        {
            var puzzlesInCategory = GetPuzzlesByCategory(category);

            foreach (var puzzle in puzzlesInCategory)
            {
                var puzzlePanel = _puzzlePanelPool.Get();
                puzzlePanel.LoadPuzzlePanel(puzzle);
                _puzzlePanelUIs.Add(puzzlePanel);

                puzzlePanel.transform.SetAsLastSibling();
            }
        }

        private List<PuzzleSO> GetPuzzlesByCategory(PuzzleCategory category)
        {
            var puzzlesInCategory = _puzzleList.List.FindAll(p => p.Category == category);
            puzzlesInCategory.Sort((p1, p2) => p1.Id.CompareTo(p2.Id)); // Sort for consistent order
            return puzzlesInCategory;
        }
        #endregion

    }
}

