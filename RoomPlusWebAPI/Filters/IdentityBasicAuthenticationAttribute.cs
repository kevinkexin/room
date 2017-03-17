using RoomPlustWebAPI.App_Code;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using RoomPlusWebAPI.App_Code;

namespace WebApiBasicAuth.Filters
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //User Name is ClientID
            //Password is secretKey
            if (!ValideDevice(userName, password))
            {
                // No user with userName/password exists.
                return null;
            }
            
            // Create a ClaimsIdentity with all the claims for this user.

            Claim nameClaim = new Claim(ClaimTypes.Name, userName);

            List<Claim> claims = new List<Claim> { nameClaim };
            // important to set the identity this way, otherwise IsAuthenticated will be false

        
            ClaimsIdentity identity = new ClaimsIdentity(claims, AuthenticationTypes.Basic);

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

		/// <summary>
		/// Valides the device identifier.
		/// </summary>
		/// <returns><c>true</c>, if device identifier was valided, <c>false</c> otherwise.</returns>
		/// <param name="DeviceId">Device identifier.</param>
		/// <param name="SecurityKey">Security key.</param>
		public bool ValideDevice(string DeviceId, string SecurityKey)
		{
			bool blnValid = false;
			try
			{
				using (DataAccess da = new DataAccess())
				{
					System.Data.DataTable dt = da.GetTerminalKey(DeviceId, SecurityKey);

					if (dt != null && dt.Rows.Count > 0)
						blnValid = true;
				}//End of Using
			}//End of Try
			catch
			{

			}

			return blnValid;
		}
    }
}