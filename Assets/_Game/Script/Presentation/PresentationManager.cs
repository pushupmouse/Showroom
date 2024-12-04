using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PresentationManager : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> imageSprites; // Assign your images in the Editor here, in the desired order.

    [SerializeField]
    private Image displayImage; // Assign your UI Image in the Inspector.

    [SerializeField]
    private Button nextButton; // Assign your "Next" button in the Inspector.

    [SerializeField]
    private Button prevButton; // Assign your "Previous" button in the Inspector.

    private int currentIndex = 0; // Track the current image index.

    void Start()
    {
        // Initialize the gallery if images are assigned.
        if (imageSprites != null && imageSprites.Count > 0)
        {
            currentIndex = 0;
            UpdateImage();
        }

        // Add button listeners.
        nextButton.onClick.AddListener(NextImage);
        prevButton.onClick.AddListener(PreviousImage);

        // Update button states initially.
        UpdateButtonInteractivity();
    }

    void NextImage()
    {
        if (currentIndex < imageSprites.Count - 1)
        {
            currentIndex++;
            UpdateImage();
        }
    }

    void PreviousImage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateImage();
        }
    }

    void UpdateImage()
    {
        if (imageSprites.Count > 0 && displayImage != null)
        {
            displayImage.sprite = imageSprites[currentIndex];
        }

        // Update button interactivity.
        UpdateButtonInteractivity();
    }

    void UpdateButtonInteractivity()
    {
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < imageSprites.Count - 1;
    }
}
