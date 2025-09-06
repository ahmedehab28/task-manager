using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Authorization
{
    public class BoardAuthorizationService : IBoardAuthorizationService
    {
        private readonly IApplicationDbContext _context;
        public BoardAuthorizationService(
            IApplicationDbContext context)
        {
            _context = context;
        }
    }
}
