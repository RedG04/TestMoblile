using UnityEngine;

public enum IngredientType
{
    Bread,
    Cheese,
    Ham,
    Tomato
}

/// Rappresenta una singola fetta.
/// Questo script contiene solamente dati.
/// Tutta la logica di movimento sarà gestita
/// da altri manager.
public class Ingredient : MonoBehaviour
{
    [Header("Tipo ingrediente")]
    public IngredientType ingredientType;

    [Header("Posizione nella griglia")]
    public Vector2Int GridPosition;

    /// Pila a cui appartiene attualmente l'ingrediente.
    [HideInInspector]
    public StackGroup CurrentStack;

    /// Posizione iniziale salvata per il Reset.
    [HideInInspector]
    public Vector2Int InitialGridPosition;

    /// Salva la posizione iniziale.
    /// Chiamata all'inizio del livello.
    public void SaveInitialPosition()
    {
        InitialGridPosition = GridPosition;
    }

    /// Riporta l'ingrediente alla posizione iniziale.
    /// Utilizzato dal pulsante Reset.
    public void ResetPosition()
    {
        GridPosition = InitialGridPosition;
    }

    /// Restituisce true se l'ingrediente è una fetta di pane.
    public bool IsBread()
    {
        return ingredientType == IngredientType.Bread;
    }
}