using UnityEngine;

public class RarityText : MonoBehaviour
{
    [SerializeField] GameObject _rarityTextR; // R�e�L�X�g
    [SerializeField] GameObject _rarityTextSR; // SR�e�L�X�g
    [SerializeField] GameObject _rarityTextSSR; // SSR�e�L�X�g
    [SerializeField] GameObject _rarityTextUR; // UR�e�L�X�g

    /// <summary>
    /// ���A���e�B�ɉ����ĊY������e�L�X�g��\��
    /// </summary>
    public void SetRarity(Rarity rarity)
    {
        // �S�Ẵ��A���e�B�e�L�X�g���\���ɂ���
        _rarityTextR.SetActive(false);
        _rarityTextSR.SetActive(false);
        _rarityTextSSR.SetActive(false);
        _rarityTextUR.SetActive(false);

        // �I�����ꂽ���A���e�B�̃e�L�X�g��\������
        if (rarity == Rarity.R)
        {
            _rarityTextR.SetActive(true); // R��\��
        }
        else if (rarity == Rarity.SR)
        {
            _rarityTextSR.SetActive(true); // SR��\��
        }
        else if (rarity == Rarity.SSR)
        {
            _rarityTextSSR.SetActive(true); // SSR��\��
        }
        else if (rarity == Rarity.UR)
        {
            _rarityTextUR.SetActive(true); // UR��\��
        }
    }
}
