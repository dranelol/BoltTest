﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour
{

    public Animator initiallyOpen;
    private int m_OpenParameterId;
    private Animator m_Open;
    private GameObject m_PreviouslySelected;

    const string k_OpenTransitionName = "Open";
    const string k_ClosedStateName = "Closed";

    public GameObject GameWindow;
    public GameObject VideoWindow;
    public GameObject AudioWindow;

    public void OnEnable()
    {
        m_OpenParameterId = Animator.StringToHash(k_OpenTransitionName);

        if (initiallyOpen == null)
            return;

        OpenPanel(initiallyOpen);
    }
    void Update()
    {

    }
    public void OpenPanel(Animator anim)
    {
        if (m_Open == anim)
            return;
        anim.gameObject.SetActive(true);
        var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;

        anim.transform.SetAsLastSibling();

        CloseCurrent();

        m_PreviouslySelected = newPreviouslySelected;

        m_Open = anim;
        m_Open.SetBool(m_OpenParameterId, true);
    }

    public void OpenPanelWithoutClose(Animator anim)
    {
        if (m_Open == anim)
            return;
        anim.gameObject.SetActive(true);
        var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;

        anim.transform.SetAsLastSibling();

        m_PreviouslySelected = newPreviouslySelected;

        anim.SetBool(m_OpenParameterId, true);
    }
    public void CloseCurrent()
    {
        if (m_Open == null)
            return;
        m_Open.SetBool(m_OpenParameterId, false);
        StartCoroutine(DisablePanelDelayed(m_Open));
        m_Open = null;
    }
    public void CloseWindow(Animator anim)
    {
        if (!anim.gameObject.active)
        {
            return;
        }
        anim.SetBool(m_OpenParameterId, false);
        StartCoroutine(DisablePanelDelayed(anim));
    }

    public IEnumerator DisablePanelDelayed(Animator anim)
    {
        bool closedStateReached = false;
        bool wantToClose = true;
        while (!closedStateReached && wantToClose)
        {
            if (!anim.IsInTransition(0))
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

            wantToClose = !anim.GetBool(m_OpenParameterId);

            yield return new WaitForEndOfFrame();
        }
        if (wantToClose)
        {
            if (anim.gameObject.tag == "Settings")
            {
                if (GameWindow.active)
                {
                    GameWindow.SetActive(false);
                }
                else if (VideoWindow.active)
                {
                    VideoWindow.SetActive(false);
                }
                else if (AudioWindow.active)
                {
                    AudioWindow.SetActive(false);
                }
            }
            anim.gameObject.SetActive(false);
        }

    }

    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ASD(string test)
    {

    }
}