using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : AIBehaviour
{
    public float moveVelocityMultiplier = 20f;
    public Transform FollowTransform;
    public float StopDistance = 1f;

    public TargetingType TargetingType;

    private Rigidbody2D _rigid;
    private GeneralStatistics _stat;
    private float _currentSpeed;
    private Vector2 _direction;
    private EnemySensor _sensor;
    private BehaviourController _bh;
    private Animator _anim;



    public override void Awake()
    {
        base.Awake();
        _anim = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _bh = GetComponent<BehaviourController>();
        _sensor = GetComponentInChildren<EnemySensor>();
        _stat = GetComponent<GeneralStatistics>();
    }

    private void Update()
    {
        if (PhotonNetwork.isMasterClient && FollowTransform)
        {
            _direction = (FollowTransform.position - transform.position).normalized;

            if (_sensor.SensedPlayers.Count == 0)
            {
                _bh.ChangeState(AIState.Calm);
            }
            else
            {
                FollowTransform = GetTarget();
            }
        }
    }

    private Transform GetTarget()
    {
        Transform Target = null;
        switch (TargetingType)
        {
            case TargetingType.FirstSee:
                Target = _sensor.SensedPlayers[0].transform;
                break;
            case TargetingType.Closest:
                float dist = float.PositiveInfinity;
                foreach (var sensed in _sensor.SensedPlayers)
                {
                    float actDis = Vector2.Distance(sensed.transform.position, transform.position);
                    if (actDis < dist)
                    {
                        dist = actDis;
                        Target = sensed.transform;
                    }
                }
                break;
            case TargetingType.LowestHp:
                float hp = float.PositiveInfinity;

                foreach (var sensed in _sensor.SensedPlayers)
                {
                    float actHp = sensed.GetComponent<GeneralStatistics>().HealthActual;
                    if (actHp < hp)
                    {
                        hp = actHp;
                        Target = sensed.transform;
                    }
                }
                break;
            case TargetingType.HighestHp:
                float hph = 0;

                foreach (var sensed in _sensor.SensedPlayers)
                {
                    float actHp = sensed.GetComponent<GeneralStatistics>().HealthActual;
                    if (actHp > hph)
                    {
                        hp = actHp;
                        Target = sensed.transform;
                    }
                }
                break;
            case TargetingType.Slowest:
                float ms = float.PositiveInfinity;

                foreach (var sensed in _sensor.SensedPlayers)
                {
                    float actms = sensed.GetComponent<GeneralStatistics>().HealthActual;
                    if (actms < ms)
                    {
                        hp = actms;
                        Target = sensed.transform;
                    }
                }
                break;
            default:
                break;
        }
        return Target;
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.isMasterClient && FollowTransform)
        {
            UpdateMove();
            UpdateAnim();
        }
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

    private void UpdateAnim()
    {
        float _enemyAngle = 0;

        _enemyAngle = AngleBetweenVector2(transform.position, FollowTransform.position);

        _anim.SetFloat("MoveSpeed", _currentSpeed);
        //_anim.SetFloat("MoveSpeed", 10);

        if (_enemyAngle < 45 && _enemyAngle > -45)
        {
            _anim.SetFloat("X", 1);
            _anim.SetFloat("Y", 0);
        }
        else if (_enemyAngle > 45 && _enemyAngle < 135)
        {
            _anim.SetFloat("X", 0);
            _anim.SetFloat("Y", 1);
        }
        else if (_enemyAngle > 135 || _enemyAngle < -135)
        {
            _anim.SetFloat("X", -1);
            _anim.SetFloat("Y", 0);
        }
        else if (_enemyAngle > -135 && _enemyAngle < -45)
        {
            _anim.SetFloat("X", 0);
            _anim.SetFloat("Y", -1);
        }
    }

    private void UpdateMove()
    {
        if (Vector2.Distance(FollowTransform.position, transform.position) > StopDistance)
        {
            _currentSpeed = _direction.magnitude * _stat.MoveSpeed;
            _rigid.velocity = (_direction * _stat.MoveSpeed * Time.deltaTime * moveVelocityMultiplier);
        }
    }

    public override void EnterBehaviour()
    {
        base.EnterBehaviour();
        if (_sensor.SensedPlayers.Count > 0 && _sensor.SensedPlayers[0])
        {
            FollowTransform = _sensor.SensedPlayers[0].transform;
        }
    }

    public override void ExitBehaviour()
    {
        base.ExitBehaviour();
        FollowTransform = null;
        _direction = Vector2.zero;
        _currentSpeed = 0;
        _rigid.velocity = Vector2.zero;
    }
}

public enum TargetingType
{
    FirstSee,
    Closest,
    LowestHp,
    HighestHp,
    Slowest,
    //AgroMeter,
}