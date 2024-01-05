using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private TransitionMask transitionMask;

        public IEnumerator hideMainCanvasCrt;
        public Action onSceneChange;

        private bool isTitlePassed = false;
        private SCENE currentScene;



        public bool IsTitlePassed
        {
            get => isTitlePassed;
            set => isTitlePassed = value;
        }

        public SCENE CurrentScene
        {
            get => currentScene;
            set => currentScene = value;
        }





        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }



        public void ChangeScene(SCENE nextScene, ITEM_CATEGORY? category = null)
        {
            if (nextScene.Equals(SCENE.SHOP)) Shop.CurrentShop = (int)category;
            StartCoroutine(ChangeSceneCrt(nextScene));
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            StartCoroutine(AfterSceneLoadedCrt());
        }



        private IEnumerator ChangeSceneCrt(SCENE nextScene)
        {
            InputActionHandler.Instance.Active(false);
            onSceneChange?.Invoke();

            if (hideMainCanvasCrt != null) yield return StartCoroutine(hideMainCanvasCrt);

            transitionMask.gameObject.SetActive(true);
            yield return StartCoroutine(transitionMask.FadeoutCrt());

            LoadingSceneManager.LoadScene(nextScene);
            yield return null;
        }

        public IEnumerator AfterSceneLoadedCrt()
        {
            if (CurrentScene.Equals(SCENE.LOADING))
            {
                transitionMask.gameObject.SetActive(false);
            }
            else yield return StartCoroutine(transitionMask.FadeinCrt());

            InputActionHandler.Instance.Active(true);
        }
    }
}