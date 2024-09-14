using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class URSceneChange : MonoBehaviour
{
    // BGM��SE�p��AudioSource
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _seSource;

    // SE��������\������p�l��
    [SerializeField] private GameObject _panel;

    // �J�ڐ�̃V�[����
    [SerializeField] private string _nextScene;

    private bool _isPlay = false;
    private void Start()
    {
        _panel.SetActive(false);
        _isPlay = true;
    }

    private void Update()
    {
        // �G���^�[�L�[�������ꂽ�����m�F
        if (_isPlay && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(HandleTransition());
        }
    }

    IEnumerator HandleTransition()
    {
        // BGM���~�߂�
        if (_bgmSource.isPlaying)
        {
            _bgmSource.Stop();
        }
        _isPlay = false;

        // 1�b�ҋ@
        yield return new WaitForSeconds(1.0f);

        // SE���Đ�
        if (_seSource != null)
        {
            _seSource.Play();
        }

        // �p�l����\��
        _panel.SetActive(true);

        // SE����I���܂őҋ@
        yield return new WaitWhile(() => _seSource.isPlaying);

        // �V�[���J��
        SceneManager.LoadScene(_nextScene);
    }
}
