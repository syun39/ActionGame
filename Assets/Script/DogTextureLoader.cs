using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class DogTextureLoader : MonoBehaviour
{
    // ���̉摜���擾����API URL
    string _urlAPI = "https://dog.ceo/api/breeds/image/random/"; // ��

    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �x�[�X�N���X
    [Serializable]
    public class ResponseData
    {
        // �摜URL���i�[
        public string message;
    }

    // ���ݕ\�����Ă���摜�̃C���f�b�N�X
    private int _currentImageIndex = 0;

    // �擾�����摜URL���i�[����z��
    private string[] _imageUrls;

    // �摜�\�������ǂ����𔻒肷��t���O
    private bool _isDisplayingImages = false;

    // �P���K�`���{�^�����N���b�N���ꂽ��
    public void OnSingleGachaClick(PointerEventData eventData)
    {
        // �P���K�`��
        StartCoroutine(GetAPI(1));
    }

    // 10�A�K�`���{�^�����N���b�N���ꂽ��
    public void OnTenGachaClick(PointerEventData eventData)
    {
        // 10�A�K�`��
        StartCoroutine(GetAPI(10));
    }


    // �N���b�N���ꂽ��
    public void OnPointerClick(PointerEventData eventData)
    {
        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        StartCoroutine("GetAPI");
    }

    // API �擾
    IEnumerator GetAPI(int count)
    {
        // �摜URL���i�[����z������N�G�X�g�񐔕�������
        _imageUrls = new string[count];

        // �\������摜�̃C���f�b�N�X�����Z�b�g
        _currentImageIndex = 0;

        // �摜�\�����t���O���I���ɂ���
        _isDisplayingImages = true;

        // ���N�G�X�g�𕡐��񑗐M���邽�߂̃��[�v (�P���Ȃ�1��A10�A�Ȃ�10��)
        for (int i = 0; i < count; i++)
        {
            // API��GET���N�G�X�g�𑗐M
            UnityWebRequest request = UnityWebRequest.Get(_urlAPI);

            // ���N�G�X�g�̊�����҂�
            yield return request.SendWebRequest();

            // ���N�G�X�g�̌��ʂɉ����ď����𕪊�
            switch (request.result)
            {
                // ���N�G�X�g���i�s���̏ꍇ
                case UnityWebRequest.Result.InProgress:
                    Debug.Log("���N�G�X�g��");
                    break;

                // ���N�G�X�g�����������ꍇ
                case UnityWebRequest.Result.Success:
                    Debug.Log("���N�G�X�g����");
                    ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                    // �擾�����摜URL��z��ɕۑ�
                    _imageUrls[i] = response.message;

                    // �ŏ��̉摜�͂����ɕ\������
                    if (i == 0)
                    {
                        StartCoroutine(GetTexture(_imageUrls[_currentImageIndex]));
                    }
                    break;

                // �ڑ��G���[�̏ꍇ
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError($"���N�G�X�g���s: {request.error}");
                    break;
            }

            // 10�A�K�`���̏ꍇ���N�G�X�g�Ԃ�1�b�Ԋu���J����
            if (count > 1)
            {
                yield return new WaitForSeconds(1f); // 1�b�ҋ@
            }
        }

        // �S�Ẵ��N�G�X�g�������������߁A�摜�\�����t���O���I�t�ɂ���
        _isDisplayingImages = false;
    }

    // �w�肵��URL����e�N�X�`�����擾
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // ���N�G�X�g����������܂őҋ@
        yield return request.SendWebRequest();

        // �e�N�X�`���̎擾�����������ꍇ
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("�摜�擾����");

            // �擾�����e�N�X�`�����V�[�����̃I�u�W�F�N�g�ɓK�p
            Texture loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            GameObject.Find("Tile").GetComponent<MeshRenderer>().material.SetTexture("_MainTex", loadedTexture);
        }
        else // �摜�̎擾�����s�����ꍇ
        {
            
            Debug.LogError($"�摜�擾���s: {request.error}");
        }
    }

    void Update()
    {
        // �摜�\�����ŁA�G���^�[�L�[�������ꂽ�ꍇ
        if (_isDisplayingImages && Input.GetKeyDown(KeyCode.Return))
        {
            // ���̉摜�����݂���ꍇ�A�C���f�b�N�X��i�߂Ď��̉摜��\��
            if (_currentImageIndex < _imageUrls.Length - 1)
            {
                _currentImageIndex++;
                StartCoroutine(GetTexture(_imageUrls[_currentImageIndex])); // ���̉摜��\��
            }
        }
    }
}