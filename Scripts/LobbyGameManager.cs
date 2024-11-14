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
    //    // ����ƽ���� �ε��� ��ȣ�� �����ؼ� ���ų� ����� �Ѿ ��� �ٽ� 0���� �ʱ�ȭ�ǰ� ����
    //    GameObject go = PhotonNetwork.Instantiate(playerPrefab.name, playerPosition[0].position, Quaternion.identity, 0);

    //    lobbyPlayer = go.GetComponent<LobbyPlayer>();
    //    lobbyPlayer.SetMaterial(PhotonNetwork.CurrentRoom.PlayerCount);
    //}

    private void SpawnPlayer()
    {
        GameObject[] go = new GameObject[4];
        // ����ƽ���� �ε��� ��ȣ�� �����ؼ� ���ų� ����� �Ѿ ��� �ٽ� 0���� �ʱ�ȭ�ǰ� ����
        for (int i = 0; i < playerPosition.Length; i++) {

            go[i] = Instantiate(playerPrefab, playerPosition[i].position, Quaternion.identity);

            lobbyPlayer = go[i].GetComponent<LobbyPlayer>();
            lobbyPlayer.SetMaterial(i+1);
        }
    }
}