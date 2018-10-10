using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralStatistics : Photon.MonoBehaviour
{
    public string Name;
    public Image HpBar;
    public bool Dead;

    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    [SerializeField]
    private float _moveSpeed;

    public float HealthMax
    {
        get { return _healthMax; }
        set { _healthMax = value; }
    }
    [SerializeField]
    private float _healthMax;

    public float HealthActual
    {
        get { return _healthActual; }
        set
        {
            if (value > HealthMax)
            {
                _healthActual = HealthMax;
            }
            else if (value <= 0)
            {
                Death();
                _healthActual = 0;
            }
            else
            {
                _healthActual = value;
            }

            if (HpBar)
                HpBar.fillAmount = _healthActual / HealthMax;
        }
    }
    [SerializeField]
    private float _healthActual;

    public Resistence Resistence;

    private List<EnemySensor> _monsterSenses = new List<EnemySensor>();
    private Animator _anim;

    protected virtual void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void AddMonsterSense(EnemySensor enemySensor)
    {

        if (!_monsterSenses.Contains(enemySensor))
        {
            _monsterSenses.Add(enemySensor);
        }
    }

    public void RemoveMonsterSenser(EnemySensor enemySensor)
    {
        if (_monsterSenses.Contains(enemySensor))
        {
            _monsterSenses.Remove(enemySensor);
        }
    }

    public virtual void Death()
    {
        _anim.SetBool("Dead", true);
        Dead = true;
        List<EnemySensor> removeSense = new List<EnemySensor>();
        foreach (var sense in _monsterSenses)
        {
            removeSense.Add(sense);
        }
        foreach (var sense in removeSense)
        {
            sense.RemovePlayer(GetComponent<PhotonPlayerController>());
        }
    }
    [PunRPC]
    public virtual void RPC_Revice(bool percent, float value)
    {
        _anim.SetBool("Dead", false);
        Dead = false;
        if (percent)
        {
            ChangeHealth(HealthMax * value, EnergyType.Positive);
        }
        else
        {
            ChangeHealth(value, EnergyType.Positive);
        }
    }

    [PunRPC]
    public void RPC_ChangeHealth(float change, string typeInString)
    {
        EnergyType type = (EnergyType)Enum.Parse(typeof(EnergyType), typeInString, true);
        ChangeHealth(change, type);
    }

    public virtual void ChangeHealth(float change, EnergyType energyType)
    {
        Debug.Log(transform.name + " Take " + change + " hpchange from " + energyType);
        float damage = CalculateResistence(change, energyType);
        if (damage < 0)
        {
            _anim.SetTrigger("Hurt");
        }

        Debug.Log(transform.name + " Get " + damage + " hp");
        HealthActual += damage;
    }

    private float CalculateResistence(float change, EnergyType energyType)
    {
        float damage = change;
        switch (energyType)
        {
            case EnergyType.Fire:
                damage = CalculateResistToOneType(damage, Resistence.Fire);
                break;
            case EnergyType.Air:
                damage = CalculateResistToOneType(damage, Resistence.Air);
                break;
            case EnergyType.Earth:
                damage = CalculateResistToOneType(damage, Resistence.Earth);
                break;
            case EnergyType.Water:
                damage = CalculateResistToOneType(damage, Resistence.Water);
                break;
            case EnergyType.Negative:
                damage = CalculateResistToOneType(damage, Resistence.Negative);
                break;
            case EnergyType.Positive:
                damage = CalculateResistToOneType(damage, Resistence.Positive);
                break;
            case EnergyType.Physical:
                damage = CalculateResistToOneType(damage, Resistence.Physical);
                break;
            default:
                break;
        }
        return damage;
    }

    private float CalculateResistToOneType(float damage, float resist)
    {
        float dr = (100 - resist) / 100;
        if (dr <= 1)
        {
            damage *= dr;
        }
        else
        {
            damage *= -(dr - 1);
        }

        return damage;
    }
}

[Serializable]
public class Resistence
{
    public float Fire
    {
        get { return _fire; }
        set { _fire = value; }
    }
    [SerializeField]
    private float _fire;

    public float Air
    {
        get { return _air; }
        set { _air = value; }
    }
    [SerializeField]
    private float _air;

    public float Earth
    {
        get { return _earth; }
        set { _earth = value; }
    }
    [SerializeField]
    private float _earth;

    public float Water
    {
        get { return _water; }
        set { _water = value; }
    }
    [SerializeField]
    private float _water;

    public float Negative
    {
        get { return _negativ; }
        set { _negativ = value; }
    }
    [SerializeField]
    private float _negativ;

    public float Positive
    {
        get { return _positive; }
        set { _positive = value; }
    }
    [SerializeField]
    private float _positive;

    public float Physical
    {
        get { return _physical; }
        set { _physical = value; }
    }
    [SerializeField]
    private float _physical;
}