using UnityEngine;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField] private SCENE goTo;

        void Start()
        {
            if (!goTo.Equals(SCENE.INKOPOLIS)) GameManager.Instance.IsTitlePassed = true;
            LoadingSceneManager.LoadScene(goTo);
        }
    }
}