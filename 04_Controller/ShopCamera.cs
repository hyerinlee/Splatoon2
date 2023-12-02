using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Splatoon2
{
    public class ShopCamera : MonoBehaviour
    {
        [SerializeField] private List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>(4);





        private void OnEnable()
        {
            InputActionHandler.onPressLR += MoveCamera;
        }

        void Start()
        {
            MoveCamera();
        }

        private void OnDisable()
        {
            InputActionHandler.onPressLR -= MoveCamera;
        }



        public void MoveCamera()
        {
            if (!PartialShopUI.State.Equals(PartialShopUI.STATE.SHOP_MAIN)) return;
            cameras[Shop.CurrentShop].MoveToTopOfPrioritySubqueue();
        }
    }
}