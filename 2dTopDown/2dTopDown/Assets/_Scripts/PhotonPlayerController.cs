using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerController : Photon.MonoBehaviour
{
    public List<GameObject> NotMineObjects;

    private void Awake()
    {
        PhotonNetworkManager.Instance.AddPlayer(this);
        if (!photonView.isMine)
        {
            foreach (var item in NotMineObjects)
            {
                item.SetActive(false);
            }        
        }
    }
    private void OnDestroy()
    {
        PhotonNetworkManager.Instance.RemovePlayer(this);
    }
}
