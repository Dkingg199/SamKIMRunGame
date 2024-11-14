using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class TitleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string rungameVersion = "0.1"; //���� üũ
    [SerializeField] private string runnickName = string.Empty; //�г���
    [SerializeField] private Button goLobbyButton = null;   //�κ���̵���ư

    private byte maxPlayerinLobby = 4;  //�ִ� �ο�

    //�г��� ���� �� üũ
    //PhotonNetwork�� ����Ǿ����� Ȯ��
    //���� Ȯ���ϰ� ����
    private void Awake()
    {
        // �����Ͱ� PhotonNetwork.LoadLevel()�� ȣ���ϸ�,
        // ��� �÷��̾ ������ ������ �ڵ����� �ε�
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void GoLobby()
    {
        //�г��� ������ ���� ���� �ȵ�.
        if(string.IsNullOrEmpty(runnickName))
        {
            Debug.Log("�г����� �Է����ּ���.");
            return;
        }

        //IsConnected �ߵ��ϸ� JoinRandomRoom() ȣ��
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else //
        {
            Debug.LogFormat("Connect : {0}", rungameVersion);
            PhotonNetwork.GameVersion = rungameVersion;
            //���� Ŭ���忡 ������ �����ϴ� ����
            //���ӿ� �����ϸ� OnConnectedToMaster �޼��� ȣ��
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //InputField_NickName�� ������ �г����� ������.
    public void OnValueChangedNickName(string _RnickName)
    {
        runnickName = _RnickName;

        //���� �̸� ����
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

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("�� ���忡 �����߽��ϴ�. ({0}): {1}", returnCode, message);

        goLobbyButton.interactable = true;

        Debug.Log("�� ����");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerinLobby });  
    }
}
