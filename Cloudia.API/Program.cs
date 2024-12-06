using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services;
using Cloudia.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using Swashbuckle.AspNetCore;

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
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IViolationService, ViolationService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IModeratorService, ModeratorService>();
builder.Services.AddScoped<ISearchService, SearchService>();

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
// <----------------------->
app.MapGet("/api/posts/user/{id}", async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var postService = scope.ServiceProvider.GetRequiredService<IPostService>();

    var result = await postService.GetUserPosts(id);

    var srlzd = JsonConvert.SerializeObject(result);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(srlzd);
});

app.MapGet("/api/posts/{id}", async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var postService = scope.ServiceProvider.GetRequiredService<IPostService>();

    var result = await postService.GetFullPost(id);

    var srlzd = JsonConvert.SerializeObject(result);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(srlzd);
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

app.MapPut("/api/posts/{id}", [Authorize] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);
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

    var result = await postService.UpdatePost(id, text!, attachments);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapDelete("/api/posts/{id}", [Authorize] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var postService = scope.ServiceProvider.GetRequiredService<IPostService>();

    postService.DeletePost(id);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Post deleted");
});
// <----------------------->

// likes
// <----------------------->

app.MapPost("/api/likes", [Authorize] async (context) =>
{
    var userEmail = context.User.FindFirst("_email")?.Value;

    var postId = int.Parse(context.Request.Form["post_id"]!);
    var attachments = context.Request.Form.Files;

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var postService = scope.ServiceProvider.GetRequiredService<ILikeService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

    var newPost = await postService.AddLike(userExists.id, postId);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(newPost);
});

app.MapDelete("/api/likes", [Authorize] async (context) =>
{
    var userEmail = context.User.FindFirst("_email")?.Value;

    var postId = int.Parse(context.Request.Form["post_id"]!);
    var attachments = context.Request.Form.Files;

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var postService = scope.ServiceProvider.GetRequiredService<ILikeService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

    var newPost = await postService.RemoveLike(userExists.id, postId);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(newPost);
});

// <----------------------->

// comments
// <----------------------->

app.MapGet("/api/comments/{id}", [Authorize] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var commentService = scope.ServiceProvider.GetRequiredService<ICommentService>();

    var result = await commentService.GetComment(id);
    if (result == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such comment");
        return;
    }

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapPost("/api/comments", [Authorize] async (context) =>
{
    var userEmail = context.User.FindFirst("_email")?.Value;

    var text = context.Request.Form["text_content"];
    var postId = int.Parse(context.Request.Form["post_id"]!);

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var commentService = scope.ServiceProvider.GetRequiredService<ICommentService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

    var newComment = await commentService.AddComment(userExists.id, postId, text!);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(newComment);
});

app.MapPut("/api/comments/{id}", [Authorize] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    var text = context.Request.Form["text_content"];

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var commentService = scope.ServiceProvider.GetRequiredService<ICommentService>();

    var newComment = await commentService.UpdateComment(id, text!);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(newComment);
});

app.MapDelete("/api/comments/{id}", [Authorize] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var commentService = scope.ServiceProvider.GetRequiredService<ICommentService>();

    await commentService.DeleteComment(id);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Comment deleted");
});

// <----------------------->

// reports
// <----------------------->

