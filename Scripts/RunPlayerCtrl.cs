using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

// RunPlayerCtrl�� �����鿡 add�� �߰��� ������. SerializeField�� ���� �ִ°� �ȵǼ� �±׷� ã�Ƽ� �ش� cnt�� �߰��ؾ��ҵ�.

// ������ ���
// ī��Ʈ �ٿ� ���۵ǰ� control true
// �÷��̾� �̵�

// �ؾ� �ϴ� ���
// playerŸ�̸� ���� ��Ű�� -> end�� ���޽� Ÿ�̸� ���߰� �ش� ����� ���
// ���� �������� �ٽ� control false

public class RunPlayerCtrl : MonoBehaviourPun
{
    [SerializeField] private Vector3 moveDis = Vector3.zero;
    private RunCountDown cnt = null;
    private bool isControl = false;
    private float startTime;

    private void Awake()
    {
        moveDis = new Vector3(0.1f, 0f, 0f);
    }

    private void Start()
    {
        cnt = GameObject.FindGameObjectWithTag("RunCountDown").GetComponent<RunCountDown>();
        cnt.CountdownFinished = PlayerStart;
    }

    // moveDis ��ŭ �÷��̾ �̵�
    private void Update()
    {
        if (isControl && Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(moveDis);
        }
    }


    private void PlayerStart()
    {
        isControl = true;
        startTime = Time.time;
    }

    private void OnTriggerEnter(Collider _other)
    {
        // Goal���� �ɸ� �ð��� �˷���.
        if (_other.CompareTag("RunGoal"))
        {
            float elapsedTime = Time.time - startTime; 
        }
    }
}
