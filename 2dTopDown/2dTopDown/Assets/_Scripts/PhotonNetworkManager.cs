// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to handle typical game management requirements
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PhotonNetworkManager : Photon.PunBehaviour
{
    public List<PhotonPlayerController> players;
    public string mapName = "Environment";

    #region Public Variables

    static public PhotonNetworkManager Instance;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject PlayerPrefab;

    public List<GameObject> SpawnPoints;

    #endregion


    #region MonoBehaviour CallBacks

    void Start()
    {
        Instance = this;

        if (!PhotonNetwork.connected)
        {
            Debug.Log("TryJoinPhoton");
            PhotonNetwork.ConnectUsingSettings("0000");
        }
    }

    #endregion


    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected() " + other.NickName);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.JoinOrCreateRoom("1", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedGame" + PhotonNetwork.player.ID);
        PhotonNetwork.Instantiate(PlayerPrefab.name, SpawnPoints[0].transform.position, SpawnPoints[0].transform.rotation, 0);
        SceneManager.LoadSceneAsync(mapName, LoadSceneMode.Additive);
    }

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    internal void AddPlayer(PhotonPlayerController photonPlayerController)
    {
        players.Add(photonPlayerController);
    }
    internal void RemovePlayer(PhotonPlayerController photonPlayerController)
    {
        players.Remove(photonPlayerController);
    }

    #endregion

}