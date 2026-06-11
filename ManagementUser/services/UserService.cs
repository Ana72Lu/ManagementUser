using ManagementUser.Data;
using ManagementUser.DTOs;
using ManagementUser.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementUser.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    // Método para criar usuário
    public async Task<UserDto.UserResponse> CreateUser(UserDto.UserCreateRequest dto)
    {
        // Validações adicionais (ex: email único)
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("Email já cadastrado!");

        // Mapear DTO para a entidade User
        var user = new User
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha), // Hash da senha!
            DataCriacao = DateTime.UtcNow
        };

        // Salvar no banco
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Retornar DTO de resposta
        return new UserDto.UserResponse
        {
            Id = user.Id,
            Nome = user.Nome,
            Email = user.Email,
            DataCriacao = user.DataCriacao
        };
    }

    public async Task<UserDto.UserResponse?> GetUserById(int id)
    {
        // Busca o usuário no banco de dados pelo Id (incluindo o Perfil se existir)
        var user = await _context.Users
            .Include(u => u.Perfil) // Carrega o relacionamento com Perfil (se necessário)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return null; // Retorna null se o usuário não existir

        // Mapeia a entidade User para UserDto.Response
        return new UserDto.UserResponse
        {
            Id = user.Id,
            Nome = user.Nome,
            Email = user.Email,
            DataCriacao = user.DataCriacao,
            PerfilId = user.PerfilId // Opcional: inclua o ID do perfil se necessário
        };
    }

    // Outros métodos (Update, Delete...)
}