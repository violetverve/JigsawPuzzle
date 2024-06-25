using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace UI.GameScene.Themes
{
    public class SingleChoiceManager : MonoBehaviour
    {
        public static event Action<ThemeToggle> OnThemeSelected;
        [SerializeField] private ToggleGroup _toggleGroup;
        private List<Toggle> _toggles;

        private void Awake()
        {
            _toggles = new List<Toggle>(_toggleGroup.GetComponentsInChildren<Toggle>());
        }

        void Start()
        {
            // // Ensure only one Toggle can be selected at a time
            foreach (Toggle toggle in _toggles)
            {
                toggle.onValueChanged.AddListener(delegate { OnToggleChanged(toggle); });
            }
        }

        void OnToggleChanged(Toggle changedToggle)
        {
            if (changedToggle.isOn)
            {
                foreach (Toggle toggle in _toggles)
                {
                    if (toggle != changedToggle)
                    {
                        toggle.isOn = false;
                    }
                }

                OnThemeSelected?.Invoke(changedToggle.GetComponent<ThemeToggle>());
            }
        }
    }
}