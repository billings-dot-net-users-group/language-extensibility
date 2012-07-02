using WebServices.AsyncHcpcsService;

namespace WebServices
{
	public static class WebServiceConnections
	{
		public static IHCPCSService AsyncClient = new HCPCSServiceClient();
		public static BeginEndHcpcsService.IHCPCSService BeginEndClient = new BeginEndHcpcsService.HCPCSServiceClient();
	}
}