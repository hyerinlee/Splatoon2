using UnityEngine;
using static Splatoon2.Define;

namespace Splatoon2
{
    [RequireComponent(typeof(BoxCollider))]
    public class Teleport : MonoBehaviour
    {
        public SCENE GoTo;
        public ITEM_CATEGORY shop;





        private void OnTriggerEnter(Collider other)
        {
            GameManager.Instance.ChangeScene(GoTo, shop);
        }
    }
}