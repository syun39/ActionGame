using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GachaResultDisplay : MonoBehaviour
{

    // GachaData
    [SerializeField] GachaData _gachaData;
    [SerializeField] Image _gachaImage; // �摜�\��
    [SerializeField] private RarityText _rarityTextComponent; // ���A�x�̕\��

    [SerializeField] Text _currentImageIndexText; // ���݂̉摜�������ڂ���\������e�L�X�g

    // ���݂̉摜�������ڂ�
    private int _currentImageIndex = 0;

    void Start()
    {
        // �ŏ��̉摜��\��
        if (_gachaData.gachaResults.Length > 0)
        {
            DisplayResult(_currentImageIndex); // �����摜�\��
        }
    }

    void Update()
    {
        // �G���^�[�L�[�������ꂽ��
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // ���݂̉摜�C���f�b�N�X���Ō�̉摜�̃C���f�b�N�X��菬�����ꍇ
            if (_currentImageIndex < _gachaData.gachaResults.Length - 1)
            {
                _currentImageIndex++;
                DisplayResult(_currentImageIndex); // ���̉摜��\��
            }
            else
            {
                // ���ׂẲ摜��\��������V�[���J��
                SceneManager.LoadScene("Result Scene");
            }
        }
    }

    /// <summary>
    /// �摜�ƃ��A�x��\������
    /// </summary>
    /// <param name="index">�\������摜�̃C���f�b�N�X</param>
    void DisplayResult(int index)
    {
        // �C���f�b�N�X�̌��ʂ��擾
        var result = _gachaData.gachaResults[index];

        // �e�N�X�`����Image�ɓK�p
        Sprite newSprite = Sprite.Create(result.texture, new Rect(0, 0, result.texture.width, result.texture.height), new Vector2(0.5f, 0.5f));
        _gachaImage.sprite = newSprite;

        // ���A�x��RarityText �R���|�[�l���g�ɓK�p
        _rarityTextComponent.SetRarity(result.rarity);

        // ���݂̉摜�������ڂ���\��
        _currentImageIndexText.text = $"{_currentImageIndex + 1}";
    }
}
