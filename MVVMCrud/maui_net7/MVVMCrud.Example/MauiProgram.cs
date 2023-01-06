using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using MVVMCrud.Example.Utils.MVVMCrud;
using Prism.Navigation;
using MVVMCrud.Example.Views.Main;
using MVVMCrud.Example.ViewModels.Main;
using MVVMCrud.Example.Views.Post;
using MVVMCrud.Example.ViewModels.Post;
using MVVMCrud.Example.Views.PostListView;
using MVVMCrud.Example.Views.Comment;
using MVVMCrud.Example.ViewModels.Comment;
using MVVMCrud.Example.Views.PostNewEdit;
using MVVMCrud.Example.ViewModels.PostNewEdit;

namespace MVVMCrud.Example;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();
        builder
        .UseMauiApp<App>()
        .UseMauiCommunityToolkit()
        .UsePrism(prism =>

            prism.RegisterTypes(containerRegistry =>
            {
                MVVMCrudApplication.RegisterServices(containerRegistry);

                containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

                containerRegistry.RegisterForNavigation<PostPage, PostPageViewModel>();
                containerRegistry.RegisterForNavigation<PostListViewPage, PostPageViewModel>();
                containerRegistry.RegisterForNavigation<PostNewEditPage, PostNewEditPageViewModel>();
                containerRegistry.RegisterForNavigation<CommentPage, CommentPageViewModel>();

            })

            .OnInitialized((IContainerProvider obj) => 
            {
                var mvvmCrud = new CustomMVVMCrud();

            })

            .OnAppStart(async navigationService =>
            {
                var navResult = await navigationService.NavigateAsync("/" + nameof(NavigationPage) + "/" + nameof(Views.Main.MainPage));

            })
        )
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
        

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();

    }


    private static void HandleNavigationError(Exception ex)
    {
        System.Diagnostics.Debug.WriteLine(ex);
        System.Diagnostics.Debugger.Break();
    }
}

