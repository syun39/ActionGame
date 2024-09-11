using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class CatTextureLoader : MonoBehaviour
{
    // �L�̉摜���擾���邽�߂�API URL
    private string _urlAPI = "https://cataas.com/cat";

    // �e�N�X�`����K�p����Ώۂ̃I�u�W�F�N�g
    private GameObject _object;

    void Start()
    {
        // �摜��\��t����I�u�W�F�N�g���V�[������擾
        _object = GameObject.Find("Tile");
    }

    // �N���b�N���ꂽ��
    public void OnPointerClick(PointerEventData eventData)
    {
        // �L�摜���擾
        StartCoroutine(GetTexture(_urlAPI));
    }

    // �w�肵��URL����e�N�X�`�����擾
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // ���N�G�X�g�̑��M�Ƒҋ@
        yield return request.SendWebRequest();

        switch (request.result)
        {
            // ���N�G�X�g���i�s���̏ꍇ
            case UnityWebRequest.Result.InProgress:
                Debug.Log("���N�G�X�g��");
                break;

            // ���N�G�X�g�����������ꍇ
            case UnityWebRequest.Result.Success:
                Debug.Log("���N�G�X�g����");

                // �e�N�X�`�����擾
                Texture loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                // �擾�����e�N�X�`�����I�u�W�F�N�g�ɓK�p
                _object.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", loadedTexture);
                break;

            // �C���^�[�l�b�g�ڑ��̖��ɂ��G���[
            case UnityWebRequest.Result.ConnectionError: 
                Debug.LogError($"���N�G�X�g���s: {request.error}");
                break;
        }
    }
}
