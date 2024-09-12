using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GachaResultDisplay : MonoBehaviour
{
    // GachaData ScriptableObject �̎Q��
    [SerializeField] GachaData _gachaData;

    // Unity UI �� Image �� Text �R���|�[�l���g�ւ̎Q��
    [SerializeField] Image _gachaImage;
    [SerializeField] Text _rarityText; // ���A�x��\�����邽�߂�Text

    private int _currentImageIndex = 0;

    void Start()
    {
        // �ŏ��̉摜��\���i�e�N�X�`�������݂���ꍇ�j
        if (_gachaData.gachaResults.Length > 0)
        {
            DisplayResult(_currentImageIndex);
        }
    }

    void Update()
    {
        // �G���^�[�L�[�������ꂽ�玟�̉摜��\��
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_currentImageIndex < _gachaData.gachaResults.Length - 1)
            {
                _currentImageIndex++;
                DisplayResult(_currentImageIndex);
            }
            else
            {
                // �S�Ẳ摜��\�����I�������V�[���J��
                SceneManager.LoadScene("Gacha Result Scene"); // �ŏI���ʂ�\������V�[���ɑJ��
            }
        }
    }

    /// <summary>
    /// �摜�ƃ��A�x��\�����郁�\�b�h
    /// </summary>
    /// <param name="index"></param>
    void DisplayResult(int index)
    {
        var result = _gachaData.gachaResults[index];

        // �e�N�X�`����Image�ɓK�p
        Sprite newSprite = Sprite.Create(result.texture, new Rect(0, 0, result.texture.width, result.texture.height), new Vector2(0.5f, 0.5f));
        _gachaImage.sprite = newSprite;

        // ���A�x��Text�ɓK�p
        _rarityText.text = result.rarity.ToString();
    }
}
