using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class WaypointMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Rigidbody2D targetRigidbody;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float reachDistance = 0.1f;
    [SerializeField] private bool loopWaypoints = true;
    [SerializeField] private bool autoStart = false;

    [Header("Waypoint Visualization")]
    [SerializeField] private Color pathColor = Color.green;
    [SerializeField] private float gizmoRadius = 0.3f;

    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;
    private bool isMoving = false;

    void Start()
    {
        if (targetRigidbody == null)
        {
            Debug.LogError("Target Rigidbody2D is not assigned!");
            enabled = false;
            return;
        }

        RefreshWaypoints();

        if (autoStart && waypoints.Count > 0)
        {
            StartMoving();
        }
    }

    void Update()
    {
        if (!isMoving || waypoints.Count == 0) return;

        MoveToCurrentWaypoint();
    }

    void MoveToCurrentWaypoint()
    {
        Transform currentWaypoint = waypoints[currentWaypointIndex];
        Vector2 direction = (currentWaypoint.position - targetRigidbody.transform.position).normalized;
        Vector2 velocity = direction * moveSpeed;

        targetRigidbody.linearVelocity = velocity;

        if (Vector2.Distance(targetRigidbody.transform.position, currentWaypoint.position) <= reachDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Count)
            {
                if (loopWaypoints)
                {
                    currentWaypointIndex = 0;
                }
                else
                {
                    StopMoving();
                    targetRigidbody.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    public void StartMoving()
    {
        isMoving = true;
        currentWaypointIndex = 0;
    }

    public void StopMoving()
    {
        isMoving = false;
        targetRigidbody.linearVelocity = Vector2.zero;
    }

    private void RefreshWaypoints()
    {
        waypoints.Clear();

        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }

        waypoints.Sort((a, b) => string.Compare(a.name, b.name));
    }

    public void AddWaypoint()
    {
        GameObject newWaypoint = new GameObject("Waypoint " + (waypoints.Count + 1));
        newWaypoint.transform.SetParent(transform);
        newWaypoint.transform.position = transform.position + Vector3.right * (waypoints.Count + 1) * 2;

        RefreshWaypoints();
    }

    public void ClearWaypoints()
    {
        foreach (Transform waypoint in waypoints)
        {
            if (waypoint != null)
                DestroyImmediate(waypoint.gameObject);
        }
        waypoints.Clear();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        DrawPathGizmos();
    }
#endif

    private void DrawPathGizmos()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        foreach (Transform waypoint in waypoints)
        {
            if (waypoint == null) continue;

            Gizmos.color = pathColor;
            Gizmos.DrawWireSphere(waypoint.position, gizmoRadius);

            GUIStyle style = new GUIStyle();
            style.normal.textColor = pathColor;
            Handles.Label(waypoint.position + Vector3.up * 0.5f,
                "Waypoint " + (waypoints.IndexOf(waypoint) + 1), style);
        }

        Gizmos.color = Color.Lerp(pathColor, Color.white, 0.7f);

        for (int i = 0; i < waypoints.Count; i++)
        {
            Transform current = waypoints[i];
            Transform next = waypoints[(i + 1) % waypoints.Count];

            if (current != null && next != null)
            {
                Gizmos.DrawLine(current.position, next.position);
            }
        }

        if (isMoving && currentWaypointIndex < waypoints.Count && waypoints[currentWaypointIndex] != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(waypoints[currentWaypointIndex].position, gizmoRadius * 1.5f);
        }
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(WaypointMover))]
public class WaypointMoverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WaypointMover mover = (WaypointMover)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Waypoint Management", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Waypoint"))
        {
            mover.AddWaypoint();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        if (GUILayout.Button("Clear All Waypoints"))
        {
            mover.ClearWaypoints();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        EditorGUILayout.Space();

        //GUILayout.Label("Current Waypoints: " + mover.GetWaypointCount());
    }
}
#endif

