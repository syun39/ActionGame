using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DogTextureLoader : MonoBehaviour
{
    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �N���X
    [Serializable]
    public class ResponseData
    {
        public string message;
    }

    [SerializeField] GachaData _gachaData;
    private int _maxImages = 1;

    public void OnSingleGachaClick()
    {
        _maxImages = 1; // �P���K�`��
        StartCoroutine(GetAPI(_maxImages));
    }

    public void OnTenGachaClick()
    {
        _maxImages = 10; // 10�A�K�`��
        StartCoroutine(GetAPI(_maxImages));
    }

    IEnumerator GetAPI(int count)
    {
        _gachaData.gachaTextures = new Texture2D[count];

        for (int i = 0; i < count; i++)
        {
            UnityWebRequest request = UnityWebRequest.Get("https://dog.ceo/api/breeds/image/random");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                ResponseData response = JsonUtility.FromJson<ResponseData>(jsonResponse);
                yield return StartCoroutine(GetTexture(response.message, i));
            }
            else
            {
                Debug.LogError($"�摜�擾���s: {request.error}");
            }
        }

        // �K�`�����I�������V�[���J��
        SceneManager.LoadScene("Gacha Main Scene"); // �摜�\���V�[���ɑJ��
    }

    IEnumerator GetTexture(string url, int index)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            _gachaData.gachaTextures[index] = texture;

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
}
