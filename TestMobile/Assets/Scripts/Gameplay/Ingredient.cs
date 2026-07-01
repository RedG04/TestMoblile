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

    [Header("Materiali ingredienti")]
    public Material breadMaterial;
    public Material cheeseMaterial;
    public Material hamMaterial;
    public Material tomatoMaterial;

    /// Pila a cui appartiene attualmente l'ingrediente.
    [HideInInspector]
    public StackGroup CurrentStack;

    /// Posizione iniziale salvata per il Reset.
    [HideInInspector]
    public Vector2Int InitialGridPosition;

    private void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        MeshRenderer meshRenderer =
            GetComponent<MeshRenderer>();

        if (meshRenderer == null)
            return;

        switch (ingredientType)
        {
            case IngredientType.Bread:
                meshRenderer.material = breadMaterial;
                break;

            case IngredientType.Cheese:
                meshRenderer.material = cheeseMaterial;
                break;

            case IngredientType.Ham:
                meshRenderer.material = hamMaterial;
                break;

            case IngredientType.Tomato:
                meshRenderer.material = tomatoMaterial;
                break;
        }
    }

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