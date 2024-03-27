using UnityEngine;


public enum ColorEnum
{
    RED, GREEN, BLUE
}
public class CardController : MonoBehaviour
{
    public ColorEnum color;
    public Transform mesh;

    public bool IsPicked;

    private void Awake()
    {

        mesh.GetChild(0).gameObject.SetActive(false);

        switch (color)
        {
            case ColorEnum.RED:
                mesh.GetChild(1).gameObject.SetActive(true);
                break;
            case ColorEnum.GREEN:
                mesh.GetChild(2).gameObject.SetActive(true);
                break;
            case ColorEnum.BLUE:
                mesh.GetChild(3).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }



    public void GetPicked()
    {

    }

}
