using Photon.Pun;
using UnityEngine;
using System.Collections;
using static RunCountDown;
using Unity.VisualScripting;

// 구현 기능
// 순차적으로 player를 만듦.

// 구현해야 하는 기능
// end에 도달시 끝났다고 check
public class RunGameManager : MonoBehaviourPunCallbacks
{
    public delegate void AllGoalInDelegate();
    public AllGoalInDelegate AllGoalInCallback;

    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Vector3[] playerPosition = new Vector3[4];
    [SerializeField] private Color[] playerColors = new Color[4];
    [SerializeField] private RunTimer timer = null;
    [SerializeField] private GameObject resultUI = null;
    
    private GameObject[] playerList = new GameObject[4];
    private bool[] isGoalIn = new bool[4];
    private bool timeEnd = false;
    private bool allGoalIn = false;


    private void Start()
    {
        if (playerPrefab != null)
        {
            //
            DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
            if (!pool.ResourceCache.ContainsKey(playerPrefab.name))
                pool.ResourceCache.Add(playerPrefab.name, playerPrefab);
            //
        }

        StartCoroutine(MakePlayerCoroutine());

        // 게임 끝남을 콜백 받음
        // result창을 setActive(true)
        // 5초뒤 다시 Looby Scene으로 돌아감.

        // timer끝남을 호출받음.
        timer.TimeEndCallback += TimeEnd;

    }

    private void Update()
    {
        if (timeEnd || allGoalIn)
        {
            // 몇초있다가 모두 result창을 뛰움.
            resultUI.SetActive(true);
        }
    }

    // 플레이어를 만듦.
    private IEnumerator MakePlayerCoroutine()
    {
        float delay = 1f * PhotonNetwork.LocalPlayer.ActorNumber;

        yield return new WaitForSeconds(delay);

        GameObject go = PhotonNetwork.Instantiate(
                playerPrefab.name,
                playerPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1],
                Quaternion.identity,
                0);
        go.AddComponent<RunPlayerCtrl>();
        go.name = $"Player_{PhotonNetwork.LocalPlayer.ActorNumber}";
        playerList[PhotonNetwork.LocalPlayer.ActorNumber - 1] = go;

        photonView.RPC("SynColor", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
    }


    // 전부 goalIn하는지 check
    [PunRPC]
    public void ApplyGoalIn(int _actNum)
    {
        isGoalIn[_actNum - 1] = true;

        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            if (!isGoalIn[i]) return;
        }

        allGoalIn = true;
        AllGoalInCallback?.Invoke();
    }

    private void TimeEnd()
    {
        timeEnd = true;
    }

    [PunRPC]
    public void SynColor(int _actNum)
    {
        playerList[_actNum-1].GetComponent<MeshRenderer>().material.color = playerColors[_actNum-1];
    }
}