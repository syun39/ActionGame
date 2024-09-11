using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GachaResultDisplay : MonoBehaviour
{
    // GachaData ScriptableObject �̎Q��
    [SerializeField] GachaData _gachaData;

    // Unity UI �� Image �R���|�[�l���g�ւ̎Q��
    [SerializeField] Image _gachaImage;

    private int _currentImageIndex = 0;

    void Start()
    {
        // �ŏ��̉摜��\���i�e�N�X�`�������݂���ꍇ�j
        if (_gachaData.gachaTextures.Length > 0)
        {
            ApplyTextureToImage(_gachaData.gachaTextures[_currentImageIndex]);
        }
    }

    void Update()
    {
        // �G���^�[�L�[�������ꂽ�玟�̉摜��\��
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_currentImageIndex < _gachaData.gachaTextures.Length - 1)
            {
                _currentImageIndex++;
                ApplyTextureToImage(_gachaData.gachaTextures[_currentImageIndex]);
            }
            else
            {
                // �S�Ẳ摜��\�����I�������V�[���J��
                SceneManager.LoadScene("Gacha Result Scene"); // �ŏI���ʂ�\������V�[���ɑJ��
            }
        }
    }

    // �e�N�X�`����Image�R���|�[�l���g�ɓK�p���郁�\�b�h
    void ApplyTextureToImage(Texture2D texture)
    {
        // �e�N�X�`���� Sprite �ɕϊ����� UI �� Image �ɓK�p
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        _gachaImage.sprite = newSprite;
    }
}
