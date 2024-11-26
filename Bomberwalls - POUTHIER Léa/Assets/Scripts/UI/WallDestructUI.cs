using TMPro;
using Unity.VisualScripting;
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
    [SerializeField]
    private GameObject _endPanel;
    [SerializeField]
    private TextMeshProUGUI _winnerUI;
    private Color _color;

    private Slider _sliderPV;

    private void Awake()
    {
        _color = GetComponent<SpriteRenderer>().color;
        _sliderPV = _goSliderPV.GetComponent<Slider>();
        _bombExplosion.OnWallTouched += RemoveWallPV;
    }

    public void RemoveWallPV(string wallName)
    {
        if (wallName == this.gameObject.name && _sliderPV.value != 0)
        {
            _sliderPV.value--;
            _numberPV.text = _sliderPV.value.ToString();

            if(_sliderPV.value == 0)
            {
                Destroy(this.gameObject);
                EndGame();
            }
        }
    }

    public void EndGame()
    {
        string playerColor;
        _endPanel.SetActive(true);
        Time.timeScale = 0;

        if (this.gameObject.name.EndsWith("R"))
        {
            playerColor = "Rouge";
        }

        else
        {
            playerColor = "Bleu";
        }

        _winnerUI.text = "Le <color=#" + _color.ToHexString() + "> Joueur " + playerColor +" </color> a gagné !";
    }
}
