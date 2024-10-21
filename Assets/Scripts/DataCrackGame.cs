using UnityEngine;
using UnityEngine.UI;

public class DataCrackGame : MonoBehaviour
{
    public RectTransform redLine; // La ligne rouge fixe
    public RectTransform[] bars;  // Les barres blanches qui bougent
    public float barSpeed = 100f; // Vitesse de d�placement des barres
    private int currentBarIndex = 0;
    private bool movingUp = true; // Indique si la barre monte ou descend
    public RectTransform BarsZone;

    void Update()
    {
        // D�placement des barres
        MoveBars();

        // V�rifier si le joueur appuie sur la barre d'espace pour valider
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckBarPosition();
        }
    }

    void MoveBars()
    {
        // D�terminer la direction du mouvement
        float direction = movingUp ? 1 : -1;

        // D�placer la barre active
        bars[currentBarIndex].anchoredPosition += new Vector2(0, barSpeed * direction * Time.deltaTime);

        // V�rifier si la barre atteint le haut ou le bas de de la bare zone
        if (bars[currentBarIndex].anchoredPosition.y > BarsZone.rect.height / 2)
        {
            movingUp = false;
        }
        else if (bars[currentBarIndex].anchoredPosition.y < -BarsZone.rect.height / 2)
        {
            movingUp = true;
        }
    }

    void CheckBarPosition()
    {
        // V�rifier si la barre active est align�e avec la ligne rouge
        float barPositionY = bars[currentBarIndex].anchoredPosition.y;
        float linePositionY = redLine.anchoredPosition.y;

        if (Mathf.Abs(barPositionY - linePositionY) < 10f) // Tol�rance d'alignement
        {
            Debug.Log("Succ�s !");
            currentBarIndex = (currentBarIndex + 1) % bars.Length;
        }
        else
        {
            Debug.Log("�chec !");
        }
    }
}
