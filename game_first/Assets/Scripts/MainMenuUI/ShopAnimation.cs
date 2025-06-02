using UnityEngine;
using System.Collections;
using TMPro;

public class ShopAnimation : MonoBehaviour
{
    public Material material;
    
    [SerializeField] GameObject musicSlider;
    [SerializeField] GameObject soundSlider;
    
    [SerializeField] GameObject costText;
    
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject buyButton;
    [SerializeField] GameObject recordsButton;
    [SerializeField] GameObject educationButton;
    
    [SerializeField] GameObject[] skinButtons;
    
    public GameObject targetObject; 
    private const float targetScaleX = 7f; 
    private const float initialScaleX = -7f; 
    private const float leftYAngle = -10f;
    private const float rightYAngle = -60f;
    private const float duration = 1f;
    private const float delay = 0.03f;

    private bool isScaledUp = true;
    
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private TextMeshProUGUI costTextMesh;
    
    public void ToggleScaleAndRotate()
    {
        if (isScaledUp)
        {
            StartCoroutine(ScaleAndRotateObject(targetObject.transform.localScale.x, initialScaleX, leftYAngle,
                rightYAngle));
            textMesh.text = "To menu";
            costTextMesh.text = "Selected";
            costText.SetActive(true);
            buyButton.SetActive(true);

            playButton.SetActive(false);
            educationButton.SetActive(false);
            recordsButton.SetActive(false);
            musicSlider.SetActive(false);
            soundSlider.SetActive(false);
            foreach (var button in skinButtons)
                button.SetActive(true);
        }
        else
        {
            StartCoroutine(ScaleAndRotateObject(targetObject.transform.localScale.x, targetScaleX, rightYAngle,
                leftYAngle));
            textMesh.text = "Shop";
            playButton.SetActive(true);
            educationButton.SetActive(true);
            recordsButton.SetActive(true);
            costText.SetActive(false);
            buyButton.SetActive(false);
            musicSlider.SetActive(true);
            soundSlider.SetActive(true);
            foreach (var button in skinButtons)
                button.SetActive(false);
            material.SetTexture("_MainTex", TextureManager.CurrentTexture);
        }

        isScaledUp = !isScaledUp;
    }

    private IEnumerator ScaleAndRotateObject(float startX, float endX, float startYRotation, float endYRotation)
    {
        float elapsedTime = 0f;
        Vector3 startScale = new Vector3(startX, targetObject.transform.localScale.y, targetObject.transform.localScale.z);
        Vector3 endScale = new Vector3(endX, targetObject.transform.localScale.y, targetObject.transform.localScale.z);
        Quaternion startRotation = Quaternion.Euler(-42.703f, startYRotation, 31.873f);
        Quaternion endRotation = Quaternion.Euler(-42.703f, endYRotation, 31.873f);

        while (elapsedTime < duration)
        {
            targetObject.transform.localScale = Vector3.Lerp(startScale, endScale, (elapsedTime / duration));
            targetObject.transform.rotation = Quaternion.Slerp(startRotation, endRotation, (elapsedTime / duration));
            elapsedTime += delay;
            yield return null; 
        }
        
        targetObject.transform.localScale = endScale;
        targetObject.transform.rotation = endRotation;
    }
}



