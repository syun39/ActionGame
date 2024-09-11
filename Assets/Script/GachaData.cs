using UnityEngine;

[CreateAssetMenu(fileName = "GachaData", menuName = "Gacha/GachaData")]
public class GachaData : ScriptableObject
{
    // �擾�����e�N�X�`����ۑ����邽�߂̕ϐ�
    [SerializeField] public Texture2D[] gachaTextures;
}