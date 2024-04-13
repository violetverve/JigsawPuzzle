using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Piece Config", menuName = "Create SO/Piece Config")]
public class PieceSO : ScriptableObject
{
    [SerializeField] private Vector2 _correctCoords;
    [SerializeField] private Vector2 _correctRotation;

    public Vector2 CorrectCoords => _correctCoords;
    public Vector2 CorrectRotation => _correctRotation;

    void Assemble(Vector2 correctCoords, Vector2 correctRotation)
    {
        _correctCoords = correctCoords;
        _correctRotation = correctRotation;
    }
}
