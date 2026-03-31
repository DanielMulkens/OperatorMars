using UnityEngine;
using System.Collections.Generic;

public class SequenceUI : MonoBehaviour
{
    [Header("UI Containers (Sequence Lines)")]
    public Transform[] sequenceLines;     // Assign lines in inspector
    public int lineCapacity = 8;          // Max commands per line

    [Header("Command Icons")]
    public GameObject moveForwardIcon;
    public GameObject turnLeftIcon;
    public GameObject turnRightIcon;
    public GameObject drillIcon;

    private List<CommandType> sequence = new List<CommandType>();

    private int TotalCapacity => sequenceLines.Length * lineCapacity;

    // Adds a command to the sequence UI (optional for your system)
    public bool AddCommand(CommandType command)
    {
        if (sequence.Count >= TotalCapacity)
            return false;

        sequence.Add(command);

        GameObject iconPrefab = GetIconPrefab(command);

        if (iconPrefab == null)
            return false;

        // Find first line with space
        for (int i = 0; i < sequenceLines.Length; i++)
        {
            if (sequenceLines[i].childCount < lineCapacity)
            {
                Instantiate(iconPrefab, sequenceLines[i]);
                break;
            }
        }

        return true;
    }

    public void RemoveLastCommand()
    {
        if (sequence.Count == 0) return;

        sequence.RemoveAt(sequence.Count - 1);

        for (int i = sequenceLines.Length - 1; i >= 0; i--)
        {
            if (sequenceLines[i].childCount > 0)
            {
                Transform lastIcon = sequenceLines[i].GetChild(sequenceLines[i].childCount - 1);
                Destroy(lastIcon.gameObject);
                break;
            }
        }
    }

    public void ClearSequence()
    {
        sequence.Clear();
        foreach (Transform line in sequenceLines)
        {
            for (int i = line.childCount - 1; i >= 0; i--)
                Destroy(line.GetChild(i).gameObject);
        }
    }

    public List<CommandType> GetSequence()
    {
        return new List<CommandType>(sequence);
    }

    private GameObject GetIconPrefab(CommandType command)
    {
        switch (command)
        {
            case CommandType.MoveForward: return moveForwardIcon;
            case CommandType.TurnLeft: return turnLeftIcon;
            case CommandType.TurnRight: return turnRightIcon;
            case CommandType.UseDrill: return drillIcon;
        }
        return null;
    }
}