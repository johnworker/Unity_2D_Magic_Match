using UnityEngine;
using UnityEngine.Tilemaps;

namespace Match3
{
    /// <summary>
    /// 這是一個編輯時間圖塊，用於定義將在其下方產生單元格的單元格的位置。在編輯時和編輯之外
    /// 在播放模式中，預覽精靈將顯示在地圖上，但在運行時，地圖將為空，因為它只是一個
    /// 遊戲邏輯佈局。
    /// </summary>
    [CreateAssetMenu(fileName = "GemSpawnerPlacerTile", menuName = "2D Match/Tile/Gem Spawner Placer")]
    public class GemSpawner : TileBase
    {
        public Sprite EditorPreviewSprite;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = !Application.isPlaying ? EditorPreviewSprite : null;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return false;
#endif

            //此圖塊僅用於編輯器中幫助設計關卡。在運行時，我們通知板，此圖塊是
            //寶石的產地。董事會將負責在那裡打造一顆璀璨的寶石。
            Board.RegisterSpawner(position);

            return base.StartUp(position, tilemap, go);
        }
    }
}
