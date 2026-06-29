using System.Collections.Generic;
using UnityEngine;

/// ScriptableObject che contiene
/// la configurazione di un livello.
///
/// Permette di creare livelli
/// direttamente dall'Inspector.
[CreateAssetMenu(
    fileName = "New Level",
    menuName = "Sandwich Puzzle/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Ingredienti del livello")]
    public List<IngredientSpawnData> Ingredients =
        new List<IngredientSpawnData>();
}