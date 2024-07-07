using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsPanelManager : Book<ButtonsPanel>
{

    public void setActive(string key)
    {
        int activeIndex = pages.FindIndex(it => it.getKey() == key);
        for (int i = 0; i < pages.Count; i++)
        {
            if (i == activeIndex) pages[i].setActive();
            else pages[i].setInactive();
        }
    }

    protected override void activate(ButtonsPanel item)
    {
        item.setActive();
    }

    protected override void deactivate(ButtonsPanel item)
    {
        item.setInactive();
    }
}
