using UnityEngine;

/// Punto centrale dell'applicazione.
///
/// Coordina i manager principali
/// e prepara il gioco all'avvio.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
}