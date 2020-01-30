using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FindFriends.Models;
using FindFriends.Context;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FindFriends.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public UsuariosController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        // CADASTRA AMIGO
        /// <summary>
        /// Api para cadastrar amigo
        /// </summary> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("add")]
        public async Task<ActionResult<Friend>> PostFriend(Friend friend)
        {
            if(CheckPositionEcxists(friend))
                return BadRequest("Usuário com a mesma localização");
            else
            {
                _context.FriendInfo.Add(friend);
                var resp = await _context.SaveChangesAsync();

                if (resp == 1) { return Ok(); }
                else { return BadRequest("ERRO"); }
            }
        }

        // CONSULTA POR AMIGO
        /// <summary>
        /// Api para buscar amigo específico
        /// </summary> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Friend>> GetFriend(int id)
        {
            var friend = await _context.FriendInfo.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }
            return friend;
        }


        // CONSULTA POR TODOS OS AMIGO
        /// <summary>
        /// Api para listar todos os amigos
        /// </summary> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("all")]
        public async Task<IEnumerable<Friend>> GetAllFriendsByUser([FromQuery] int userId)
        {
            return await _context.FriendInfo.Where(x => x.UserID == userId).ToListAsync();
        }


        // RETORNA OS LOGS DOS CÁLCULOS
        /// <summary>
        /// Api para pegar o log dos cálculos realizados na aplicação
        /// </summary> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("log")]
        public async Task<IEnumerable<CalculoHistoricoLog>> GetLog()
        {
            return await _context.CalculoHistoricoLog.ToListAsync();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IEnumerable<Friend>> GetAllFriends(int userId)
        {
            return await _context.FriendInfo.Where(x => x.UserID == userId).ToListAsync();
        }


        /// <summary>
        /// Api para salvar as informações do usuário
        /// </summary> 
        [HttpGet("users")]
        public User GetAllUsers([FromQuery] string email)
        {
            return _context.User.Single(e => e.Email.Equals(email));
        }

        // REALIZA OS CÁLCULOS DE DISTANCIA
        [ApiExplorerSettings(IgnoreApi = true)]
        public double CalculateDistance(double p1LA, double p1LO, double p2LA, double p2LO)
        {
            double r = 6371.0;

            p1LA = (p1LA * System.Math.PI) / 180.0;
            p1LO = (p1LO * System.Math.PI) / 180.0;
            p2LA = (p2LA * System.Math.PI) / 180.0;
            p2LO = (p2LO * System.Math.PI) / 180.0;

            double dLat = (p2LA - p1LA);
            double dLong = (p2LO - p1LO);

            double a = (System.Math.Sin(dLat / 2) * System.Math.Sin(dLat / 2)) + (System.Math.Cos(p1LA) * System.Math.Cos(p2LA) * System.Math.Sin(dLong / 2) * System.Math.Sin(dLong / 2));
            double c = 2 * (System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 + (a * -1))));


            double distanciaMT = System.Math.Round(r * c * 1000);

            return distanciaMT;
        }

        //CALCULA 3 AMIGOS PRÓXIMOS
        /// <summary>
        /// Api para listar os 3 amigos mais próximos a partir da localização informada
        /// </summary> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("closefriends")]
        public async Task<List<CalculoHistoricoLog>> getCloseThreFriends([FromQuery] int userId, [FromQuery] double LatitudeA, [FromQuery] double LongitudeA)
        {
            var b = new List<CalculoHistoricoLog>();
            int i = 1;
            
            foreach (var e in await GetAllFriends(userId))
            {
                double distance = CalculateDistance(LatitudeA, LongitudeA, e.LatitudeB, e.LongitudeB);
                if (distance <= 5000.0)
                {
                    if(i <= 3) { 
                        var Calculate = new CalculoHistoricoLog {FriendId = e.FriendId ,UserID = userId, Name = e.Name, LatitudeA = LatitudeA, LongitudeA = LongitudeA, LatitudeB = e.LatitudeB, LongitudeB = e.LongitudeB, Distance = distance };
                        _context.CalculoHistoricoLog.Add(Calculate);
                        _context.SaveChanges();
                    
                        b.Add(Calculate);

                        i++;
                    }
                }
                
            }
            return b;
        }

        // CHECA SE AMIGO EXISTE
        //[HttpGet("friendexists")]
        [ApiExplorerSettings(IgnoreApi = true)]
        private bool FriendExists(int id)
        {
            return _context.FriendInfo.Any(e => e.FriendId == id);
        }

        //CHECA SE LATITUDE/LONGITUDE JÁ EXISTEM
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool CheckPositionEcxists(Friend friend)
        {
            return _context.FriendInfo.Any(s => s.LatitudeB == friend.LatitudeB && s.LongitudeB == friend.LongitudeB && s.UserID == friend.UserID);
        }


        // ALTERA AMIGO
        /// <summary>
        /// Api para alterar informações de um amigo
        /// </summary> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriend(int id, Friend friend)
        {
            if (id != friend.FriendId)
            {
                return BadRequest();
            }

            if (CheckPositionEcxists(friend))
                return BadRequest("Usuário com a mesma localização já cadastrada!");
            else
            {
                _context.Entry(friend).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
        }


        // DELETE: api/usuarios/5
        /// <summary>
        /// Api para deletear um amigo
        /// </summary> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Friend>> DeleteFriend(int id)
        {
            var friend = await _context.FriendInfo.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }
            _context.FriendInfo.Remove(friend);
            await _context.SaveChangesAsync();
            return friend;
        }


        // CHECA SE USUÁRIO EXISTE
        [ApiExplorerSettings(IgnoreApi = true)]
        private bool UserExists(string email)
        {
            return _context.User.Any(e => e.Email == email);
        }

        // REFRESH TOKEN
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UserRefreshToken(string email, User users)
        {
            if (!UserExists(email))
            {
                _context.User.Add(users);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                var information = _context.User.Where(s => s.Email == email).First();

                information.Token = users.Token;
                information.Expiration = users.Expiration;

                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        //CRIA USUARIO
        /// <summary>
        /// Api para criar um usuário
        /// </summary> 
        [HttpPost("Criar")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return BuildToken(model);
            }
            else
            {
                return BadRequest("Usuário ou senha inválidos");
            }
        }


        // REALIZA LOGIN
        /// <summary>
        /// Api para fazer o login na aplicação
        /// </summary> 
        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password,
                 isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return BuildToken(userInfo);
            }
            else
            {
                ModelState.AddModelError("erro", "login inválido.");
                return BadRequest(ModelState);
            }
        }

        private UserToken BuildToken(UserInfo userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // tempo de expiração do token: 1 hora
            var expiration = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            var tokenAuth = new JwtSecurityTokenHandler().WriteToken(token);
            var refresh = new User { Email = userInfo.Email, Token = tokenAuth, Expiration = expiration };

            UserRefreshToken(userInfo.Email, refresh);

            var a = new UserToken()
            {
                Token = tokenAuth,
                Expiration = expiration
            };

            return a;

          
        }
    }
}
