using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour
{
    [Header("Movement")]
    public float moveDistance = 1f;
    public float moveSpeed = 2f;
    public float rotateSpeed = 180f;

    [Header("Obstacle Detection")]
    public LayerMask obstacleLayer;
    public LayerMask destructableLayer;

    public float checkDistance = 1.1f;
    public float drillDistance = 1.2f;
    public float rayHeight = 0.5f;

    private LayerMask movementBlockLayer;

    void Start()
    {
        // Combine both layers so both block movement
        movementBlockLayer = obstacleLayer | destructableLayer;
    }

    public IEnumerator MoveForward()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * rayHeight;

        // Block movement if obstacle OR destructable object
        if (Physics.Raycast(rayOrigin, transform.forward, checkDistance, movementBlockLayer))
        {
            Debug.Log("Move blocked");

            Debug.DrawRay(rayOrigin, transform.forward * checkDistance, Color.red, 1f);

            yield return new WaitForSeconds(0.1f);
            yield break;
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
        RaycastHit hit;

        Vector3 rayOrigin = transform.position + Vector3.up * rayHeight;

        // Only detect destructable objects
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, drillDistance, destructableLayer))
        {
            Debug.Log("Drilled object: " + hit.collider.name);

            Debug.DrawRay(rayOrigin, transform.forward * drillDistance, Color.blue, 1f);

            Destructible destructible = hit.collider.GetComponentInParent<Destructible>();

            if (destructible != null)
            {
                destructible.DestroyObject();
            }

            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Debug.Log("Nothing to drill");

            Debug.DrawRay(rayOrigin, transform.forward * drillDistance, Color.yellow, 1f);

            yield return new WaitForSeconds(0.2f);
        }
    }
}