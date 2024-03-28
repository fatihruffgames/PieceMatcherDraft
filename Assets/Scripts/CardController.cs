using UnityEngine;


public enum ColorEnum
{
    RED, GREEN, BLUE
}
public class CardController : MonoBehaviour
{
    [Header("Config")]
    public ColorEnum color;

    [Header("References")]
    public Transform mesh;
    [SerializeField] Material[] darkMaterials;
    Material brightMaterial;
    Transform _currentMesh;
    int _currentMaterialIndex;
    Renderer _renderer;

    private void Awake()
    {
        mesh.GetChild(0).gameObject.SetActive(false);
        switch (color)
        {
            case ColorEnum.RED:
                mesh.GetChild(1).gameObject.SetActive(true);
                _currentMaterialIndex = 0;
                _currentMesh = mesh.GetChild(1);
                break;
            case ColorEnum.GREEN:
                mesh.GetChild(2).gameObject.SetActive(true);
                _currentMaterialIndex = 1;
                _currentMesh = mesh.GetChild(2);
                break;
            case ColorEnum.BLUE:
                mesh.GetChild(3).gameObject.SetActive(true);
                _currentMaterialIndex = 2;
                _currentMesh = mesh.GetChild(3);
                break;
            default:
                break;
        }

        _renderer = _currentMesh.transform.GetComponent<Renderer>();
        brightMaterial = _renderer.material;

        if (!transform.parent.GetComponent<StackController>().isInTopLayer)
            _renderer.material = darkMaterials[_currentMaterialIndex];
    }

    public void GetActivated()
    {
        _renderer.material = brightMaterial;
    }

    /* void AddOutline()
     {
         Color color = new Color(0, 0, 0, 200);
         _currentMesh.GetComponent<Renderer>().materials[1].SetFloat("_Width", 0.7f);
         _currentMesh.GetComponent<Renderer>().materials[1].SetColor("_Color", _currentMesh.GetComponent<Renderer>().materials[1].GetColor("_Color") + color);
     }
     void RemoveOutline()
     {
         Color color = new Color(0, 0, 0, 100);
         _currentMesh.GetComponent<Renderer>().materials[1].SetFloat("_Width", 0.2f);
         _currentMesh.GetComponent<Renderer>().materials[1].SetColor("_Color", _currentMesh.GetComponent<Renderer>().materials[1].GetColor("_Color") + color);
     }*/
}
