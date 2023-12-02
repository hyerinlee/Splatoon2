using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Splatoon2
{
    public class PlayerResultPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelTxt, playerNameTxt, pointTxt;
        [SerializeField] private Image weaponImg;





        public void SetPlayerResultUI(int pid)
        {
            levelTxt.text = DataManager.Instance.PlayerDataDict[pid].level.ToString();
            playerNameTxt.text = DataManager.Instance.PlayerDataDict[pid].name;
            pointTxt.text = Ingame.Instance.PlayerPoints[pid].ToString();
            weaponImg.sprite = ResourceManager.Instance.ItemSpriteDict[
                DataManager.Instance.GetEquippedWeaponKey(pid)];
        }
    }
}