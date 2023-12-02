using UnityEngine;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private CharacterController controller;
        [SerializeField] private AnimController anim;
        [SerializeField] private Transform camTr;

        public int teamIndex = 0;
        public float curRotation;

        private bool isMoving;
        private bool isAttack;
        private bool isJumping;
        private int jumpForce;
        private float currentSpeed;
        private Vector2 moveInput;
        private Vector3 moveDirection;
        private Vector3 velocity;



        public bool IsAttack
        {
            get => isAttack;
            set
            {
                isAttack = value;
                anim.SetBool(AnimationParam.IS_ATTACK, isAttack);
            }
        }

        public bool IsJumping
        {
            get => isJumping;
            set
            {
                isJumping = value;
                if (isJumping) anim.SetBool(AnimationParam.IS_JUMP, isJumping);
            }
        }





        private void Awake()
        {
            GetComponent<Avatar>().Pid = DataManager.Instance.MyID;
        }

        private void OnEnable()
        {
            InputActionHandler.onMove += Move;
            InputActionHandler.onJump += Jump;
            InputActionHandler.onAttack += Attack;
        }

        private void Start()
        {
            if (!GameManager.Instance.CurrentScene.Equals(SCENE.INKOPOLIS) &&
                !GameManager.Instance.CurrentScene.Equals(SCENE.INGAME))
            {
                controller.enabled = false;
                enabled = false;
            }

            curRotation = 0;
            isMoving = false;
            isAttack = false;
            isJumping = false;
            currentSpeed = moveSpeed;
            jumpForce = 7;
        }

        private void OnDisable()
        {
            InputActionHandler.onMove -= Move;
            InputActionHandler.onJump -= Jump;
            InputActionHandler.onAttack -= Attack;
        }

        private void Update()
        {
            // 애니메이션(Idle/Walk)
            anim.SetFloat(AnimationParam.SPEED, Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y));

            // 중력 처리
            if (!controller.isGrounded)
            {
                velocity.y -= GRAVITY * Time.deltaTime;
            }
            else if (GameManager.Instance.CurrentScene.Equals(SCENE.INGAME))
            {
                if (IsJumping)
                {
                    velocity.y = jumpForce;
                    anim.SetBool(AnimationParam.IS_JUMP, true);
                    IsJumping = false;
                }
                else if (anim.GetBool(AnimationParam.IS_JUMP))
                {
                    anim.SetBool(AnimationParam.IS_JUMP, false);
                }
            }

            controller.Move(velocity * Time.deltaTime);

            // 이동 방향 지정 및 이동
            if (isMoving)
            {
                moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
                transform.rotation = Quaternion.LookRotation(moveDirection);
                transform.rotation *= Quaternion.Euler(new Vector3(0, curRotation + camTr.rotation.eulerAngles.y, 0));
                transform.Translate(currentSpeed * Time.deltaTime * Vector3.forward);
            }

            // 페인트
            if (GameManager.Instance.CurrentScene.Equals(SCENE.INGAME) && IsAttack)
            {
                PaintManager.Instance.Paint(transform, DataManager.Instance.GetEquippedWeaponKey(), teamIndex);
            }
        }



        public void Move(bool isMoving, Vector2 value)
        {
            this.isMoving = isMoving;
            moveInput = value;
        }

        public void Jump()
        {
            if (controller.isGrounded && !IsJumping)
            {
                IsJumping = true;
            }
        }

        public void Attack()
        {
            IsAttack = !IsAttack;
        }
    }

}