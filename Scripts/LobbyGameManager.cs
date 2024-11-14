using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab = null;
    [SerializeField]
    private GameObject nicknamePrefab = null;
    [SerializeField]
    private Transform[] playerPosition = null;
    [SerializeField]
    private TextMeshProUGUI[] nametext = null;


    private LobbyPlayer lobbyPlayer = null;
    LobbyPlayerNickname nick = null;

    private void Start()
    {
        SpawnPlayer();
    }

    // ���� �Ѿ �� Start �޼ҵ�� �ѹ� �� ���� ���� ����
    // ���� ������ ���ư��ٰ� ���� ���ϴ� ����� �ҷ���
    // ��ư�� ������ ���� �̺�Ʈ ó���� �ϴ°� ���� ����

    //Photon.Realtime.Player otherPlayer�� �г����� ������
    // �� �г����� ������ ����� �г���, �׷��� ������ ��� �Ѹ��� �г��Ӹ�
    // ������ �� �ֵ��� �ϸ��, �����÷��̾�� ����

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
        GameObject[] nickname = new GameObject[4];
        // ����ƽ���� �ε��� ��ȣ�� �����ؼ� ���ų� ����� �Ѿ ��� �ٽ� 0���� �ʱ�ȭ�ǰ� ����
        for (int i = 0; i < playerPosition.Length; i++) {

            go[i] = Instantiate(playerPrefab, playerPosition[i].position, Quaternion.identity);
            nickname[i] = Instantiate(nicknamePrefab);
            GameObject playerInfoGo = GameObject.FindGameObjectWithTag("PlayerInfo");
            nickname[i].transform.SetParent(playerInfoGo.transform);
            nick = nickname[i].GetComponent<LobbyPlayerNickname>();
            nick.UpdatePosition(playerPosition[i].position);

            nametext[i].text = "Player1";

            lobbyPlayer = go[i].GetComponent<LobbyPlayer>();
            lobbyPlayer.SetMaterial(i+1);
        }
    }


}