using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerController : MonoBehaviour {

    private void Awake()
    {
        PhotonNetworkManager.Instance.AddPlayer(this);
    }
    private void OnDestroy()
    {
        PhotonNetworkManager.Instance.RemovePlayer(this);
    }
}
