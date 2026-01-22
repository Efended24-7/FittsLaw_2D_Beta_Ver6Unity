using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SquareController : MonoBehaviour
{
    [Header("UI (optional)")]
    public TMP_Text stageLabel;

    [Header("Stage Settings")]
    public float minMoveDistance = 1.0f;
    public int maxPositionTries = 20;

    private Camera cam;
    private Collider2D col;

    private int stage = 1;
    private Vector3 lastPos;

    void Awake()
    {
        cam = Camera.main;
        col = GetComponent<Collider2D>();

        if (cam == null) Debug.LogError("Main Camera not found. Tag your camera as MainCamera.");
        if (col == null) Debug.LogError("Collider2D missing on Square (add BoxCollider2D).");
    }

    void Start()
    {
        lastPos = transform.position;
        UpdateStageLabel();
    }

    void Update()
    {
        if (cam == null || col == null) return;

        // Mouse click
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            TryClick(screenPos);
            return;
        }
    }

    private void TryClick(Vector2 screenPos)
    {
        Vector2 worldPos = cam.ScreenToWorldPoint(screenPos);

        // Only react if THIS square was clicked
        if (!col.OverlapPoint(worldPos))
            return;

        // Advance stage + move square here
        stage++;
        MoveSquareToNewRandomSpot();
        UpdateStageLabel();

        Debug.Log($"Square clicked! Stage {stage}");
    }

    private void MoveSquareToNewRandomSpot()
    {
        // Assumes an orthographic camera for 2D
        float vertExtent = cam.orthographicSize;
        float horizExtent = vertExtent * cam.aspect;

        // Use SpriteRenderer bounds to keep the square fully on-screen
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer missing on Square.");
            return;
        }

        Vector3 halfSize = sr.bounds.extents;

        float camX = cam.transform.position.x;
        float camY = cam.transform.position.y;

        float minX = camX - horizExtent + halfSize.x;
        float maxX = camX + horizExtent - halfSize.x;
        float minY = camY - vertExtent + halfSize.y;
        float maxY = camY + vertExtent - halfSize.y;

        // Safety: if the square is bigger than the view, clamp to camera center
        if (minX > maxX) { minX = maxX = camX; }
        if (minY > maxY) { minY = maxY = camY; }

        Vector3 candidate = transform.position;

        for (int i = 0; i < maxPositionTries; i++)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            candidate = new Vector3(x, y, transform.position.z);

            if (Vector2.Distance(candidate, lastPos) >= minMoveDistance)
                break;
        }

        transform.position = candidate;
        lastPos = candidate;
    }

    private void UpdateStageLabel()
    {
        if (stageLabel != null)
            stageLabel.text = $"Stage {stage}";
    }
}
