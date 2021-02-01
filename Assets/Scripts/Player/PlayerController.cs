using System.Collections;
using UnityEngine;

public class PlayerController : CharacterController
{
    private LevelController _levelController;

    [SerializeField] private Material _viewMaterial;
    [SerializeField] private Material _hideMaterial;
    
    public Vector2Int GridPosition
    {
        get
        {
            var characterMovement = GetComponent<CharacterMovement>();
            if (characterMovement != null)
            {
                return characterMovement.GridPosition;
            }
            else
            {
                return Vector2Int.zero;
            }
        }
    }
    void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
    }
    
    public override void ChangeCharacterStatus(ECharacterStatus newCharacterStatus)
    {
        base.ChangeCharacterStatus(newCharacterStatus);

        if (WasMoving)
        {
            _levelController.PlayerMoveToNewCell(this);
        }
    }

    public bool WasMoving => LastCharacterStatus == ECharacterStatus.Moving &&
                             CharacterStatus == ECharacterStatus.Idle;

    public void SetVisible()
    {
        StartCoroutine(ApplyAlphaColorCo(Color.clear, Color.white, 
            _viewMaterial, _viewMaterial));
    }
    
    public void SetNotVisible()
    {
        StartCoroutine(ApplyAlphaColorCo(Color.white, Color.clear, 
            _viewMaterial, _hideMaterial));
    }

    private IEnumerator ApplyAlphaColorCo(Color colorOrigin, Color colorDestiny, Material initialMaterial, Material finalMaterial)
    {
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer.color == colorDestiny )
        {
            yield break;
        }

        spriteRenderer.material = initialMaterial;
        float timeStamp = 0;
        while (timeStamp < CharacterData.timeToFade)
        {
            timeStamp += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(colorOrigin, colorDestiny, timeStamp / CharacterData.timeToFade);
            yield return new WaitForEndOfFrame();
        }
        spriteRenderer.material = finalMaterial;
    }
}
