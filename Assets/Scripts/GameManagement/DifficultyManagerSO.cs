using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Difficulty/DifficultyManagerSO")]
public class DifficultyManagerSO : ScriptableObject
{
    [SerializeField] private List<DifficultySO> _difficulties;

    [SerializeField] private float _rotationBonusPercentage;

    public List<DifficultySO> Difficulties => _difficulties;
    public float RotationBonusPercentage => _rotationBonusPercentage;

    public DifficultySO GetDifficulty(int index)
    {
        return _difficulties[index];
    }

}
