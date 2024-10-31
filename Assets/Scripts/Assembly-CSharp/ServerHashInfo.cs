public class ServerHashInfo
{
	public int cardID = -1;

	public string hashpng = string.Empty;

	public string hashparam = string.Empty;

	public string userId = string.Empty;

	public ServerHashInfo(int _cardID, string _hashpng, string _hashparam, string _userId)
	{
		cardID = _cardID;
		hashpng = _hashpng;
		hashparam = _hashparam;
		userId = _userId;
	}
}
