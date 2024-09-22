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
    // ���A�x���Ƃ̔r�o�����Ǘ�
    [Serializable]
    public class RarityRate
    {
        public Rarity rarity; // ���A�x
        public float rate; // �r�o��
    }

    // �r�o����ݒ�
    [SerializeField]
    RarityRate[] rarityRates =
    {
        new RarityRate { rarity = Rarity.R, rate = 70f }, // R�̔r�o��
        new RarityRate { rarity = Rarity.SR, rate = 26f }, // SR�̔r�o��
        new RarityRate { rarity = Rarity.SSR, rate = 3f }, // SSR�̔r�o��
        new RarityRate { rarity = Rarity.UR, rate = 1f } // UR�̔r�o��
    };

    // �r�o�����擾����v���p�e�B
    public RarityRate[] RarityRates => rarityRates;
}