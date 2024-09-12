using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SpecialTextureLoader : MonoBehaviour
{
    // JSON �����M����f�[�^
    [Serializable]
    public class ResponseData
    {
        public string url;
    }

    [SerializeField] private GachaData _gachaData;
    [SerializeField] private GachaSetting _gachaSetting; // ���A�x�ݒ�
    private int _maxImages = 1;

    // �P���K�`�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void OnSingleGachaClick()
    {
        _maxImages = 1; // �P���K�`��
        StartCoroutine(GetAPI(_maxImages));
    }

    // 10�A�K�`�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void OnTenGachaClick()
    {
        _maxImages = 10; // 10�A�K�`��
        StartCoroutine(GetAPI(_maxImages));
    }

    // API���g���ĉ摜���擾���A���A�x�ƈꏏ��GachaData�ɕۑ�
    IEnumerator GetAPI(int count)
    {
        // �K�`�����ʂ̔z���������
        _gachaData.gachaResults = new GachaData.GachaResult[count];

        for (int i = 0; i < count; i++)
        {
            // ���A�x�������_���Ɍ���
            Rarity selectedRarity = GetRandomRarity();
            Debug.Log($"�r�o���ꂽ���A�x: {selectedRarity}");

            UnityWebRequest request = UnityWebRequest.Get("https://nekos.best/api/v2/waifu");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                ResponseData response = JsonUtility.FromJson<ResponseData>(jsonResponse);
                yield return StartCoroutine(GetTexture(response.url, i, selectedRarity));
            }
            else
            {
                Debug.LogError($"�摜�擾���s: {request.error}");
            }
        }

        // �K�`�����ʂ̎擾������
        Debug.Log("�K�`�����ʂ̎擾���������܂���");

        // �K�`�����I�������V�[���J��
        SceneManager.LoadScene("Gacha Main Scene"); // �摜�\���V�[���ɑJ��
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
