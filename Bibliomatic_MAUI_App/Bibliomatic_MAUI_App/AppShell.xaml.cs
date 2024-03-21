using Bibliomatic_MAUI_App.Views;

namespace Bibliomatic_MAUI_App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AuthenticationView), typeof(AuthenticationView));
            Routing.RegisterRoute(nameof(ProfileView), typeof(ProfileView));
            Routing.RegisterRoute(nameof(CreationView), typeof(CreationView));

            Routing.RegisterRoute(nameof(ArticleView), typeof(ArticleView));            
            Routing.RegisterRoute(nameof(ArticleDetailsView), typeof(ArticleDetailsView));
            Routing.RegisterRoute(nameof(ArticleDocumentView), typeof(ArticleDocumentView));
            Routing.RegisterRoute(nameof(ArticleInformationView), typeof(ArticleInformationView));

            Routing.RegisterRoute(nameof(TestEditorView), typeof(TestEditorView));
            Routing.RegisterRoute(nameof(TestInformationView), typeof(TestInformationView));
            Routing.RegisterRoute(nameof(CreatorTestInformationView), typeof(CreatorTestInformationView));
            Routing.RegisterRoute(nameof(TestView), typeof(TestView));
            Routing.RegisterRoute(nameof(CreatorTestView), typeof(CreatorTestView));
            Routing.RegisterRoute(nameof(TestsView), typeof(TestsView));
            Routing.RegisterRoute(nameof(TestResultView), typeof(TestResultView));

            Routing.RegisterRoute(nameof(QuestionsView), typeof(QuestionsView));
            Routing.RegisterRoute(nameof(QuestionView), typeof(QuestionView));
            Routing.RegisterRoute(nameof(AnswerView), typeof(AnswerView));                  
            Routing.RegisterRoute(nameof(QuestionBodyView), typeof(QuestionBodyView));
            Routing.RegisterRoute(nameof(QuestionDetailsView), typeof(QuestionDetailsView));           
        }
    }
}