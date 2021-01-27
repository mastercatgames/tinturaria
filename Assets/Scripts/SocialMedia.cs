using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialMedia : MonoBehaviour
{
    public void OpenInstagram()
    {
        Application.OpenURL("https://www.instagram.com/mastercatgamestudio/");
    }

    public void OpenTwitter()
    {
        Application.OpenURL("https://twitter.com/mastercatgames/");
    }

    public void OpenFacebook()
    {
        Application.OpenURL("https://www.facebook.com/mastercatgamestudio/");
    }

    public void OpenCustomURL(string url)
    {
        Application.OpenURL(url);
    }
}
