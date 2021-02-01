using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Data/New Character Data", order = 1)]
public class CharacterData : ScriptableObject
{
    public float _characterSpeedBySquare;
    public float _characterDelayStartWalk;
    
    public float timeToFade;
}
