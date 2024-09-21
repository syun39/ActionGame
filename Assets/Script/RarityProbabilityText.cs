using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �e�L�X�g�̏�Ԃ��Ǘ�
/// </summary>
enum TextState
{
    /// <summary>
    /// �����\�����Ȃ�
    /// </summary>
    None,

    /// <summary>
    /// �K�`���m���̃e�L�X�g��\��
    /// </summary>
    RarityRatesText,

    /// <summary>
    /// �m��g�̊m����\��
    /// </summary>
    DecisionRarityRatesText
};

public class RarityProbabilityText : MonoBehaviour
{
    [SerializeField] Text _rarityRatesText; // �K�`���m���̃e�L�X�g
    [SerializeField] Text _decisionRarityRatesText; // �m��g�̊m���̃e�L�X�g

    private TextState _currentState = TextState.None; // ���݂̃e�L�X�g�̏��

    private void Start()
    {
        // �S�Ẵe�L�X�g���\��
        _rarityRatesText.gameObject.SetActive(false);
        _decisionRarityRatesText.gameObject.SetActive(false);
    }

    /// <summary>
    /// �{�^�����N���b�N���ꂽ��
    /// </summary>
    public void OnButtonClick()
    {
        // �����\������Ă��Ȃ��Ȃ�
        if (_currentState == TextState.None)
        {
            _rarityRatesText.gameObject.SetActive(true); // �K�`���m���̃e�L�X�g��\��
            _currentState = TextState.RarityRatesText; // �e�L�X�g��Ԃ��X�V
        }
        else if (_currentState == TextState.RarityRatesText) // �K�`���m���̃e�L�X�g���\������Ă���Ȃ�
        {
            _rarityRatesText.gameObject.SetActive(false); // �K�`���m���̃e�L�X�g���\��
            _decisionRarityRatesText.gameObject.SetActive(true); // �m��g�̊m���e�L�X�g��\��
            _currentState = TextState.DecisionRarityRatesText; // �e�L�X�g��Ԃ��X�V
        }
        else if (_currentState == TextState.DecisionRarityRatesText) // �m��g�̊m�����\������Ă���Ȃ�
        {
            _rarityRatesText.gameObject.SetActive(false);
            _decisionRarityRatesText.gameObject.SetActive(false); // �m��g�̊m���̃e�L�X�g���\��
            _currentState = TextState.None; // �e�L�X�g��Ԃ��X�V
        }
    }
}
