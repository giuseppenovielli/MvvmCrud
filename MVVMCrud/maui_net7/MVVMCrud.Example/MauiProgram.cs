using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using MVVMCrud.Example.Utils.MVVMCrud;
using Prism.Navigation;

namespace MVVMCrud.Example;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();
        builder
        .UseMauiApp<App>()
        .UsePrism(prism =>

            prism.RegisterTypes(containerRegistry =>
            {
                MVVMCrudApplication.RegisterServices(containerRegistry);

                containerRegistry.RegisterForNavigation<Views.Main.MainPage, ViewModels.Main.MainPageViewModel>();

                containerRegistry.RegisterForNavigation<Views.Post.PostPage, ViewModels.Post.PostPageViewModel>();
                containerRegistry.RegisterForNavigation<Views.PostListView.PostListViewPage, ViewModels.Post.PostPageViewModel>();
                containerRegistry.RegisterForNavigation<Views.PostNewEdit.PostNewEditPage, ViewModels.PostNewEdit.PostNewEditPageViewModel>();
                containerRegistry.RegisterForNavigation<Views.Comment.CommentPage, ViewModels.Comment.CommentPageViewModel>();

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

