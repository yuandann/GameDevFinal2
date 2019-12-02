using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private bool activated;
    private int combos;
    private float max_combo_end_time = 120;
    private float combo_end_time;

    // Start is called before the first frame update
    void Start()
    {
        //initializeComboTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            combo_end_time--;
            if (combo_end_time <= 0)
            {
                endCombo();
            }
        }
    }

    private void initializeComboTime()
    {
        combo_end_time = 0 + max_combo_end_time;
    }

    private void endCombo()
    {
        activated = false;
        combos = 0;
        initializeComboTime();
    }

    public void add_combo()
    {
        initializeComboTime();
        activated = true;
        combos++;
    }
}
