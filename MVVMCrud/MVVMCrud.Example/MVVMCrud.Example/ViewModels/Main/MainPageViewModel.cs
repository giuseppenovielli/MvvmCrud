using System.Windows.Input;
using MVVMCrud.Example.Views.Post;
using MVVMCrud.Example.Views.PostListView;
using MVVMCrud.ViewModels.Base;
using Prism.Navigation;
using Xamarin.Forms;

namespace MVVMCrud.Example.ViewModels.Main
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand PostPageCommand { get; set; }
        public ICommand PostPageListViewCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService): base(navigationService)
        {
            PostPageCommand = new Command(async (obj) =>
            {
                var navResult = await NavigationService.NavigateAsync(nameof(PostPage));
            });

            PostPageListViewCommand = new Command(async (obj) =>
            {
                var navResult = await NavigationService.NavigateAsync(nameof(PostListViewPage));
            });
        }


    }
}
