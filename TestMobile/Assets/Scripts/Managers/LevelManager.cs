using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Gestisce:
/// - caricamento livelli
/// - generazione ingredienti
/// - passaggio livello successivo
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Lista livelli")]
    public List<LevelData> Levels =
        new List<LevelData>();

    [Header("Prefab ingredienti")]
    public Ingredient IngredientPrefab;

    private int currentLevelIndex;

    private readonly List<StackGroup> spawnedStacks =
        new List<StackGroup>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadLevel(0);
    }

    /// Carica un livello.
    public void LoadLevel(int index)
    {
        ClearCurrentLevel();

        currentLevelIndex = index;

        LevelData level =
            Levels[currentLevelIndex];

        GenerateLevel(level);

        UndoManager.Instance.SaveInitialState();

        UpdateNextLevelButton();
    }

    /// Passa al livello successivo.
    public void LoadNextLevel()
    {
        if (currentLevelIndex >= Levels.Count - 1)
            return;

        LoadLevel(currentLevelIndex + 1);
    }

    /// Genera tutte le pile del livello.
    private void GenerateLevel(LevelData level)
    {
        foreach (IngredientSpawnData spawnData
                 in level.Ingredients)
        {
            CreateSingleIngredientStack(
                spawnData
            );
        }
    }

    /// Crea una pila composta
    /// da un solo ingrediente.
    private void CreateSingleIngredientStack(
        IngredientSpawnData spawnData)
    {
        GameObject stackObject =
            new GameObject(
                "Stack_" +
                spawnData.IngredientType
            );

        StackGroup stack =
            stackObject.AddComponent<StackGroup>();

        Ingredient ingredient =
            Instantiate(
                IngredientPrefab
            );

        // L'ingrediente diventa figlio dello Stack
        ingredient.transform.SetParent(stack.transform);

        // Lo posizioniamo al centro dello Stack
        ingredient.transform.localPosition = Vector3.zero;

        ingredient.ingredientType =
            spawnData.IngredientType;

        ingredient.GridPosition =
            spawnData.GridPosition;

        ingredient.SaveInitialPosition();

        stack.AddIngredient(
            ingredient
        );

        GridManager.Instance.RegisterStack(
            stack
        );

        GridManager.Instance.PlaceStack(
            stack
        );

        spawnedStacks.Add(stack);
    }

    /// Elimina completamente
    /// il livello corrente.
    private void ClearCurrentLevel()
    {
        foreach (StackGroup stack
                 in spawnedStacks)
        {
            if (stack != null)
            {
                Destroy(stack.gameObject);
            }
        }

        spawnedStacks.Clear();

        GridManager.Instance.ClearGrid();

        UIManager.Instance.HideVictoryPanel();
    }

    /// Aggiorna il pulsante Next Level.
    private void UpdateNextLevelButton()
    {
        bool hasNextLevel =
            currentLevelIndex <
            Levels.Count - 1;

        if (hasNextLevel)
        {
            UIManager.Instance.EnableNextLevel();
        }
        else
        {
            UIManager.Instance.DisableNextLevel();
        }
    }

    /// Restituisce il livello corrente.
    public int GetCurrentLevel()
    {
        return currentLevelIndex;
    }

    /// Restituisce il numero totale
    /// di livelli.
    public int GetLevelCount()
    {
        return Levels.Count;
    }
}