using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RunResult : MonoBehaviour
{
    private RunPlayerCtrl[] players = new RunPlayerCtrl[4]; //�迭 

    //private void Start()
    //{
    //    StartCoroutine(FindPlayerCoroutine());
    //}

    ////RunPlayerCtrl ��ũ��Ʈ�� ���� Player���� Hierarchyâ �̸��� �̿��Ͽ� �迭�� �ʱ�ȭ
    //private IEnumerator FindPlayerCoroutine()
    //{
    //    yield return new WaitForSeconds(6f);

    //    players[0] = GameObject.Find("Player_1").GetComponent<RunPlayerCtrl>();
    //    players[1] = GameObject.Find("Player_2").GetComponent<RunPlayerCtrl>();
    //    players[2] = GameObject.Find("Player_3").GetComponent<RunPlayerCtrl>();
    //    players[3] = GameObject.Find("Player_4").GetComponent<RunPlayerCtrl>();
    //}
}