using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Splatoon2
{
    public class TransitionMask : MonoBehaviour
    {
        [SerializeField] private PlayableDirector transitionPd;

        private void Awake()
        {
            transitionPd.timeUpdateMode = DirectorUpdateMode.Manual;
        }

        public IEnumerator FadeoutCrt()
        {
            float time = 0;
            while (time < 1f)
            {
                time += Time.deltaTime;
                transitionPd.time = time;
                transitionPd.Evaluate();
                yield return null;
            }
            transitionPd.time = 1;
            transitionPd.Evaluate();
        }
        public IEnumerator FadeinCrt()
        {
            float time = 1;
            while (time > 0f)
            {
                time -= Time.deltaTime;
                transitionPd.time = time;
                transitionPd.Evaluate();
                yield return null;
            }
            transitionPd.time = 0;
            transitionPd.Evaluate();
        }
    }
}