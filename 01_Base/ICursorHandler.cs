namespace Splatoon2
{
    interface ICursorHandler
    {
        void UpdateCursorPos();

        void MoveHorizontal(int dir);

        void MoveVertical(int dir);

        /// <summary> 'A'키에 대응 </summary>
        void Select();

        /// <summary> 'B'키에 대응 </summary>
        void MoveBack();
    }

}