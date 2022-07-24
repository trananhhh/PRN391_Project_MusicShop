using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_MusicShop.Models
{
    public class ProductModel
    {
        private List<Album> albums;

        public ProductModel()
        {
            PRN391_Project_MusicShopContext context = new PRN391_Project_MusicShopContext();
            albums = context.Albums.ToList();
        }

        public List<Album> findAll()
        {
            return this.albums;
        }

        public Album find(int? id)
        {
            foreach(Album album in albums)
            {
                if(album.AlbumId == id)
                {
                    return album;
                }
            }
            return null;
        }

    }
}
