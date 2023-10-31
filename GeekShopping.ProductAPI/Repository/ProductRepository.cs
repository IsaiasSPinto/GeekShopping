using AutoMapper;
using GeekShopping.ProductAPI.Data.Dtos;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repository;

public class ProductRepository : IProductRepository
{
    private readonly MySQLContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(IMapper mapper, MySQLContext context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> FindAll()
    {
        var products = await _context.Products.ToListAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto> FindById(long id)
    {
        var product = await _context.Products.FindAsync(id);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> Create(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> Update(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}