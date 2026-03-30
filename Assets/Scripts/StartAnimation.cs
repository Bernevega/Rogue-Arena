// Ann Bernevega - edited 4.2.2025

using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    private Animation anim; // Reference to the animation component

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();  // Get the animation component attached to the GameObject
        anim.Play(); // Play the animation as soon as the object is initialized
    }
}
