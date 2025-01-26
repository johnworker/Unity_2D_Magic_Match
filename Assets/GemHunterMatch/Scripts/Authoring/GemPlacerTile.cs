using UnityEngine;
using UnityEngine.Tilemaps;

namespace Match3
{
    /// <summary>
    /// Tile 僅在編輯/載入時用於定義可包含 Tile 的儲存格。如果 PlacedGem 為空，則計為
    /// 隨機寶石，將從可用寶石清單中隨機挑選一種寶石類型。
    /// 此圖塊在播放模式或建置中不會具有任何視覺組件，因為它僅在編輯期間有用。
    /// </summary>
    [CreateAssetMenu(fileName = "GemPlacerTile", menuName = "2D Match/Tile/Gem Placer")]
    public class GemPlacerTile : TileBase
    {
        public Sprite PreviewEditorSprite;
        [Tooltip("If null this will be a random gem")]
        public Gem PlacedGem = null;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            //當沒有播放時（因此在編輯器中處於播放模式之外）傳回給定的預覽 Sprite，否則傳回 null
            //在遊戲過程中，圖塊是「不可見的」（寶石是我們的系統處理的遊戲對象，而不是圖塊地圖）
            tileData.sprite = !Application.isPlaying ? PreviewEditorSprite : null;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return false;
#endif
            //此圖塊僅用於編輯器中幫助設計關卡。在運行時，我們通知板，此圖塊是
            //放置寶石的地方，然後刪除設計時僅作為視覺輔助的遊戲物件。董事會將負責
            //在那裡創造出寶石。
            Board.RegisterCell(position, PlacedGem);

            return base.StartUp(position, tilemap, go);
        }
    }
}
