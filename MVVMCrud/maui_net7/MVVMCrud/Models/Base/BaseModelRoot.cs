using System;
using System.Net;
using System.Net.Http.Headers;
using MVVMCrud.Models.ItemRoot;
using Newtonsoft.Json.Linq;

namespace MVVMCrud.Models.Base
{
    public class BaseModelRoot
    {
        public PaginationItem PaginationItem { get; set; }
        public RootItemBase RootItemBase { get; set; }

        public bool CustomDataExtract { get; }

        public BaseModelRoot()
        {
        }


        public BaseModelRoot(string item, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader, object data = null, bool customDataExtract = true)
        {
            CustomDataExtract = customDataExtract;
            try
            {
                OnInitialize(data);

                RootItemBase = new RootItemBase(item, httpStatus, responseHeader);

                if (!RootItemBase.IsError)
                {
                    var jObject = JObject.Parse(item);

                    OnDataObject(jObject);

                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

        }


        public BaseModelRoot(string response, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader, bool pagination = true, bool extra = false, bool customDataExtract = true, object data = null)
        {
            CustomDataExtract = customDataExtract;
            try
            {
                OnInitialize(data);
                StringData(response, httpStatus, pagination, extra, responseHeader);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public void OnInitialize(string response, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader, bool pagination = true, bool extra = false, object data = null)
        {
            try
            {
                OnInitialize(data);
                StringData(response, httpStatus, pagination, extra, responseHeader);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public void OnInitializeItem(string item, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader, object data = null)
        {
            try
            {
                OnInitialize(data);

                var jObject = JObject.Parse(item);

                RootItemBase = new RootItemBase(item, httpStatus, responseHeader);

                if (!RootItemBase.IsError)
                {
                    OnDataObject(jObject);
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }


        void StringData(string response, HttpStatusCode httpStatus, bool pagination = true, bool extra = false, HttpResponseHeaders responseHeader = null)
        {
            try
            {
                if (pagination)
                {
                    if (!extra)
                    {
                        //Paginazione
                        ProcessWithPagination(response, httpStatus, responseHeader);
                    }
                    else
                    {
                        //Paginazione Extra Data
                        ProcessWithPaginationExtraData(response, httpStatus, responseHeader);
                    }        
                    
                }
                else
                {
                    if (!extra)
                    {
                        //Non Paginazione
                        ProcessWithoutPagination(response, httpStatus, responseHeader);
                    }
                    else
                    {
                        //Non Paginazione Extra Data
                        ProcessWithoutPaginationExtraData(response, httpStatus, responseHeader);
                    }
                    
                }

            }
            catch (System.Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine(ex.ToString());

                try
                {
                    //Paginazione
                    ProcessWithPagination(response, httpStatus, responseHeader);
                }
                catch (System.Exception ex2)
                {
                    //System.Diagnostics.Debug.WriteLine(ex2.ToString());
                    try
                    {
                        //Paginazione con root Array
                        ProcessWithPaginationArray(response, httpStatus, responseHeader);
                    }
                    catch (System.Exception ex3)
                    {
                        //System.Diagnostics.Debug.WriteLine(ex3.ToString());
                        try
                        {
                            //Non Paginazione
                            ProcessWithoutPagination(response, httpStatus, responseHeader);
                        }
                        catch (System.Exception ex4)
                        {
                            //System.Diagnostics.Debug.WriteLine(ex4.ToString());

                            try
                            {
                                //Paginazione Con Extra Data
                                ProcessWithPaginationExtraData(response, httpStatus, responseHeader);
                            }
                            catch (System.Exception ex5)
                            {
                                //System.Diagnostics.Debug.WriteLine(ex5.ToString());

                                try
                                {
                                    //Non Paginazione Con Extra Data
                                    ProcessWithoutPaginationExtraData(response, httpStatus, responseHeader);
                                }
                                catch (System.Exception ex6)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex6.ToString());

                                    //Errore
                                    RootItemBase = new RootItemBase(response, httpStatus, responseHeader);

                                }
                            }


                        }
                    }
                    
                }


            }
        }

        private void ProcessWithPaginationArray(string response, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader)
        {
            //Paginazione Array
            RootItemBase = new RootItemBase(response, httpStatus, responseHeader);

            if (!RootItemBase.IsError)
            {
                var itemsArray = JArray.Parse(response);
                PaginationItem = new PaginationItem(response, responseHeader);
                OnDataArray(itemsArray);
            }
        }

        void ProcessWithPagination(string response, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader)
        {
            var item = JObject.Parse(response);
            RootItemBase = new RootItemBase(response, httpStatus, responseHeader);

            if (!RootItemBase.IsError)
            {
                var itemsArray = item.GetValue(MVVMCrudApplication.GetResultKeyJSON()).ToObject<JArray>();
                PaginationItem = new PaginationItem(response, responseHeader);

                OnDataArray(itemsArray);
            }
        }

        void ProcessWithoutPagination(string response, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader)
        {
            //Non Paginazione
            RootItemBase = new RootItemBase(response, httpStatus, responseHeader);

            if (!RootItemBase.IsError)
            {
                var itemsArray = JArray.Parse(response);
                OnDataArray(itemsArray);
            }
        }

        void ProcessWithPaginationExtraData(string response, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader)
        {
            //Paginazione e Extra Data
            var item = JObject.Parse(response);
            RootItemBase = new RootItemBase(response, httpStatus, responseHeader);

            if (!RootItemBase.IsError)
            {
                var itemsArray = item.GetValue(MVVMCrudApplication.GetResultKeyJSON()).ToObject<JArray>();
                PaginationItem = new PaginationItem(response, responseHeader);

                OnDataObject(item);
                OnDataArray(itemsArray);
            }
        }

        void ProcessWithoutPaginationExtraData(string response, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader)
        {
            //Non Paginazione e Extra Data
            var item = JObject.Parse(response);
            RootItemBase = new RootItemBase(response, httpStatus, responseHeader);

            if (!RootItemBase.IsError)
            {
                var itemsArray = item.GetValue(MVVMCrudApplication.GetResultKeyJSON()).ToObject<JArray>();

                OnDataObject(item);
                OnDataArray(itemsArray);
            }
        }

        protected virtual void OnInitialize(object data = null)
        {
        }

        protected virtual void OnDataArray(JArray jArray)
        {

        }

        protected virtual void OnDataObject(JObject jObject)
        {
        }
    }
}
