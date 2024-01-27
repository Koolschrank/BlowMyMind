using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    public PlayerCharacter playerCharacter;
    public void EnableHitBox()
    {
        playerCharacter.EnableItemHitBox();
        
    }

    public void DisableHitBox()
    {
        playerCharacter.DisableItemHitBox();
    }

    public void Throw()
    {
        playerCharacter.ThrowItem();
    }
}
