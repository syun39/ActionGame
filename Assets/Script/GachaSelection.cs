using UnityEngine;

public class GachaSelection : MonoBehaviour
{
    [SerializeField] GameObject _dogGachaCanvas; // ���̃L�����o�X
    [SerializeField] GameObject _catGachaCanvas; // �L�̃L�����o�X

    private void Start()
    {
        ShowDogGachaCanvas(); // �����͌�
    }

    /// <summary>
    /// ���̃K�`�� Canvas ��\��
    /// </summary>
    public void ShowDogGachaCanvas()
    {
        if (_dogGachaCanvas != null)
        {
            _dogGachaCanvas.SetActive(true);
        }

        if (_catGachaCanvas != null)
        {
            _catGachaCanvas.SetActive(false);
        }
    }

    /// <summary>
    /// �L�̃K�`�� Canvas ��\��
    /// </summary>
    public void ShowCatGachaCanvas()
    {
        if (_dogGachaCanvas != null)
        {
            _dogGachaCanvas.SetActive(false);
        }

        if (_catGachaCanvas != null)
        {
            _catGachaCanvas.SetActive(true);
        }
    }
}
