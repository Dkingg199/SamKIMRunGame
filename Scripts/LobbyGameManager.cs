using UnityEngine;
using Photon.Pun;

using Photon.Realtime;


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

        if (playerPrefab != null)
        {
            // ���� ��Ʈ��ũ Ǯ�� �����ϴ� pool�� �����
            // ���ҽ� �ֻ����� ���� �ʰ� �ϱ� ���ؼ� �� �ڵ带 ������
            DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
            if (!pool.ResourceCache.ContainsKey(playerPrefab.name) && !pool.ResourceCache.ContainsKey(nicknamePrefab[0].name))
            {
                pool.ResourceCache.Add(playerPrefab.name, playerPrefab);
                for (int i = 0; i < nicknamePrefab.Length; i++)
                {
                    pool.ResourceCache.Add(nicknamePrefab[i].name, nicknamePrefab[i]);
                }
            }
            Invoke("SpawnPlayer", 0.5f);
        }
    }

    // ���� �Ѿ �� Start �޼ҵ�� �ѹ� �� ���� ���� ����
    // ���� ������ ���ư��ٰ� ���� ���ϴ� ����� �ҷ���
    // ��ư�� ������ ���� �̺�Ʈ ó���� �ϴ°� ���� ����

    //Photon.Realtime.Player otherPlayer�� �г����� ������
    // �� �г����� ������ ����� �г���, �׷��� ������ ��� �Ѹ��� �г��Ӹ�
    // ������ �� �ֵ��� �ϸ��, �����÷��̾�� ����

    private void SpawnPlayer()
    {
        int playcount = PhotonNetwork.CountOfPlayersInRooms;


        // ����ƽ���� �ε��� ��ȣ�� �����ؼ� ���ų� ����� �Ѿ ��� �ٽ� 0���� �ʱ�ȭ�ǰ� ����
        GameObject go = PhotonNetwork.Instantiate(playerPrefab.name, playerPosition[playcount].position, Quaternion.identity, 0);
        GameObject nick = PhotonNetwork.Instantiate(nicknamePrefab[playcount].name, playerPosition[playcount].position, Quaternion.identity, 0);

        GameObject playerInfoGo = GameObject.FindGameObjectWithTag("PlayerInfo");
        nick.transform.SetParent(playerInfoGo.transform);

        Lobbynickname = nick.GetComponent<LobbyPlayerNickname>();
        Lobbynickname.UpdatePosition(playerPosition[playcount].position);
        Lobbynickname.SetNickName();


        lobbyPlayer = go.GetComponent<LobbyPlayer>();
        Debug.Log(lobbyPlayer);
        lobbyPlayer.SetMaterial(PhotonNetwork.CurrentRoom.PlayerCount);
    }


    //private void SpawnPlayer()
    //{
    //    GameObject[] go = new GameObject[4];
    //    GameObject[] nickname = new GameObject[4];

    //    // ����ƽ���� �ε��� ��ȣ�� �����ؼ� ���ų� ����� �Ѿ ��� �ٽ� 0���� �ʱ�ȭ�ǰ� ����
    //    for (int i = 0; i < playerPosition.Length; i++) {
    //        // �÷��̾�, �г��� ����
    //        go[i] = Instantiate(playerPrefab, playerPosition[i].position, Quaternion.identity);
    //        nickname[i] = Instantiate(nicknamePrefab[i]);

    //        // ĵ�������ִ� PlayerInfo�� ã�Ƽ� �� ��ġ�� �г����� �̵���Ŵ
    //        GameObject playerInfoGo = GameObject.FindGameObjectWithTag("PlayerInfo");
    //        nickname[i].transform.SetParent(playerInfoGo.transform);

    //        // �г��� �������� 1~4�� ��ȸ�ؼ� �� 1~4�� �÷��̾� �ڸ��� ��ġ��Ŵ
    //        Lobbynickname = nickname[i].GetComponent<LobbyPlayerNickname>();
    //        Lobbynickname.UpdatePosition(playerPosition[i].position);
    //        Lobbynickname.SetNickName();

    //        // �÷��̾� ���� ������
    //        lobbyPlayer = go[i].GetComponent<LobbyPlayer>();
    //        lobbyPlayer.SetMaterial(i+1);
    //    }
    //}


}