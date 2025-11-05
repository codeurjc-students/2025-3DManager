using _3DMANAGER_APP.BLL.Models.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService
{
    private readonly string _key;
    private readonly string _issuer;

    public JwtService(IConfiguration config)
    {
        _key = config["Jwt:Key"]!;
        _issuer = config["Jwt:Issuer"]!;
    }

    public string GenerateToken(UserObject user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim("userEmail", user.UserEmail),
            new Claim("userId",user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrEmpty(user.RolId))
            claims.Add(new Claim(ClaimTypes.Role, user.RolId));

        if (user.GroupId.HasValue)
            claims.Add(new Claim("groupId", user.GroupId.Value.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
