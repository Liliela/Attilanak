using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Targeter : Photon.MonoBehaviour
{
    public float AutoLostFocus = 5f;
    public GameObject TargetPanel;
    public Text NameText;
    public Image HpBarImage;
    public Text HpText;
    public LayerMask TargetLayer;
    public float DistanseToLoseLock = 5f;
    public GeneralStatistics CurrentFocusedCharacter;
    public Camera Camera;
    private bool _lock;
    public GameObject LockMarker;

    private void Awake()
    {
        LoseFocus();
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            FindTarget();
            if (!CurrentFocusedCharacter)
            {
                LoseFocus();
            }
            else
            {
                UpdateLocked();
                UpdateStat();
            }
        }       
    }

    private void UpdateStat()
    {
            LockMarker.transform.position = CurrentFocusedCharacter.transform.position;
            NameText.text = CurrentFocusedCharacter.Name;
            HpBarImage.fillAmount = CurrentFocusedCharacter.HealthActual / CurrentFocusedCharacter.HealthMax;
            HpText.text = string.Format("{0}/{1}", CurrentFocusedCharacter.HealthActual, CurrentFocusedCharacter.HealthMax);        
    }

    private void UpdateLocked()
    {
        if (_lock)
        {
            if (Vector2.Distance(CurrentFocusedCharacter.transform.position, transform.position) > DistanseToLoseLock)
            {
                _lock = false;
                LoseFocus();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                _lock = false;
                LoseFocus();
            }
        }
    }

    private void FindTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.ScreenToWorldPoint(Input.mousePosition).x, Camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f, TargetLayer);
        if (hit)
        {
            GeneralStatistics newStat = hit.transform.gameObject.GetComponentInParent<GeneralStatistics>();
            if (newStat)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _lock = true;
                    Focus(newStat);
                }
                else if (!_lock)
                {
                    Focus(newStat);
                }
            }
        }
    }

    private void Focus(GeneralStatistics newStat)
    {
        CurrentFocusedCharacter = newStat;
        TargetPanel.SetActive(true);
        LockMarker.SetActive(true);

        CancelInvoke();

        if (!_lock)
        {
            Invoke("LoseFocus", AutoLostFocus);
        }
    }

    public void LoseFocus()
    {
        LockMarker.SetActive(false);
        CurrentFocusedCharacter = null;
        TargetPanel.SetActive(false);
    }
}
