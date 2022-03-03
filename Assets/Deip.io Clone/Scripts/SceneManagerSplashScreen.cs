using System;
using UnityEngine;

public class SceneManagerSplashScreen : MonoBehaviour {
    [SerializeField] private ConnectToServer m_ConnectToServer;

    private void OnEnable() => m_ConnectToServer.OnJoinedLobbyEvent += HasJoinedLobby;
    private void OnDisable() => m_ConnectToServer.OnJoinedLobbyEvent -= HasJoinedLobby;

    private void Start() {
        m_ConnectToServer.Initialize();
    }

    private void HasJoinedLobby() {
        SceneLoader.Instance.LoadSceneWithFadeEffect("Menu");
    }
}
