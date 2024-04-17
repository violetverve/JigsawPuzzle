using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Piece Config", menuName = "Create SO/Piece Config")]
public class PieceSO : ScriptableObject
{
    [SerializeField] private int _idColumn;
    [SerializeField] private int _idRow;

    [Space]

    [SerializeField] private Vector2 _correctCoords;
    [SerializeField] private Quaternion _correctRotation; 

    public Vector2 CorrectCoords => _correctCoords;
    public Quaternion CorrectRotation => _correctRotation;
    public int IdColumn => _idColumn;
    public int IdRow => _idRow;

    public void Assemble(Vector2 correctCoords, Quaternion correctRotation)
    {
        _correctCoords = correctCoords;
        _correctRotation = correctRotation;
    }
}
