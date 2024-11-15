using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

// RunPlayerCtrl는 프리펩에 add로 추가될 예정임. SerializeField로 직접 넣는게 안되서 태그로 찾아서 해당 cnt를 추가해야할듯.

// 구현한 기능
// 카운트 다운 시작되고 control true
// 플레이어 이동

// 해야 하는 기능
// player타이머 실행 시키고 -> end에 도달시 타이머 멈추고 해당 기록을 기록
// 게임 끝났을떄 다시 control false

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
        // 카운트다운이 끝나면 player 시작
        cnt = GameObject.FindGameObjectWithTag("RunCountDown").GetComponent<RunCountDown>();
        cnt.CountdownFinishedCallback += PlayerStart;

        // 타이머가 끝났을때 플레이어 비활성화
        // 타이머 끝났을때 움직임 false
        timer = GameObject.FindGameObjectWithTag("RunTimer").GetComponent<RunTimer>();
        timer.TimeEndCallback += NotMove;

    }

    // moveDis 만큼 플레이어가 이동
    private void Update()
    {
        if (isControl && Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(moveDis);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.LogError(" 현재 플레이어의 actNum :  " + PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }


    private void PlayerStart()
    {
        isControl = true;
        startTime = Time.time;
    }

    private void OnTriggerEnter(Collider _other)
    {
        // Goal까지 걸린 시간을 알려줌.
        if (_other.CompareTag("RunGoal"))
        {
            elapsedTime = Time.time - startTime;

            // 도착한걸 알려줘야 함. -> RGM한테 알려줘서 (1번이 도착했다 => 모든 컴에 1번도착!) RPC
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
