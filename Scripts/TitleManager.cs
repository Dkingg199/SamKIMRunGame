using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class TitleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string rungameVersion = "0.1"; //버전 체크
    [SerializeField] private string runnickName = string.Empty; //닉네임
    [SerializeField] private Button goLobbyButton = null;   //로비씬이동버튼

    private byte maxPlayerinLobby = 4;  //최대 인원

    //닉네임 없는 것 체크
    //PhotonNetwork에 연결되었는지 확인
    //버전 확인하고 연결
    private void Awake()
    {
        // 마스터가 PhotonNetwork.LoadLevel()을 호출하면,
        // 모든 플레이어가 동일한 레벨을 자동으로 로드
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void GoLobby()
    {
        //닉네임 없으면 게임 접속 안됨.
        if(string.IsNullOrEmpty(runnickName))
        {
            Debug.Log("닉네임을 입력해주세요.");
            return;
        }

        //IsConnected 발동하면 JoinRandomRoom() 호출
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else //
        {
            Debug.LogFormat("Connect : {0}", rungameVersion);
            PhotonNetwork.GameVersion = rungameVersion;
            //포톤 클라우드에 접속을 시작하는 지점
            //접속에 성공하면 OnConnectedToMaster 메서드 호출
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //InputField_NickName과 연결해 닉네임을 가져옴.
    public void OnValueChangedNickName(string _RnickName)
    {
        runnickName = _RnickName;

        //유저 이름 지정
        PhotonNetwork.NickName = runnickName;
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("Connected to Master: {0}", runnickName);

        goLobbyButton.interactable = false;

        PhotonNetwork.JoinRandomRoom();
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

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("방 입장에 실패했습니다. ({0}): {1}", returnCode, message);

        goLobbyButton.interactable = true;

        Debug.Log("방 생성");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerinLobby });  
    }
}
