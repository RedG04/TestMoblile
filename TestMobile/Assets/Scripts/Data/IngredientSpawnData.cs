using UnityEngine;

/// Contiene le informazioni necessarie
/// per generare un ingrediente nel livello.
///
/// Ogni ingrediente avrà:
/// - Tipo
/// - Posizione nella griglia
[System.Serializable]
public class IngredientSpawnData
{
    [Header("Tipo ingrediente")]
    public IngredientType IngredientType;

    [Header("Posizione nella griglia")]
    public Vector2Int GridPosition;
}