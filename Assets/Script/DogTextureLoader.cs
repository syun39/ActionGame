using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DogTextureLoader : MonoBehaviour
{
    // JSON �����M����f�[�^
    [Serializable]
    public class ResponseData
    {
        public string message;
    }

    [SerializeField] GachaData _gachaData; // GachaData

    [SerializeField] GachaSetting _gachaSetting; // ���A�x�ݒ�

    [Tooltip("�K�`���̓V��")]
    [SerializeField] int _ceilingCount = 200;

    [SerializeField] Text _loadingText; // ���[�f�B���O�e�L�X�g

    [SerializeField] Image _singleButton; // �P���K�`���{�^��

    [SerializeField] Image _tenButton; // 10�A�K�`���{�^��

    [SerializeField] Image _changeButton; // �K�`���؂�ւ��{�^��

    [SerializeField] Image _probabilityButton; // �m���\���{�^��

    [SerializeField] Text _remainingText; // �V��܂Ŏc�艽��

    [SerializeField] Text _text; // UR�m��܂łƕ\��

    // ���A�x���Ƃ̃V�[����
    [SerializeField] string _normalScene = ""; // �����V�[��

    [SerializeField] string _oneSSRScene = ""; // SSR�m��V�[��

    [SerializeField] string _moreSSRScene = ""; // SSR2���ȏ�V�[��

    [SerializeField] string _oneURScene = ""; // UR�m��V�[��

    private bool _isGachaInProgress = false; // �K�`�����i�s����

    private void Start()
    {
        // �Q�[���J�n���Ƀf�[�^��ǂݍ���
        _gachaData.LoadData();

        // ���������Ɏc��񐔂̕\�����X�V
        UpdateDogRemainingCount();
    }

    /// <summary>
    /// �P���K�`��
    /// </summary>
    public void OnSingleGachaClick()
    {
        // �K�`�����i�s���̏ꍇ
        if (_isGachaInProgress) return;

        _remainingText.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
        StartCoroutine(GetAPI(1));
        _isGachaInProgress = true; // �K�`�����i�s��

        // �{�^���𖳌������鏈��
        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    /// <summary>
    /// 10�A�K�`��
    /// </summary>
    public void OnTenGachaClick()
    {
        // �K�`�����i�s���̏ꍇ
        if (_isGachaInProgress) return;

        _remainingText.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
        StartCoroutine(GetAPI(10));
        _isGachaInProgress = true; // �K�`�����i�s��

        // �{�^���𖳌������鏈��
        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    // API���g���ĉ摜���擾���A���A�x�ƈꏏ��GachaData�ɕۑ�
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
            if (_gachaData.totalGachaCount % _ceilingCount == _ceilingCount - 1) // _ceilingCount�̔{����1����Ȃ���ԂȂ�
            {
                // _ceilingCount �񂲂ƂɕK��UR���o��
                selectedRarity = Rarity.UR;
            }
            else
            {
                selectedRarity = GetRandomRarity();
            }

            // API���N�G�X�g�𑗐M
            UnityWebRequest request = UnityWebRequest.Get("https://dog.ceo/api/breeds/image/random");
            yield return request.SendWebRequest();

            // ����������
            if (request.result == UnityWebRequest.Result.Success)
            {
                // ���X�|���X�̏���
                string jsonResponse = request.downloadHandler.text;
                ResponseData response = JsonUtility.FromJson<ResponseData>(jsonResponse);
                yield return StartCoroutine(GetTexture(response.message, i, selectedRarity));

                // �K�`���񐔂��C���N�������g
                _gachaData.totalGachaCount++;

                _gachaData.SaveData(); // �f�[�^��ۑ�

                if (i == count - 1) // �Ō�̉摜�̃��[�h�����������ꍇ
                {
                    _loadingText.gameObject.SetActive(false); // ���[�f�B���O�e�L�X�g���\����
                    _isGachaInProgress = false; // �K�`���̐i�s��Ԃ����Z�b�g
                    _remainingText.gameObject.SetActive(true);
                    _text.gameObject.SetActive(true);

                    // �{�^�����ēx�L���ɂ��鏈��
                    _singleButton.raycastTarget = true;
                    _tenButton.raycastTarget = true;
                    _changeButton.raycastTarget = true;
                    _probabilityButton.raycastTarget = true;
                }
            }
            else
            {
                Debug.LogError($"�摜�擾���s: {request.error}");

                Animator animator = _loadingText.GetComponent<Animator>(); // �e�L�X�g�̃A�j���[�^�[���擾

                if (animator != null)
                {
                    animator.enabled = false;  // �A�j���[�V�������ꎞ�I�ɖ�����
                }
                _loadingText.color = Color.red;
                _loadingText.text = "���[�h���s"; // �G���[���b�Z�[�W�ɍX�V
                
                yield return new WaitForSeconds(1.5f);

                // �^�C�g���ɖ߂�
                SceneManager.LoadScene("Title");
            }
        }

        // �K�`�����ʂ̎擾������
        Debug.Log("�K�`�����ʂ̎擾���������܂���");

        // ���A�x�̃J�E���g���擾
        int ssrCount = _gachaData.gachaResults.Count(result => result.rarity == Rarity.SSR);
        int urCount = _gachaData.gachaResults.Count(result => result.rarity == Rarity.UR);

        // �J�ڐ�̃V�[��������
        string sceneToLoad = RarityChangeScene(ssrCount, urCount);
        SceneManager.LoadScene(sceneToLoad);
    }

    // �e�N�X�`�����擾���� GachaData �ɕۑ�
    IEnumerator GetTexture(string url, int index, Rarity rarity)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);

            // GachaData �ɉ摜�ƃ��A�x��ۑ�
            _gachaData.gachaResults[index] = new GachaData.GachaResult
            {
                texture = texture,
                rarity = rarity
            };

