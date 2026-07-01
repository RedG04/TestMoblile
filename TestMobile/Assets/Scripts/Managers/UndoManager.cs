using System.Collections.Generic;
using UnityEngine;

/// Gestisce:
/// - Undo ultima mossa
/// - Reset livello
public class UndoManager : MonoBehaviour
{
    public static UndoManager Instance;

    /// Stato iniziale del livello.
    /// Utilizzato dal Reset.
    private LevelSnapshot initialSnapshot;

    private void Awake()
    {
        Instance = this;
    }

    /// Salva lo stato iniziale del livello.
    /// Chiamato una sola volta quando
    /// il livello viene generato.
    public void SaveInitialState()
    {
        initialSnapshot =
            CreateSnapshot();
    }

    /// Undo dell'ultima mossa.
    public void Undo()
    {
        MoveData move =
            MoveManager.Instance.LastMove;

        if (move == null)
            return;

        if (move.SourceStack == null)
            return;

        if (move.TargetStack == null)
            return;

        //------------------------------------------------
        // RIPRISTINO STACK ORIGINALE
        //------------------------------------------------

        foreach (Ingredient ingredient
                 in move.MovedIngredients)
        {
            move.TargetStack.Ingredients
                .Remove(ingredient);

            move.SourceStack.Ingredients
                .Add(ingredient);

            ingredient.CurrentStack =
                move.SourceStack;
            ingredient.transform.SetParent(
                move.SourceStack.transform
);
        }

        //------------------------------------------------
        // RIPRISTINO POSIZIONE
        //------------------------------------------------

        foreach (Ingredient ingredient
                 in move.SourceStack.Ingredients)
        {
            ingredient.GridPosition =
                move.SourcePosition;
        }

        Vector2Int targetPosition =
    move.TargetStack.GridPosition;

        foreach (Ingredient ingredient
                 in move.TargetStack.Ingredients)
        {
            ingredient.GridPosition =
                targetPosition;
        }

        //------------------------------------------------
        // RIATTIVO LO STACK
        //------------------------------------------------

        move.SourceStack.gameObject.SetActive(true);

        //------------------------------------------------
        // REGISTRO NUOVAMENTE
        //------------------------------------------------

        GridManager.Instance.RegisterStack(
            move.SourceStack
        );

        //------------------------------------------------
        // AGGIORNO VISUAL
        //------------------------------------------------

        GridManager.Instance.PlaceStack(
            move.SourceStack
        );

        GridManager.Instance.PlaceStack(
            move.TargetStack
        );

        move.SourceStack.UpdateVisualStack();

        move.TargetStack.UpdateVisualStack();

        //------------------------------------------------
        // CONSUMO UNDO
        //------------------------------------------------

        MoveManager.Instance.LastMove = null;

        UIManager.Instance.DisableUndo();
    }

    /// Riporta il livello
    /// alla configurazione iniziale.
    public void ResetLevel()
    {
        if (initialSnapshot == null)
            return;

        RestoreSnapshot(initialSnapshot);

        MoveManager.Instance.LastMove = null;

        UIManager.Instance.HideVictoryPanel();
        UIManager.Instance.DisableUndo();
        UIManager.Instance.DisableReset();
    }


    /// Crea una fotografia completa
    /// dello stato corrente.
    private LevelSnapshot CreateSnapshot()
    {
        LevelSnapshot snapshot =
            new LevelSnapshot();

        List<StackGroup> stacks =
            GridManager.Instance.GetAllStacks();

        foreach (StackGroup stack in stacks)
        {
            StackSnapshot stackData =
                new StackSnapshot();

            stackData.StackReference = stack;

            foreach (Ingredient ingredient
                     in stack.Ingredients)
            {
                IngredientSnapshot ingredientData =
                    new IngredientSnapshot();

                ingredientData.IngredientReference =
                    ingredient;

                ingredientData.GridPosition =
                    ingredient.GridPosition;

                stackData.Ingredients
                    .Add(ingredientData);
            }

            snapshot.Stacks.Add(stackData);
        }

        return snapshot;
    }

    /// Ripristina una fotografia.
    private void RestoreSnapshot(
        LevelSnapshot snapshot)
    {
        GridManager.Instance.ClearGrid();

        foreach (StackSnapshot stackData
                 in snapshot.Stacks)
        {
            StackGroup stack =
                stackData.StackReference;

            stack.gameObject.SetActive(true);

            stack.Ingredients.Clear();

            foreach (IngredientSnapshot ingredientData
                     in stackData.Ingredients)
            {
                Ingredient ingredient =
                    ingredientData
                    .IngredientReference;

                ingredient.GridPosition =
                    ingredientData.GridPosition;

                ingredient.CurrentStack =
                    stack;

                stack.Ingredients.Add(
                    ingredient
                );

                ingredient.transform.SetParent(
                    stack.transform
);
            }

            GridManager.Instance.RegisterStack(
                stack
            );

            GridManager.Instance.PlaceStack(
                stack
            );

            stack.UpdateVisualStack();
        }
    }
}