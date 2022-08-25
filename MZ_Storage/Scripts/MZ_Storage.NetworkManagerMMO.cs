using Mirror;

public partial class NetworkManagerMMO
{
    // -----------------------------------------------------------------------------------
    // OnServerDisconnect
    // @Server
    // -----------------------------------------------------------------------------------
    [DevExtMethods("OnServerDiconnect")]
    private void OnServerDisconnect_UCE_GuildUCE_warehouse(NetworkConnection conn)
    {
        if (conn.identity != null)
            Database.singleton.MZ_SaveStorage(conn.identity.GetComponent<Player>());
    }
}
