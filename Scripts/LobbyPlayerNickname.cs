using UnityEngine;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class LobbyPlayerNickname : MonoBehaviourPun
{
    [SerializeField]
    private TextMeshProUGUI nametext = null;

    public TextMeshProUGUI Nametext { get { return nametext; } }

    public void SetText(Vector3 _pos)
    {
        photonView.RPC("UpdatePosition", RpcTarget.All, _pos);
        photonView.RPC("TextName", RpcTarget.All);
    }

    [PunRPC]
    public Vector3 UpdatePosition(Vector3 _pos)
    {
        Vector3 worldToScreen = Camera.main.WorldToScreenPoint(_pos);
        worldToScreen.y -= 100f;
        transform.position = worldToScreen;

        return transform.position;
    }

    [PunRPC]
    public void TextName()
    {
        nametext.text = PhotonNetwork.NickName;
    }
}
