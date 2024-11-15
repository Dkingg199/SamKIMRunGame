using UnityEngine;
using TMPro;
using Photon.Pun;

public class LobbyPlayer : MonoBehaviourPun
{
    [SerializeField]
    private Color[] colors = null;
    [SerializeField]
    private TextMeshPro nameText = null;
    private bool isReady = false;

    public bool IsReady { get { return isReady; } }
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
        
        nameText.text = PhotonNetwork.NickName;
        
    }
}
