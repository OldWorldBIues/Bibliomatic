using BibliomaticData;
using BibliomaticData.AppContext;
using BibliomaticData.Models;
using BibliomaticData.Repository;
using BibliomaticWebApi;
using Microsoft.Graph;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.OData;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using System.Text.Json.Serialization;
using BibliomaticData.Models.DTOs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddControllers(options =>
                {
                    options.InputFormatters.Insert(0, JsonPatchExtension.GetJsonPatchInputFormatter());
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                })
                .AddOData(options =>
                {
                    options.Select()
                           .Expand()
                           .Filter()
                           .Count()
                           .SetMaxTop(20)
                           .OrderBy();
                });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var credential = new ClientSecretCredential(
    tenantId: builder.Configuration.GetSection("AzureKeyVault:TenantId").Value!,
    clientId: builder.Configuration.GetSection("AzureKeyVault:ClientId").Value!,    
    clientSecret: builder.Configuration.GetSection("AzureKeyVault:ClientSecretId").Value!);

var client = new SecretClient(
    vaultUri: new Uri(builder.Configuration.GetSection("AzureKeyVault:KeyVaultUrl").Value!), credential);

var clientSecretCredential = new ClientSecretCredential(
    tenantId: builder.Configuration.GetSection("GraphService:TenantId").Value!,
    clientId: builder.Configuration.GetSection("GraphService:ClientId").Value!,
    clientSecret: builder.Configuration.GetSection("GraphService:ClientSecret").Value!);

GraphApi.GraphServiceClient = new GraphServiceClient(clientSecretCredential);

builder.Configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());

builder.Services.AddScoped<IRepository<Article, ArticleDTO>, ArticleRepository>();
builder.Services.AddScoped<IRepository<BaseQuestion, BaseQuestionDTO>, BaseQuestionRepository>();

builder.Services.AddScoped<IRelatedRepository<Answer>, AnswerRepository>();

builder.Services.AddScoped<ISocialRepository<ArticleLike, ArticleDislike, ArticleComment>, ArticleRepository>();
builder.Services.AddScoped<ISocialRepository<TestLike, TestDislike, TestComment>, TestRepository>();
builder.Services.AddScoped<ISocialRepository<QuestionLike, QuestionDislike, QuestionComment>, BaseQuestionRepository>();
builder.Services.AddScoped<ISocialRepository<AnswerLike, AnswerDislike, AnswerComment>, AnswerRepository>();

builder.Services.AddScoped<IUserScoresRepository<UserScore>, TestRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IUserDataRepository, UserDataRepository>();

builder.Services.AddSingleton(provider =>
new BlobServiceClient(builder.Configuration.GetSection("BlobStorageConnectionString").Value!));

builder.Services.AddDbContext<BibliomaticAppContext>(options =>
{
    var sqlConnectionString = builder.Configuration.GetSection("SqlConnectionString").Value!;
    options.UseSqlServer(sqlConnectionString, ob => ob.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));   
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
