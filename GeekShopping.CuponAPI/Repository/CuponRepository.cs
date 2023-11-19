using AutoMapper;
using GeekShopping.CuponAPI.Data.Dtos;
using GeekShopping.CuponAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CuponAPI.Repository;

public class CuponRepository : ICuponRepository
{
    private readonly MySQLContext _context;
    private readonly IMapper _mapper;
    public CuponRepository(MySQLContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<CuponDto> GetCupon(string code)
    {
        var cupon = await _context.Cupon.FirstOrDefaultAsync(c => c.CuponCode == code);
        return _mapper.Map<CuponDto>(cupon);
    }
}
