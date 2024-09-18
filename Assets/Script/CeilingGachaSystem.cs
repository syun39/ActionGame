using UnityEngine;

public class CeilingGachaSystem : MonoBehaviour
{
    [SerializeField] private GachaSetting _gachaSetting;
    private int _totalGachaCount = 0; // �S�K�`���񐔂��L�^����ϐ�
    private const int GuaranteedURCount = 200; // 200�A��UR���m��r�o

    /// <summary>
    /// �K�`�������s���A���A�x���擾����
    /// </summary>
    public Rarity PerformGacha()
    {
        _totalGachaCount++;

        // 200�A�ڂ�UR���m��r�o
        if (_totalGachaCount % GuaranteedURCount == 0)
        {
            return Rarity.UR;
        }

        // �ʏ�̊m���ŃK�`�����s��
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        float cumulativeRate = 0f;

        foreach (var rate in _gachaSetting.rarityRates)
        {
            cumulativeRate += rate.rate;
            if (randomValue <= cumulativeRate)
            {
                return rate.rarity;
            }
        }

        return Rarity.R; // �����ꉽ���Y�����Ȃ��ꍇ�͍Œ჌�A���e�B��Ԃ�
    }
}
