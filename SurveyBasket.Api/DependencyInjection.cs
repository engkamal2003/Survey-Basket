﻿namespace SurveyBasket.Api
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddDependecies(this IServiceCollection services)
		{
            services.AddControllers();
            services
                .AddSwaggerServices()
                .AddMapsterConf()
                .AddFluentValidationConf();
            services.AddScoped<IPollService, PollService>();
            return services;
		}

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        public static IServiceCollection AddMapsterConf(this IServiceCollection services)
        {
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));
            return services;
        }

        public static IServiceCollection AddFluentValidationConf(this IServiceCollection services)
        {
            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}

