namespace Splatoon2
{
    // Shop ���� Ŀ�� ���ۿ� �ʿ��� �ʵ� �� ����Ǵ� �޼ҵ带 �����ϴ�.
    public partial class PartialShopUI : ICursorHandler
    {
        public void MoveHorizontal(int dir)
        {
            if(State.Equals(STATE.SHOP_MAIN))
            {
                itemUIList[shop.SelectedItemIndex].Idle();
                shop.SelectedItemIndex += dir;
                itemUIList[shop.SelectedItemIndex].Focus();
                SetItemDetailCard();
            }
            else if(State.Equals(STATE.SHOP_BUY))
            {
                onBuy = !onBuy;
                ChangeSelectedBuyButton(onBuy);
            }
            UpdateCursorPos();
        }

        public void MoveVertical(int dir)
        {
            // �ƹ��͵� �������� �ʽ��ϴ�.
        }

        public void Select()
        {
            if (!itemUIList[shop.SelectedItemIndex].CanBuy) return;

            if (State.Equals(STATE.SHOP_MAIN))
            {
                State = STATE.SHOP_BUY;
                SetActiveBuyPopup(true);
            }
            else if (State.Equals(STATE.SHOP_BUY))
            {
                if (onBuy) shop.Buy(shop.ItemKeys[shop.SelectedItemIndex]);
                State = STATE.SHOP_MAIN;
                SetActiveBuyPopup(false);
            }
            UpdateCursorPos();
        }

        public void MoveBack()
        {
            if (State.Equals(STATE.SHOP_MAIN))
            {
                shop.Exit();
            }
            else if (State.Equals(STATE.SHOP_BUY))
            {
                SetActiveBuyPopup(false);
                State = STATE.SHOP_MAIN;
                UpdateCursorPos();
            }
        }
        private void InitCursor()
        {
            cursor.rt.gameObject.SetActive(true);
            UpdateCursorPos();
        }

        public void UpdateCursorPos()
        {
            if (State.Equals(STATE.SHOP_MAIN))
            {
                cursor.rt.position = itemUIList[shop.SelectedItemIndex].CursorPointRt.position;
            }
            else if (State.Equals(STATE.SHOP_BUY))
            {
                if (onBuy) cursor.rt.position = buyButton.CursorPointRt.position;
                else cursor.rt.position = cancelBuyButton.CursorPointRt.position;
            }
        }
    }

}