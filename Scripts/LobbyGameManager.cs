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
            // 포톤 네트워크 풀을 관리하는 pool을 만들고
            // 리소스 최상위에 있지 않게 하기 위해서 이 코드를 구현함
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

    // 씬이 넘어갈 때 Start 메소드는 한번 더 실행 되지 않음
    // 이전 씬으로 돌아갔다가 색을 정하는 방법을 할려면
    // 버튼을 눌렀을 때의 이벤트 처리로 하는게 좋아 보임

    //Photon.Realtime.Player otherPlayer로 닉네임을 가져옴
    // 이 닉네임은 입장한 사람의 닉네임, 그러면 입장한 사람 한명의 닉네임만
    // 생성할 수 있도록 하면됨, 스폰플레이어에서 가능

    private void SpawnPlayer()
    {
        int playcount = PhotonNetwork.CountOfPlayersInRooms;


        // 스태틱으로 인덱스 번호를 관리해서 끄거나 장면이 넘어갈 경우 다시 0으로 초기화되게 만듦
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
        // 현재 방에 접속해 있는 플레이어의 수
        Debug.LogError("CurrentRoom PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount);

        // 현재 생성되어 있는 모든 포톤뷰 가져오기
        //PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        // 만들어진 순서를 알 수 없으니
        // 배열에 넣음
        PhotonView[] photonViews =
            FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

        // 매번 재정렬을 하는게 좋으므로 플레이어 게임오브젝트 리스트를 초기화
        System.Array.Clear(playerGoList, 0, playerGoList.Length);

        // 현재 생성되어 있는 포톤뷰 전체와
        // 접속중인 플레이어들의 액터넘버를 비교해,
        // 액터넘버를 기준으로 플레이어 게임오브젝트 배열을 채움
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            // 키는 0이 아닌 1부터 시작
            int key = i + 1;
            for (int j = 0; j < photonViews.Length; ++j)
            {
                // 만약 PhotonNetwork.Instantiate를 통해서 생성된 포톤뷰가 아니라면 넘김
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                // 만약 현재 키 값이 딕셔너리 내에 존재하지 않는다면 넘김
                // 0부터가 아니라 1부터 시작하기 때문에 1부터 넣음
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                // 포톤뷰의 액터넘버
                int viewNum = photonViews[j].Owner.ActorNumber;
                // 접속중인 플레이어의 액터넘버
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;

                Debug.LogError(viewNum + " " + playerNum);

                // 액터넘버가 같은 오브젝트가 있다면,
                if (viewNum == playerNum)
                {
                    Debug.Log("viewNum == playerNum");
                    // 실제 게임오브젝트를 배열에 추가
                    playerGoList[viewNum - 1] = photonViews[j].gameObject;
                    // 게임오브젝트 이름도 알아보기 쉽게 변경
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

    //    // 스태틱으로 인덱스 번호를 관리해서 끄거나 장면이 넘어갈 경우 다시 0으로 초기화되게 만듦
    //    for (int i = 0; i < playerPosition.Length; i++) {
    //        // 플레이어, 닉네임 생성
    //        go[i] = Instantiate(playerPrefab, playerPosition[i].position, Quaternion.identity);
    //        nickname[i] = Instantiate(nicknamePrefab[i]);

    //        // 캔버스에있는 PlayerInfo를 찾아서 그 위치에 닉네임을 이동시킴
    //        GameObject playerInfoGo = GameObject.FindGameObjectWithTag("PlayerInfo");
    //        nickname[i].transform.SetParent(playerInfoGo.transform);

    //        // 닉네임 프리팹을 1~4번 순회해서 각 1~4번 플레이어 자리에 위치시킴
    //        Lobbynickname = nickname[i].GetComponent<LobbyPlayerNickname>();
    //        Lobbynickname.UpdatePosition(playerPosition[i].position);
    //        Lobbynickname.SetNickName();

    //        // 플레이어 색을 지정함
    //        lobbyPlayer = go[i].GetComponent<LobbyPlayer>();
    //        lobbyPlayer.SetMaterial(i+1);
    //    }
    //}


}