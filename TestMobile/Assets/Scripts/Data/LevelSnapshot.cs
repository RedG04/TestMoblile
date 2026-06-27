using System.Collections.Generic;

/// Fotografia completa del livello.
/// Utilizzata dal Reset.
[System.Serializable]
public class LevelSnapshot
{
    public List<StackSnapshot> Stacks =
        new List<StackSnapshot>();
}