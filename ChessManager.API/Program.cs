using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Applications.Interfaces.Services;
using ChessManager.Applications.Services;
using ChessManager.Infrastructure.Mail;
using ChessManager.Infrastructure.Repository;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<SqlConnection>(_ => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddScoped<IMemberService, MemberService> ();

builder.Services.AddTransient<IMailService>(provider =>
{
    IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
    
    string noReplyName = configuration.GetValue<string>("Smtp:NoReply:Name")!;
    string noReplyEmail = configuration.GetValue<string>("Smtp:NoReply:From")!;
    string smtpHost = configuration.GetValue<string>("Smtp:Host")!;
    int smtpPort = configuration.GetValue<int>("Smtp:Port")!;
    
    return new MailService(noReplyName, noReplyEmail, smtpHost, smtpPort);
});

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