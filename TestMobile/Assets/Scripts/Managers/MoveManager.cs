using System.Collections.Generic;
using UnityEngine;

/// Gestisce tutti i movimenti del gioco.
///
/// Regole implementate:
///
/// - Movimento solo verso celle occupate
/// - Movimento solo verso celle adiacenti
/// - Blocco del pane
/// - Fusione pile
/// - Salvataggio ultima mossa
/// - Controllo vittoria
public class MoveManager : MonoBehaviour
{
    public static MoveManager Instance;

    /// Ultima mossa eseguita.
    /// Serve all'Undo.
    public MoveData LastMove;

    private void Awake()
    {
        Instance = this;
    }

    /// Tenta di muovere una pila.
    public bool TryMoveStack(
        StackGroup movingStack,
        SwipeDirection direction)
    {
        if (movingStack == null)
            return false;

        //------------------------------------------------
        // BLOCCO PANE
        //------------------------------------------------

        if (ContainsBread(movingStack))
        {
            if (!CanMoveBread())
            {
                Debug.Log("Pane bloccato.");
                return false;
            }
        }

        //------------------------------------------------
        // VERIFICA ADIACENZA
        //------------------------------------------------

        if (!GridManager.Instance.HasAdjacentStack(
                movingStack.GridPosition,
                direction))
        {
            Debug.Log("Nessuna pila adiacente.");

            return false;
        }

        //------------------------------------------------
        // RECUPERO DESTINAZIONE
        //------------------------------------------------

        StackGroup targetStack =
            GridManager.Instance.GetAdjacentStack(
                movingStack.GridPosition,
                direction);

        if (targetStack == null)
            return false;

        //------------------------------------------------
        // SALVATAGGIO UNDO
        //------------------------------------------------

        SaveMove(
            movingStack,
            targetStack
        );

        //------------------------------------------------
        // RIMOZIONE DALLA GRIGLIA
        //------------------------------------------------

        GridManager.Instance.UnregisterStack(
            movingStack
        );

        //------------------------------------------------
        // ANIMAZIONE ROTAZIONE
        //------------------------------------------------

        StartCoroutine(
            movingStack.RotateStackAnimation()
        );

        //------------------------------------------------
        // FUSIONE
        //------------------------------------------------

        List<Ingredient> movedIngredients =
            movingStack.MergeInto(targetStack);

        LastMove.MovedIngredients =
            movedIngredients;

        //------------------------------------------------
        // ELIMINO STACK VUOTA
        //------------------------------------------------

        Destroy(
            movingStack.gameObject
        );

        //------------------------------------------------
        // SBLOCCO UI
        //------------------------------------------------

        UIManager.Instance.EnableUndo();
        UIManager.Instance.EnableReset();

        //------------------------------------------------
        // CONTROLLO VITTORIA
        //------------------------------------------------

        CheckVictory();

        return true;
    }

    /// Salva i dati della mossa.
    private void SaveMove(
        StackGroup source,
        StackGroup target)
    {
        LastMove = new MoveData();

        LastMove.SourceStack = source;

        LastMove.TargetStack = target;

        LastMove.SourcePosition =
            source.GridPosition;
    }

    /// Verifica se una pila contiene pane.
    private bool ContainsBread(
        StackGroup stack)
    {
        foreach (Ingredient ingredient
                 in stack.Ingredients)
        {
            if (ingredient.IsBread())
            {
                return true;
            }
        }

        return false;
    }

    /// Le fette di pane non possono muoversi
    /// finché tutti gli altri ingredienti
    /// non sono stati impilati su una fetta
    /// di pane.
    public bool CanMoveBread()
    {
        List<StackGroup> stacks =
            GridManager.Instance.GetAllStacks();

        foreach (StackGroup stack in stacks)
        {
            bool containsBread =
                stack.ContainsBread();

            foreach (Ingredient ingredient
                     in stack.Ingredients)
            {
                if (ingredient.IsBread())
                    continue;

                if (!containsBread)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// Controlla se il livello è completato.
    public bool CheckVictory()
    {
        List<StackGroup> stacks =
            GridManager.Instance.GetAllStacks();

        int totalIngredients = 0;

        StackGroup remainingStack = null;

        foreach (StackGroup stack in stacks)
        {
            totalIngredients += stack.Count();

            if (stack.Count() > 0)
            {
                remainingStack = stack;
            }
        }

        if (remainingStack == null)
            return false;

        if (stacks.Count != 1)
            return false;

        if (!remainingStack.HasBreadOnTopAndBottom())
            return false;

        Debug.Log("VITTORIA!");

        UIManager.Instance.ShowVictoryPanel();

        return true;
    }
}