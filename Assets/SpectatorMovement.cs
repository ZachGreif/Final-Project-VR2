using UnityEngine;

public class SpectatorCrowd : MonoBehaviour
{
    [Header("Parts")]
    public Transform body;
    public Transform head;
    public Renderer bodyRenderer;
    public Renderer headRenderer;

    [Header("Bobbing")]
    public float bobHeight = 0.08f;
    public float bobSpeed = 2f;

    [Header("Color Options")]
    public Color[] bodyColors;
    public Color[] headColors;

    private Vector3 startPos;
    private float randomOffset;

    void Start()
    {
        startPos = transform.localPosition;
        randomOffset = Random.Range(0f, 10f);

        bobHeight += Random.Range(-0.02f, 0.02f);
        bobSpeed += Random.Range(-0.4f, 0.4f);

        ApplyRandomColors();
    }

    void Update()
    {
        float y = Mathf.Sin((Time.time + randomOffset) * bobSpeed) * bobHeight;
        transform.localPosition = startPos + new Vector3(0f, y, 0f);
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