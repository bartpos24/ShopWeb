using ShopWeb.Application.Interfaces;

namespace ShopWeb.Services
{
	public class TokenMonitorBackgroundService : BackgroundService
	{
		private readonly IServiceProvider serviceProvider;
		private readonly ILogger<TokenMonitorBackgroundService> logger;
		private readonly TimeSpan checkInterval = TimeSpan.FromMinutes(1); // Check every minute

		public TokenMonitorBackgroundService(IServiceProvider _serviceProvider, ILogger<TokenMonitorBackgroundService> _logger)
		{
			serviceProvider = _serviceProvider;
			logger = _logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			logger.LogInformation("Token Monitor Background Service is starting.");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(checkInterval, stoppingToken);

					// Create a new scope for each check
					using var scope = serviceProvider.CreateScope();
					var tokenRefreshService = scope.ServiceProvider.GetRequiredService<ITokenRefreshService>();

					await tokenRefreshService.RefreshTokenIfNeededAsync();
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "Error occurred in Token Monitor Background Service");
				}
			}

			logger.LogInformation("Token Monitor Background Service is stopping.");
		}
	}
}
