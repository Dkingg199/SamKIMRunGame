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

    private GameObject[] playerGoList = new GameObject[4];


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
        //GameObject nick = PhotonNetwork.Instantiate(nicknamePrefab[playcount].name, playerPosition[playcount].position, Quaternion.identity, 0);

        //GameObject playerInfoGo = GameObject.FindGameObjectWithTag("PlayerInfo");
        //nick.transform.SetParent(playerInfoGo.transform);

        //Lobbynickname = nick.GetComponent<LobbyPlayerNickname>();
        //Lobbynickname.UpdatePosition(playerPosition[playcount].position);
        //Lobbynickname.TextName();


        photonView.RPC("ApplyPlayerList", RpcTarget.All);
        //SetNickname(playcount);

        //lobbyPlayer = go.GetComponent<LobbyPlayer>();
        Debug.Log(lobbyPlayer);
        //lobbyPlayer.SetMaterial(PhotonNetwork.CurrentRoom.PlayerCount);
    }

    
    private void SetNickname(int playcount)
    {
        GameObject nick = PhotonNetwork.Instantiate(nicknamePrefab[playcount].name, playerPosition[playcount].position, Quaternion.identity, 0);

        GameObject playerInfoGo = GameObject.FindGameObjectWithTag("PlayerInfo");
        nick.transform.SetParent(playerInfoGo.transform);


        Lobbynickname = nick.GetComponent<LobbyPlayerNickname>();
        Lobbynickname.SetText(playerPosition[playcount].position);
    }

    [PunRPC]
    public void ApplyPlayerList()
    {
        // ���� �濡 ������ �ִ� �÷��̾��� ��
        Debug.LogError("CurrentRoom PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount);

        // ���� �����Ǿ� �ִ� ��� ����� ��������
        //PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        // ������� ������ �� �� ������
        // �迭�� ����
        PhotonView[] photonViews =
            FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

        // �Ź� �������� �ϴ°� �����Ƿ� �÷��̾� ���ӿ�����Ʈ ����Ʈ�� �ʱ�ȭ
        System.Array.Clear(playerGoList, 0, playerGoList.Length);

        // ���� �����Ǿ� �ִ� ����� ��ü��
        // �������� �÷��̾���� ���ͳѹ��� ����,
        // ���ͳѹ��� �������� �÷��̾� ���ӿ�����Ʈ �迭�� ä��
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            // Ű�� 0�� �ƴ� 1���� ����
            int key = i + 1;
            for (int j = 0; j < photonViews.Length; ++j)
            {
                // ���� PhotonNetwork.Instantiate�� ���ؼ� ������ ����䰡 �ƴ϶�� �ѱ�
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                // ���� ���� Ű ���� ��ųʸ� ���� �������� �ʴ´ٸ� �ѱ�
                // 0���Ͱ� �ƴ϶� 1���� �����ϱ� ������ 1���� ����
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                // ������� ���ͳѹ�
                int viewNum = photonViews[j].Owner.ActorNumber;
                // �������� �÷��̾��� ���ͳѹ�
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;

                Debug.LogError(viewNum + " " + playerNum);

                // ���ͳѹ��� ���� ������Ʈ�� �ִٸ�,
                if (viewNum == playerNum)
                {
                    Debug.Log("viewNum == playerNum");
                    // ���� ���ӿ�����Ʈ�� �迭�� �߰�
                    playerGoList[viewNum - 1] = photonViews[j].gameObject;
                    // ���ӿ�����Ʈ �̸��� �˾ƺ��� ���� ����
                    playerGoList[viewNum - 1].name = "Player_" + photonViews[j].Owner.NickName;

                    //SetNickname(PhotonNetwork.CountOfPlayersInRooms);
                    //photonViews[j].gameObject.GetComponent<LobbyPlayer>().SetMaterial(viewNum);
                }
            }
        }

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