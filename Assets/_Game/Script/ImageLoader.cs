using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private RawImage image;

    private FirebaseStorage storage;
    private StorageReference storageReference;

    private void Start()
    {
        //storage = FirebaseStorage.DefaultInstance;
        //storageReference = storage.GetReferenceFromUrl("gs://showroom-73238.appspot.com/");

        //StorageReference image = storageReference.Child("studentmale.jpg");

        //image.GetDownloadUrlAsync().ContinueWith(task =>
        //{
        //    if (task.IsCompleted && !task.IsFaulted)
        //    {
        //        StartCoroutine(LoadImage(task.Result.ToString()));
        //    }
        //});
    }

    public void SetImage(string imageUrl)
    {
        StartCoroutine(LoadImage(imageUrl));
    }

    public IEnumerator LoadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error loading image: " + request.error);
        }
        else
        {
            image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
}
