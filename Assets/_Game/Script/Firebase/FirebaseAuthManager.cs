using Firebase.Auth;
using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuthManager Instance;

    private FirebaseAuth auth;
    private FirebaseUser user;

    public FirebaseUser User => user;
    public event Action OnLoginSuccess;
    public event Action OnRegisterSuccess;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Login(string email, string password)
    {
        StartCoroutine(LoginAsync(email, password));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        //check for exception
        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;


            string failedMessage = "Login Failed! Because ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }

            Debug.Log(failedMessage);
        }
        else
        {
            //get the user info
            user = loginTask.Result.User;

            Debug.LogFormat("Successfully Logged In as {0}", user.DisplayName);

            OnLoginSuccess?.Invoke();
        }
    }

    public void Register(string username, string email, string password, string role)
    {
        StartCoroutine(RegisterAsync(username, email, password, role));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string role)
    {
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        //check for exception
        if (registerTask.Exception != null)
        {
            Debug.LogError(registerTask.Exception);

            FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "";
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Registration Failed";
                    break;
            }

            Debug.Log(failedMessage);
        }
        else
        {
            //get the user info
            user = registerTask.Result.User;

            UserProfile userProfile = new UserProfile { DisplayName = name };

            var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

            yield return new WaitUntil(() => updateProfileTask.IsCompleted);

            //check for exception (user profile)
            if (updateProfileTask.Exception != null)
            {
                // Delete the user if user update failed
                user.DeleteAsync();

                Debug.LogError(updateProfileTask.Exception);

                FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;


                string failedMessage = "Profile update Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Profile update Failed";
                        break;
                }

                Debug.Log(failedMessage);
            }
            else
            {
                string selectedRole = role; // Get selected role
                SaveUserRole(user.UserId, selectedRole);
                Debug.Log("Registration Sucessful. Welcome " + user.DisplayName);
                OnRegisterSuccess?.Invoke();
            }
        }
        
    }

    private void SaveUserRole(string userId, string role)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        // Create a dictionary to store only role data
        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "Role", role }
        };

        // Save user role to Firestore
        db.Collection("users").Document(userId).SetAsync(userData);
    }

    public void GetUserRole(Action<string> onRoleFetched)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        db.Collection("users").Document(user.UserId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string role = snapshot.GetValue<string>("Role");
                    onRoleFetched?.Invoke(role);
                }
                else
                {
                    onRoleFetched?.Invoke(null);
                }
            }
            else
            {
                onRoleFetched?.Invoke(null);
            }
        });
    }

    public void LogOut()
    {
        if (auth != null && user != null)
        {
            auth.SignOut();
        }
    }
}
