using UnityEngine;

public class GachaDataManager : MonoBehaviour
{
    // �V���O���g���C���X�^���X
    public static GachaDataManager Instance { get; private set; }

    // GachaData 
    [SerializeField] private GachaData _gachaData;

    private void Awake()
    {
        // �C���X�^���X�����݂��Ȃ��ꍇ
        if (Instance == null)
        {
            Instance = this; // �V���O���g���Ƃ��Đݒ�
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // ���ɑ��݂���ꍇ�͎������g��j��
        }
    }
}
