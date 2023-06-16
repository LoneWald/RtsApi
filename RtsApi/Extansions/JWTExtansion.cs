using Microsoft.IdentityModel.Tokens;
using RtsApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RtsApi.Extensions;

public static class JWTExtansion
{
    readonly static byte[] key = Encoding.ASCII.GetBytes("IWantToBeHappyButIAmIdiot");
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(key);
    public static IEnumerable<Claim> ValidateToken(string token)
    {
        try
        {
            if (token.IsNullOrEmpty()) throw new Exception();

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims;
        }
        catch
        {
            return Array.Empty<Claim>();
        }
    }
    public static string GetToken(IEnumerable<Claim> claims)
    {
        return GetToken(claims, JWTConfig.Default);
    }
    public static string GetToken(IEnumerable<Claim> claims, 
                                    JWTConfig jwtConfig)
    {
        var jwt = new JwtSecurityToken(
            audience: jwtConfig.Audience,
            issuer: jwtConfig.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(jwtConfig.LifeTime)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), 
                                                        SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}