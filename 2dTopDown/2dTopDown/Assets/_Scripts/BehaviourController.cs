using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourController : MonoBehaviour
{
    public List<AIBehaviour> CalmBehaviours;
    private List<AIBehaviour> _usedCalmBehaviours = new List<AIBehaviour>();
    public bool ShuffleCalm;

    public List<AIBehaviour> SenseBehaviours;
    private List<AIBehaviour> _usedSenseBehaviours = new List<AIBehaviour>();

    public bool ShuffleSense;

    public List<AIBehaviour> AggressiveBehaviours;
    private List<AIBehaviour> _usedAggressiveBehaviours = new List<AIBehaviour>();
    public bool AggressiveCalm;

    public AIState AIState;

    private AIBehaviour _currentBehaviour;
    private System.Random _rnd;

    private void Awake()
    {
        if (PhotonNetwork.isMasterClient)
        {
            _rnd = new System.Random();
            //CalmBehaviours.Shuffle(_rnd);
            //SenseBehaviours.Shuffle(_rnd);
            //AggressiveBehaviours.Shuffle(_rnd);
        }
    }

    private void Start()
    {
        ChangeState(AIState.Calm);
    }

    public void ChangeState(AIState newstate)
    { 
        if (AIState == newstate) return;
        Debug.Log(newstate);
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
            case AIState.Dead:
                _currentBehaviour = null;
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
        //  baseList.Shuffle(_rnd);
    }


    public void Death()
    {
        ChangeState(AIState.Dead);
    }
}

public enum AIState
{
    None = 0,
    Calm = 10,
    Sense = 20,
    Agessive = 30,
    Dead = 40,
}