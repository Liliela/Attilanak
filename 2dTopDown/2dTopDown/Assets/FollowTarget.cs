using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : AIBehaviour
{
    public float moveVelocityMultiplier = 20f;

    private Rigidbody2D _rigid;
    private GeneralStatistics _stat;
    private float _currentSpeed;
    private Vector2 _direction;
    private EnemySensor _sensor;
    private Transform _followTransform;
    private BehaviourController _bh;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _bh = GetComponent<BehaviourController>();
        _sensor = GetComponentInChildren<EnemySensor>();
        _stat = GetComponent<GeneralStatistics>();
    }

    private void Update()
    {
        if (PhotonNetwork.isMasterClient && _followTransform)
        {
            _direction = (_followTransform.position - transform.position).normalized;
          
            if (_sensor.SensedPlayers.Count == 0)
            {
                _bh.ChangeState(AIState.Calm);
            }
            else
            {
                _followTransform = _sensor.SensedPlayers[0].transform;
            }
        }
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.isMasterClient && _followTransform)
        {
            UpdateMove();
        }
    }

    private void UpdateMove()
    {
        _currentSpeed = _direction.magnitude * _stat.MoveSpeed;
        _rigid.velocity = (_direction * _stat.MoveSpeed * Time.deltaTime * moveVelocityMultiplier);
    }

    public void TakeDamage(float damage)
    {

    }

    public override void EnterBehaviour()
    {
        base.EnterBehaviour();
        if (_sensor.SensedPlayers.Count > 0 && _sensor.SensedPlayers[0])
        {
            _followTransform = _sensor.SensedPlayers[0].transform;
        }
    }

    public override void ExitBehaviour()
    {
        base.ExitBehaviour();
        _followTransform = null;
        _direction = Vector2.zero;
        _currentSpeed = 0;
        _rigid.velocity = Vector2.zero;
    }
}
