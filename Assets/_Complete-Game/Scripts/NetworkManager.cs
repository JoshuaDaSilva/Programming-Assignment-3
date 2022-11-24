using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using System;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager _instance = null;
    public int Token;

    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<NetworkManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("NetworkManager");
                    _instance = go.AddComponent<NetworkManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator PostRequestExample(string uri, string initials)
    {
        LoginRequestData reqData = new LoginRequestData();
        reqData.Name = initials;

        string dataAsJSON = JsonUtility.ToJson(reqData);

        UnityWebRequest req = new UnityWebRequest(uri);

        req.method = UnityWebRequest.kHttpVerbPOST;

        DownloadHandlerBuffer buff = new DownloadHandlerBuffer();
        req.downloadHandler = buff;

        byte[] bytes = Encoding.ASCII.GetBytes(dataAsJSON);
        UploadHandlerRaw uH = new UploadHandlerRaw(bytes);
        req.uploadHandler = uH;

        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        Debug.Log("Response Code:" + req.responseCode);

        if (req.isNetworkError)
        {
            Debug.Log("error: " + req.error);
        }
        else
        {
            Debug.Log("Response Received " + req.downloadHandler.text);
            LoginResponseData LDresponse = JsonUtility.FromJson<LoginResponseData>(req.downloadHandler.text);

            Debug.Log(LDresponse.SessionToken);
            UnityEngine.Random.InitState(LDresponse.SessionToken);

        }
    }

    public IEnumerator MakeGetRequest(string uri)
    {
        UnityWebRequest req = new UnityWebRequest(uri);

        req.SetRequestHeader("UserID", "1");

        DownloadHandlerBuffer buff = new DownloadHandlerBuffer();
        req.downloadHandler = buff;

        yield return req.SendWebRequest();
        // waiting until response or timeout.

        Debug.Log("Response Code:" + req.responseCode);

        if (req.isNetworkError)
        {
            Debug.Log("error: " + req.error);
        }
        else
        {
            Debug.Log("Response Received " + req.downloadHandler.text);
            PlayerDiedResponseData PDresponse = JsonUtility.FromJson<PlayerDiedResponseData>(req.downloadHandler.text);

            Debug.Log(PDresponse.UserID);
        }
    }

    public IEnumerator GetRandomRequest(string uri, Action callback)
    {
        UnityWebRequest req = new UnityWebRequest(uri);

        req.SetRequestHeader("RandVal", "1");

        DownloadHandlerBuffer buff = new DownloadHandlerBuffer();
        req.downloadHandler = buff;

        yield return req.SendWebRequest();
        // waiting until response or timeout.

        Debug.Log("Response Code:" + req.responseCode);

        if (req.isNetworkError)
        {
            Debug.Log("error: " + req.error);
        }
        else
        {
            Debug.Log("Response Received " + req.downloadHandler.text);
            OutputResponseData ODresponse = JsonUtility.FromJson<OutputResponseData>(req.downloadHandler.text);

            Debug.Log(ODresponse.RandVal);
            UnityEngine.Random.InitState(ODresponse.RandVal);
        }
        callback();
    }

    public IEnumerator UpdateScore(string uri, string initials, int level)
    {
        UpdateScoreRequestData reqData = new UpdateScoreRequestData();
        reqData.Name = initials;
        reqData.Score = level;

        string dataAsJSON = JsonUtility.ToJson(reqData);

        UnityWebRequest req = new UnityWebRequest(uri);

        req.method = UnityWebRequest.kHttpVerbPOST;

        DownloadHandlerBuffer buff = new DownloadHandlerBuffer();
        req.downloadHandler = buff;

        byte[] bytes = Encoding.ASCII.GetBytes(dataAsJSON);
        UploadHandlerRaw uH = new UploadHandlerRaw(bytes);
        req.uploadHandler = uH;

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Token", "41");

        yield return req.SendWebRequest();

        Debug.Log("Response Code:" + req.responseCode);

        if (req.isNetworkError)
        {
            Debug.Log("error: " + req.error);
        }
        else
        {
            Debug.Log("Response Received " + req.downloadHandler.text);
        }
    }
}