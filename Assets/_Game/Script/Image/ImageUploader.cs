using Firebase.Extensions;
using Firebase.Storage;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ImageUploader : MonoBehaviour
{
    public static ImageUploader Instance;

    private FirebaseStorage storage;
    private StorageReference storageReference;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://firestoreshowroom.appspot.com/");

        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".jpg");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
    }

    public void UploadImage()
    {
        StartCoroutine(ShowLoadDialogCoroutine(callback =>
        {
            Debug.Log(callback);
        }));
    }

    public IEnumerator ShowLoadDialogCoroutine(Action<string> callback)
    {
        // Wait for the user to select a file
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Select Files", "Load");

        if (FileBrowser.Success)
        {
            string filePath = FileBrowser.Result[0];
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(filePath);

            var newMetaData = new MetadataChange
            {
                ContentType = "image/jpg"
            };

            string fileName = Path.GetFileName(filePath);

            // Copy the selected file to the persistent data path
            string destinationPath = Path.Combine(Application.persistentDataPath, fileName);
            FileBrowserHelpers.CopyFile(filePath, destinationPath);

            StorageReference uploadReference = storageReference.Child("images/" + fileName);

            // Upload the file to Firebase Storage
            uploadReference.PutBytesAsync(bytes, newMetaData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError(task.Exception);
                }
                else
                {
                    callback?.Invoke(fileName);
                }
            });
        }
    }
}
