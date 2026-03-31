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

    /// <summary>
    /// Adds a command to the first available sequence line.
    /// </summary>
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

    /// <summary>
    /// Removes the last command added
    /// </summary>
    public void RemoveLastCommand()
    {
        if (sequence.Count == 0)
            return;

        sequence.RemoveAt(sequence.Count - 1);

        // Remove icon from last non-empty line
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

    /// <summary>
    /// Clears the entire sequence
    /// </summary>
    public void ClearSequence()
    {
        sequence.Clear();

        foreach (Transform line in sequenceLines)
        {
            for (int i = line.childCount - 1; i >= 0; i--)
            {
                Destroy(line.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// Returns the command sequence
    /// </summary>
    public List<CommandType> GetSequence()
    {
        return new List<CommandType>(sequence);
    }

    /// <summary>
    /// Number of commands currently in sequence
    /// </summary>
    public int CommandCount
    {
        get { return sequence.Count; }
    }

    /// <summary>
    /// Gets the correct icon prefab for a command
    /// </summary>
    GameObject GetIconPrefab(CommandType command)
    {
        switch (command)
        {
            case CommandType.MoveForward:
                return moveForwardIcon;

            case CommandType.TurnLeft:
                return turnLeftIcon;

            case CommandType.TurnRight:
                return turnRightIcon;

            case CommandType.UseDrill:
                return drillIcon;
        }

        return null;
    }
}