using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Splatoon2
{
    public class StatBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text specialCoinTxt, cashTxt, levelTxt, expTxt;
        [SerializeField] private Slider expSlider;

        void Start()
        {
            PlayerData pd = DataManager.Instance.GetMyData();
            specialCoinTxt.text = "0";
            cashTxt.text = pd.cash.ToString();
            levelTxt.text = pd.level.ToString();
            expTxt.text =
                $"{pd.exp}/{DataManager.Instance.GetMaxExp(pd.level)}";
            expSlider.value = (float)(pd.exp) /
                DataManager.Instance.GetMaxExp(pd.level);
        }
    }

}