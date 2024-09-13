using System.Collections;
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

    [SerializeField] Text  _loadingText; // ���[�f�B���O�e�L�X�g�̒ǉ�

    // �K�`���̉�
    private int _maxImages = 1;

    private void Start()
    {
        _loadingText.gameObject.SetActive(false);
    }

    // �P���K�`�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void OnSingleGachaClick()
    {
        _maxImages = 1; // �P���K�`����1�񂾂�
        StartCoroutine(GetAPI(_maxImages));
        _loadingText.gameObject.SetActive(true);
    }

    // 10�A�K�`�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void OnTenGachaClick()
    {
        _maxImages = 10; // 10�A�K�`����10��
        StartCoroutine(GetAPI(_maxImages));
        _loadingText.gameObject.SetActive(true);
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

#if UNITY_EDITOR
            Debug.Log($"�r�o���ꂽ���A�x: {selectedRarity}");
#endif

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(_urlAPI);

            // ���N�G�X�g�̑��M�Ƒҋ@
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // ...�������̏���...
                if (i == count - 1) // �Ō�̉摜�̃��[�h�����������ꍇ
                {
                    _loadingText.gameObject.SetActive(false); // ���[�f�B���O�e�L�X�g���\����
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

                _loadingText.text = "���[�h���s"; // �G���[���b�Z�[�W�ɍX�V
            }
        }

        _loadingText.gameObject.SetActive(false);

        // �K�`�����ʂ̎擾������
        Debug.Log("�K�`�����ʂ̎擾���������܂���");

        // �K�`�����I�������V�[���J��
        SceneManager.LoadScene("Normal direction Scene"); // �摜�\���V�[���ɑJ��
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
}

