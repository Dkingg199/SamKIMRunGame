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

    // moveDis 만큼 플레이어가 이동
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
        // Goal까지 걸린 시간을 알려줌.
        if (_other.CompareTag("RunGoal"))
        {
            float elapsedTime = Time.time - startTime; 
        }
    }
}
