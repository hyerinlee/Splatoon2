using UnityEngine;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class AnimController : MonoBehaviour
    {
        [SerializeField] private Animator animator;





        private void Awake()
        {
            switch (GameManager.Instance.CurrentScene)
            {
                case SCENE.SHOP:
                case SCENE.MODESELECT:
                case SCENE.EQUIPMENT:
                    SetAnimationController(ANIM_CONTROLLER.DEFAULT);
                    break;
                case SCENE.INKOPOLIS:
                    SetAnimationController(ANIM_CONTROLLER.INKOPOLIS);
                    break;
                case SCENE.INGAME:
                    SetAnimationController(ANIM_CONTROLLER.INGAME);
                    break;
            }
        }



        public bool GetBool(string param)
        {
            return animator.GetBool(param);
        }

        public void SetBool(string param, bool value)
        {
            animator.SetBool(param, value);
        }

        public void SetFloat(string param, float value)
        {
            animator.SetFloat(param, value);
        }

        public void SetAnimationController(ANIM_CONTROLLER animController)
        {
            animator.runtimeAnimatorController = ResourceManager.Instance.PlayerAnimControllerDict[animController];
        }
    }

}