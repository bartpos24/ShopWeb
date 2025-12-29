using AutoMapper;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.TransferObjects.User;
using ShopWeb.Domain.Interfaces;

namespace ShopWeb.Application.Services
{
	public class LoginService : ILoginService
	{
		private readonly ILoginRepository loginRepository;
        private readonly IMapper mapper;
        public LoginService(ILoginRepository _loginRepository, IMapper _mapper)
		{
			loginRepository = _loginRepository;
            mapper = _mapper;
        }
		public async Task<string> Login(string username, string password, string? ssaid = null)
		{
			return await loginRepository.Login(username, password, ssaid);
		}

		public async Task<string> RefreshToken(string refreshToken, string? ssaid = null)
		{
			return await loginRepository.RefreshToken(refreshToken, ssaid);
		}

		public async Task Logout()
		{
			await loginRepository.Logout();
		}
		public async Task Register(RegisterModelVm registerModel)
		{
			var register = mapper.Map<Domain.Models.RegisterModel>(registerModel);
            await loginRepository.Register(register);
        }

    }
}
