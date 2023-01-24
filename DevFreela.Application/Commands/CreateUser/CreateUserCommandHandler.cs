﻿using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DevFreela.Core.Services;

namespace DevFreela.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly DevFreelaDbContext _dbContext;
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(DevFreelaDbContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _authService.ComputeSha256Hash(request.Password);
            
            var user = new User(request.FullName, request.Email, request.BirthDate, passwordHash, request.Role);

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}