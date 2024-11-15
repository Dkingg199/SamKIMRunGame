using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.Cockpit;

public class LobbyPlayer : MonoBehaviourPun
{
    [SerializeField]
    private Color[] colors = null;
    private bool isReady = false;

    public bool IsReady { get { return isReady; } }
    public void SetMaterial(int _playerNum)
    {
        Debug.LogError(_playerNum + " : " + colors.Length);
        if (_playerNum > colors.Length) return;

        this.GetComponent<MeshRenderer>().material.color = colors[_playerNum - 1];
    }
}
