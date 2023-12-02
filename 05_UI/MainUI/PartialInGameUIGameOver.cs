using System;
using System.Collections;
using UnityEngine;
using static Splatoon2.Define;

namespace Splatoon2
{
    public partial class PartialInGameUI : MonoBehaviour
    {
        private PlayerData pd;
        private static readonly WaitForSeconds wait = new WaitForSeconds(1f);





        public void UpdateTeamGauge()
        {
            StartCoroutine(UpdateTeamGaugeCrt());
        }

        public void UpdateCashAndExp()
        {
            StartCoroutine(UpdateExpAndLevelCrt());
        }



        private void SetResultUI()
        {
            percentageSpriteRenderer.material.SetColor("_Color1", Ingame.Instance.MatchColor.color[0]);
            percentageSpriteRenderer.material.SetColor("_Color2", Ingame.Instance.MatchColor.color[1]);
            percentageSpriteRenderer.material.SetFloat("_LeftPercentage", 0);
            percentageSpriteRenderer.material.SetFloat("_RightPercentage", 0);

            juddFlagImages[0].color = Ingame.Instance.MatchColor.color[0];
            juddFlagImages[1].color = Ingame.Instance.MatchColor.color[1];

            for (int i = 0; i < 2; i++)
            {
                percentageTxt[i].text = $"{0:F1}%";
                pointTxt[i].text = $"{(int)MathF.Round(Ingame.Instance.TeamPercentages[i] * 10)}p";
            }

            if(Ingame.Instance.TeamPercentages[0] > Ingame.Instance.TeamPercentages[1]){
                victoryOrDefeatTxt.text = VICTORY;
                juddAnimators[0].runtimeAnimatorController = juddAnimContollers[0];
                juddAnimators[1].runtimeAnimatorController = lilJuddAnimControllers[1];
            }
            else
            {
                victoryOrDefeatTxt.text = DEFEAT;
                juddAnimators[0].runtimeAnimatorController = juddAnimContollers[1];
                juddAnimators[1].runtimeAnimatorController = lilJuddAnimControllers[0];
            }
            
            playerNameTxt.text = DataManager.Instance.GetMyData().name;


            myPointTxt.text = $"{Ingame.Instance.PlayerPoints[DataManager.Instance.MyID]}";
            for (int i=0; i<teamResultCards.Count; i++)
            {
                teamResultCards[i].SetTeamResultCard(i);
            }

            pd = DataManager.Instance.GetMyData();
            cashTxt.text = Ingame.Instance.LastCash.ToString();
            levelTxt.text = Ingame.Instance.LastLevel.ToString();
            expTxt.text = Ingame.Instance.LastExp.ToString();
            expSlider.value = (float)Ingame.Instance.LastExp / DataManager.Instance.GetMaxExp(Ingame.Instance.LastLevel);

            gearCards[0].SetGearCard(GEAR_CATEGORY.HEAD);
            gearCards[1].SetGearCard(GEAR_CATEGORY.CLOTHES);
            gearCards[2].SetGearCard(GEAR_CATEGORY.SHOES);
        }

        private IEnumerator UpdateTeamGaugeCrt()
        {
            // 1초 동안 페인팅 퍼센티지 및 포인트를 일정 수치만큼 올림(0 ~ totalAmount)
            float time = 0f;
            float duration = 2f;
            while (time < duration)
            {
                time += Time.deltaTime;
                for (int i = 0; i < 2; i++)
                {
                    percentageTxt[i].text = $"{Mathf.Lerp(0, Ingame.Instance.TeamPercentages[i], time / duration):F1}%";
                }
                percentageSpriteRenderer.material.SetFloat("_LeftPercentage",
                   Mathf.Lerp(0, Ingame.Instance.TeamPercentages[0], time / duration));
                percentageSpriteRenderer.material.SetFloat("_RightPercentage",
                   Mathf.Lerp(0, Ingame.Instance.TeamPercentages[1], time / duration));
                yield return null;
            }
            yield return null;
        }

        private IEnumerator UpdateExpAndLevelCrt()
        {
            StartCoroutine(UpdateCashCrt());
            int lv = Ingame.Instance.LastLevel;
            int startExpSum = Ingame.Instance.LastExp;
            int endExpSum = pd.exp;
            int currentExp = startExpSum;
            int lastLevelmaxExpSum = 0;
            int levelUpBoundary = DataManager.Instance.GetMaxExp(lv);
            for (int i=1; i<lv; i++)
            {
                lastLevelmaxExpSum += DataManager.Instance.GetMaxExp(i);
            }
            startExpSum += lastLevelmaxExpSum;
            for(int i=1; i<pd.level; i++)
            {
                endExpSum += DataManager.Instance.GetMaxExp(i);
            }

            float time = 0f;
            while (time < 2f)
            {
                time += Time.deltaTime;
                currentExp = (int)Mathf.Lerp(startExpSum, endExpSum, time / 2f) - lastLevelmaxExpSum;
                expTxt.text = currentExp.ToString();
                expSlider.value = (float)currentExp / levelUpBoundary;
                if (levelUpBoundary <= currentExp)
                {
                    levelUpTxt.enabled = true;
                    lv++;
                    levelTxt.text = lv.ToString();
                    lastLevelmaxExpSum += levelUpBoundary;
                    levelUpBoundary = DataManager.Instance.GetMaxExp(lv);
                }
                yield return null;
            }
            levelTxt.text = pd.level.ToString();
            expTxt.text = pd.exp.ToString();
            expSlider.value = (float)pd.exp / DataManager.Instance.GetMaxExp(pd.level);

            Ingame.Instance.State = Ingame.STATE.CAN_EXIT;
        }



        private IEnumerator UpdateCashCrt()
        {
            float time = 0f;
            while (time < 2f)
            {
                time += Time.deltaTime;
                cashTxt.text = ((int)(Mathf.Lerp(Ingame.Instance.LastCash, pd.cash, time / 2f))).ToString();
                yield return null;
            }
        }
    }

}