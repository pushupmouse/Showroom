using Firebase.Storage;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Unity.Profiling;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;

    private FirebaseStorage storage;
    private StorageReference storageReference;

    private void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://firestoreshowroom.appspot.com/images/");
    }

    public IEnumerator SetImage(string imageName)
    {
        StorageReference image = storageReference.Child(imageName);

        var task = image.GetDownloadUrlAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted || task.IsCanceled)
        {
            yield break;
        }

        yield return StartCoroutine(LoadImage(task.Result.ToString()));
    }

    public IEnumerator LoadImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error loading image: " + request.error);
        }
        else
        {
            rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
}
