using UnityEngine;

public class Hopper : MonoBehaviour
{
    public SequenceUI sequenceUI;

    private void OnTriggerEnter(Collider other)
    {
        CommandBlock block = other.GetComponent<CommandBlock>();
        if (block != null)
        {
            // Only add if sequence has space
            bool added = sequenceUI.AddCommand(block.commandType);
            if (added)
            {
                Destroy(other.gameObject);
            }
            else
            {
                // Optional: feedback if full
                Debug.Log("Sequence is full! Cannot add more commands.");
            }
        }
    }
}