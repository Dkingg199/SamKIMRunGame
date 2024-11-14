using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

// 구현 기능 : 플레이어 이동
public class RunPlayerCtrl : MonoBehaviourPun
{
    [SerializeField] private Vector3 moveDis = Vector3.zero;
    private bool isControl = false;

    private void Awake()
    {
        moveDis = new Vector3(0.1f, 0f, 0f);
    }

    // moveDis 만큼 플레이어가 이동
    private void Update()
    {
        if (isControl && Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(moveDis);
        }
    }
}
