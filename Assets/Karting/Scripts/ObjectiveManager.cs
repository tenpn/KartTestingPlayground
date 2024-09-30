using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectiveManager : MonoBehaviour
{
    List<Objective> m_Objectives = new List<Objective>();

    public List<Objective> Objectives => m_Objectives;

    public static Action<Objective> RegisterObjective;

    public void OnEnable()
    {
        RegisterObjective += OnRegisterObjective;
    }

    /// <returns>false if no objectives, or if any objective is blocking</returns>
    public bool AreAllObjectivesCompleted()
    {
        if (m_Objectives.Count == 0)
            return false;

        for (int i = 0; i < m_Objectives.Count; i++)
        {
            Assert.IsNotNull(m_Objectives[i], "expected valid objective");
            // pass every objectives to check if they have been completed
            if (m_Objectives[i].isBlocking())
            {
                // break the loop as soon as we find one uncompleted objective
                return false;
            }
        }

        // found no uncompleted objective
        return true;
    }

    /// <param name="objective">should not be null, _may_ be blocking</param>
    public void OnRegisterObjective(Objective objective)
    {
        Assert.IsNotNull(objective, "expected valid objective");
        m_Objectives.Add(objective);
    }
}
