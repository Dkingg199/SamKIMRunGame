using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class TitleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameVersion = "0.1"; //버전 체크
    [SerializeField] private string nickName = string.Empty; //닉네임
    [SerializeField] private Button goLobbyButton = null;   //로비씬이동버튼

    private byte maxPlayerinLobby = 4;  //최대 인원

    public InputField inputField;   //닉네임 적는 칸

    //닉네임 없는 것 체크
    //PhotonNetwork에 연결되었는지 확인
    //버전 확인하고 연결
    private void Awake()
    {
        // 마스터가 PhotonNetwork.LoadLevel()을 호출하면,
        // 모든 플레이어가 동일한 레벨을 자동으로 로드
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        //닉네임 없으면 게임 접속 안됨.
        if(string.IsNullOrEmpty(nickName))
        {
            Debug.Log("닉네임을 입력해주세요.");
            return;
        }

        //IsConnected 발동하면 JoinRandomRoom() 호출
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
            //PhotonNetwork.JoinLobby();
        }
        else //
        {
            Debug.LogFormat("Connect : {0}", gameVersion);
            PhotonNetwork.GameVersion = gameVersion;
            //포톤 클라우드에 접속을 시작하는 지점
            //접속에 성공하면 OnConnectedToMaster 메서드 호출
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //InputField_NickName과 연결해 닉네임을 가져옴.
    public void OnValueChangedNickName(string _nickName)
    {
        nickName = _nickName;

        if (_nickName.Length > 8)
        {
            inputField.text = _nickName.Substring(0, 8);
        }

        //유저 이름 지정
        PhotonNetwork.NickName = nickName;
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("Connected to Master: {0}", nickName);

        goLobbyButton.interactable = false;

        PhotonNetwork.JoinRandomRoom();
        //JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected: {0}", cause);

        goLobbyButton.interactable = true;

        //방 생성 시 OnGoLobby 호출
        Debug.Log("방 만들기");
        PhotonNetwork.CreateRoom(
            null,
            new RoomOptions
            { MaxPlayers = maxPlayerinLobby });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방에 입장했습니다.");
        //각자 씬을 호출
        SceneManager.LoadScene("Lobby");
    }

    //public void JoinLobby()
    //{
    //    Debug.Log("로비에 입장했습니다.");
    //    PhotonNetwork.JoinLobby();

    //    SceneManager.LoadScene("Lobby");
    //}

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("방 입장에 실패했습니다. ({0}): {1}", returnCode, message);

        goLobbyButton.interactable = true;

        Debug.Log("방 생성");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerinLobby });
    }
}
