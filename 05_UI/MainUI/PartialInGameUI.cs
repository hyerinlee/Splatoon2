using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;
using TMPro;
using System;

namespace Splatoon2
{
    public partial class PartialInGameUI : MonoBehaviour
    {
        [Header("타임라인")]
        [SerializeField] private GameObject waitTimeline;
        [SerializeField] private GameObject readyGoTimeline;

        [Header("인게임 주요 UI")]
        [SerializeField] private GameObject waitingPanel;
        [SerializeField] private GameObject playingPanel;
        [SerializeField] private Image whiteFadingPanel;
        [SerializeField] private GameObject countdown;

        [Header("상단 UI")]
        [SerializeField] private TMP_Text txt_clock;
        [SerializeField] private TMP_Text txt_point;
        [SerializeField] private List<TeamPanel> teamPanels;

        [Header("카메라")]
        [SerializeField] private CinemachineVirtualCamera waitingCamera;
        [SerializeField] private CinemachineFreeLook thirdPersonCamera;

        [Header("게임 결과 UI")]
        [SerializeField] private SpriteRenderer percentageSpriteRenderer;
        [SerializeField] private List<TMP_Text> percentageTxt;
        [SerializeField] private List<TMP_Text> pointTxt;
        [SerializeField] private List<Image> juddFlagImages;
        [SerializeField] private List<Animator> juddAnimators;
        [SerializeField] private List<RuntimeAnimatorController> juddAnimContollers, lilJuddAnimControllers;
        [SerializeField] private TMP_Text victoryOrDefeatTxt;
        [SerializeField] private TMP_Text playerNameTxt;
        [SerializeField] private TMP_Text myPointTxt;
        [SerializeField] private List<TeamResultCard> teamResultCards;
        [SerializeField] private TMP_Text cashTxt;
        [SerializeField] private TMP_Text levelTxt;
        [SerializeField] private TMP_Text levelUpTxt;
        [SerializeField] private TMP_Text expTxt;
        [SerializeField] private Slider expSlider;
        [SerializeField] private List<GearCard> gearCards;





        private void OnEnable()
        {
            Ingame.OnWaiting += ShowWaitingCutScene;
            Ingame.OnWaiting += SetTeamBar;
            Ingame.OnGameReady += FinishWaitingAndReady;
            Ingame.OnGameStart += SetPlayingUI;
            Ingame.OnScoreChanged += UpdatePoint;
            Ingame.OnTimeChanged += UpdateClock;
            Ingame.OnOneMinLeft += ShowCountdown;
            Ingame.OnGameOver += SetResultUI;
        }

        private void OnDisable()
        {
            Ingame.OnWaiting -= ShowWaitingCutScene;
            Ingame.OnWaiting -= SetTeamBar;
            Ingame.OnGameReady -= FinishWaitingAndReady;
            Ingame.OnGameStart -= SetPlayingUI;
            Ingame.OnScoreChanged -= UpdatePoint;
            Ingame.OnTimeChanged -= UpdateClock;
            Ingame.OnOneMinLeft -= ShowCountdown;
            Ingame.OnGameOver -= SetResultUI;
        }



        /// <summary>
        /// 대기 컷신을 보여주는 타임라인 재생(루프).
        /// </summary>
        private void ShowWaitingCutScene()
        {
            waitingPanel.SetActive(true);
            waitTimeline.SetActive(true);
        }

        private void SetTeamBar()
        {
            int[] idx = new int[]{ 0, 0 };
            foreach (KeyValuePair<int, int> pair in Ingame.TeamPlayerIds)
            {
                teamPanels[pair.Value].bgImg[idx[pair.Value]].color = Ingame.Instance.MatchColor.color[pair.Value];
                teamPanels[pair.Value].gearImg[idx[pair.Value]].sprite = ResourceManager.Instance.ItemSpriteDict[
                    DataManager.Instance.GetEquippedWeaponKey(pair.Key)];
                idx[pair.Value]++;
            }
        }

        /// <summary>
        /// 참가 플레이어 로딩 완료 시 호출. <br/>
        /// 타임라인 플로우: 대기 컷신 중지 > 페이드아웃 > 카메라 변경 > 페이드인 > 준비 > 시작
        /// </summary>
        private void FinishWaitingAndReady()
        {
            StartCoroutine(FinishWaitingAndReadyCrt());
        }

        /// <summary>
        /// 게임이 시작되었을 때 호출. <br/>
        /// 양 팀의 참가자 정보 및 남은 시간, 포인트를 표시.
        /// </summary>
        private void SetPlayingUI()
        {
            playingPanel.SetActive(true);
        }

        private void ShowCountdown()
        {
            countdown.SetActive(true);
        }

        private void UpdateClock(float leftTime)
        {
            txt_clock.text = $"{(int)(leftTime / 60)}:{(int)(leftTime % 60):D2}";
        }

        private void UpdatePoint(int value)
        {
            txt_point.text = $"{value}";
        }



        private IEnumerator FinishWaitingAndReadyCrt()
        {
            yield return whiteFadingPanel.DOFade(1, 0.5f).WaitForCompletion();
            thirdPersonCamera.MoveToTopOfPrioritySubqueue();
            waitingPanel.SetActive(false);
            waitTimeline.SetActive(false);
            yield return whiteFadingPanel.DOFade(0, 0.5f).WaitForCompletion();
            readyGoTimeline.SetActive(true);
        }
    }

    [Serializable]
    public class TeamPanel
    {
        public List<Image> bgImg;
        public List<Image> gearImg;
    }
}