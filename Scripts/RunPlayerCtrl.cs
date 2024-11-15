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
    private RunGameManager RunGM = null;
    private bool isControl = false;
    private float startTime;
    private float elapsedTime;

    public float ElapsedTime { get{ return elapsedTime; } }

    private void Awake()
    {
        moveDis = new Vector3(0.1f, 0f, 0f);
    }

    private void Start()
    {
        cnt = GameObject.FindGameObjectWithTag("RunCountDown").GetComponent<RunCountDown>();
        cnt.CountdownFinishedCallback += PlayerStart;
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
            elapsedTime = Time.time - startTime; 
        }

        // �����Ѱ� �˷���� ��. -> RGM���� �˷��༭ (1���� �����ߴ� => ��� �Ŀ� 1������!) RPC
        RunGM = GameObject.FindGameObjectWithTag("RunGameManager").GetComponent<RunGameManager>();
        RunGM.photonView.RPC("ApplyGoalIn", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
    }
}
