using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultDisplay : MonoBehaviour
{
    // Singleton �C���X�^���X
    public static GachaResultDisplay Instance { get; private set; }

    // GachaData ScriptableObject �̎Q��
    [SerializeField] GachaData _gachaData;

    // Unity UI �� Image �� Text �R���|�[�l���g�ւ̎Q��
    [SerializeField] Image _gachaImage;
    [SerializeField] private RarityText _rarityTextComponent; // RarityText �X�N���v�g�̎Q��

    private int _currentImageIndex = 0;

    void Start()
    {
        // �ŏ��̉摜��\���i�e�N�X�`�������݂���ꍇ�j
        if (_gachaData.gachaResults.Length > 0)
        {
            DisplayResult(_currentImageIndex);
        }
    }

    private void Awake()
    {
        // Singleton �p�^�[���̓K�p
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // ���ɑ��݂���ꍇ�͎������g��j��
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
                EditorApplication.isPlaying = false;
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

        // ���A�x��RarityText �R���|�[�l���g�ɓK�p
        _rarityTextComponent.SetRarity(result.rarity);
    }

    // GachaResultDisplay �����Z�b�g���郁�\�b�h
    public void Reset()
    {
        _currentImageIndex = 0;
        DisplayResult(_currentImageIndex);
    }
}
