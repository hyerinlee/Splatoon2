using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class Opponent : MonoBehaviour
    {
        [SerializeField] private SimpleWaypoint waypointManager;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private AnimController anim;

        private static int pid = 1235;

        private bool isAttack;
        private int teamIndex = 1;
        private string weaponKey;



        public bool IsAttack
        {
            get => isAttack;
            set
            {
                isAttack = value;
                anim.SetBool(AnimationParam.IS_ATTACK, isAttack);
            }
        }





        private void Awake()
        {
            GetComponent<Avatar>().Pid = pid;
        }

        private void OnEnable()
        {
            agent.isStopped = true;
            Ingame.OnGameStart += Run;
            Ingame.OnGameOver += Stop;
        }

        private void Start()
        {
            weaponKey = DataManager.Instance.PlayerDataDict[pid].equippedWeapon;
        }

        private void OnDisable()
        {
            Ingame.OnGameStart -= Run;
            Ingame.OnGameOver -= Stop;
        }



        private void Run()
        {
            StartCoroutine(MoveCoroutine());
        }

        private void Stop()
        {
            IsAttack = false;
            agent.isStopped = true;
            anim.SetFloat(AnimationParam.SPEED, 0);
            enabled = false;
        }



        private IEnumerator MoveCoroutine()
        {
            agent.isStopped = false;
            IsAttack = true;

            while (!agent.isStopped)
            {
                if (IsAttack)
                {
                    PaintManager.Instance.Paint(transform, weaponKey, teamIndex);
                }
                anim.SetFloat(AnimationParam.SPEED, agent.speed);

                if (agent.remainingDistance <= 0.1f)
                {
                    agent.destination = waypointManager.UpdatePos();
                    agent.isStopped = false;
                    yield return new WaitForSeconds(Random.Range(0, 3));
                }
                yield return null;
            }
        }
    }
}