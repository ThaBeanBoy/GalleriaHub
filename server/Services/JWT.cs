using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Galleria.Services;

public class JWTService
{
    private readonly IConfigurationSection JwtSettings;

    public JWTService(IConfigurationSection JwtSettings)
    {
        this.JwtSettings = JwtSettings;
    }

    public JwtSecurityToken GenerateToken(Models.User User)
    {
        List<Claim> Claims = new(){
            // new Claim(ClaimTypes.NameIdentifier, User.UserID.ToString())
            new Claim(ClaimTypes.NameIdentifier, User.UserID.ToString())
        };

        // ! The way I made the JwtKey is kinda sus, 
        var Jwtkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings["Key"]));
        var creds = new SigningCredentials(Jwtkey, SecurityAlgorithms.HmacSha256);
        
        // Creating the token
        return new JwtSecurityToken(
            issuer: JwtSettings["Issuer"],
            audience: JwtSettings["Audience"],
            claims: Claims,
            expires: DateTime.UtcNow.AddHours(5),
            signingCredentials: creds
        );
    }

    public ClaimsPrincipal? GetClaims(string jwtTokenString)
    {
        try
        {
            JwtSecurityTokenHandler TokenHandler = new();
            return TokenHandler.ValidateToken(
                jwtTokenString,
                new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings["key"])),
                    ValidateIssuer = false, // ! set to true if we want to validate the issuer
                    ValidateAudience = false // ! set to true if we want to validate the audience
                },
                out SecurityToken validatedToken
            );
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static string TokenString(JwtSecurityToken Token)
    {
        return new JwtSecurityTokenHandler().WriteToken(Token);
    }
}

public static class JWTServiceExtension
{
    public static IServiceCollection AddGalleriaJWT(this IServiceCollection serviceCollection, IConfigurationSection JwtSettings){
        return serviceCollection.AddSingleton(sp => {
            return new JWTService(JwtSettings);
        });
    }
}