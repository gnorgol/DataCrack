using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataCrackGame : MonoBehaviour
{
    public RectTransform redLine; // La ligne rouge fixe
    public RectTransform[] bars;  // Les barres blanches qui bougent
    public float baseSpeed;       // Vitesse de base pour les barres
    public RectTransform BarsZone; // Zone des barres
    public int tolerance = 10;         // Tol�rance d'alignement
    private int currentBarIndex = 0;
    private bool[] movingUp;      // Indique si chaque barre monte ou descend
    private bool[] isBarLocked;   // Indique si la barre est verrouill�e en place
    public GameObject GameResultInfo;

    void Start()
    {
        GameResultInfo.SetActive(false);
        //Dont set focus on input field
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        // Initialisation des directions de mouvement et du verrouillage
        movingUp = new bool[bars.Length];
        isBarLocked = new bool[bars.Length];

        // S�lectionner la premi�re barre
        ColorSelectBar(bars[0].gameObject, true);

        // Toutes les barres commencent en montant
        for (int i = 0; i < bars.Length; i++)
        {
            movingUp[i] = true;
            isBarLocked[i] = false;
        }
    }

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
    void ColorSelectBar(GameObject bar, bool isSelected)
    {
        // Get child of the bar and set the outline color
        Outline[] outlines = bar.GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = isSelected;
            outline.effectColor = isSelected ? Color.yellow : Color.white;
            outline.effectDistance = new Vector2(6, -6);
        }
    }

    void MoveBars()
    {
        // D�placer chaque barre
        for (int i = 0; i < bars.Length; i++)
        {
            // Si la barre est verrouill�e, ne pas la d�placer
            if (isBarLocked[i]) continue;

            // Calculer la vitesse en fonction de la position de la barre dans la liste
            float speed = baseSpeed * (1 + i * 0.1f); // Les barres suivantes sont plus rapides

            // D�terminer la direction du mouvement
            float direction = movingUp[i] ? 1 : -1;

            // D�placer la barre en fonction de sa vitesse sp�cifique
            bars[i].anchoredPosition += new Vector2(0, speed * direction * Time.deltaTime);

            // V�rifier si la barre atteint le haut ou le bas de la zone
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

    public void NewGame()
    {
        GameResultInfo.SetActive(false);
        // Remettre � z�ro les barres
        for (int i = 0; i < bars.Length; i++)
        {
            bars[i].anchoredPosition = new Vector2(bars[i].anchoredPosition.x, 0);
            movingUp[i] = true;
            isBarLocked[i] = false;
        }

        // S�lectionner la premi�re barre
        currentBarIndex = 0;
        ColorSelectBar(bars[0].gameObject, true);
        //Dont set focus on input field
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }
    private void DisplayGameResult(string result, Color color)
    {
        GameResultInfo.SetActive(true);
        GameResultInfo.GetComponent<TextMeshProUGUI>().text = result;
        GameResultInfo.GetComponent<TextMeshProUGUI>().color = color;
    }

    void CheckBarPosition()
    {
        // V�rifier si la barre active est align�e avec la ligne rouge
        float barPositionY = bars[currentBarIndex].anchoredPosition.y;
        float linePositionY = redLine.anchoredPosition.y;

        if (Mathf.Abs(barPositionY - linePositionY) < tolerance) // Tol�rance d'alignement
        {
            Debug.Log("Succ�s !");
            // Placer la barre exactement au milieu de la ligne rouge
            bars[currentBarIndex].anchoredPosition = new Vector2(bars[currentBarIndex].anchoredPosition.x, linePositionY);
            isBarLocked[currentBarIndex] = true; // Verrouiller la barre en place
            ColorSelectBar(bars[currentBarIndex].gameObject, false); // D�s�lectionner la barre
            currentBarIndex++; // Passer � la barre suivante
             // S�lectionner la barre suivante
            if (currentBarIndex >= bars.Length)
            {
                Debug.Log("Toutes les barres sont plac�es !");
                // Toutes les barres sont plac�es, le joueur a gagn�
                ColorSelectBar(bars[currentBarIndex-1].gameObject, false);
                DisplayGameResult("Gagn� !", Color.green);
            }
            else
            {
                ColorSelectBar(bars[currentBarIndex].gameObject, true);
            }
        }
        else
        {
            Debug.Log("�chec !");
            Debug.Log("Barre : " + barPositionY + " Ligne : " + linePositionY);
            Debug.Log("Diff�rence : " + Mathf.Abs(barPositionY - linePositionY));
            // Revenir � la barre pr�c�dente si elle existe
            if (currentBarIndex > 0)
            {
                isBarLocked[currentBarIndex] = false; // D�verrouiller la barre actuelle
                ColorSelectBar(bars[currentBarIndex].gameObject, false); // D�s�lectionner la barre actuelle
                currentBarIndex--; // Revenir � la barre pr�c�dente
                ColorSelectBar(bars[currentBarIndex].gameObject, true); // S�lectionner la barre pr�c�dente
                isBarLocked[currentBarIndex] = false; // Remettre en mouvement la barre N-1
            }
        }
    }

}
