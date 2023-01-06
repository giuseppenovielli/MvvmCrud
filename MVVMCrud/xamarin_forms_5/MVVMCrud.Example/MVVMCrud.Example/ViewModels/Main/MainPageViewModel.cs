using System.Windows.Input;
using MVVMCrud.Example.Views.Post;
using MVVMCrud.Example.Views.PostListView;
using MVVMCrud.ViewModels.Base;
using Prism.Navigation;
using Xamarin.Forms;

namespace MVVMCrud.Example.ViewModels.Main
{
    public class MainPageViewModel : BasePageViewModel
    {
        public ICommand PostPageCollectionViewCommand { get; set; }
        public ICommand PostPageListViewCommand { get; set; }
        public ICommand PostPageCollectionViewScroolToItemCommand { get; set; }
        public ICommand PostPageListViewScroolToItemCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService): base(navigationService)
        {
            PostPageCollectionViewCommand = new Command(async (obj) =>
            {
                var navResult = await NavigationService.NavigateAsync(nameof(PostPage));
            });

            PostPageListViewCommand = new Command(async (obj) =>
            {
                var navResult = await NavigationService.NavigateAsync(nameof(PostListViewPage));
            });

            PostPageCollectionViewScroolToItemCommand = new Command(async (obj) =>
            {
                //{
                //    "userId": 1,
                //    "id": 5,
                //    "title": "nesciunt quas odio",
                //    "body": "repudiandae veniam quaerat sunt sed\nalias aut fugiat sit autem sed est\nvoluptatem omnis possimus esse voluptatibus quis\nest aut tenetur dolor neque"
                //},
                var navParams = new NavigationParameters
                {
                    { "id", 5 }
                };

                var navResult = await NavigationService.NavigateAsync(nameof(PostPage), navParams);
            });

            PostPageListViewScroolToItemCommand = new Command(async (obj) =>
            {
                //{
                //    "userId": 1,
                //    "id": 5,
                //    "title": "nesciunt quas odio",
                //    "body": "repudiandae veniam quaerat sunt sed\nalias aut fugiat sit autem sed est\nvoluptatem omnis possimus esse voluptatibus quis\nest aut tenetur dolor neque"
                //},
                var navParams = new NavigationParameters
                {
                    { "id", 5 }
                };

                var navResult = await NavigationService.NavigateAsync(nameof(PostListViewPage), navParams);
            });
        }
    }
}
