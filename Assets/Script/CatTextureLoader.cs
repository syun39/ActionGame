using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CatTextureLoader : MonoBehaviour
{
    // �L�̉摜���擾���邽�߂�API URL
    private string _urlAPI = "https://cataas.com/cat";

    // GachaData ScriptableObject �̎Q��
    [SerializeField] GachaData _gachaData;

    // GachaSetting �̎Q�� (�r�o���ݒ�)
    [SerializeField] private GachaSetting _gachaSetting;

    [Tooltip("�K�`���̓V��")]
    [SerializeField] int _ceilingCount = 200;

    [SerializeField] Text _loadingText; // ���[�f�B���O�e�L�X�g�̒ǉ�

    [SerializeField] private Image _singleButton;

    [SerializeField] private Image _tenButton;

    [SerializeField] private Image _changeButton;

    [SerializeField] private Image _probabilityButton;

    [SerializeField] private Text _remainingText; // �V��܂Ŏc�艽��

    [SerializeField] private Text _text; // UR�m��܂łƕ\��

    // ���A�x���Ƃ̃V�[����
    [SerializeField] private string _normalScene = "";

    [SerializeField] private string _oneSSRScene = "";

    [SerializeField] private string _moreSSRScene = "";

    [SerializeField] private string _oneURScene = "";

    private bool _isGachaInProgress = false;

    private void Start()
    {
        // �Q�[���J�n���Ƀf�[�^��ǂݍ���
        _gachaData.LoadData();

        // ���������Ɏc��񐔂̕\�����X�V
        UpdateCatRemainingCount();
    }

    // �P���K�`�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void OnSingleGachaClick()
    {
        if (_isGachaInProgress) return;

        _remainingText.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
        _isGachaInProgress = true;
        StartCoroutine(GetAPI(1));

        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    // 10�A�K�`�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void OnTenGachaClick()
    {
        if (_isGachaInProgress) return;

        _remainingText.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
        _isGachaInProgress = true;
        StartCoroutine(GetAPI(10));

        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    // �w�肳�ꂽ�񐔁icount�j�̔L�摜���擾���AGachaData �ɕۑ�����
    IEnumerator GetAPI(int count)
    {
        _loadingText.gameObject.SetActive(true); // ���[�f�B���O�e�L�X�g��\��

        // GachaData ���̌��ʔz���������
        _gachaData.gachaResults = new GachaData.GachaResult[count];

        for (int i = 0; i < count; i++)
        {
            // ���A�x�������_���Ɍ���
            Rarity selectedRarity = GetRandomRarity();

            // �V��V�X�e���̎���
            if (_gachaData.totalGachaCount % _ceilingCount == _ceilingCount - 1)
            {
                // 200�񂲂ƂɕK��UR���o��
                selectedRarity = Rarity.UR;
            }
            else
            {
                selectedRarity = GetRandomRarity();
            }

            

#if UNITY_EDITOR
            //Debug.Log($"�r�o���ꂽ���A�x: {selectedRarity}");
#endif

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(_urlAPI);

            // ���N�G�X�g�̑��M�Ƒҋ@
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // �K�`���񐔂��C���N�������g
                _gachaData.totalGachaCount++;

                _gachaData.SaveData(); // �f�[�^��ۑ�

                // ...�������̏���...
                if (i == count - 1) // �Ō�̉摜�̃��[�h�����������ꍇ
                {
                    _loadingText.gameObject.SetActive(false); // ���[�f�B���O�e�L�X�g���\����
                    _isGachaInProgress = false; // �K�`���̐i�s��Ԃ����Z�b�g
                    _remainingText.gameObject.SetActive(true);
                    _text.gameObject.SetActive(true);

                    _singleButton.raycastTarget = true;
                    _tenButton.raycastTarget = true;
                    _changeButton.raycastTarget = true;
                    _probabilityButton.raycastTarget = true;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                // Editor ���ł̂݉摜�̃T�C�Y�����O�o��
#if UNITY_EDITOR
                Debug.Log($"Image Width: {texture.width}");
                Debug.Log($"Image Height: {texture.height}");
#endif

                // �擾�����e�N�X�`���ƃ��A�x�� GachaData �ɕۑ�
                _gachaData.gachaResults[i] = new GachaData.GachaResult
                {
                    texture = texture,
                    rarity = selectedRarity
                };
            }
            else
            {
                Debug.LogError($"���N�G�X�g���s: {request.error}");

                Animator animator = _loadingText.GetComponent<Animator>(); // �e�L�X�g�̃A�j���[�^�[���擾
                if (animator != null)
                {
                    animator.enabled = false;  // �A�j���[�V�������ꎞ�I�ɖ�����
                }
                _loadingText.color = Color.red;  // �e�L�X�g�̐F��ԂɕύX
                _loadingText.text = "���[�h���s";  // �G���[���b�Z�[�W��ݒ�
                yield return new WaitForSeconds(1.5f);

                // �^�C�g���ɖ߂�
                SceneManager.LoadScene("Title");
            }
        }

        _loadingText.gameObject.SetActive(false);

        // �K�`�����ʂ̎擾������
        Debug.Log("�K�`�����ʂ̎擾���������܂���");

        // ���A�x�̃J�E���g���擾
        int ssrCount = _gachaData.gachaResults.Count(result => result.rarity == Rarity.SSR);
        int urCount = _gachaData.gachaResults.Count(result => result.rarity == Rarity.UR);

        // �J�ڐ�̃V�[��������
        string sceneToLoad = RarityChangeScene(ssrCount, urCount);
        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// ���A�x�ɉ������V�[���������肷��
    /// </summary>
    /// <param name="ssrCount">SSR�̖���</param>
    /// <param name="urCount">UR�̖���</param>
    /// <returns>�V�[����</returns>
    private string RarityChangeScene(int ssrCount, int urCount)
    {
        if (urCount > 0)
        {
            return _oneURScene; // UR���ꖇ�ȏ�̏ꍇ
        }
        else if (ssrCount >= 2)
        {
            return _moreSSRScene; // SSR���񖇈ȏ�̏ꍇ
        }
        else if (ssrCount == 1)
        {
            return _oneSSRScene; // SSR���ꖇ�̏ꍇ
        }
        else
        {
            return _normalScene; // SSR��UR���Ȃ��ꍇ
        }
    }

    /// <summary>
    /// ���A�x�������_���Ɍ��肷��
    /// </summary>
    /// <returns></returns>
    private Rarity GetRandomRarity()
    {
        float total = 0f;
        foreach (var rate in _gachaSetting.rarityRates)
        {
            total += rate.rate;
        }

        float randomValue = Random.Range(0, total);
        float cumulative = 0f;

        foreach (var rate in _gachaSetting.rarityRates)
        {
            cumulative += rate.rate;
            if (randomValue <= cumulative)
            {
                return rate.rarity;
            }
        }

        return Rarity.R; // �f�t�H���g�� R
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateCatRemainingCount()
    {
        int remainingToUR = _ceilingCount - (_gachaData.totalGachaCount % _ceilingCount);
        _remainingText.text = $"�c�� {remainingToUR}��";
    }
}

