using UnityEngine;
using System.Collections;

public class TraceManager : MonoBehaviour
{
    public static TraceManager Instance { get; private set; }
    

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void DrawLine(Vector3 startPoint, Vector3 endPoint, GameObject linePrefab, float lineLifetime)
    {
        if (linePrefab == null)
        {
            Debug.LogError("Line prefab is null!");
            return;
        }

        (GameObject lineInstance, bool isNew) = Pooler.PoolSpawn(linePrefab, transform.position, Quaternion.identity);
        LineRenderer lineRenderer = lineInstance.GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            Debug.LogError("Line prefab doesn't have LineRenderer component!");
            Destroy(lineInstance);
            return;
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

        if (lineLifetime > 0)
        {
            StartCoroutine(QueryDespawn(lineInstance, lineLifetime));
        }
    }

    public void DrawLineOverTime(Vector3 startPoint, Vector3 endPoint, GameObject linePrefab, float drawDuration, float lineLifetime)
    {
        if (linePrefab == null)
        {
            Debug.LogError("Line prefab is null!");
            return;
        }

        (GameObject lineInstance, bool isNew) = Pooler.PoolSpawn(linePrefab, transform.position, Quaternion.identity);
        LineRenderer lineRenderer = lineInstance.GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            Debug.LogError("Line prefab doesn't have LineRenderer component!");
            Destroy(lineInstance);
            return;
        }

        StartCoroutine(DrawLineAnimation(lineRenderer, startPoint, endPoint, drawDuration, lineLifetime));
    }

    private IEnumerator QueryDespawn(GameObject go, float destroyAfter)
    {
        yield return new WaitForSeconds(destroyAfter);
        Pooler.PoolDespawn(go);
    }

    private IEnumerator DrawLineAnimation(LineRenderer lineRenderer, Vector3 startPoint, Vector3 endPoint, float drawDuration, float totalLifetime)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, startPoint);

        float elapsedTime = 0f;

        while (elapsedTime < drawDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / drawDuration);

            Vector3 currentEndPoint = Vector3.Lerp(startPoint, endPoint, progress);
            lineRenderer.SetPosition(1, currentEndPoint);
            yield return null;
        }

        lineRenderer.SetPosition(1, endPoint);

        float remainingLifetime = totalLifetime - drawDuration;
        if (remainingLifetime > 0)
        {
            StartCoroutine(QueryDespawn(lineRenderer.gameObject, remainingLifetime));
        }
        else
        {
            Pooler.PoolDespawn(lineRenderer.gameObject);
        }
    }
}
