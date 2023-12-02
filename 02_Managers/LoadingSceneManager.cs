using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using DG.Tweening;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class LoadingSceneManager : MonoBehaviour
    {
        [SerializeField] private Image blackMask;

        public static SCENE scene;

        private AsyncOperationHandle<SceneInstance> opHandle;
        private bool isLoadingIconAppeared;





        private void OnEnable()
        {
            SceneManager.sceneLoaded += GameManager.Instance.OnSceneLoaded;
        }

        private void Start()
        {
            isLoadingIconAppeared = false;
            blackMask.DOFade(0f, 1f).OnComplete(() => {
                isLoadingIconAppeared = true;
            });

            StartCoroutine(LoadSceneCrt());
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= GameManager.Instance.OnSceneLoaded;
        }



        public static void LoadScene(SCENE scene)
        {
            ResourceManager.Instance.ReleaseResources(GameManager.Instance.CurrentScene);
            GameManager.Instance.CurrentScene = SCENE.LOADING;
            LoadingSceneManager.scene = scene;
            SceneManager.LoadSceneAsync("Loading");
        }



        private IEnumerator LoadSceneCrt()
        {
            if (scene.Equals(SCENE.INGAME))
                yield return StartCoroutine(SaveAndLoad.Instance.LoadPlayersDataCrt(Ingame.TeamPlayerIds.Keys));
            else yield return StartCoroutine(SaveAndLoad.Instance.LoadPlayerDataCrt(DataManager.Instance.MyID));

            yield return StartCoroutine(ResourceManager.Instance.LoadSceneResourcesCrt(scene));

            opHandle = Addressables.LoadSceneAsync(SCENE_TO_STR[scene], LoadSceneMode.Single, false);
            yield return opHandle;

            if (opHandle.Status.Equals(AsyncOperationStatus.Succeeded))
            {
                while (!isLoadingIconAppeared)
                {
                    yield return null;
                }

                GameManager.Instance.CurrentScene = scene;

                switch (GameManager.Instance.CurrentScene)
                {
                    case SCENE.SHOP:
                    case SCENE.MODESELECT:
                    case SCENE.EQUIPMENT:
                        InputActionHandler.Instance.SetDefaultActionMap(ACTION_MAP.UI);
                        InputActionHandler.Instance.ChangeActionMap(ACTION_MAP.UI);
                        break;
                    case SCENE.INKOPOLIS:
                        if (GameManager.Instance.IsTitlePassed)
                        {
                            InputActionHandler.Instance.SetDefaultActionMap(ACTION_MAP.PLAYER_INKOPOLIS);
                            InputActionHandler.Instance.ChangeActionMap(ACTION_MAP.PLAYER_INKOPOLIS);
                        }
                        break;
                    case SCENE.INGAME:
                        InputActionHandler.Instance.SetDefaultActionMap(ACTION_MAP.PLAYER_INGAME);
                        InputActionHandler.Instance.ChangeActionMap(ACTION_MAP.PLAYER_INGAME);
                        InputActionHandler.Instance.Active(false);
                        break;
                    default:
                        break;
                }
                yield return blackMask.DOFade(1f, 0.5f).WaitForCompletion();
                yield return opHandle.Result.ActivateAsync();
            }
        }
    }
}