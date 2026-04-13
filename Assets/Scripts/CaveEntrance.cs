using UnityEngine;

public class CaveEntrance : MonoBehaviour
{
    public CaveEntrance linkedEntrance;
    public Transform exitPoint;

    public void Teleport(RobotController robot)
    {
        if (linkedEntrance == null)
        {
            Debug.LogWarning("No linked entrance assigned on CaveEntrance: " + gameObject.name);
            return;
        }

        Transform target = linkedEntrance.exitPoint;

        if (target == null)
        {
            Debug.LogWarning("Linked entrance has no exit point: " + linkedEntrance.gameObject.name);
            return;
        }

        robot.transform.position = target.position;
        robot.transform.rotation = target.rotation;

        Debug.Log($"Robot teleported from {gameObject.name} to {target.name}");
    }
}