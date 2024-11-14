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

    // 씬이 넘어갈 때 Start 메소드는 한번 더 실행 되지 않음
    // 이전 씬으로 돌아갔다가 색을 정하는 방법을 할려면
    // 버튼을 눌렀을 때의 이벤트 처리로 하는게 좋아 보임

    //Photon.Realtime.Player otherPlayer로 닉네임을 가져옴
    // 이 닉네임은 입장한 사람의 닉네임, 그러면 입장한 사람 한명의 닉네임만
    // 생성할 수 있도록 하면됨, 스폰플레이어에서 가능

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
        GameObject[] nickname = new GameObject[4];
        // 스태틱으로 인덱스 번호를 관리해서 끄거나 장면이 넘어갈 경우 다시 0으로 초기화되게 만듦
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