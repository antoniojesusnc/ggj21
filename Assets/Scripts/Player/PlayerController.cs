public class PlayerController : CharacterController
{
    private LevelController _levelController;
    void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
    }
    
    public override void ChangeCharacterStatus(ECharacterStatus newCharacterStatus)
    {
        base.ChangeCharacterStatus(newCharacterStatus);
        if (LastCharacterStatus == ECharacterStatus.Moving &&
            CharacterStatus == ECharacterStatus.Idle)
        {
            _levelController.CheckForCollectable(this);
        }
    }
}
