using System.Collections.Generic;
using UnityEngine;

/// Contiene tutte le informazioni necessarie
/// per annullare l'ultima mossa.
[System.Serializable]
public class MoveData
{
    /// Pila che ha effettuato il movimento.
    public StackGroup SourceStack;

    /// Pila su cui è stata effettuata la fusione.
    public StackGroup TargetStack;

    /// Posizione iniziale della pila sorgente.
    public Vector2Int SourcePosition;

    /// Ingredienti trasferiti.
    public List<Ingredient> MovedIngredients =
        new List<Ingredient>();
}