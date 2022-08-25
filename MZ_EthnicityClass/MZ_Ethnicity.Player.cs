using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
public partial class Player 
{
    [Header("-=-=-=- MZ ethnicity -=-=-=-")]
    [SyncVar]
     public string ethnicity ="trainee";
    [SyncVar]
    [HideInInspector] public int grade = 0;
    
    public ethnicityClass[] characterClass ;

    [Serializable]
    public partial struct ethnicityClass
    {
        public string name;
        public int grade;
        [SerializeField]
        public ScriptableSkill[] skillTemplates;
    }

    [Command]
    public void Cmd_UpdateEthnicityClass(int index)
    {
        ethnicity = characterClass[index].name;
        grade = characterClass[index].grade;
        Database.singleton.SaveEthnicity(this);
        for (int i = 0; i < characterClass[grade].skillTemplates.Length; i++)
        {
            characterClass[grade].skillTemplates.CopyTo(skillTemplates, skillTemplates.Length);
        }
    }
}
