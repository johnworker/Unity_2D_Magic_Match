using UnityEngine;
using UnityEngine.Tilemaps;

namespace Match3
{
    /// <summary>
    /// Tile �Ȧb�s��/���J�ɥΩ�w�q�i�]�t Tile ���x�s��C�p�G PlacedGem ���šA�h�p��
    /// �H���_�ۡA�N�q�i���_�۲M�椤�H���D��@���_�������C
    /// ���϶��b����Ҧ��Ϋظm�����|�㦳�����ı�ե�A�]�����Ȧb�s��������ΡC
    /// </summary>
    [CreateAssetMenu(fileName = "GemPlacerTile", menuName = "2D Match/Tile/Gem Placer")]
    public class GemPlacerTile : TileBase
    {
        public Sprite PreviewEditorSprite;
        [Tooltip("If null this will be a random gem")]
        public Gem PlacedGem = null;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            //��S������ɡ]�]���b�s�边���B�󼽩�Ҧ����~�^�Ǧ^���w���w�� Sprite�A�_�h�Ǧ^ null
            //�b�C���L�{���A�϶��O�u���i�����v�]�_�۬O�ڭ̪��t�γB�z���C����H�A�Ӥ��O�϶��a�ϡ^
            tileData.sprite = !Application.isPlaying ? PreviewEditorSprite : null;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return false;
#endif
            //���϶��ȥΩ�s�边�����U�]�p���d�C�b�B��ɡA�ڭ̳q���O�A���϶��O
            //��m�_�۪��a��A�M��R���]�p�ɶȧ@����ı���U���C������C���Ʒ|�N�t�d
            //�b���̳гy�X�_�ۡC
            Board.RegisterCell(position, PlacedGem);

            return base.StartUp(position, tilemap, go);
        }
    }
}
