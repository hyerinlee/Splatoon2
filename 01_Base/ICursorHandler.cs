namespace Splatoon2
{
    interface ICursorHandler
    {
        void UpdateCursorPos();

        void MoveHorizontal(int dir);

        void MoveVertical(int dir);

        /// <summary> 'A'Ű�� ���� </summary>
        void Select();

        /// <summary> 'B'Ű�� ���� </summary>
        void MoveBack();
    }

}