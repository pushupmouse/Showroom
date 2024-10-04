using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthView : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailLoginField;
    [SerializeField] private TMP_InputField passwordLoginField;
    [SerializeField] private TMP_InputField usernameRegisterField;
    [SerializeField] private TMP_InputField emailRegisterField;
    [SerializeField] private TMP_InputField passwordRegisterField;
    [SerializeField] private TMP_InputField confirmPasswordRegisterField;
    [SerializeField] private TMP_Dropdown roleDropdown;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button gotoLoginButton;
    [SerializeField] private Button gotoRegisterButton;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;

    private void Start()
    {
        loginButton.onClick.AddListener(Login);
        registerButton.onClick.AddListener(Register);
        gotoLoginButton.onClick.AddListener(ShowLogin);
        gotoRegisterButton.onClick.AddListener(ShowRegister);

        FirebaseAuthManager.Instance.OnLoginSuccess += HandleLoginSuccess;
        FirebaseAuthManager.Instance.OnRegisterSuccess += HandleRegisterSuccess;
    }

    private void HandleLoginSuccess()
    {
        Destroy(gameObject);
    }

    private void HandleRegisterSuccess()
    {
        ShowLogin();
    }

    public void ShowLogin()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    public void ShowRegister()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    private void Login()
    {
        if (emailLoginField.text == "")
        {
            Debug.Log("Email field is empty");
        }
        else if (passwordLoginField.text == "")
        {
            Debug.Log("Password is empty");
        }
        else
        {
            FirebaseAuthManager.Instance.Login(emailLoginField.text, passwordLoginField.text);
        }
    }

    private void Register()
    {
        if (usernameRegisterField.text == "")
        {
            Debug.Log("Username is empty");
        }
        else if (emailRegisterField.text == "")
        {
            Debug.Log("Email field is empty");
        }
        else if (passwordRegisterField.text != confirmPasswordRegisterField.text)
        {
            Debug.Log("Password does not match");
        }
        else
        {
            FirebaseAuthManager.Instance.Register(usernameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, roleDropdown.options[roleDropdown.value].text);

        }
    }
}
