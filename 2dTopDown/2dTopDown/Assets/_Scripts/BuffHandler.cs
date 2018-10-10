using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    public List<BuffInstance> CurrentBuffs;
    public List<BuffElement> AvalibleBuffElements;
    public GameObject BuffPrefab;
    public Transform BuffParent;
    private GeneralStatistics _stat;
    public bool Visualize;

    private void Awake()
    {
        _stat = GetComponent<GeneralStatistics>();
        if (Visualize)
            BuffPrefab.SetActive(false);
    }

    private void Update()
    {
        List<BuffInstance> removeBuffs = new List<BuffInstance>();
        foreach (var buff in CurrentBuffs)
        {
            buff.Duration -= Time.deltaTime;
            if (buff.Duration < 0)
            {
                removeBuffs.Add(buff);
                if (Visualize)
                    buff.BuffElement.Deactivate();
            }
            else
            {
                ApplyBuffEffect(buff);
                if (Visualize)
                    buff.BuffElement.Refresh(buff.Duration, buff.FullDuration);
            }
        }
        foreach (var item in removeBuffs)
        {
            CurrentBuffs.Remove(item);
        }
    }

    private void ApplyBuffEffect(BuffInstance buff)
    {
        switch (buff.buffDescriptor.BuffType)
        {
            case BuffCategory.PositiveEnergyWave:
                switch (buff.buffDescriptor.BuffChangeType)
                {
                    case BuffChangeType.StatPercentChange:
                        _stat.ChangeHealth(-buff.buffDescriptor.Change * Time.deltaTime, EnergyType.Positive);
                        //_stat.photonView.RPC("RPC_ChangeHealthPercent", PhotonTargets.AllBuffered, buff.buffDescriptor.Change, EnergyType.Positive.ToString());
                        break;
                    case BuffChangeType.StatChange:
                        _stat.ChangeHealthPercent(-buff.buffDescriptor.Change * Time.deltaTime, EnergyType.Positive);
                        //_stat.photonView.RPC("RPC_ChangeHealth", PhotonTargets.AllBuffered, buff.buffDescriptor.Change, EnergyType.Positive.ToString());               
                        break;
                    default:
                        break;
                }
                break;
            case BuffCategory.NegativeEnergyWave:
                switch (buff.buffDescriptor.BuffChangeType)
                {
                    case BuffChangeType.StatPercentChange:
                        _stat.ChangeHealth(-buff.buffDescriptor.Change * Time.deltaTime, EnergyType.Negative);
                        //_stat.photonView.RPC("RPC_ChangeHealthPercent", PhotonTargets.AllBuffered, buff.buffDescriptor.Change, EnergyType.Positive.ToString());
                        break;
                    case BuffChangeType.StatChange:
                        _stat.ChangeHealthPercent(-buff.buffDescriptor.Change * Time.deltaTime, EnergyType.Negative);
                        //_stat.photonView.RPC("RPC_ChangeHealth", PhotonTargets.AllBuffered, buff.buffDescriptor.Change, EnergyType.Positive.ToString());               
                        break;
                    default:
                        break;
                }
                break;
            case BuffCategory.ManaRegen:
                break;
            case BuffCategory.DamageChange:
                break;
            case BuffCategory.Bleed:
                break;
            case BuffCategory.Poison:
                break;
            case BuffCategory.Burn:
                break;
            case BuffCategory.Freeze:
                break;
            case BuffCategory.MoveChange:
                break;
            case BuffCategory.Inmobile:
                break;
            case BuffCategory.Stun:
                break;
            default:
                break;
        }
    }

    internal void ApplyBuff(BuffDescriptor newBuffDescriptor)
    {
        BuffInstance match = CurrentBuffs.Find(x => x.buffDescriptor.name == newBuffDescriptor.name);
        if (match != null)
        {
            StackBuff(match);
        }
        else
        {
            if (Visualize)
            {
                BuffElement buffElement = GetAvalibleBuffElement();
                if (buffElement != null)
                {
                    BuffInstance newBUff = new BuffInstance();
                    newBUff.buffDescriptor = newBuffDescriptor;
                    newBUff.Duration = newBuffDescriptor.Duration;
                    newBUff.FullDuration = newBuffDescriptor.Duration;
                    newBUff.BuffElement = buffElement;
                    buffElement.Activate(newBUff.buffDescriptor);
                    CurrentBuffs.Add(newBUff);
                }
                else
                {
                    Debug.LogWarning("Dont Get Buff Element");
                }
            }
            else
            {
                BuffInstance newBUff = new BuffInstance();
                newBUff.buffDescriptor = newBuffDescriptor;
                newBUff.Duration = newBuffDescriptor.Duration;
                newBUff.FullDuration = newBuffDescriptor.Duration;
                CurrentBuffs.Add(newBUff);
            }
        }
    }

    private BuffElement GetAvalibleBuffElement()
    {
        BuffElement buffElement = null;
        foreach (var be in AvalibleBuffElements)
        {
            if (!be.Active)
            {
                buffElement = be;
            }
        }

        if (!buffElement)
        {
            GameObject go = Instantiate(BuffPrefab, BuffParent);
            buffElement = go.GetComponent<BuffElement>();
            AvalibleBuffElements.Add(buffElement);
        }

        return buffElement;
    }

    private void StackBuff(BuffInstance match)
    {
        switch (match.buffDescriptor.BuffStackingType)
        {
            case BuffStackingType.Refresh:
                match.Duration = match.buffDescriptor.Duration;
                break;
            case BuffStackingType.StackInTime:
                match.Duration += match.buffDescriptor.Duration;
                match.FullDuration += match.buffDescriptor.Duration;
                break;
            default:
                break;
        }
    }
}

[Serializable]
public class BuffInstance
{
    public BuffElement BuffElement;
    public BuffDescriptor buffDescriptor;
    public float Duration;
    public float FullDuration;
}
