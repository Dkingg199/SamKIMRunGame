using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

// ���� ��� : �÷��̾� �̵�
public class RunPlayerCtrl : MonoBehaviourPun
{
    [SerializeField] private Vector3 moveDis = Vector3.zero;
    private bool isControl = false;

    private void Awake()
    {
        moveDis = new Vector3(0.1f, 0f, 0f);
    }

    // moveDis ��ŭ �÷��̾ �̵�
    private void Update()
    {
        if (isControl && Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(moveDis);
        }
    }
}
