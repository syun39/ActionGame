using System;
using UnityEngine;

/// <summary>
/// ���A�x���`
/// </summary>
public enum Rarity
{
    R,   // ���A
    SR,  // �X�[�p�[���A
    SSR, // �X�[�p�[�X�y�V�������A
    UR   // �E���g�����A
}

[CreateAssetMenu(fileName = "GachaSetting", menuName = "Gacha/GachaSetting")]
public class GachaSetting : ScriptableObject
{
    // ���A�x���Ƃ̔r�o�����Ǘ�����N���X
    [Serializable]
    public class RarityRate
    {
        public Rarity rarity;
        public float rate; // �r�o��
    }

    // RarityRate �̔z����g���Ĕr�o����ݒ�
    [SerializeField]
    public RarityRate[] rarityRates = new RarityRate[]
    {
        new RarityRate { rarity = Rarity.R, rate = 70f },
        new RarityRate { rarity = Rarity.SR, rate = 26.5f },
        new RarityRate { rarity = Rarity.SSR, rate = 3f },
        new RarityRate { rarity = Rarity.UR, rate = 0.5f }
    };
}