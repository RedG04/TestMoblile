using System.Collections.Generic;

/// Fotografia di una pila.
[System.Serializable]
public class StackSnapshot
{
    public StackGroup StackReference;

    public List<IngredientSnapshot> Ingredients =
        new List<IngredientSnapshot>();
}