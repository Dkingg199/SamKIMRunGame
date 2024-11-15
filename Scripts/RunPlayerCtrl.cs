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
    private Color[] colors = new Color[4];
    private RunCountDown cnt = null;
    private RunGameManager RunGM = null;
    private bool isControl = false;
    private float startTime;
    private float elapsedTime;
    private RunTimer timer = null;

    public float ElapsedTime { get{ return elapsedTime; } }

    private void Awake()
    {
        moveDis = new Vector3(0.5f, 0f, 0f);

        colors[0] = Color.red;
        colors[1] = Color.yellow;
        colors[2] = Color.green;
        colors[3] = Color.blue;
    }

    private void Start()
    {
        // ī��Ʈ�ٿ��� ������ player ����
        cnt = GameObject.FindGameObjectWithTag("RunCountDown").GetComponent<RunCountDown>();
        cnt.CountdownFinishedCallback += PlayerStart;

        // Ÿ�̸Ӱ� �������� �÷��̾� ��Ȱ��ȭ
        // Ÿ�̸� �������� ������ false
        timer = GameObject.FindGameObjectWithTag("RunTimer").GetComponent<RunTimer>();
        timer.TimeEndCallback += NotMove;

    }

    // moveDis ��ŭ �÷��̾ �̵�
    private void Update()
    {
        if (isControl && Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(moveDis);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.LogError(" ���� �÷��̾��� actNum :  " + PhotonNetwork.LocalPlayer.ActorNumber);
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

            // �����Ѱ� �˷���� ��. -> RGM���� �˷��༭ (1���� �����ߴ� => ��� �Ŀ� 1������!) RPC
            RunGM = GameObject.FindGameObjectWithTag("RunGameManager").GetComponent<RunGameManager>();
            RunGM.photonView.RPC("ApplyGoalIn", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);

            isControl = false;
        }
    }

    private void NotMove()
    {
        isControl = false;
    }

    public void SetMaterial(int _playerNum)
    {

        Debug.LogError(_playerNum + " : " + colors.Length);
        if (_playerNum > colors.Length) return;

        this.GetComponent<MeshRenderer>().material.color = colors[_playerNum - 1];
    }
}
