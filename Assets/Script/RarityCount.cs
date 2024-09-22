using UnityEngine;
using UnityEngine.UI;

public class RarityCount : MonoBehaviour
{
    // GachaData
    [SerializeField] GachaData _gachaData;
    [SerializeField] Text _rCountText; // R�̃J�E���g��\������e�L�X
    [SerializeField] Text _srCountText; // SR�̃J�E���g��\������e�L�X�g
    [SerializeField] Text _ssrCountText; // SSR�̃J�E���g��\������e�L�X�g
    [SerializeField] Text _urCountText; // UR�̃J�E���g��\������e�L�X�g

    private void Start()
    {
        RarityCounts();
    }

    /// <summary>
    /// ���A�x���Ƃ̃J�E���g��\�����郁�\�b�h
    /// </summary>
    void RarityCounts()
    {
        // ���A�x���Ƃ̃J�E���g��ۑ�
        int rCount = 0;
        int srCount = 0;
        int ssrCount = 0;
        int urCount = 0;

        // �K�`�����ʂ����[�v���ă��A�x���ƂɃJ�E���g����
        foreach (var result in _gachaData.GachaResults)
        {
            if (result.rarity == Rarity.R) // ���A�x��R��������
            {
                rCount++; // �J�E���g�𑝂₷
            }
            else if (result.rarity == Rarity.SR) // ���A�x��SR��������
            {
                srCount++;
            }
            else if (result.rarity == Rarity.SSR) // ���A�x��SSR��������
            {
                ssrCount++;
            }
            else if (result.rarity == Rarity.UR) // ���A�x��UR��������
            {
                urCount++;
            }
        }

        // Text�ŕ\��
        _rCountText.text = $"{rCount} ��";
        _srCountText.text = $"{srCount} ��";
        _ssrCountText.text = $"{ssrCount} ��";
        _urCountText.text = $"{urCount} ��";
    }
}
