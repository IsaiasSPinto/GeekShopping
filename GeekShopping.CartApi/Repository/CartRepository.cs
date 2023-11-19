using AutoMapper;
using GeekShopping.CartApi.Data.Dtos;
using GeekShopping.CartApi.Model;
using GeekShopping.CartApi.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartApi.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly MySQLContext _context;
        private readonly IMapper _mapper;
        public CartRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ApplyCupon(string userId, string couponCode)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);

            if (cartHeader != null)
            {
                cartHeader.CuponCode = couponCode;
                _context.CartHeaders.Update(cartHeader);

                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);

            if (cartHeader != null)
            {
                _context.CartDatails.RemoveRange(_context.CartDatails.Where(x => x.CartHeaderId == cartHeader.Id));
                _context.CartHeaders.Remove(cartHeader);

                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<CartDto> FindCartByUserId(string userId)
        {
            var cart = new Cart
            {
                CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId),
            };

            cart.CartDatails = await _context.CartDatails.Where(x => x.CartHeaderId == cart.CartHeader.Id).Include(x => x.Product).ToListAsync();

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveCupon(string userId)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);

            if (cartHeader != null)
            {
                cartHeader.CuponCode = "";
                _context.CartHeaders.Update(cartHeader);

                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> RemoveProductFromCart(long cartDetailsId)
        {
            try
            {
                CartDatail cartDatail = await _context.CartDatails.FirstOrDefaultAsync(x => x.Id == cartDetailsId);

                int total = _context.CartDatails.Where(x => x.CartHeaderId == cartDatail.CartHeaderId).Count();

                _context.CartDatails.Remove(cartDatail);

                if (total == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(x => x.Id == cartDatail.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<CartDto> SaveOurUpdateCart(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == cart.CartDatails.FirstOrDefault().ProductId);

            if (product == null)
            {
                _context.Products.Add(cart.CartDatails.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }

            var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cart.CartHeader.UserId);

            if (cartHeader == null)
            {
                _context.CartHeaders.Add(cart.CartHeader);
                await _context.SaveChangesAsync();
                cart.CartDatails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
                cart.CartDatails.FirstOrDefault().Product = null;

                _context.CartDatails.Add(cart.CartDatails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                var cartDetail = await _context.CartDatails.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CartHeaderId == cartHeader.Id && x.ProductId == cart.CartDatails.FirstOrDefault().ProductId);

                if (cartDetail == null)
                {
                    cart.CartDatails.FirstOrDefault().CartHeaderId = cartHeader.Id;
                    cart.CartDatails.FirstOrDefault().Product = null;

                    _context.CartDatails.Add(cart.CartDatails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cart.CartDatails.FirstOrDefault().Product = null;
                    cart.CartDatails.FirstOrDefault().Count += cartDetail.Count;
                    cart.CartDatails.FirstOrDefault().Id = cartDetail.Id;
                    cart.CartDatails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;

                    _context.CartDatails.Update(cartDetail);
                    await _context.SaveChangesAsync();
                }
            }


            return _mapper.Map<CartDto>(cart);

        }
    }
}