#if UNITY_EDITOR
            Debug.Log($"Image Width: {texture.width}");
            Debug.Log($"Image Height: {texture.height}");
#endif

        }
        else
        {
            Debug.LogError($"�e�N�X�`���擾���s: {request.error}");

            Animator animator = _loadingText.GetComponent<Animator>(); // �e�L�X�g�̃A�j���[�^�[���擾

            if (animator != null)
            {
                animator.enabled = false;  // �A�j���[�V�������ꎞ�I�ɖ�����
            }
            _loadingText.color = Color.red;
            _loadingText.text = "�e�N�X�`���擾���s"; // �G���[���b�Z�[�W�ɍX�V
            
            yield return new WaitForSeconds(1.5f);

            // �^�C�g���ɖ߂�
            SceneManager.LoadScene("Title");
        }

    }

    /// <summary>
    /// ���A�x�ɉ������V�[���������肷��
    /// </summary>
    /// <param name="ssrCount">SSR�̖���</param>
    /// <param name="urCount">UR�̖���</param>
    /// <returns>�V�[����</returns>
    string RarityChangeScene(int ssrCount, int urCount)
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
    /// ���A�x�������_���Ɍ���(�ݐϊm��)
    /// </summary>
    Rarity GetRandomRarity()
    {
        float total = 0f;
        foreach (var rate in _gachaSetting.rarityRates)
        {
            total += rate.rate; // �e���A�x�̊m�������v
        }

        float randomValue = UnityEngine.Random.Range(0, total); // �����_���Ȓl�𐶐�
        float cumulative = 0f;

        foreach (var rate in _gachaSetting.rarityRates)
        {
            cumulative += rate.rate; // �m���̗ݐϒl���v�Z
            if (randomValue <= cumulative)
            {
                return rate.rarity; // �����_���Ȓl�ɑΉ����郌�A�x��Ԃ�
            }
        }

        return Rarity.R; // �f�t�H���g�� R
    }

    /// <summary>
    /// 
    /// </summary>
    void UpdateDogRemainingCount()
    {
        // ���݂̃K�`���񐔂� _ceilingCount �̔{������ǂꂾ������Ă��邩
        int remainingToUR = _ceilingCount - (_gachaData.totalGachaCount % _ceilingCount);
        _remainingText.text = $"�c�� {remainingToUR}��";
    }
}
