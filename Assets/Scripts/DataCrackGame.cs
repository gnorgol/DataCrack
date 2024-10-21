using UnityEngine;
using UnityEngine.UI;

public class DataCrackGame : MonoBehaviour
{
    public RectTransform redLine; // La ligne rouge fixe
    public RectTransform[] bars;  // Les barres blanches qui bougent
    public float baseSpeed;       // Vitesse de base pour les barres
    public RectTransform BarsZone; // Zone des barres
    private int currentBarIndex = 0;
    private bool[] movingUp;      // Indique si chaque barre monte ou descend
    private bool[] isBarLocked;   // Indique si la barre est verrouillée en place

    void Start()
    {
        // Initialisation des directions de mouvement et du verrouillage
        movingUp = new bool[bars.Length];
        isBarLocked = new bool[bars.Length];

        // Toutes les barres commencent en montant
        for (int i = 0; i < bars.Length; i++)
        {
            movingUp[i] = true;
            isBarLocked[i] = false;
        }
    }

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
        // Déplacer chaque barre
        for (int i = 0; i < bars.Length; i++)
        {
            // Si la barre est verrouillée, ne pas la déplacer
            if (isBarLocked[i]) continue;

            // Calculer la vitesse en fonction de la position de la barre dans la liste
            float speed = baseSpeed * (1 + i * 0.1f); // Les barres suivantes sont plus rapides

            // Déterminer la direction du mouvement
            float direction = movingUp[i] ? 1 : -1;

            // Déplacer la barre en fonction de sa vitesse spécifique
            bars[i].anchoredPosition += new Vector2(0, speed * direction * Time.deltaTime);

            // Vérifier si la barre atteint le haut ou le bas de la zone
            if (bars[i].anchoredPosition.y > BarsZone.rect.height / 2)
            {
                movingUp[i] = false;
            }
            else if (bars[i].anchoredPosition.y < -BarsZone.rect.height / 2)
            {
                movingUp[i] = true;
            }
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
            // Placer la barre exactement au milieu de la ligne rouge
            bars[currentBarIndex].anchoredPosition = new Vector2(bars[currentBarIndex].anchoredPosition.x, linePositionY);
            isBarLocked[currentBarIndex] = true; // Verrouiller la barre en place
            currentBarIndex++; // Passer à la barre suivante
            if (currentBarIndex >= bars.Length)
            {
                Debug.Log("Toutes les barres sont placées !");
                // Toutes les barres sont placées, le joueur a gagné
            }
        }
        else
        {
            Debug.Log("Échec !");
            // Revenir à la barre précédente si elle existe
            if (currentBarIndex > 0)
            {
                isBarLocked[currentBarIndex] = false; // Déverrouiller la barre actuelle
                currentBarIndex--; // Revenir à la barre précédente
                isBarLocked[currentBarIndex] = false; // Remettre en mouvement la barre N-1
            }
        }
    }

}
