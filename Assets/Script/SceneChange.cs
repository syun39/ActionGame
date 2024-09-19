using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [Tooltip("�J�ڐ�̃V�[����")]
    [SerializeField] private string _nextScene = null;

    [Tooltip("BGM��AudioSource"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private AudioSource _bgmSource = null;

    [Tooltip("SE��AudioSource"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private AudioSource _seSource = null;

    [Tooltip("SE��������\������p�l��"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _panel = null;

    [Tooltip("SE��������\������C���X�g"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _image = null;

    [Tooltip("�G���^�[�������ꂽ��\������C���X�g"), Header("SSRTwo�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _mikuRin = null;

    // �G���^�[�L�[�𖳌��ɂ��邩�ǂ���
    private bool _isInvalid = false;

    // UR�V�[�����ǂ���
    private bool _isURScene = false;

    // SSR�V�[�����ǂ���
    private bool _isSSRTwoScene = false;

    // Result�V�[�����ǂ���
    private bool _isResultScene = false;

    private void Start()
    {
        _isInvalid = true; // �G���^�[�L�[�L��

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
        else if (SceneManager.GetActiveScene().name == "Result Scene")
        {
            _isResultScene = true;
        }
    }

    private void Update()
    {
        // �G���^�[�L�[�������ꂽ��
        if (_isInvalid && Input.GetKeyDown(KeyCode.Return))
        {
            if (_isURScene)
            {
                StartCoroutine(URTransition());
            }
            else if (_isSSRTwoScene)
            {
                StartCoroutine(SSRTwoTransition());
            }
            else if (_isResultScene)
            {
                return;
            }
            else
            {
                ChangeScene();
            }
        }
    }

    /// <summary>
    /// �V�[���J��
    /// </summary>
    public void ChangeScene()
    {
        if(_nextScene != null)
        {
            // �w�肳�ꂽ�V�[���ɑJ�ڂ���
            SceneManager.LoadScene(_nextScene);
        }
       
    }

    /// <summary>
    /// �V�[���J�ڂ�x�点��ꍇ
    /// </summary>
    /// <param name="sceneName"></param>
    public void ChangeTitleScene(string sceneName)
    {
        StartCoroutine(WaitLoadScene(sceneName));
    }

    /// <summary>
    /// 
    /// </summary>
    private IEnumerator WaitLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// SSR2���ȏ�̎��̃V�[���J��
    /// </summary>
    IEnumerator SSRTwoTransition()
    {
        _mikuRin.SetActive(true);
        _isInvalid = false; // �G���^�[�L�[�𖳌�
        yield return new WaitForSeconds(1.7f);
        ChangeScene();
    }

    /// <summary>
    /// UR1���ȏ�
    /// </summary>
    IEnumerator URTransition()
    {
        // BGM�Đ����Ȃ�~�߂�
        if (_bgmSource?.isPlaying == true)
        {
            _bgmSource.Stop();
        }
        _isInvalid = false; // �G���^�[�L�[�𖳌�

        // 1�b�ҋ@
        yield return new WaitForSeconds(1.0f);

        // SE���Đ�
        if (_seSource != null)
        {
            _seSource.Play();
        }

        // �C���X�g��\��
        _image?.SetActive(true);

        // 0.5�b�ҋ@
        yield return new WaitForSeconds(0.5f);

        // �p�l����\��
        _panel?.SetActive(true);

        _image?.SetActive(false);

        // SE����I���܂őҋ@
        yield return new WaitWhile(() => _seSource.isPlaying);

        // �V�[���J��
        ChangeScene();
    }
}
