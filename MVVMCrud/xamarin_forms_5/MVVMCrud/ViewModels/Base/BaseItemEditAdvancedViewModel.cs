using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MVVMCrud.Models.Base;
using MVVMCrud.Services.Request;
using Newtonsoft.Json;
using Prism.Navigation;
using Xamarin.Forms;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseItemEditAdvancedViewModel<TItem, TItemInput, TItemRoot> : BaseItemEditViewModel
        where TItem : BaseItem, new()
        where TItemInput : BaseItem, new()
        where TItemRoot : BaseModelItemRoot<TItem>, new()
    {
        public TItemInput ItemInput { get; set; }

        public int Position { get; set; }
        public int Section { get; set; }

        public BaseItemEditAdvancedViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {
            Position = -1;
            Section = -1;
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.ContainsKey("endpoint"))
            {
                Endpoint = parameters.GetValue<string>("endpoint");
            }

            if (parameters.ContainsKey("itemSerialized"))
            {
                var itemSerialized = parameters["itemSerialized"] as string;
                Position = (int)parameters["position"];

                if (!string.IsNullOrWhiteSpace(itemSerialized))
                {
                    TItem item = null;
                    if (SetupIsDeserializeItem())
                    {
                        item = SetupDeserializeItem(itemSerialized);
                    }

                    ItemInput = SetupDeserializeItemInput(itemSerialized);

                    await SetupGetItem(ItemInput, item);
                    SetupManageItemInput(ItemInput, item);
                }
            }
            else
            {
                Position = -1;
                Section = -1;
            }

            if (ItemInput == null)
            {
                ItemInput = new TItemInput();
            }

            if (string.IsNullOrWhiteSpace(Endpoint))
            {
                Endpoint = SetupEndpoint();
            }

            SetupValidations();
            SetupInterface();
        }

        public virtual JsonSerializerSettings SetupJsonDeserializerSettings()
        {
            return MVVMCrudApplication.Instance.SetupJsonSettingsDeserialize();
        }

        public virtual JsonSerializerSettings SetupJsonSerializerSettings()
        {
            return MVVMCrudApplication.Instance.SetupJsonSettingsSerialize();
        }

        public virtual bool SetupIsDeserializeItem()
        {
            return false;
        }

        public virtual TItem SetupDeserializeItem(string itemSerialized)
        {
            try
            {
                return JsonConvert.DeserializeObject<TItem>(itemSerialized, SetupJsonDeserializerSettings());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return null;
            
        }


        public virtual TItemInput SetupDeserializeItemInput(string itemSerialized)
        {
            try
            {
                return JsonConvert.DeserializeObject<TItemInput>(itemSerialized, SetupJsonDeserializerSettings());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return new TItemInput();
        }

        public virtual void SetupManageItemInput(TItemInput itemInput, TItem item)
        {
        }

        public virtual async Task SetupGetItem(TItemInput itemInput, TItem item)
        {
            await Task.CompletedTask;
        }

        public virtual void SetupValidations()
        {
        }

        public virtual string SetupTitlePageNew()
        {
            var pageVMName = GetType().Name;

            var pageContext = pageVMName.Split(new[] { "NewEditPageViewModel" }, StringSplitOptions.None);
            if (pageContext?.Length == 2)
            {
                var pageName = pageContext[0].ToLower();

                var label = string.Format("title_page_{0}_new", pageName);
                return MVVMCrudApplication.GetAppResourceManager().GetString(label);
            }

            return null;
        }

        public virtual string SetupTitlePageEdit()
        {
            var pageVMName = GetType().Name;

            var pageContext = pageVMName.Split(new[] { "NewEditPageViewModel" }, StringSplitOptions.None);
            if (pageContext?.Length == 2)
            {
                var pageName = pageContext[0].ToLower();

                var label = string.Format("title_page_{0}_edit", pageName);
                return MVVMCrudApplication.GetAppResourceManager().GetString(label);
            }

            return null;
        }

        public override string SetupTitlePage()
        {
            if (string.IsNullOrWhiteSpace(ItemInput?.Id))
            {
                return SetupTitlePageNew();
            }
            else
            {
                return SetupTitlePageEdit();
            }

        }

        public virtual void SetupInterface()
        {
            TitlePage = SetupTitlePage();
        }

        public virtual void SetupItemBeforeUpdate(TItemInput itemInput)
        {

        }

        public override void TlbSendClick()
        {
            base.TlbSendClick();

            _ = CheckAndUpload();
        }

        public virtual bool SetupAreFieldsValid()
        {
            return true;
        }

        Task<bool> DisplayUploadConfirm(Func<Task<bool>> displayDialog)
        {
            return displayDialog();
        }

        async Task CheckAndUpload()
        {
            if (SetupAreFieldsValid())
            {
                SetupItemBeforeUpdate(ItemInput);

                if (await DisplayUploadConfirm(SetupMessageUploadConfirm))
                {
                    await UploadAndReturnItem();
                    
                }

            }
        }

        public virtual string GetConfirmUploadText()
        {
            return MVVMCrudApplication.GetConfirmUploadText();
        }

        public virtual string GetOKText()
        {
            return MVVMCrudApplication.GetYesText();
        }

        public virtual string GetCancelText()
        {
            return MVVMCrudApplication.GetNoText();
        }

        public virtual async Task<bool> SetupMessageUploadConfirm()
        {
            var message = GetConfirmUploadText();
            var ok = GetOKText();
            var cancel = GetCancelText();

            return await Application.Current.MainPage.DisplayAlert(TitlePage, message, ok, cancel);

        }

        public virtual List<HttpStatusCode> GetUploadItemHttpStatusCodes()
        {
            List<HttpStatusCode> httpCode = null;
            if (string.IsNullOrWhiteSpace(ItemInput.Id))
            {
                httpCode = new List<HttpStatusCode>() { HttpStatusCode.Created };
            }
            return httpCode;
        }

        public virtual async Task UploadAndReturnItem()
        {
            AfterUploadItem(await UploadItem());
        }

        public virtual async Task<TItemRoot> UploadItem()
        {
            return await RequestService.RequestItem<TItemRoot, TItem>(
                Request,
                TitlePage,
                false,
                true,
                GetUploadItemHttpStatusCodes());
        }

        public virtual void AfterUploadItem(TItemRoot rootItem)
        {
            if (rootItem != null)
            {
                var item = rootItem.Item;
                if (item != null)
                {
                    SetupReturnItem(item);
                }

            }
        }

        async Task<TItemRoot> Request()
        {
            return await SetupRequest(ItemInput);
        }

        public virtual JsonSerializerSettings GetJsonSettingsSerialize()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            settings.Converters = MVVMCrudApplication.Instance.GetJsonConverters();

            return settings;
        }

        public virtual MultipartFormDataContent GetFormDataToUpload(TItemInput item)
        {
            return Utils.Utils.GetFormDataToUpload(item, GetJsonSettingsSerialize());
        }

        public virtual async Task<TItemRoot> SetupRequest(TItemInput item)
        {
            if (!string.IsNullOrWhiteSpace(Endpoint))
            {

                string pk = null;
                if (!string.IsNullOrWhiteSpace(item.Id))
                {
                    pk = item.Id;
                }

                return await RequestService.CreateUpdate<TItemRoot, TItem>(Endpoint, pk, GetFormDataToUpload(item), httpClient: GetHttpClient());
            }

            return await Task.FromResult<TItemRoot>(null);

        }

        public virtual async void SetupReturnItem(TItem item)
        {
            var navParams = new NavigationParameters
            {
                { "newEditItem", new NewEditItem<TItem>(item, Position, Section) }
            };

            var navResult = await NavigationService.GoBackAsync(parameters: navParams);
        }

    }

    public class NewEditItem<TItem>
    {
        public int Position { get; private set; }
        public int Section { get; private set; }

        public TItem Item { get; private set; }


        public NewEditItem(TItem item, int position = -1, int section = -1)
        {
            Item = item;
            Position = position;
            Section = section;
        }
    }
}
