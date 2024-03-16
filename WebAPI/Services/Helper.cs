using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Services
{
    public static class Helper
    {
        public static int GetImageTypeId(string ImageName)
        {
            switch (ImageName)
            {
                case "PreEvent":
                    return 0;
                case "PostEvent":
                    return 1;
                case "Cover":
                    return 2;
                case "Logo":
                    return 3;
                case "SocialMedia":
                    return 4;
                case "Tiles":
                    return 5;
                case "Featured":
                    return 6;
                case "Thumbnail":
                    return 7;
                default:
                    return-1;
            }
        }
    }
}

public enum ImageType
{
    PreEvent,
    PostEvent,
    Cover,
    Logo,
    SocialMedia,
    Tiles,
    Featured,
    Thumbnail
}