using UnityEngine;

public class LobbyPlayerNickname : MonoBehaviour
{
    public Vector3 UpdatePosition(Vector3 _pos)
    {
        Vector3 worldToScreen = Camera.main.WorldToScreenPoint(_pos);
        worldToScreen.y -= 100f;
        transform.position = worldToScreen;

        return transform.position;
    }
}
