using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Photon.MonoBehaviour
{
    public Transform Crosshair;
    public float MouseSpeed = 2f;
    private Vector2 _direction;
    private PlayerStat _ps;
    private Animator _anim;
    private float _currentSpeed;
    private Rigidbody2D _rigid;
    public float moveVelocityMultiplier = 20f;
    private float _mouseAngle;
    private Camera _cam;
    public Transform Stick;

    private void Awake()
    {
        _ps = GetComponent<PlayerStat>();
        _cam = GetComponentInChildren<Camera>();
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            UpdateControll();
            UpdateMouse();
        }
    }

    private void FixedUpdate()
    {
        if (photonView.isMine)
        {
            UpdateMove();
            UpdateAnim();
            UpdateStick();
        }
    }

    private void UpdateStick()
    {
        Vector3 vectorToTarget = Crosshair.position - Stick.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        Stick.transform.rotation = Quaternion.Slerp(Stick.transform.rotation, q, Time.deltaTime * 10);
    }

    private void UpdateMouse()
    {
        Vector3 crosshairPos = _cam.ScreenToWorldPoint(Input.mousePosition);

        Crosshair.transform.position = new Vector2(crosshairPos.x, crosshairPos.y);
        _mouseAngle = AngleBetweenVector2(transform.position, Crosshair.transform.position);
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

    private void UpdateAnim()
    {
        _anim.SetFloat("MoveSpeed", _currentSpeed);
        //_anim.SetFloat("MoveSpeed", 10);
        _anim.SetFloat("AnimSpeed", 1);
        if (_mouseAngle < 45 && _mouseAngle > -45)
        {
            _anim.SetInteger("X", 1);
            _anim.SetInteger("Y", 0);
            if (_direction.x < 0)
            {
                _anim.SetFloat("AnimSpeed", -1);
            }
        }
        else if (_mouseAngle > 45 && _mouseAngle < 135)
        {
            _anim.SetInteger("X", 0);
            _anim.SetInteger("Y", 1);
            if (_direction.y < 0)
            {
                _anim.SetFloat("AnimSpeed", -1);
            }
        }
        else if (_mouseAngle > 135 || _mouseAngle < -135)
        {
            _anim.SetInteger("X", -1);
            _anim.SetInteger("Y", 0);
            if (_direction.x > 0)
            {
                _anim.SetFloat("AnimSpeed", -1);
            }
        }
        else if (_mouseAngle > -135 && _mouseAngle < -45)
        {
            _anim.SetInteger("X", 0);
            _anim.SetInteger("Y", -1);
            if (_direction.y > 0)
            {
                _anim.SetFloat("AnimSpeed", -1);
            }
        }
    }

    private void UpdateMove()
    {
        _currentSpeed = _direction.magnitude * _ps.MoveSpeed;
        _rigid.velocity = (_direction * _ps.MoveSpeed * Time.deltaTime * moveVelocityMultiplier);
        //  transform.Translate(_direction * _ps.MoveSpeed * Time.deltaTime);
    }

    private void UpdateControll()
    {
        _direction = Vector2.zero;
        _currentSpeed = 0;
        if (Input.GetKey(KeyCode.A))
        {
            _direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.W))
        {
            _direction += Vector2.up;
        }
    }
}
