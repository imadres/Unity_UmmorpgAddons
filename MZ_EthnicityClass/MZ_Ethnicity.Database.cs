using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
public partial class Database
{
    private class CharacterEthnicity
    {
        [PrimaryKey]
        public string CharacterName { get; set; }
        public string Ethnicity { get; set; }
        public string className { get; set; }
        public int class_classification { get; set; }
    }

    [DevExtMethods("Connect")]
    private void Connect_Ethnicity()
    {
        connection.CreateTable<CharacterEthnicity>();
    }
    [DevExtMethods("CharacterLoad")]
    public void LoadEthnicity(Player player)
    {
        var loadedPoly = connection.FindWithQuery<CharacterEthnicity>(
       "SELECT * FROM CharacterEthnicity WHERE character=?", player.name);

        if (loadedPoly != null)
        {
            player.grade = loadedPoly.class_classification;
            player.ethnicity = loadedPoly.className;

        }
    }
    [DevExtMethods("CharacterSave")]
    public void SaveEthnicity(Player player)
    {
        connection.Execute("DELETE FROM CharacterEthnicity WHERE character =?", player.name);
        connection.InsertOrReplace(new CharacterEthnicity
        {
            CharacterName = player.name,
            Ethnicity = player.className,
            className = player.ethnicity,
            class_classification = player.grade
        });
    }
}
