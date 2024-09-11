using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CatTextureLoader : MonoBehaviour
{
    // �L�̉摜���擾���邽�߂�API URL
    private string _urlAPI = "https://cataas.com/cat";

    // GachaData ScriptableObject �̎Q��
    [SerializeField] GachaData _gachaData;

    // �K�`���̉�
    private int _maxImages = 1;

    // �P���K�`�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void OnSingleGachaClick()
    {
        _maxImages = 1; // �P���K�`����1�񂾂�
        StartCoroutine(GetAPI(_maxImages));
    }

    // 10�A�K�`�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void OnTenGachaClick()
    {
        _maxImages = 10; // 10�A�K�`����10��
        StartCoroutine(GetAPI(_maxImages));
    }

    // �w�肳�ꂽ�񐔁icount�j�̔L�摜���擾���AGachaData �ɕۑ�����
    IEnumerator GetAPI(int count)
    {
        // GachaData ���̃e�N�X�`���z���������
        _gachaData.gachaTextures = new Texture2D[count];

        for (int i = 0; i < count; i++)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(_urlAPI);

            // ���N�G�X�g�̑��M�Ƒҋ@
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                // Editor ���ł̂݉摜�̃T�C�Y�����O�o��
#if UNITY_EDITOR
                Debug.Log($"Image Width: {texture.width}");
                Debug.Log($"Image Height: {texture.height}");
#endif

                // �擾�����e�N�X�`���� GachaData �ɕۑ�
                _gachaData.gachaTextures[i] = DownloadHandlerTexture.GetContent(request);
            }
            else
            {
                Debug.LogError($"���N�G�X�g���s: {request.error}");
            }
        }

        // �K�`�����ʂ̎擾������
        Debug.Log("�K�`�����ʂ̎擾���������܂���");

        // �K�`�����I�������V�[���J��
        SceneManager.LoadScene("Gacha Main Scene"); // �摜�\���V�[���ɑJ��
    }
}
