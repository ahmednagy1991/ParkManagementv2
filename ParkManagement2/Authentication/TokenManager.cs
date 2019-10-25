using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Configuration;

namespace ParkManagement2.Authentication
{
    public class TokenManager
    {
        private static string Secret = WebConfigurationManager.AppSettings["JWT_SECRET"];

        public static HttpCookie CreateCookie(string Token)
        {
            HttpCookie Cookies = new HttpCookie("ParkManagement_token");
            Cookies.Value = Token;
            Cookies.Expires = DateTime.Now.AddMinutes(int.Parse(WebConfigurationManager.AppSettings["JWT_EXP_MIN"]));
            return Cookies;
        }

        public static HttpCookie Logout()
        {
            HttpCookie Cookies = new HttpCookie("ParkManagement_token");
            Cookies.Value = "";
            Cookies.Expires = DateTime.Now;
            return Cookies;
        }



        public static string GenerateToken(string username, string Id)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.NameIdentifier, Id)}),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(WebConfigurationManager.AppSettings["JWT_EXP_MIN"])),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }


        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }



        public static string ValidateToken(string token)
        {
            string username = null;
            string user_id = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;

            Claim userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            user_id = userIdClaim.Value;


            return user_id;
        }


    }
}