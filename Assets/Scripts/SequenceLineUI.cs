using UnityEngine;
using System.Collections.Generic;

public class SequenceUI : MonoBehaviour
{
    [Header("UI Containers (Sequence Lines)")]
    public Transform[] sequenceLines; // assign 4 lines in inspector
    public int lineCapacity = 8;      // max per line

    [Header("Command Icons")]
    public GameObject moveForwardIcon;
    public GameObject turnLeftIcon;
    public GameObject turnRightIcon;
    public GameObject drillIcon;

    private List<CommandType> sequence = new List<CommandType>();

    private int totalCapacity => sequenceLines.Length * lineCapacity;

    /// <summary>
    /// Adds a command to the first available sequence line.
    /// Returns true if successfully added, false if full.
    /// </summary>
    public bool AddCommand(CommandType command)
    {
        if (sequence.Count >= totalCapacity)
            return false; // cannot add more than total capacity (32)

        sequence.Add(command);

        // Find the first line with space
        for (int i = 0; i < sequenceLines.Length; i++)
        {
            if (sequenceLines[i].childCount < lineCapacity)
            {
                GameObject iconPrefab = GetIconPrefab(command);
                if (iconPrefab != null)
                    Instantiate(iconPrefab, sequenceLines[i]);
                break;
            }
        }

        return true;
    }

    GameObject GetIconPrefab(CommandType command)
    {
        switch (command)
        {
            case CommandType.MoveForward: return moveForwardIcon;
            case CommandType.TurnLeft:    return turnLeftIcon;
            case CommandType.TurnRight:   return turnRightIcon;
            case CommandType.UseDrill:    return drillIcon;
        }
        return null;
    }

    /// <summary>
    /// Clears all sequence lines
    /// </summary>
    public void ClearSequence()
    {
        sequence.Clear();
        foreach (Transform line in sequenceLines)
        {
            foreach (Transform child in line)
                Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Remove the last command from the sequence
    /// </summary>
    public void RemoveLastCommand()
    {
        if (sequence.Count == 0) return;

        sequence.RemoveAt(sequence.Count - 1);

        // Remove the last icon from the last non-empty line
        for (int i = sequenceLines.Length - 1; i >= 0; i--)
        {
            if (sequenceLines[i].childCount > 0)
            {
                Destroy(sequenceLines[i].GetChild(sequenceLines[i].childCount - 1).gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// Total number of commands currently in sequence
    /// </summary>
    public int CommandCount => sequence.Count;
}