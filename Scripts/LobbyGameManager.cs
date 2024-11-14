using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab = null;
    [SerializeField]
    private GameObject[] nicknamePrefab = null;
    [SerializeField]
    private Transform[] playerPosition = null;
    //[SerializeField]
    //private TextMeshProUGUI[] nametext = null;


    private LobbyPlayer lobbyPlayer = null;
    private LobbyPlayerNickname Lobbynickname = null;

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
            // �÷��̾�, �г��� ����
            go[i] = Instantiate(playerPrefab, playerPosition[i].position, Quaternion.identity);
            nickname[i] = Instantiate(nicknamePrefab[i]);

            // ĵ�������ִ� PlayerInfo�� ã�Ƽ� �� ��ġ�� �г����� �̵���Ŵ
            GameObject playerInfoGo = GameObject.FindGameObjectWithTag("PlayerInfo");
            nickname[i].transform.SetParent(playerInfoGo.transform);

            // �г��� �������� 1~4�� ��ȸ�ؼ� �� 1~4�� �÷��̾� �ڸ��� ��ġ��Ŵ
            Lobbynickname = nickname[i].GetComponent<LobbyPlayerNickname>();
            Lobbynickname.UpdatePosition(playerPosition[i].position);

            // �÷��̾� ���� ������
            lobbyPlayer = go[i].GetComponent<LobbyPlayer>();
            lobbyPlayer.SetMaterial(i+1);
        }
    }


}