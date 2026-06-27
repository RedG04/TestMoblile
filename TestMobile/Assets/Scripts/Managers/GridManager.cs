using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

/// Gestisce la griglia di gioco.
///
/// Responsabilità:
/// - Registrare tutte le pile presenti.
/// - Sapere quali celle sono occupate.
/// - Restituire la pila presente in una posizione.
/// - Calcolare celle adiacenti.
/// - Aggiornare la posizione visiva delle pile.
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Dimensione di una cella nel mondo")]
    [SerializeField]
    private float cellSize = 2f;

    /// Dizionario che associa:
    ///
    /// Posizione Griglia -> StackGroup
    ///
    /// Esempio:
    /// (0,0) -> Stack A
    /// (1,0) -> Stack B
    private Dictionary<Vector2Int, StackGroup> grid =
        new Dictionary<Vector2Int, StackGroup>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region REGISTRAZIONE

    /// Registra una pila nella griglia.
    public void RegisterStack(StackGroup stack)
    {
        if (stack == null)
            return;

        Vector2Int position = stack.GridPosition;

        if (!grid.ContainsKey(position))
        {
            grid.Add(position, stack);
        }
    }

    /// Rimuove una pila dalla griglia.
    public void UnregisterStack(StackGroup stack)
    {
        if (stack == null)
            return;

        Vector2Int position = stack.GridPosition;

        if (grid.ContainsKey(position))
        {
            grid.Remove(position);
        }
    }

    /// Svuota completamente la griglia.
    /// Utilizzato durante il caricamento
    /// di un nuovo livello.
    public void ClearGrid()
    {
        grid.Clear();
    }

    #endregion

    #region QUERY

    /// Restituisce true se una cella è occupata.
    public bool IsCellOccupied(Vector2Int position)
    {
        return grid.ContainsKey(position);
    }

    /// Restituisce la pila presente
    /// in una determinata cella.
    public StackGroup GetStackAt(Vector2Int position)
    {
        if (grid.TryGetValue(position, out StackGroup stack))
        {
            return stack;
        }

        return null;
    }

    /// Restituisce tutte le pile registrate.
    public List<StackGroup> GetAllStacks()
    {
        List<StackGroup> result =
            new List<StackGroup>();

        foreach (var pair in grid)
        {
            result.Add(pair.Value);
        }

        return result;
    }

    #endregion

    #region POSIZIONAMENTO

    /// Converte una posizione di griglia
    /// in una posizione del mondo.
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(
            gridPosition.x * cellSize,
            0f,
            gridPosition.y * cellSize
        );
    }

    /// Posiziona visivamente una pila.
    public void PlaceStack(StackGroup stack)
    {
        if (stack == null)
            return;

        stack.transform.position =
            GridToWorld(stack.GridPosition);

        stack.UpdateVisualStack();
    }

    /// Aggiorna la posizione della pila
    /// nella struttura dati interna.
    public void UpdateStackPosition(
        StackGroup stack,
        Vector2Int oldPosition,
        Vector2Int newPosition)
    {
        if (grid.ContainsKey(oldPosition))
        {
            grid.Remove(oldPosition);
        }

        if (!grid.ContainsKey(newPosition))
        {
            grid.Add(newPosition, stack);
        }

        PlaceStack(stack);
    }

    #endregion

    #region ADIACENZA

    /// Restituisce la cella adiacente
    /// nella direzione specificata.
    public Vector2Int GetAdjacentCell(
        Vector2Int currentCell,
        SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeDirection.Up:
                return currentCell + Vector2Int.up;

            case SwipeDirection.Down:
                return currentCell + Vector2Int.down;

            case SwipeDirection.Left:
                return currentCell + Vector2Int.left;

            case SwipeDirection.Right:
                return currentCell + Vector2Int.right;
        }

        return currentCell;
    }

    /// Verifica se esiste una pila
    /// nella direzione indicata.
    public bool HasAdjacentStack(
        Vector2Int currentCell,
        SwipeDirection direction)
    {
        Vector2Int targetCell =
            GetAdjacentCell(currentCell, direction);

        return IsCellOccupied(targetCell);
    }

    /// Restituisce la pila adiacente.
    public StackGroup GetAdjacentStack(
        Vector2Int currentCell,
        SwipeDirection direction)
    {
        Vector2Int targetCell =
            GetAdjacentCell(currentCell, direction);

        return GetStackAt(targetCell);
    }

    #endregion

    #region DEBUG

    /// Disegna la griglia nella Scene View.
    /// Molto utile durante lo sviluppo.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        int size = 10;

        for (int x = -size; x <= size; x++)
        {
            for (int y = -size; y <= size; y++)
            {
                Vector3 position =
                    new Vector3(
                        x * cellSize,
                        0f,
                        y * cellSize
                    );

                Gizmos.DrawWireCube(
                    position,
                    new Vector3(
                        cellSize,
                        0.1f,
                        cellSize
                    )
                );
            }
        }
    }

    #endregion
}