using Photon.Pun;
using UnityEngine;
using System.Collections;
using TMPro;

// 구현 기능
// 순차적으로 player를 만듦.

// 구현해야 하는 기능
// end에 도달시 끝났다고 check
public class RunGameManager : MonoBehaviourPunCallbacks
{
    public delegate void AllGoalInDelegate();
    public delegate void AllMakeDelegate();
    public AllMakeDelegate AllMakeEndCallback;
    public AllGoalInDelegate AllGoalInCallback;


    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Vector3[] playerPosition = new Vector3[4];
    // [SerializeField] private Color[] playerColors = new Color[4];
    [SerializeField] private Material[] playerMaterials = new Material[4];
    [SerializeField] private RunTimer timer = null;
    [SerializeField] private GameObject resultUI = null;
    [SerializeField] private GameObject[] playerList = new GameObject[4];
    private bool[] isGoalIn = new bool[4];
    private string[] playerNickNames = new string[4];
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


        // 게임 끝남을 콜백 받음
        // result창을 setActive(true)
        // 5초뒤 다시 Looby Scene으로 돌아감.

        // timer끝남을 호출받음.
        timer.TimeEndCallback += TimeEnd;

        StartCoroutine(MakePlayerCoroutine());
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

        yield return null;

        GameObject go = PhotonNetwork.Instantiate(
                playerPrefab.name,
                playerPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1],
                Quaternion.identity,
                0);
        go.AddComponent<RunPlayerCtrl>();

        photonView.RPC("ApplyPlayerList", RpcTarget.All);

        yield return new WaitForSeconds(delay);

        photonView.RPC("SynData", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    // 전부 goalIn하는지 check
    [PunRPC]
    public void ApplyGoalIn(int _actNum)
    {
        isGoalIn[_actNum - 1] = true;

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
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
    public void SynData(int _actNum)
    {
        //Debug.LogError("SynData : " + _actNum + " Player : " + playerList[_actNum - 1] + " SetColor : " + playerMaterials[_actNum - 1] + "Time.time : " + Time.time);

        //playerList[_actNum - 1].GetComponent<MeshRenderer>().material.SetColor("_Color", playerMaterials[_actNum - 1].color);
        //Debug.LogError(playerList[_actNum - 1].GetComponent<MeshRenderer>().material.color);
        playerList[_actNum - 1].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        // playerNickNames[_actNum - 1] = PhotonNetwork.NickName;
        // playerList[_actNum - 1].transform.GetChild(0).GetComponent<TextMeshPro>().text = playerNickNames[_actNum - 1];
    }

    [PunRPC]
    public void ApplyPlayerList()
    {
        // 현재 방에 접속해 있는 플레이어의 수
        Debug.LogError("CurrentRoom PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount);

        // 현재 생성되어 있는 모든 포톤뷰 가져오기
        //PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        PhotonView[] photonViews =
            FindObjectsByType<PhotonView>(FindObjectsSortMode.None);



        // 매번 재정렬을 하는게 좋으므로 플레이어 게임오브젝트 리스트를 초기화
        System.Array.Clear(playerList, 0, playerList.Length);

        // 현재 생성되어 있는 포톤뷰 전체와
        // 접속중인 플레이어들의 액터넘버를 비교해,
        // 액터넘버를 기준으로 플레이어 게임오브젝트 배열을 채움
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            // 키는 0이 아닌 1부터 시작
            int key = i + 1;
            for (int j = 0; j < photonViews.Length; ++j)
            {
                // 만약 PhotonNetwork.Instantiate를 통해서 생성된 포톤뷰가 아니라면 넘김
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                // 만약 현재 키 값이 딕셔너리 내에 존재하지 않는다면 넘김
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                // 포톤뷰의 액터넘버
                int viewNum = photonViews[j].Owner.ActorNumber;
                // 접속중인 플레이어의 액터넘버
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;

                // 액터넘버가 같은 오브젝트가 있다면,
                if (viewNum == playerNum)
                {
                    // 실제 게임오브젝트를 배열에 추가
                    playerList[playerNum - 1] = photonViews[j].gameObject;
                    // 게임오브젝트 이름도 알아보기 쉽게 변경
                    playerList[playerNum - 1].name = "Player_" + photonViews[j].Owner.NickName;

                    // playerList[playerNum - 1].GetComponent<RunPlayerCtrl>().SetMaterial(PhotonNetwork.LocalPlayer.ActorNumber);
                }


            }
        }
    }

    }