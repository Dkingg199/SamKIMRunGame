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

        // 로컬에서 플레이어 이름 설정
        nameText.text = name;

        // RPC 호출로 다른 클라이언트에서도 이름 업데이트
        photonView.RPC("UpdateName", RpcTarget.AllBuffered, name);
    }

    [PunRPC]
    public void UpdateName(string playerName)
    {
        // 모든 클라이언트에서 텍스트 업데이트
        nameText.text = playerName;
    }
}
