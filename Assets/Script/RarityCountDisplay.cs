using UnityEngine;
using UnityEngine.UI;

public class RarityCountDisplay : MonoBehaviour
{
    // GachaData ScriptableObject �̎Q��
    [SerializeField] private GachaData _gachaData;

    // ���A�x���Ƃ̃J�E���g��\������ Text UI
    [SerializeField] private Text _rCountText;

    [SerializeField] private Text _srCountText;

    [SerializeField] private Text _ssrCountText;

    [SerializeField] private Text _urCountText;

    private void Start()
    {
        DisplayRarityCounts();
    }

    /// <summary>
    /// ���A�x���Ƃ̃J�E���g��\�����郁�\�b�h
    /// </summary>
    void DisplayRarityCounts()
    {
        // ���A�x���Ƃ̃J�E���g��ۑ����邽�߂̕ϐ�
        int rCount = 0;
        int srCount = 0;
        int ssrCount = 0;
        int urCount = 0;

        // �K�`�����ʂ����[�v���ă��A�x���ƂɃJ�E���g����
        foreach (var result in _gachaData.gachaResults)
        {
            switch (result.rarity)
            {
                case Rarity.R:
                    rCount++;
                    break;
                case Rarity.SR:
                    srCount++;
                    break;
                case Rarity.SSR:
                    ssrCount++;
                    break;
                case Rarity.UR:
                    urCount++;
                    break;
            }
        }

        // Text �R���|�[�l���g�ɔ��f����
        _rCountText.text = $"{rCount} ��";
        _srCountText.text = $"{srCount} ��";
        _ssrCountText.text = $"{ssrCount} ��";
        _urCountText.text = $"{urCount} ��";
    }
}
