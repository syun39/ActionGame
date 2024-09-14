using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // �J�ڂ���V�[���̖��O���C���X�y�N�^�[�Őݒ肷��
    [SerializeField] private string _sceneToLoad;

    void Update()
    {
        // �G���^�[�L�[�������ꂽ���`�F�b�N
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // �w�肳�ꂽ�V�[���ɑJ�ڂ���
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
