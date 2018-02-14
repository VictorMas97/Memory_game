using UnityEngine;

public class TokenController : MonoBehaviour
{
    private Animation anim;

    void Awake()
    {
        anim = GetComponent<Animation>(); 
    }

    public void flip() // Lanza la animación que hace la ficha
    {
        anim.PlayQueued("Turn");
    }

    public void contraryFlip() // Lanza la animación que hace la ficha
    {
        anim.PlayQueued("ContraryTurn");
    }
}