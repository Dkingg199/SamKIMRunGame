using UnityEngine;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class LobbyPlayerNickname : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nametext = null;

    public TextMeshProUGUI Nametext { get { return nametext; } }

    public Vector3 UpdatePosition(Vector3 _pos)
    {
        Vector3 worldToScreen = Camera.main.WorldToScreenPoint(_pos);
        worldToScreen.y -= 100f;
        transform.position = worldToScreen;

        return transform.position;
    }

    public void SetNickName()
    {
        nametext.text = PhotonNetwork.NickName;
    }
}
