using UnityEngine;
using UnityEngine.UI;

public class DataCrackGame : MonoBehaviour
{
    public RectTransform redLine; // La ligne rouge fixe
    public RectTransform[] bars;  // Les barres blanches qui bougent
    public float barSpeed = 100f; // Vitesse de déplacement des barres
    private int currentBarIndex = 0;
    private bool movingUp = true; // Indique si la barre monte ou descend
    public RectTransform BarsZone;

    void Update()
    {
        // Déplacement des barres
        MoveBars();

        // Vérifier si le joueur appuie sur la barre d'espace pour valider
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckBarPosition();
        }
    }

    void MoveBars()
    {
        // Déterminer la direction du mouvement
        float direction = movingUp ? 1 : -1;

        // Déplacer la barre active
        bars[currentBarIndex].anchoredPosition += new Vector2(0, barSpeed * direction * Time.deltaTime);

        // Vérifier si la barre atteint le haut ou le bas de de la bare zone
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
        // Vérifier si la barre active est alignée avec la ligne rouge
        float barPositionY = bars[currentBarIndex].anchoredPosition.y;
        float linePositionY = redLine.anchoredPosition.y;

        if (Mathf.Abs(barPositionY - linePositionY) < 10f) // Tolérance d'alignement
        {
            Debug.Log("Succès !");
            currentBarIndex = (currentBarIndex + 1) % bars.Length;
        }
        else
        {
            Debug.Log("Échec !");
        }
    }
}
