using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using DG.Tweening;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class SaveAndLoad : Singleton<SaveAndLoad>
    {
        [SerializeField] private CanvasGroup saveIconCanvasGroup;
        private static string readUserInfoURL = "https://splatoon-php-dshzi.run.goorm.site/splatoon-php/read.php";
        private static string writeUserInfoURL = "https://splatoon-php-dshzi.run.goorm.site/splatoon-php/write.php";

        private bool isSaving = false;
        private int saveCount;
        public bool IsSaving
        {
            get => isSaving;
            set
            {
                if (isSaving != value)
                {
                    if (value) saveIconCanvasGroup.gameObject.SetActive(true);
                    saveIconCanvasGroup.DOFade(value ? 1 : 0, 0.5f).OnComplete(() => {
                        if (!value) saveIconCanvasGroup.gameObject.SetActive(false);
                        isSaving = value;
                    });
                }
            }
        }





        protected override void Awake()
        {
            base.Awake();
        }

        public void Save()
        {
            saveCount = 1;
            StartCoroutine(SavePlayerDataCrt(DataManager.Instance.MyID));
        }

        public void Save(List<int> pids)
        {
            saveCount = pids.Count;
            for (int i = 0; i < pids.Count; i++)
            {
                StartCoroutine(SavePlayerDataCrt(pids[i]));
            }
        }

        public IEnumerator SavePlayerDataCrt(int pid)
        {
            IsSaving = true;

            WWWForm form = new WWWForm();
            form.AddField(PID, pid);
            form.AddField("playerdata", JsonConvert.SerializeObject(DataManager.Instance.PlayerDataDict[pid]));

            using (UnityWebRequest www = UnityWebRequest.Post(writeUserInfoURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                   www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    //string data = www.downloadHandler.text;
                    saveCount--;
                }
            }
            if (saveCount == 0)
            {
                while (saveIconCanvasGroup.alpha < 1) yield return null;
                IsSaving = false;
            }
        }

        public IEnumerator LoadPlayerDataCrt(int pid)
        {
            if (DataManager.Instance.PlayerDataDict == null ||
                !DataManager.Instance.PlayerDataDict.ContainsKey(pid))
            {
                WWWForm form = new WWWForm();
                form.AddField(PID, pid);

                using (UnityWebRequest www = UnityWebRequest.Post(readUserInfoURL, form))
                {
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.ConnectionError ||
                       www.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        string data = www.downloadHandler.text;
                        Debug.Log(data);
                        DataManager.Instance.CachePlayerData(pid, JsonConvert.DeserializeObject<PlayerData>(data));
                    }
                }
            }
        }

        public IEnumerator SavePlayersDataCrt(Dictionary<int, int>.KeyCollection pids)
        {
            foreach (int pid in pids)
            {
                yield return SavePlayerDataCrt(pid);
            }
        }

        public IEnumerator LoadPlayersDataCrt(Dictionary<int, int>.KeyCollection pids)
        {
            foreach (int pid in pids)
            {
                yield return LoadPlayerDataCrt(pid);
            }
        }
    }
}