app.MapGet("/api/reports/{id}", [Authorize] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();

    var result = await reportService.GetReport(id);
    if (result == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such report");
        return;
    }

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapPost("/api/reports", [Authorize] async (context) =>
{
    var userEmail = context.User.FindFirst("_email")?.Value;
    var postId = int.Parse(context.Request.Form["post_id"]!);
    var violationId = int.Parse(context.Request.Form["violation_id"]!);

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

    var newReport = await reportService.AddReport(userExists.id, postId, violationId);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(newReport);
});

// <----------------------->

// profiles
// <----------------------->

app.MapGet("/api/profiles/subscribers", [Authorize] async (context) =>
{
    var userEmail = context.User.FindFirst("_email")?.Value;

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var profileService = scope.ServiceProvider.GetRequiredService<IUserProfileService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

    var result = await profileService.GetSubscribers(userExists.id);

    var srlzd = JsonConvert.SerializeObject(result);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapGet("/api/profiles/subscribers/{id}", async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var profileService = scope.ServiceProvider.GetRequiredService<IUserProfileService>();


    var result = await profileService.GetSubscribers(id);

    var srlzd = JsonConvert.SerializeObject(result);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapGet("/api/profiles/subscriptions", [Authorize] async (context) =>
{
    var userEmail = context.User.FindFirst("_email")?.Value;

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var profileService = scope.ServiceProvider.GetRequiredService<IUserProfileService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

    var result = await profileService.GetSubscriptions(userExists.id);

    var srlzd = JsonConvert.SerializeObject(result);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapGet("/api/profiles/subscriptions/{id}", async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var profileService = scope.ServiceProvider.GetRequiredService<IUserProfileService>();


    var result = await profileService.GetSubscriptions(id);

    var srlzd = JsonConvert.SerializeObject(result);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapGet("/api/profiles/feed", [Authorize] async (context) =>
{
    var userEmail = context.User.FindFirst("_email")?.Value;

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var profileService = scope.ServiceProvider.GetRequiredService<IUserProfileService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

    var result = await profileService.GetFeed(userExists.id);

    var srlzd = JsonConvert.SerializeObject(result);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(srlzd);
});

app.MapPost("/api/profiles/subscribe/{id}", [Authorize] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);
    var userEmail = context.User.FindFirst("_email")?.Value;

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var profileService = scope.ServiceProvider.GetRequiredService<IUserProfileService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

    await profileService.SubscribeTo(userExists.id, id);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Subscribed");
});

app.MapDelete ("/api/profiles/subscribe/{id}", [Authorize] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);
    var userEmail = context.User.FindFirst("_email")?.Value;

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var profileService = scope.ServiceProvider.GetRequiredService<IUserProfileService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

    await profileService.UnsubscribeFrom(userExists.id, id);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Unsubscribed");
});

app.MapPut("/api/profiles", [Authorize] async (context) =>
{
    var userEmail = context.User.FindFirst("_email")?.Value;

    var firstName = context.Request.Form["first_name"];
    var lastName = context.Request.Form["last_name"];
    var avatar = context.Request.Form.Files["avatar"];
    var location = context.Request.Form["location"];
    var biography = context.Request.Form["biography"];
    int? age = (context.Request.Form["age"].IsNullOrEmpty()) ? null : int.Parse(context.Request.Form["age"]!);

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var profileService = scope.ServiceProvider.GetRequiredService<IUserProfileService>();

    var userExists = await userService.UserExists(userEmail!);
    if (userExists == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such user");
        return;
    }

    await profileService.UpdateUserProfile(userExists.id, firstName!, lastName!, avatar, location, biography, age);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Updated");
});

// <----------------------->

// admin
// <----------------------->

app.MapPut("/api/admin/ban/{id}", [Authorize(Roles = "3")] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var profileService = scope.ServiceProvider.GetRequiredService<IAdminService>();

    await profileService.BanUser(id);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Subscribed");
});

app.MapPut("/api/admin/unban/{id}", [Authorize(Roles = "3")] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var profileService = scope.ServiceProvider.GetRequiredService<IAdminService>();

    await profileService.UnbanUser(id);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Subscribed");
});

// <----------------------->

// violations (only admin)
// <----------------------->

app.MapGet("/api/violations/{id}", [Authorize] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var commentService = scope.ServiceProvider.GetRequiredService<IViolationService>();

    var result = await commentService.GetViolation(id);
    if (result == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("No such violation");
        return;
    }

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapPost("/api/violations", [Authorize(Roles = "3")] async (context) =>
{

    var name = context.Request.Form["name"];
    var description = context.Request.Form["description"];
    var banDays = int.Parse(context.Request.Form["ban_days"]!);

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var violationService = scope.ServiceProvider.GetRequiredService<IViolationService>();

    var newViolation = await violationService.AddViolation(name!, description!, banDays);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(newViolation);
});

app.MapPut("/api/violations/{id}", [Authorize(Roles = "3")] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    var name = context.Request.Form["name"];
    var description = context.Request.Form["description"];
    var banDays = int.Parse(context.Request.Form["ban_days"]!);

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var violationService = scope.ServiceProvider.GetRequiredService<IViolationService>();

    var newViolation = await violationService.UpdateViolation(id, name!, description!, banDays);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(newViolation);
});

app.MapDelete("/api/violations/{id}", [Authorize(Roles = "3")] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var violationService = scope.ServiceProvider.GetRequiredService<IViolationService>();

    await violationService.DeleteViolation(id);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Violation deleted");
});

// <----------------------->

// moderation
// <----------------------->

app.MapDelete("/api/moderation/remove/{id}", [Authorize(Roles = "2")] async (context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"]!.ToString()!);

    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var moderatorService = scope.ServiceProvider.GetRequiredService<IModeratorService>();

    var newViolation = await moderatorService.DeletePost(id);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(newViolation);
});

// <----------------------->

// search
// <----------------------->

app.MapGet("/api/search/users/location", async (context) =>
{
    var textQuery = context.Request.Form["text_query"];

    using var scope = app.Services.CreateScope();
    var searchService = scope.ServiceProvider.GetRequiredService<ISearchService>();

    var result = await searchService.SearchUsersByLocation(textQuery);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapGet("/api/search/users/name", async (context) =>
{
    var textQuery = context.Request.Form["text_query"];

    using var scope = app.Services.CreateScope();
    var searchService = scope.ServiceProvider.GetRequiredService<ISearchService>();

    var result = await searchService.SearchUsersByName(textQuery!);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapGet("/api/search/users", async (context) =>
{
    var textQuery = context.Request.Form["text_query"]!;

    using var scope = app.Services.CreateScope();
    var searchService = scope.ServiceProvider.GetRequiredService<ISearchService>();

    var result = await searchService.SearchUsersGeneral(textQuery!);

    context.Response.StatusCode = 200;
    await context.Response.WriteAsJsonAsync(result);
});

// <----------------------->

app.Run();
