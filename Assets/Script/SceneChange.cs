using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [Tooltip("�J�ڐ�̃V�[����")]
    [SerializeField] private string _nextScene;

    [Tooltip("BGM��AudioSource"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private AudioSource _bgmSource = null;

    [Tooltip("SE��AudioSource"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private AudioSource _seSource = null;

    [Tooltip("SE��������\������p�l��"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _panel = null;

    [Tooltip("SE��������\������C���X�g"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _image = null;

    [Tooltip("�G���^�[�������ꂽ��\������C���X�g"),Header("SSRTwo�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _mikuRin = null;

    private bool _isPlay = false;

    // UR�V�[�����ǂ���
    private bool _isURScene = false;

    // SSR�V�[�����ǂ���
    private bool _isSSRTwoScene = false;

    private void Start()
    {
        _isPlay = true;

        if (SceneManager.GetActiveScene().name == "UR Scene")
        {
            _panel.SetActive(false);
            _image.SetActive(false);
            _isURScene = true;
        }
        else if (SceneManager.GetActiveScene().name == "SSR Two Scene")
        {
            _mikuRin.SetActive(false);
            _isSSRTwoScene = true;
        }
    }

    private void Update()
    {
        // �G���^�[�L�[�������ꂽ�����m�F
        if (_isPlay && Input.GetKeyDown(KeyCode.Return))
        {
            if (_isURScene)
            {
                StartCoroutine(URTransition());
            }
            else if (_isSSRTwoScene)
            {
                StartCoroutine(SSRTwoTransition());
            }
            else
            {
                // �w�肳�ꂽ�V�[���ɑJ�ڂ���
                SceneManager.LoadScene(_nextScene);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator SSRTwoTransition()
    {
        _mikuRin.SetActive(true);
        _isPlay = false;
        yield return new WaitForSeconds(1.7f);
        SceneManager.LoadScene(_nextScene);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator URTransition()
    {
        // BGM���~�߂�
        if (_bgmSource?.isPlaying == true)
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

        _image?.SetActive(true);

        // 0.5�b�ҋ@
        yield return new WaitForSeconds(0.5f);

        // �p�l����\��
        _panel?.SetActive(true);

        _image?.SetActive(false);

        // SE����I���܂őҋ@
        yield return new WaitWhile(() => _seSource.isPlaying);

        // �V�[���J��
        SceneManager.LoadScene(_nextScene);
    }
}
