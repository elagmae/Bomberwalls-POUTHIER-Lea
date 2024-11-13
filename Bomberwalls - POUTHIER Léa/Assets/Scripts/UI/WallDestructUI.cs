using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WallDestructUI : MonoBehaviour
{
    [SerializeField]
    private BombExplosion _bombExplosion;
    [SerializeField]
    private GameObject _goSliderPV;
    [SerializeField]
    private TextMeshProUGUI _numberPV;

    private Slider _sliderPV;

    private void Awake()
    {
        _sliderPV = _goSliderPV.GetComponent<Slider>();
        _bombExplosion.OnWallTouched += RemoveWallPV;
    }

    public void RemoveWallPV(string wallName)
    {
        print(wallName + "ça remove là");
        if (wallName == this.gameObject.name && _sliderPV.value != 0)
        {
            _sliderPV.value--;
            _numberPV.text = _sliderPV.value.ToString();

            if(_sliderPV.value == 0)
            {
                Destroy(this.gameObject);
                Debug.Log("end game");
            }
        }
    }
}
