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

    [SerializeField] private GachaData _gachaData;
    [SerializeField] private GachaSetting _gachaSetting; // ���A�x�ݒ�
    [SerializeField] Text _loadingText; // ���[�f�B���O�e�L�X�g�̒ǉ�

    [SerializeField] private Image _singleButton;

    [SerializeField] private Image _tenButton;

    [SerializeField] private Image _changeButton;

    [SerializeField] private Image _probabilityButton;

    // ���A�x���Ƃ̃V�[����
    [SerializeField] private string _normalScene = "";

    [SerializeField] private string _oneSSRScene = "";

    [SerializeField] private string _moreSSRScene = "";

    [SerializeField] private string _oneURScene = "";

    private int _maxImages = 1;

    private bool _isGachaInProgress = false;

    // �P���K�`�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void OnSingleGachaClick()
    {
        if (_isGachaInProgress) return;

        _maxImages = 1; // �P���K�`��
        StartCoroutine(GetAPI(_maxImages));
        _loadingText.gameObject.SetActive(true); // ���[�f�B���O�e�L�X�g��\��
        _isGachaInProgress = true;

        // �{�^���𖳌������鏈��
        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    // 10�A�K�`�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void OnTenGachaClick()
    {
        if (_isGachaInProgress) return;

        _maxImages = 10; // 10�A�K�`��
        StartCoroutine(GetAPI(_maxImages));
        _loadingText.gameObject.SetActive(true); // ���[�f�B���O�e�L�X�g��\��
        _isGachaInProgress = true;

        // �{�^���𖳌������鏈���i�K�v�ɉ����āj
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

            UnityWebRequest request = UnityWebRequest.Get("https://dog.ceo/api/breeds/image/random");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                ResponseData response = JsonUtility.FromJson<ResponseData>(jsonResponse);
                yield return StartCoroutine(GetTexture(response.message, i, selectedRarity));

                if (i == count - 1) // �Ō�̉摜�̃��[�h�����������ꍇ
                {
                    _loadingText.gameObject.SetActive(false); // ���[�f�B���O�e�L�X�g���\����
                    _isGachaInProgress = false; // �K�`���̐i�s��Ԃ����Z�b�g

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

                _loadingText.text = "���[�h���s"; // �G���[���b�Z�[�W�ɍX�V
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
        }
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

    // ���A�x�������_���Ɍ���
    private Rarity GetRandomRarity()
    {
        float total = 0f;
        foreach (var rate in _gachaSetting.rarityRates)
        {
            total += rate.rate;
        }

        float randomValue = UnityEngine.Random.Range(0, total);
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
}
