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
            if (!ValidateClientId(userName) || !ValideSecrectKey(password))
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

        public bool ValideSecrectKey(string Key)
        {            
            try
            {
                if (string.IsNullOrEmpty(ConstantValues.SecrectKey))
                {
                    using (DataAccess da = new DataAccess())
                    {
                        System.Data.DataTable dt = da.GetTerminalKey(Key);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string TerminalId = dt.Rows[0]["TerminalID"].ToString().ToUpper();
                            string strPhrase = dt.Rows[0]["Phrase"].ToString();
                            KEXIN.EncryptionUtility encryptor = new KEXIN.EncryptionUtility();
                            encryptor.passPhrase = TerminalId.Substring(0, 1) +
                                                   TerminalId.Substring(9, 1) +
                                                   TerminalId.Substring(14, 1) +
                                                   TerminalId.Substring(19, 1) +
                                                   TerminalId.Substring(24, 1);
                            if (strPhrase == encryptor.Encrypt(Key))
                                ConstantValues.SecrectKey = Key;
                        }//End of Each
                    }//End of Using
                }//End of if checking
            }//End of Try
            catch
            {
                ConstantValues.SecrectKey = null;
            }

            if (ConstantValues.SecrectKey == Key)
                return true;
            else
                return false;
        }

        public bool ValidateClientId(string ClientId)
        {
            try
            {
                if (string.IsNullOrEmpty(ConstantValues.ClientId))
                {
                    //string TerminalId =   cn.Singsoft.FingerPrint.Value();
                    string TerminalId = System.Configuration.ConfigurationManager.AppSettings["PrivateKey"];

                    KEXIN.EncryptionUtility encryptor = new KEXIN.EncryptionUtility();
                    TerminalId = encryptor.Decrypt(TerminalId);

                    encryptor.passPhrase = TerminalId.Substring(0, 1) +
                                            TerminalId.Substring(9, 1) +
                                            TerminalId.Substring(14, 1) +
                                            TerminalId.Substring(19, 1) +
                                            TerminalId.Substring(24, 1);

                    ConstantValues.ClientId = encryptor.Decrypt(System.Configuration.ConfigurationManager.AppSettings["ClientId"]);
                }//End of if checking
            }//End of Try
            catch (System.Exception ex)
            {
                ConstantValues.ClientId = null;
            }

            if (ConstantValues.ClientId == ClientId)
                return true;
            else
                return false;
        }
    }

}