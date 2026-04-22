using UnityEngine;

public class SpectatorCrowd : MonoBehaviour
{
    [Header("Parts")]
    public Renderer bodyRenderer;
    public Renderer headRenderer;

    [Header("Bobbing")]
    public float bobHeight = 0.08f;
    public float bobSpeed = 2f;

    [Header("Rotation Sway")]
    public float swayAngle = 5f;

    [Header("Random Scale")]
    public float minScale = 0.9f;
    public float maxScale = 1.1f;

    [Header("Color Options")]
    public Color[] bodyColors;
    public Color[] headColors;

    private Vector3 startPos;
    private Vector3 startScale;
    private float randomOffset;

    void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
        randomOffset = Random.Range(0f, 10f);

        bobHeight += Random.Range(-0.02f, 0.02f);
        bobSpeed += Random.Range(-0.4f, 0.4f);

        float scaleOffset = Random.Range(minScale, maxScale);
        transform.localScale = startScale * scaleOffset;

        ApplyRandomColors();
    }

    void Update()
    {
        float bob = Mathf.Sin((Time.time + randomOffset) * bobSpeed) * bobHeight;
        transform.localPosition = startPos + new Vector3(0f, bob, 0f);

        float angle = Mathf.Sin((Time.time + randomOffset) * bobSpeed) * swayAngle;
        transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }

    void ApplyRandomColors()
    {
        if (bodyRenderer != null && bodyColors.Length > 0)
        {
            bodyRenderer.material.color = bodyColors[Random.Range(0, bodyColors.Length)];
        }

        if (headRenderer != null && headColors.Length > 0)
        {
            headRenderer.material.color = headColors[Random.Range(0, headColors.Length)];
        }
    }
}