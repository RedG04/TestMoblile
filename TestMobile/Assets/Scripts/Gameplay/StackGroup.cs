using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Gestisce una pila di ingredienti.
/// Tutti gli ingredienti presenti nella lista
/// sono considerati parte della stessa pila.
public class StackGroup : MonoBehaviour
{
    [Header("Ingredienti contenuti nella pila")]
    public List<Ingredient> Ingredients = new();

    [Header("Altezza tra una fetta e l'altra")]
    [SerializeField]
    private float sliceHeight = 0.25f;

    /// Posizione occupata dalla pila nella griglia.
    /// Coincide sempre con la posizione
    /// del primo ingrediente.
    public Vector2Int GridPosition
    {
        get
        {
            if (Ingredients.Count == 0)
                return Vector2Int.zero;

            return Ingredients[0].GridPosition;
        }
    }

    private void Start()
    {
        UpdateVisualStack();
    }

    /// Aggiunge un ingrediente alla pila.
    public void AddIngredient(Ingredient ingredient)
    {
        Ingredients.Add(ingredient);
        ingredient.CurrentStack = this;

        UpdateVisualStack();
    }

    /// Rimuove un ingrediente dalla pila.
    public void RemoveIngredient(Ingredient ingredient)
    {
        Ingredients.Remove(ingredient);

        UpdateVisualStack();
    }

    /// Controlla se nella pila è presente almeno un pane.
    public bool ContainsBread()
    {
        foreach (Ingredient ingredient in Ingredients)
        {
            if (ingredient.ingredientType == IngredientType.Bread)
            {
                return true;
            }
        }

        return false;
    }

    /// Restituisce il numero di ingredienti presenti.
    public int Count()
    {
        return Ingredients.Count;
    }

    /// Aggiorna la posizione visiva di tutte le fette.
    /// Ogni fetta viene posizionata leggermente sopra
    /// quella sottostante.

    public void UpdateVisualStack()
    {
        for (int i = 0; i < Ingredients.Count; i++)
        {
            Ingredient ingredient = Ingredients[i];

            float offsetX =
                (i % 2 == 0) ? 0.08f : -0.08f;

            float offsetZ =
                (i % 3 == 0) ? 0.05f : -0.05f;

            Vector3 position =
                transform.position +
                Vector3.up * (sliceHeight * i) +
                new Vector3(offsetX, 0f, offsetZ);

            ingredient.transform.position = position;

            ingredient.transform.rotation =
                Quaternion.Euler(0f, i * 8f, 0f);
        }
    }

    /// Unisce questa pila ad un'altra pila.
    /// Tutti gli ingredienti vengono trasferiti
    /// nella pila di destinazione.
    public List<Ingredient> MergeInto(StackGroup targetStack)
    {
        List<Ingredient> movedIngredients =
            new List<Ingredient>(Ingredients);

        foreach (Ingredient ingredient in movedIngredients)
        {
            // Rimuove l'ingrediente dalla pila corrente
            Ingredients.Remove(ingredient);

            // Lo aggiunge alla pila di destinazione
            targetStack.Ingredients.Add(ingredient);

            // Aggiorna il riferimento alla pila
            ingredient.CurrentStack = targetStack;
            ingredient.GridPosition = targetStack.GridPosition;

            // IMPORTANTE: cambia il padre nella gerarchia
            ingredient.transform.SetParent(targetStack.transform);
        }

        targetStack.UpdateVisualStack();

        return movedIngredients;
    }
    /*public void MergeInto(StackGroup targetStack)
    {
        List<Ingredient> ingredientsToMove =
            new List<Ingredient>(Ingredients);

        foreach (Ingredient ingredient in ingredientsToMove)
        {
            targetStack.Ingredients.Add(ingredient);
            ingredient.CurrentStack = targetStack;
        }

        Ingredients.Clear();

        targetStack.UpdateVisualStack();
    }*/

    /// Muove la pila verso una nuova posizione.
    public void MoveToGridPosition(Vector2Int newPosition)
    {
        foreach (Ingredient ingredient in Ingredients)
        {
            ingredient.GridPosition = newPosition;
        }
    }

    /// Restituisce l'ingrediente in cima.
    public Ingredient GetTopIngredient()
    {
        if (Ingredients.Count == 0)
            return null;

        return Ingredients[Ingredients.Count - 1];
    }

    /// Restituisce l'ingrediente in fondo.
    public Ingredient GetBottomIngredient()
    {
        if (Ingredients.Count == 0)
            return null;

        return Ingredients[0];
    }

    /// Verifica se la pila rispetta la regola finale:
    /// pane in basso e pane in alto.
    public bool HasBreadOnTopAndBottom()
    {
        if (Ingredients.Count < 2)
            return false;

        Ingredient bottom = GetBottomIngredient();
        Ingredient top = GetTopIngredient();

        return
            bottom.ingredientType == IngredientType.Bread &&
            top.ingredientType == IngredientType.Bread;
    }

    /// Esegue la rotazione.
    /// Quando una pila viene spostata sopra un'altra,
    /// ruota di 180 gradi.
    public IEnumerator RotateStackAnimation()
    {
        Quaternion startRotation = transform.rotation;

        Quaternion targetRotation =
            startRotation *
            Quaternion.Euler(0f, 0f, 180f);

        float duration = 0.25f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration;

            transform.rotation =
                Quaternion.Lerp(
                    startRotation,
                    targetRotation,
                    t
                );

            yield return null;
        }

        transform.rotation = targetRotation;
    }

    /// Controlla se la pila contiene
    /// uno specifico tipo di ingrediente.
    public bool ContainsIngredientType(IngredientType type)
    {
        foreach (Ingredient ingredient in Ingredients)
        {
            if (ingredient.ingredientType == type)
            {
                return true;
            }
        }

        return false;
    }

    /// Svuota completamente la pila.
    public void ClearStack()
    {
        Ingredients.Clear();
    }
}