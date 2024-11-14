using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField]
    private Color[] colors = null;
    public void SetMaterial(int _playerNum)
    {
        Debug.LogError(_playerNum + " : " + colors.Length);
        if (_playerNum > colors.Length) return;

        this.GetComponent<MeshRenderer>().material.color = colors[_playerNum - 1];
    }
}
