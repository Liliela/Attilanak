using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerController : Photon.MonoBehaviour
{
    public GameObject Camera;

    private void Awake()
    {
        PhotonNetworkManager.Instance.AddPlayer(this);
        if (!photonView.isMine)
        {
            Camera.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        PhotonNetworkManager.Instance.RemovePlayer(this);
    }
}
