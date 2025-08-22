
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using orders.application.CommandHandlers;
using orders.application.QueryHandlers;
using orders.domain.Interfaces;
using orders.infrastructure.Data;
using orders.shared;

namespace orders.webapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddShared(builder.Configuration); //Reading


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddAuthentication().AddJwtBearer(op =>
            {
                op.Authority = "https://localhost:5001";
                op.Audience = "order-service";
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "https://localhost:5001",
                    ValidAudience = "order-service"
                };
            });
            builder.Services.AddAuthorization(op =>
            {
                op.AddPolicy("policy-read-order", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "scope-read-order-service");
                });

                op.AddPolicy("policy-write-order", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "scope-write-order-service");
                });

                op.AddPolicy("policy-delete-order", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "scope-delete-order-service");
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlServer(connectionString,
                sqlOptions => sqlOptions.MigrationsAssembly("orders.database"))
            );

            // Register repositories
            builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            // Register handlers
            builder.Services.AddScoped<CreateOrderCommandHandler>();
            builder.Services.AddScoped<GetOrderQueryHandler>();
            builder.Services.AddScoped<UpdateOrderStatusCommandHandler>();
            builder.Services.AddScoped<GetUserOrdersQueryHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
