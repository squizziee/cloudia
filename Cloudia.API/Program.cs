using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services;
using Cloudia.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IApplicationContext, ApplicationContext>();

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostAttachmentService, PostAttachmentService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //options.sa
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = "Cloudia Api Server",
            // будет ли валидироваться потребитель токена
            //ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = "any",
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("92nf8i3bfb383fb383dasdadfgbkgsduhfgquwkefqywgefiq3wy8ergfvqo8gbver87fg78fv1b238fg478f8")),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{

});

app.MapGet("/api/base", async (context) =>
{
	context.Response.StatusCode = 200;
	await context.Response.WriteAsync("Wassup");
});

// auth
app.Map("/auth/login", async (context) =>
{
    var form = context.Request.Form;
    var email = form["email"].ToString();
    var password = form["password"].ToString();

    using (var scope = app.Services.CreateScope())
    {
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

        // check if user exists
        var user = await userService.UserExists(email);
        if (user == null)
        {
            context.Response.StatusCode = 302;
            await context.Response.WriteAsync("No user with such email");
            return;
        }

        var valid = userService.AuthenticateUser(email, password);
        if (valid)
        {
            var token = jwtService.GenerateToken(user);
            await context.Response.WriteAsJsonAsync(token);
            return;
        }

        context.Response.StatusCode = 302;
        await context.Response.WriteAsync("Wrong password");

    }
});

app.Map("/auth/register", async (context) =>
{
    var form = context.Request.Form;
    var email = form["email"].ToString();
    var password = form["password"].ToString();
    var firstName = form["first_name"].ToString();
    var lastName = form["last_name"].ToString();

    using (var scope = app.Services.CreateScope())
    {
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

        // check if user exists
        var user = await userService.UserExists(email);
        if (user != null)
        {
            context.Response.StatusCode = 302;
            await context.Response.WriteAsync("User already exists");
            return;
        }

        var newUser = await userService.RegisterUser(email, password, firstName, lastName);

        await context.Response.WriteAsJsonAsync(newUser);
    }
});

// posts CRUD
app.MapGet("/api/posts", [Authorize(Roles = "1")] async (context) =>
{
	using (var scope = app.Services.CreateScope())
	{
		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
		var result = dbContext.Posts
			.FromSqlRaw($"SELECT * FROM posts")
			.ToList();
		context.Response.StatusCode = 200;
		await context.Response.WriteAsJsonAsync(result);
	}
});

app.MapGet("/api/posts/{id}", async (context) =>
{
	using var scope = app.Services.CreateScope();
	var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);
    var result = dbContext.Posts
		.FromSql($"SELECT * FROM posts WHERE id = {id}")
		.ToList();
	context.Response.StatusCode = 200;
	await context.Response.WriteAsJsonAsync(result);
});

app.MapPost("/api/posts", [Authorize] async (context) =>
{
    var userEmail = context.User.FindFirst("_email")?.Value;

    var text = context.Request.Form["text_content"];
    var attachments = context.Request.Form.Files;

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var postService = scope.ServiceProvider.GetRequiredService<IPostService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

	var newPost = await postService.AddPost(userExists.id, text!, attachments);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(newPost);
});

app.MapGet("/checkuser", [Authorize] async (context) =>
{
    var userId = context.User.FindFirst("_email")?.Value;

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(userId);
});

app.MapDelete("/api/posts/{id}", async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var postService = scope.ServiceProvider.GetRequiredService<IPostService>();

    postService.DeletePost(id);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Post deleted");
});


app.Run();
