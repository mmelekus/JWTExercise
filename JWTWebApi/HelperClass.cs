using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace JWTWebApi;

public class HelperClass
{
    private readonly byte[] _secretKey;

    public HelperClass(ConfigurationManager configuration)
    {
        string secretKey = configuration["jwt:secretKey"]!; // Stored in user secrets; is a GUID
        byte[] utf8Array = Encoding.UTF8.GetBytes(secretKey);
        string base64String = Convert.ToBase64String(utf8Array);
        _secretKey = Convert.FromBase64String(base64String);
    }

    public string GenerateJwtToken(string secretKey, string issuer, string audience, int expireMinutes = 30)
    {
        SymmetricSecurityKey securityKey = new(_secretKey);
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "your_user_id"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("validator", "true")
            }),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string ReadJwtTokenString(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        // JwtSecurityToken tokenValue = tokenHandler.ReadJwtToken(token);
        IEnumerable<Claim>? claims = ValidateJwtToken(token)?.Claims;
        Dictionary<string, string> claimsDictionary = new();

        if (claims is null) { throw new ApplicationException($"Token failed validation: {token}"); }

        foreach(var claim in claims!)
        {
            string type = claim.Type.Contains("nameidentifier") ? "sub" : claim.Type;
            claimsDictionary.Add(type, claim.Value);
        }

        JsonSerializerOptions options = new() { WriteIndented = true };
        string jsonClaims = JsonSerializer.Serialize(claimsDictionary, options);

        return jsonClaims;
    }

    public ClaimsPrincipal? ValidateJwtToken(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateActor = true,
            IssuerSigningKey = new SymmetricSecurityKey(_secretKey)
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            throw;
        }
    }
}
