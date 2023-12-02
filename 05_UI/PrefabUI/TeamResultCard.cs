using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class TeamResultCard : MonoBehaviour
    {
        [SerializeField] private Image CardImg;
        [SerializeField] private TMP_Text victoryOrDefeatTxt;
        [SerializeField] List<PlayerResultPanel> playerResultPanels;

        public void SetTeamResultCard(int teamIndex)
        {
            CardImg.color = Ingame.Instance.MatchColor.color[teamIndex];
            victoryOrDefeatTxt.text = ((teamIndex.Equals(Ingame.Instance.VictoryTeamIndex)) ?
                VICTORY : DEFEAT);

            int idx = 0;
            foreach (KeyValuePair<int, int> pair in Ingame.TeamPlayerIds)
            {
                if (pair.Value == teamIndex)
                {
                    playerResultPanels[idx].SetPlayerResultUI(pair.Key);
                    idx++;
                }
            }
        }
    }
}