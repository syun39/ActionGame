using UnityEngine;
using UnityEngine.UI;

public class RarityText : MonoBehaviour
{
    [SerializeField] private GameObject _rarityTextR;
    [SerializeField] private GameObject _rarityTextSR;
    [SerializeField] private GameObject _rarityTextSSR;
    [SerializeField] private GameObject _rarityTextUR;

    public void SetRarity(Rarity rarity)
    {
        // �S�Ẵ��A���e�B�e�L�X�g���\���ɂ���
        _rarityTextR.SetActive(false);
        _rarityTextSR.SetActive(false);
        _rarityTextSSR.SetActive(false);
        _rarityTextUR.SetActive(false);

        // �I�����ꂽ���A���e�B�̃e�L�X�g��\������
        switch (rarity)
        {
            case Rarity.R:
                _rarityTextR.SetActive(true);
                break;
            case Rarity.SR:
                _rarityTextSR.SetActive(true);
                break;
            case Rarity.SSR:
                _rarityTextSSR.SetActive(true);
                break;
            case Rarity.UR:
                _rarityTextUR.SetActive(true);
                break;
        }
    }
}
