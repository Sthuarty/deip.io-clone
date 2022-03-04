using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour {
    [SerializeField] private GameObject m_PlayerPrefab;
    [SerializeField] private float m_MinX, m_MaxX, m_MinY, m_MaxY;


    private void Start() {
        Vector2 randomPosition = new Vector2(Random.Range(m_MinX, m_MaxX), Random.Range(m_MinY, m_MaxY));
        PhotonNetwork.Instantiate(m_PlayerPrefab.name, randomPosition, Quaternion.identity);
    }
}
