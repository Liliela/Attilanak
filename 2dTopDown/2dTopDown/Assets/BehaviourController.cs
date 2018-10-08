using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourController : MonoBehaviour
{
    public List<AIBehaviour> CalmBehaviours;
    private List<AIBehaviour> _usedCalmBehaviours = new List<AIBehaviour>();

    public List<AIBehaviour> SenseBehaviours;
    private List<AIBehaviour> _usedSenseBehaviours = new List<AIBehaviour>();

    public List<AIBehaviour> AggressiveBehaviours;
    private List<AIBehaviour> _usedAggressiveBehaviours = new List<AIBehaviour>();

    public AIState AIState;

    private AIBehaviour _currentBehaviour;
    private System.Random _rnd;

    private void Awake()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            _rnd = new System.Random();
            CalmBehaviours.Shuffle(_rnd);
            SenseBehaviours.Shuffle(_rnd);
            AggressiveBehaviours.Shuffle(_rnd);
        }
        else
        {
            foreach (var item in CalmBehaviours)
            {
                Destroy(item);
            }
            foreach (var item in SenseBehaviours)
            {
                Destroy(item);
            }
            foreach (var item in AggressiveBehaviours)
            {
                Destroy(item);
            }
        }
    }

    public void ChangeState(AIState newstate)
    {
        if (AIState == newstate) return;

        if (_currentBehaviour)
        {
            _currentBehaviour.ExitBehaviour();
        }

        AIState = newstate;

        switch (newstate)
        {
            case AIState.Calm:
                _currentBehaviour = GetNextBehaviour(CalmBehaviours, _usedCalmBehaviours);
                break;
            case AIState.Sense:
                _currentBehaviour = GetNextBehaviour(SenseBehaviours, _usedSenseBehaviours);
                break;
            case AIState.Agessive:
                _currentBehaviour = GetNextBehaviour(AggressiveBehaviours, _usedAggressiveBehaviours);
                break;
            default:
                break;
        }

        if (_currentBehaviour)
        {
            _currentBehaviour.EnterBehaviour();
        }
    }

    private AIBehaviour GetNextBehaviour(List<AIBehaviour> baseList, List<AIBehaviour> usedList)
    {
        AIBehaviour b = null;
        if (baseList.Count == 0 && usedList.Count == 0)
        {
            return b;
        }

        if (baseList.Count > 0)
        {
            b = baseList[0];
            usedList.Add(b);
            baseList.Remove(b);
            return b;
        }
        else
        {
            Reshuffle(baseList, usedList);
            return GetNextBehaviour(baseList, usedList);
        }
    }

    private void Reshuffle(List<AIBehaviour> baseList, List<AIBehaviour> usedList)
    {
        baseList.Clear();
        foreach (var item in usedList)
        {
            baseList.Add(item);
        }
        usedList.Clear();
        baseList.Shuffle(_rnd);
    }
}

public enum AIState
{
    Calm = 0,
    Sense = 10,
    Agessive = 20,
}