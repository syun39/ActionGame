using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaData", menuName = "Gacha/GachaData")]
public class GachaData : ScriptableObject
{
    // �摜�ƃ��A�x���Z�b�g�ŕۑ�
    [Serializable] public class GachaResult
    {
        public Texture2D texture; // �摜
        public Rarity rarity;     // ���A�x
    }

    // �K�`�����ʂ̔z��
    public GachaResult[] gachaResults;

    // �K�`������
    public int totalGachaCount = 0;

    /// <summary>
    /// �K�`�����񐔕ۑ�
    /// </summary>
    public void SaveData()
    {
        PlayerPrefs.SetInt("TotalGachaCount", totalGachaCount);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// �K�`�����񐔓ǂݍ���
    /// </summary>
    public void LoadData()
    {
        totalGachaCount = PlayerPrefs.GetInt("TotalGachaCount", 0);
    }
}

