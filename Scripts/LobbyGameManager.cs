using UnityEngine;

using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab = null;
    [SerializeField]
    private Transform[] playerPosition = null;


    private LobbyPlayer lobbyPlayer = null;

    private void Start()
    {
        Invoke("SpawnPlayer", 0.5f);
    }

    //private void SpawnPlayer()
    //{
    //    // 스태틱으로 인덱스 번호를 관리해서 끄거나 장면이 넘어갈 경우 다시 0으로 초기화되게 만듦
    //    GameObject go = PhotonNetwork.Instantiate(playerPrefab.name, playerPosition[0].position, Quaternion.identity, 0);

    //    lobbyPlayer = go.GetComponent<LobbyPlayer>();
    //    lobbyPlayer.SetMaterial(PhotonNetwork.CurrentRoom.PlayerCount);
    //}

    private void SpawnPlayer()
    {
        GameObject[] go = new GameObject[4];
        // 스태틱으로 인덱스 번호를 관리해서 끄거나 장면이 넘어갈 경우 다시 0으로 초기화되게 만듦
        for (int i = 0; i < playerPosition.Length; i++) {

            go[i] = Instantiate(playerPrefab, playerPosition[i].position, Quaternion.identity);

            lobbyPlayer = go[i].GetComponent<LobbyPlayer>();
            lobbyPlayer.SetMaterial(i+1);
        }
    }
}