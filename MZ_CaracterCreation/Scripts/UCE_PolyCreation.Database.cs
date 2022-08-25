using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if _MYSQL
using MySql.Data;								// From MySql.Data.dll in Plugins folder
using MySql.Data.MySqlClient;                   // From MySql.Data.dll in Plugins folder
#elif _SQLITE
using SQLite;
#endif


public partial class Database
{
    // Start is called before the first frame update

    private class uce_poly
    {
        [PrimaryKey]
        public string character { get; set; }
        public string gender { get; set; }
        public int hair { get; set; }
        public int face { get; set; }
        public string HairColor { get; set; }
        public string SkinColor { get; set; }
    }
    [DevExtMethods("Connect")]
    private void Connect_PolyIntegration()
    {
       
#if _MYSQL
         ExecuteNonQueryMySql(@"CREATE TABLE IF NOT EXISTS uce_poly (`character` VARCHAR(32) NOT NULL, gender VARCHAR(1000) NOT NULL,hair INT(4),face INT(4)) CHARACTER SET=utf8mb4");
#elif _SQLITE
      connection.CreateTable<uce_poly>();
#endif
    }
    [DevExtMethods("CharacterLoad")]
    public void LoadPoly(Player player)
    {
#if _MYSQL
        var table = ExecuteReaderMySql("SELECT gender, hair, face FROM uce_poly WHERE `character`=@character", new MySqlParameter("@character", player.name));
        if (table.Count == 1)
        {
            List<object> mainrow = table[0];
            string tempdna = (string)mainrow[0];
            player.gender = tempdna;
        player.face = (int)mainrow[2];
        player.hair = (int)mainrow[1];
        }
#elif _SQLITE
        var loadedPoly = connection.FindWithQuery<uce_poly>(
         "SELECT * FROM uce_poly WHERE character=?", player.name);
       
        if (loadedPoly !=null)
        {
            Debug.Log("poly load success");
            player.gender = loadedPoly.gender;
            player.face = loadedPoly.face;
            player.hair = loadedPoly.hair;
            player.HairColor = loadedPoly.HairColor;
            player.SkinColor = loadedPoly.SkinColor;
        }else
        {
            Debug.Log("poly load faild");
        }
#endif
    }

    [DevExtMethods("CharacterSave")]
    private void CharacterSave_UmaIntegration(Player player)
    {
        savePoly(player);
    }

    
    public void savePoly(Player player)
    {
#if _MYSQL
        ExecuteNonQueryMySql("DELETE FROM uce_uma WHERE `character`=@character", new MySqlParameter("@character", player.name));
        ExecuteNonQueryMySql("INSERT INTO uce_uma VALUES (@character, @gender,@hair,@face)",
         	new MySqlParameter("@character", player.name),
         	new MySqlParameter("@gender", player.gender),
            new MySqlParameter("@hair", player.hair),
            new MySqlParameter("@face", player.face));
#elif _SQLITE
        connection.Execute("DELETE FROM uce_poly WHERE character =?", player.name);
        connection.InsertOrReplace(new uce_poly
        {
            character = player.name,
            gender = player.gender,
            hair = player.hair,
            face = player.face,
            HairColor=player.HairColor,
            SkinColor=player.SkinColor
        });
#endif
    }
}
