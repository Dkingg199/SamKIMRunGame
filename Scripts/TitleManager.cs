using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class TitleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameVersion = "0.1"; //���� üũ
    [SerializeField] private string nickName = string.Empty; //�г���
    [SerializeField] private Button goLobbyButton = null;   //�κ���̵���ư

    private byte maxPlayerinLobby = 4;  //�ִ� �ο�

    public InputField inputField;   //�г��� ���� ĭ

    //�г��� ���� �� üũ
    //PhotonNetwork�� ����Ǿ����� Ȯ��
    //���� Ȯ���ϰ� ����
    private void Awake()
    {
        // �����Ͱ� PhotonNetwork.LoadLevel()�� ȣ���ϸ�,
        // ��� �÷��̾ ������ ������ �ڵ����� �ε�
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        //�г��� ������ ���� ���� �ȵ�.
        if(string.IsNullOrEmpty(nickName))
        {
            Debug.Log("�г����� �Է����ּ���.");
            return;
        }

        //IsConnected �ߵ��ϸ� JoinRandomRoom() ȣ��
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
            //PhotonNetwork.JoinLobby();
        }
        else //
        {
            Debug.LogFormat("Connect : {0}", gameVersion);
            PhotonNetwork.GameVersion = gameVersion;
            //���� Ŭ���忡 ������ �����ϴ� ����
            //���ӿ� �����ϸ� OnConnectedToMaster �޼��� ȣ��
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //InputField_NickName�� ������ �г����� ������.
    public void OnValueChangedNickName(string _nickName)
    {
        nickName = _nickName;

        if (_nickName.Length > 8)
        {
            inputField.text = _nickName.Substring(0, 8);
        }

        //���� �̸� ����
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

        //�� ���� �� OnGoLobby ȣ��
        Debug.Log("�� �����");
        PhotonNetwork.CreateRoom(
            null,
            new RoomOptions
            { MaxPlayers = maxPlayerinLobby });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�濡 �����߽��ϴ�.");
        //���� ���� ȣ��
        SceneManager.LoadScene("Lobby");
    }

    //public void JoinLobby()
    //{
    //    Debug.Log("�κ� �����߽��ϴ�.");
    //    PhotonNetwork.JoinLobby();

    //    SceneManager.LoadScene("Lobby");
    //}

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("�� ���忡 �����߽��ϴ�. ({0}): {1}", returnCode, message);

        goLobbyButton.interactable = true;

        Debug.Log("�� ����");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerinLobby });
    }
}
