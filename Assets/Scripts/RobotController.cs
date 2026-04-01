using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 2f;
    public float rotateSpeed = 180f;

    [Header("Obstacle Detection")]
    public LayerMask obstacleLayer;     // Assign "Obstacle" layer in inspector
    public float checkDistance = 1f;    // Distance to check in front of robot

    public IEnumerator MoveForward()
    {
        // Check if an obstacle is directly in front of the robot
        if (Physics.Raycast(transform.position, transform.forward, checkDistance, obstacleLayer))
        {
            Debug.Log("Move blocked by obstacle.");

            // Optional: small delay so the command still "feels" executed
            yield return new WaitForSeconds(0.1f);

            yield break; // Skip movement but consume command
        }

        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * moveDistance;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }

    public IEnumerator TurnLeft()
    {
        Quaternion start = transform.rotation;
        Quaternion end = start * Quaternion.Euler(0, -90, 0);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * (rotateSpeed / 90f);
            transform.rotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }
    }

    public IEnumerator TurnRight()
    {
        Quaternion start = transform.rotation;
        Quaternion end = start * Quaternion.Euler(0, 90, 0);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * (rotateSpeed / 90f);
            transform.rotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }
    }

    public IEnumerator UseDrill()
    {
        // Placeholder for drill animation
        yield return new WaitForSeconds(0.5f);
    }
}