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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int actNum = PhotonNetwork.LocalPlayer.ActorNumber;

            GameObject GMgo = GameObject.FindGameObjectWithTag("LobbyGameManager");
            LobbyGameManager LGM = GMgo.GetComponent<LobbyGameManager>();

            LGM.photonView.RPC("ApplyReady", RpcTarget.All, actNum);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.LogError(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
}
