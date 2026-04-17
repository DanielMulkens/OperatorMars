using UnityEngine;

public class SimpleCavePortal : MonoBehaviour
{
    [Header("Relative Teleport Offset")]
    public Vector3 teleportOffset;

    public bool robotInside = false;

    private void OnEnable()
    {
        CaveManager.Register(this);
    }

    private void OnDisable()
    {
        CaveManager.Unregister(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        RobotController robot = other.GetComponentInParent<RobotController>();

        if (robot != null)
        {
            robotInside = true;
            Debug.Log($"{gameObject.name}: Robot ENTERED");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RobotController robot = other.GetComponentInParent<RobotController>();

        if (robot != null)
        {
            robotInside = false;
            Debug.Log($"{gameObject.name}: Robot EXITED");
        }
    }

    public void Teleport(RobotController robot)
    {
        robot.transform.position += teleportOffset;
        Debug.Log($"{gameObject.name}: Teleport executed after sequence");
    }
}