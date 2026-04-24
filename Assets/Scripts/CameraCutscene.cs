using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{
    [System.Serializable]
    public class Waypoint
    {
        public Transform target;
        public float duration = 2f;
    }

    [Header("Waypoints")]
    public List<Waypoint> waypoints = new List<Waypoint>();
    public float delayBeforeStart = 1f;

    [Header("Camera References")]
    public Camera playerCamera;
    public MonoBehaviour playerCameraScript; // optional

    private Camera thisCamera;
    private Transform originalParent;

    void Awake()
    {
        thisCamera = GetComponent<Camera>();
        originalParent = transform.parent;

        // Disable player camera
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);

        // Enable cutscene camera
        gameObject.SetActive(true);

        // Ensure this camera is active
        thisCamera.enabled = true;
        thisCamera.tag = "MainCamera";

        // Disable player camera controller if assigned
        if (playerCameraScript != null)
            playerCameraScript.enabled = false;

        // Move instantly to first waypoint
        if (waypoints.Count > 0 && waypoints[0].target != null)
        {
            transform.position = waypoints[0].target.position;
            transform.rotation = waypoints[0].target.rotation;
        }
    }

    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        foreach (Waypoint wp in waypoints)
        {
            yield return StartCoroutine(MoveToWaypoint(wp));
        }

        EndCutscene();
    }

    IEnumerator MoveToWaypoint(Waypoint wp)
    {
        float time = 0f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (time < wp.duration)
        {
            float t = time / wp.duration;
            t = Mathf.SmoothStep(0, 1, t);

            transform.position = Vector3.Lerp(startPos, wp.target.position, t);
            transform.rotation = Quaternion.Slerp(startRot, wp.target.rotation, t);

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = wp.target.position;
        transform.rotation = wp.target.rotation;
    }

    void EndCutscene()
    {
        // Disable cutscene camera
        thisCamera.enabled = false;

        // Enable player camera
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
            playerCamera.tag = "MainCamera";
        }

        // Re-enable player control
        if (playerCameraScript != null)
            playerCameraScript.enabled = true;

        Debug.Log("Cutscene Finished");
    }
}