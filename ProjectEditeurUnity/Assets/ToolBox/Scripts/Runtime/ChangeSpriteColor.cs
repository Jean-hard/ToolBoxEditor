using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChangeSpriteColor : MonoBehaviour
{
    public enum CHANGE_MODE
    {
        RANDOM = 0,
        CUSTOM
    }

    public CHANGE_MODE changeMode = CHANGE_MODE.RANDOM;
    public Color customColor = Color.white;

}
