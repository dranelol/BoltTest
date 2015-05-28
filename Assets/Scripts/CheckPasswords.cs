﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class CheckPasswords : MonoBehaviour 
{
    private string passwordOne;
    private string passwordTwo;

    public UnityEvent OnPasswordMatch;
    public UnityEvent OnPasswordMismatch;

	public void SetPasswordOne(InputField password)
    {
        passwordOne = password.text;
    }

    public void SetPasswordTwo(InputField password)
    {
        passwordTwo = password.text;
    }

    public void CheckPassword()
    {
        if(passwordOne == passwordTwo)
        {
            OnPasswordMatch.Invoke();
        }

        else
        {
            OnPasswordMismatch.Invoke();
        }
    }
}
