using UnityEngine;

public class CollectableController : MonoBehaviour
{
    public ECollectableType CollectableType => _collectableType;
    [SerializeField]
    private ECollectableType _collectableType;
    
    public CollectableData CollectableData => _collectableData;
    [SerializeField]
    private CollectableData _collectableData;

    public ECollectableStatus LastCollectableStatus { get; private set; }
    public ECollectableStatus CollectableStatus { get; private set; }
    
    void Start()
    {
        
    }

    public virtual void ChangeCollectableStatus(ECollectableStatus newCollectableStatus)
    {
        if (newCollectableStatus != CollectableStatus)
        {
            LastCollectableStatus = CollectableStatus;
            CollectableStatus = newCollectableStatus;

            if (newCollectableStatus == ECollectableStatus.Collected)
            {
                Collect();
            }
        }
    }

    private void Collect()
    {
        gameObject.SetActive(false);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
