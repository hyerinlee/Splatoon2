using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class Ingame : MonoBehaviour
    {
        public static Ingame Instance;

        private static event Action onWaiting, onGameReady, onGameStart, onOneMinLeft, onGameOver, onFinished;
        private static event Action<float> onTimeChanged;
        private static event Action<int> onScoreChanged;
        private static Dictionary<int, int> teamPlayerIds = new Dictionary<int, int> { { 1234, 0 }, { 1235, 1 } };

        private float leftTime;
        private int playTime = 20;
        private int validScoreAreaCount = 0;
        private int playerNum, loadedPlayerNum;
        private int victoryTeamIndex, lastCash, lastLevel, lastExp;
        private int matchColorIndex;
        private float[] teamPercentages = new float[2];
        private int[] teamScores = new int[2];
        private MatchColorSO matchColor;
        private STATE state = STATE.NONE;
        private Dictionary<int, int> playerPoints;

        public enum STATE { WAITING, READY, PLAYING, MIN_LEFT, GAMEOVER, CAN_EXIT, NONE }



        public static Dictionary<int, int> TeamPlayerIds
        {
            get => teamPlayerIds;
        }
        public int MatchColorIndex
        {
            get => matchColorIndex;
        }
        public MatchColorSO MatchColor
        {
            get => matchColor;
            set => matchColor = value;
        }
        public int LoadedPlayerNum
        {
            get => loadedPlayerNum;
            set
            {
                loadedPlayerNum = value;
                if (loadedPlayerNum == playerNum)
                {
                    State = STATE.READY;
                    DOVirtual.DelayedCall(3f, () => { State = STATE.PLAYING; });
                }
            }
        }
        public int VictoryTeamIndex
        {
            get => victoryTeamIndex;
            set => victoryTeamIndex = value;
        }
        public int LastCash
        {
            get => lastCash;
        }
        public int LastLevel
        {
            get => lastLevel;
        }
        public int LastExp
        {
            get => lastExp;
        }
        public STATE State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    state = value;
                    switch (state)
                    {
                        case STATE.WAITING:
                            onWaiting?.Invoke();
                            break;
                        case STATE.READY:
                            onGameReady?.Invoke();
                            break;
                        case STATE.PLAYING:
                            onGameStart?.Invoke();
                            break;
                        case STATE.MIN_LEFT:
                            onOneMinLeft?.Invoke();
                            break;
                        case STATE.GAMEOVER:
                            onGameOver?.Invoke();
                            break;
                        case STATE.CAN_EXIT:
                            onFinished?.Invoke();
                            break;
                    }
                }
            }
        }
        public float LeftTime
        {
            get => leftTime;
            set
            {
                if (leftTime != value)
                {
                    leftTime = value;
                    onTimeChanged?.Invoke(leftTime);
                }
            }
        }
        public int[] TeamScores
        {
            get => teamScores;
        }
        public float[] TeamPercentages
        {
            get => teamPercentages;
        }
        public Dictionary<int, int> PlayerPoints
        {
            get => playerPoints;
            set => playerPoints = value;
        }
        public static event Action OnWaiting
        {
            add => onWaiting += value;
            remove => onWaiting -= value;
        }
        public static event Action OnGameReady
        {
            add => onGameReady += value;
            remove => onGameReady -= value;
        }
        public static event Action OnGameStart
        {
            add => onGameStart += value;
            remove => onGameStart -= value; 
        }
        public static event Action OnOneMinLeft
        {
            add => onOneMinLeft += value;
            remove => onOneMinLeft -= value;
        }
        public static event Action OnGameOver
        {
            add => onGameOver += value;
            remove => onGameOver -= value;
        }
        public static event Action OnFinished
        {
            add => onFinished += value;
            remove => onFinished -= value;
        }
        public static event Action<float> OnTimeChanged
        {
            add => onTimeChanged += value;
            remove => onTimeChanged -= value;
        }
        public static event Action<int> OnScoreChanged
        {
            add => onScoreChanged += value;
            remove => onScoreChanged -= value;
        }
        public int ValidScoreAreaNum
        {
            get => validScoreAreaCount;
            set => validScoreAreaCount = value;
        }





        private void Awake()
        {
            Instance = this;
            MatchColor = ResourceManager.Instance.MatchColors[MatchColorIndex];
            PlayerPoints = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> pair in TeamPlayerIds)
            {
                PlayerPoints.Add(pair.Key, 0);
            }
        }

        private void OnEnable()
        {
            OnGameStart += GameStart;
            OnGameOver += GameOver;
            OnFinished += ChangeActionMap;
        }

        private void Start()
        {
            PlayerData myData = DataManager.Instance.GetMyData();
            lastCash = myData.cash;
            lastLevel = myData.level;
            lastExp = myData.exp;
            TeamScores[0] = 0;
            TeamScores[1] = 0;

            State = STATE.WAITING;
            SetAllPlayersLoaded();
        }

        private void OnDisable()
        {
            OnGameStart -= GameStart;
            OnGameOver -= GameOver;
            OnFinished -= ChangeActionMap;
            InputActionHandler.onOK -= Exit;
        }



        public void SaveResult()
        {
            SaveAndLoad.Instance.Save(teamPlayerIds.Keys.ToList());
        }

        public void UpdateScore(int teamIndex, int value)
        {
            if (value == 0) return;
            TeamScores[teamIndex] += value;
            TeamPercentages[teamIndex] = (float)Math.Round((double)TeamScores[teamIndex] / ValidScoreAreaNum * 100, 1);

            foreach (KeyValuePair<int, int> pair in TeamPlayerIds)
            {
                PlayerPoints[pair.Key] = (int)MathF.Round(TeamPercentages[pair.Value] * 10);
            }

            if (teamIndex == 0) onScoreChanged?.Invoke(PlayerPoints[DataManager.Instance.MyID]);
        }

        /// <summary>
        /// [임시] P2P 배틀로 가정, 4초 후 플레이어 로드가 완료된 상황으로 만듦.
        /// </summary>
        private void SetAllPlayersLoaded()
        {
            playerNum = 2;
            DOVirtual.DelayedCall(4f, () => { LoadedPlayerNum = playerNum; });
        }

        private void GameStart()
        {
            InputActionHandler.Instance.Active(true);
            StartCoroutine(TimerCrt());
        }

        private void GameOver()
        {
            InputActionHandler.Instance.Active(false);

            VictoryTeamIndex = (TeamPercentages[0] > TeamPercentages[1] ? 0 : 1);

            foreach (KeyValuePair<int, int> pair in TeamPlayerIds)
            {
                PlayerData pd = DataManager.Instance.PlayerDataDict[pair.Key];
                pd.cash += PlayerPoints[pair.Key];
                pd.exp += PlayerPoints[pair.Key];
                if (DataManager.Instance.GetMaxExp(LastLevel) <= pd.exp)
                {
                    pd.exp -= DataManager.Instance.GetMaxExp(LastLevel);
                    pd.level++;
                }
            }
        }

        private void ChangeActionMap()
        {
            InputActionHandler.Instance.Active(true);
            InputActionHandler.Instance.ChangeActionMap(ACTION_MAP.UI);
            InputActionHandler.onOK += Exit;
        }

        private void Exit()
        {
            GameManager.Instance.ChangeScene(SCENE.INKOPOLIS);
        }



        private IEnumerator TimerCrt()
        {
            LeftTime = playTime;
            while (LeftTime > 1)
            {
                LeftTime -= Time.deltaTime;
                if (LeftTime < 11) State = STATE.MIN_LEFT;
                yield return null;
            }
            LeftTime = 0;
            State = STATE.GAMEOVER;
        }
    }
}