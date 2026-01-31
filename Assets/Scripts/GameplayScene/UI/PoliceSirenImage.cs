using UnityEngine;
using UnityEngine.UI;

public class PoliceSirenImage : MonoBehaviour
{
    [Header("References")]
    public Image image;

    [Header("Siren Colors")]
    public Color redColor = new Color(1f, 0.15f, 0.15f);
    public Color blueColor = new Color(0.15f, 0.35f, 1f);

    [Header("Siren Feel")]
    public float pulseSpeed = 6f;     // how fast the siren alternates
    public float brightness = 1.2f;   // intensity boost (police feel)

    private float t;

    void Awake()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    void Update()
    {
        t += Time.deltaTime * pulseSpeed;

        // Ping-pong between red and blue
        float lerp = Mathf.PingPong(t, 1f);

        Color sirenColor = Color.Lerp(redColor, blueColor, lerp) * brightness;
        image.color = sirenColor;
    }
}
