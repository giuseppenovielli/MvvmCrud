using MVVMCrud.Example.Utils.MVVMCrud;
using MVVMCrud.Example.ViewModels.Comment;
using MVVMCrud.Example.ViewModels.Main;
using MVVMCrud.Example.ViewModels.Post;
using MVVMCrud.Example.ViewModels.PostNewEdit;
using MVVMCrud.Example.Views.Comment;
using MVVMCrud.Example.Views.Main;
using MVVMCrud.Example.Views.Post;
using MVVMCrud.Example.Views.PostListView;
using MVVMCrud.Example.Views.PostNewEdit;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MVVMCrud.Example
{
    public partial class App : PrismApplication
    {
        public CustomMVVMCrud MVVMCrudApp { get; set; }


        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            SetupMVVMCrud();

            var navResult = await NavigationService.NavigateAsync("/" + nameof(Xamarin.Forms.NavigationPage) + "/" + nameof(Views.Main.MainPage));

        }

        void SetupMVVMCrud()
        {
            MVVMCrudApp = new CustomMVVMCrud();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            MVVMCrudApplication.RegisterServices(containerRegistry);

            containerRegistry.RegisterForNavigation<Xamarin.Forms.NavigationPage>();

            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            containerRegistry.RegisterForNavigation<PostPage, PostPageViewModel>();
            containerRegistry.RegisterForNavigation<PostListViewPage, PostPageViewModel>();
            containerRegistry.RegisterForNavigation<PostNewEditPage, PostNewEditPageViewModel>();

            containerRegistry.RegisterForNavigation<CommentPage, CommentPageViewModel>();
        }
    }
}
