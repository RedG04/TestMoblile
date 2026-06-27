using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Bottoni")]
    public Button undoButton;
    public Button resetButton;
    public Button nextLevelButton;

    [Header("Pannello vittoria")]
    public GameObject victoryPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DisableUndo();
        DisableReset();

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void EnableUndo()
    {
        undoButton.interactable = true;
    }

    public void DisableUndo()
    {
        undoButton.interactable = false;
    }

    public void EnableReset()
    {
        resetButton.interactable = true;
    }

    public void DisableReset()
    {
        resetButton.interactable = false;
    }

    public void EnableNextLevel()
    {
        nextLevelButton.interactable = true;
    }

    public void DisableNextLevel()
    {
        nextLevelButton.interactable = false;
    }

    public void ShowVictoryPanel()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    public void HideVictoryPanel()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void OnUndoPressed()
    {
        UndoManager.Instance.Undo();
    }

    public void OnResetPressed()
    {
        UndoManager.Instance.ResetLevel();
    }

    public void OnNextLevelPressed()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}