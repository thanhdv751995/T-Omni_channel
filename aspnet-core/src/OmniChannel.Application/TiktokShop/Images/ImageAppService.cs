using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniChannel.Files;
using OmniChannel.HttpClients;
using OmniChannel.MultiTenancy;
using OmniChannel.Shares;
using Volo.Abp;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OmniChannel.HttpMethods;
using OmniChannel.TiktokShop.ProductImages;
using Microsoft.Extensions.Configuration;

namespace OmniChannel.Images
{
    [RemoteService(true)]
    public class ImageAppService : OmniChannelAppService
    {
        private readonly int IMG_SCENE = 1;

        private readonly ShareAppService _shareAppService;
        //private readonly string shopId = OmniChannelConsts.shopId;
        //private readonly string access_token = OmniChannelConsts.access_token;
        private readonly string TiktokShopUploadImage = OmniChannelConsts.TiktokShopUploadImage;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ProductImageManager _productImageManager;
        private readonly IConfiguration _configuration;
        public ImageAppService(ShareAppService shareAppService , IProductImageRepository productImageRepository,
            ProductImageManager productImageManager, IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _productImageRepository = productImageRepository;
            _productImageManager = productImageManager;
            _configuration = configuration;
        }
        /// <summary>
        /// Upload mutil images
        /// </summary>
        /// <param name="files"></param>
        /// <param name="img_scene"> <p> 1 : Hình ảnh sản phẩm </p>
        ///                          <p> 2 : Hình ảnh mô tả sản phẩm </p>
        ///                          <p> 3 : Hình ảnh thuộc tính sản phẩm </p>
        ///                          <p> 4 : Hình ảnh mô tả kích thước sản phẩm</p>                                               
        /// </param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<List<UploadFileDto>> UploadImage(IList<IFormFile> files, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            List<UploadFileDto> listFile = new();

            foreach (var file in files)
            {
                //parse sang base64
                string encodedStr = ShareAppService.ConvertToBase64(file).Content;

                var url = _shareAppService.GetTiktokShopUrl(TiktokShopUploadImage, 
                    _configuration["AppTiktokShopSetting:App_key"], 
                    channel.Access_token,
                    channel.Shop_id, 
                    _configuration["AppTiktokShopSetting:App_secret"], 
                    new List<string>());

                BodyCreateUpLoadImage bodyCreateUpLoadImage = new()
                {
                    img_data = encodedStr,
                    img_scene = IMG_SCENE
                };

                var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, bodyCreateUpLoadImage);

                var reponse = JsonConvert.DeserializeObject<UploadFileDto>(await httpResponseMessage.Content.ReadAsStringAsync());

                var imageChannel = _productImageManager.CreateAsync(reponse.data.img_id, reponse.data.img_url, reponse.data.img_scene);

                await _productImageRepository.InsertAsync(imageChannel);

                listFile.Add(reponse);

            }
            return listFile;
        }
    }
}
