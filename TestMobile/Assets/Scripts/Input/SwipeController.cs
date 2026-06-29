using UnityEngine;

/// Gestisce il sistema di input mobile.
///
/// Funzionamento:
/// 1. Il giocatore tocca una pila.
/// 2. Trascina il dito.
/// 3. Viene calcolata la direzione.
/// 4. La pila viene passata al MoveManager.
public class SwipeController : MonoBehaviour
{
    public static SwipeController Instance;

    [Header("Distanza minima per considerare uno swipe")]
    [SerializeField]
    private float minimumSwipeDistance = 50f;

    private Vector2 startPosition;
    private Vector2 endPosition;

    private StackGroup selectedStack;

    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
#if UNITY_EDITOR

        HandleMouseInput();

#else

        HandleTouchInput();

#endif
    }

    #region MOBILE

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:

                startPosition = touch.position;

                SelectStack(touch.position);

                break;

            case TouchPhase.Ended:

                endPosition = touch.position;

                ProcessSwipe();

                break;
        }
    }

    #endregion

    #region EDITOR

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;

            SelectStack(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPosition = Input.mousePosition;

            ProcessSwipe();
        }
    }

    #endregion

    #region SELEZIONE

    /// Individua la pila toccata dal giocatore.
    private void SelectStack(Vector2 screenPosition)
    {
        selectedStack = null;

        Ray ray =
            mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            selectedStack =
                hit.collider.GetComponent<StackGroup>();

            if (selectedStack != null)
            {
                Debug.Log(
                    "Pila selezionata: "
                    + selectedStack.name
                );
            }
        }
    }

    #endregion

    #region SWIPE

    /// Analizza il movimento del dito.
    private void ProcessSwipe()
    {
        if (selectedStack == null)
            return;

        Vector2 delta =
            endPosition - startPosition;

        if (delta.magnitude <
            minimumSwipeDistance)
        {
            return;
        }

        SwipeDirection direction =
            CalculateDirection(delta);

        MoveManager.Instance.TryMoveStack(
            selectedStack,
            direction
        );
    }

    /// Converte il vettore di movimento
    /// in una direzione.
    private SwipeDirection CalculateDirection(
        Vector2 delta)
    {
        if (Mathf.Abs(delta.x) >
            Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
            {
                return SwipeDirection.Right;
            }

            return SwipeDirection.Left;
        }
        else
        {
            if (delta.y > 0)
            {
                return SwipeDirection.Up;
            }

            return SwipeDirection.Down;
        }
    }

    #endregion
}