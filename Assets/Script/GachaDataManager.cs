using UnityEngine;

public class GachaDataManager : MonoBehaviour
{
    // �V���O���g���C���X�^���X
    public static GachaDataManager Instance { get; private set; }

    // GachaData 
    [SerializeField] private GachaData _gachaData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // ���ɑ��݂���ꍇ�͎������g��j��
        }
    }
}
