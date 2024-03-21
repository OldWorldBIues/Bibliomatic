using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.ViewModels;
using Bibliomatic_MAUI_App.Views;
using Bibliomatic_MAUI_App.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Syncfusion.Maui.Core.Hosting;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;
using Bibliomatic_MAUI_App.Services.AzureB2CAuthService;
using System.Reflection;

namespace Bibliomatic_MAUI_App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()                
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });                 

            #if DEBUG
		    builder.Logging.AddDebug();
            #endif

            builder.AddAppSettings();
            builder.SetAzureB2CConstants();            
            builder.SetSyncfusionLicense();

            builder.Services.AddScoped<IDataService<ArticleResponce>, DataRequestService<ArticleResponce>>();
            builder.Services.AddScoped<IDataService<TestResponse>, DataRequestService<TestResponse>>();
            builder.Services.AddScoped<IDataService<TestQuestionResponse>, DataRequestService<TestQuestionResponse>>();
            builder.Services.AddScoped<IDataService<TestAnswerResponse>, DataRequestService<TestAnswerResponse>>();
            builder.Services.AddScoped<IDataService<BaseQuestionResponce>, DataRequestService<BaseQuestionResponce>>();
            builder.Services.AddScoped<IDataService<UserData>, DataRequestService<UserData>>();

            builder.Services.AddScoped<IRelatedDataService<BaseQuestionResponce, AnswerResponce>, RelatedDataRequestService<BaseQuestionResponce, AnswerResponce>>();
            builder.Services.AddScoped<IRelatedDataService<BaseQuestionResponce, AnswerLike>, RelatedDataRequestService<BaseQuestionResponce, AnswerLike>>();
            builder.Services.AddScoped<IRelatedDataService<BaseQuestionResponce, AnswerDislike>, RelatedDataRequestService<BaseQuestionResponce, AnswerDislike>>();
            builder.Services.AddScoped<IRelatedDataService<BaseQuestionResponce, AnswerComment>, RelatedDataRequestService<BaseQuestionResponce, AnswerComment>>();

            builder.Services.AddScoped<IRelatedDataService<BaseQuestionResponce, QuestionResponce>, RelatedDataRequestService<BaseQuestionResponce, QuestionResponce>>();
            builder.Services.AddScoped<IRelatedDataService<BaseQuestionResponce, QuestionLike>, RelatedDataRequestService<BaseQuestionResponce, QuestionLike>>();
            builder.Services.AddScoped<IRelatedDataService<BaseQuestionResponce, QuestionDislike>, RelatedDataRequestService<BaseQuestionResponce, QuestionDislike>>();
            builder.Services.AddScoped<IRelatedDataService<BaseQuestionResponce, QuestionComment>, RelatedDataRequestService<BaseQuestionResponce, QuestionComment>>();

            builder.Services.AddScoped<IRelatedDataService<TestResponse, TestLike>, RelatedDataRequestService<TestResponse, TestLike>>();
            builder.Services.AddScoped<IRelatedDataService<TestResponse, TestDislike>, RelatedDataRequestService<TestResponse, TestDislike>>();
            builder.Services.AddScoped<IRelatedDataService<TestResponse, TestComment>, RelatedDataRequestService<TestResponse, TestComment>>();
            builder.Services.AddScoped<IRelatedDataService<TestResponse, UserScore>, RelatedDataRequestService<TestResponse, UserScore>>();

            builder.Services.AddScoped<IRelatedDataService<ArticleResponce, ArticleLike>, RelatedDataRequestService<ArticleResponce, ArticleLike>>();
            builder.Services.AddScoped<IRelatedDataService<ArticleResponce, ArticleDislike>, RelatedDataRequestService<ArticleResponce, ArticleDislike>>();
            builder.Services.AddScoped<IRelatedDataService<ArticleResponce, ArticleComment>, RelatedDataRequestService<ArticleResponce, ArticleComment>>();

            builder.Services.AddScoped<IFileService, FileRequestService>();            
            builder.Services.AddScoped<ILocalObjectKeeperService<TestResponse>, LocalObjectKeeperService<TestResponse>>();

            builder.Services.AddSingleton<ILocalFileKeeperService, LocalFileKeeperService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IUserDataService, UserDataRequestService>();

            builder.Services.AddTransient<ArticleView>();
            builder.Services.AddTransient<ArticleDetailsView>();
            builder.Services.AddTransient<ArticleDocumentView>();
            builder.Services.AddTransient<CreationView>();
            builder.Services.AddTransient<ArticleInformationView>();              
            builder.Services.AddTransient<TestEditorView>();
            builder.Services.AddTransient<TestsView>();
            builder.Services.AddTransient<TestView>();
            builder.Services.AddTransient<CreatorTestView>();
            builder.Services.AddTransient<TestResultView>();
            builder.Services.AddTransient<QuestionView>();
            builder.Services.AddTransient<QuestionBodyView>();
            builder.Services.AddTransient<QuestionsView>();
            builder.Services.AddTransient<QuestionDetailsView>();
            builder.Services.AddTransient<AnswerView>();
            builder.Services.AddTransient<AuthenticationView>();
            builder.Services.AddTransient<ProfileView>();
            builder.Services.AddTransient<TestInformationView>();
            builder.Services.AddTransient<CreatorTestInformationView>();

            builder.Services.AddTransient<ArticlesViewModel>();
            builder.Services.AddTransient<ArticleDetailsViewModel>();
            builder.Services.AddTransient<ArticleDocumentViewModel>();
            builder.Services.AddTransient<CreationViewModel>();
            builder.Services.AddTransient<ArticleInformationViewModel>();          
            builder.Services.AddTransient<TestEditorViewModel>();
            builder.Services.AddTransient<TestsViewModel>();
            builder.Services.AddTransient<TestViewModel>();
            builder.Services.AddTransient<CreatorTestViewModel>();
            builder.Services.AddTransient<TestResultViewModel>();
            builder.Services.AddTransient<QuestionViewModel>();
            builder.Services.AddTransient<QuestionBodyViewModel>();
            builder.Services.AddTransient<QuestionsViewModel>();
            builder.Services.AddTransient<QuestionDetailsViewModel>();
            builder.Services.AddTransient<AnswerViewModel>();
            builder.Services.AddTransient<AuthenticationViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<TestInformationViewModel>();
            builder.Services.AddTransient<CreatorTestInformationViewModel>();

            return builder.Build();
        }

        public static void AddAppSettings(this MauiAppBuilder builder)
        {
            using Stream stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("Bibliomatic_MAUI_App.appsettings.json");

            if(stream != null)
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                builder.Configuration.AddConfiguration(config);
            }
        }

        public static void SetAzureB2CConstants(this MauiAppBuilder builder)
        {
            Constants.ClientId = builder.Configuration.GetSection("AzureADB2C:ClientId").Value!;
            Constants.TenantName = builder.Configuration.GetSection("AzureADB2C:TenantName").Value!;            
            Constants.Scopes = builder.Configuration.GetSection("AzureADB2C:Scopes").Get<string[]>();
            Constants.BaseApiRoute = builder.Configuration.GetSection("AzureADB2C:BaseApiRoute").Value!;
            Constants.SignInPolicy = builder.Configuration.GetSection("AzureADB2C:SignInPolicy").Value!;
            Constants.EditProfilePolicy = builder.Configuration.GetSection("AzureADB2C:EditProfilePolicy").Value!;
        }

        public static void SetSyncfusionLicense(this MauiAppBuilder builder)
        {
            string syncfusionLicense = builder.Configuration.GetValue<string>("SyncfusionLicense");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);
        }
    }
}