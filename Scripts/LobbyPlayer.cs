using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyPlayer : MonoBehaviourPun
{
    [SerializeField]
    private Color[] colors = null;
    [SerializeField]
    private TextMeshPro nameText = null;
    private bool isReady = false;

    public bool IsReady { get { return isReady; } }


    private void Start()
    {
       string name = PhotonNetwork.NickName;
    }

    public void SetMaterial(int _playerNum)
    {
        Debug.LogError(_playerNum + " : " + colors.Length);
        if (_playerNum > colors.Length) return;

        this.GetComponent<MeshRenderer>().material.color = colors[_playerNum - 1];
    }

    public void TextName()
    {
        if (!photonView.IsMine) return;
        if (nameText.text.Length != 0) return;

        // ���ÿ��� �÷��̾� �̸� ����
        nameText.text = name;

        // RPC ȣ��� �ٸ� Ŭ���̾�Ʈ������ �̸� ������Ʈ
        photonView.RPC("UpdateName", RpcTarget.AllBuffered, name);
    }

    [PunRPC]
    public void UpdateName(string playerName)
    {
        // ��� Ŭ���̾�Ʈ���� �ؽ�Ʈ ������Ʈ
        nameText.text = playerName;
    }
}
