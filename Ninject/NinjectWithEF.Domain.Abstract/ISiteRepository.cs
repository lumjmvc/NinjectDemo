using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjectWithEF.Domain.Models;

namespace NinjectWithEF.Domain.Abstract
{
    /// <summary>
    /// Defines all the database methods required for this site
    /// this is the interface instanciated in the controller
    /// </summary>
    public interface ISiteRepository
    {
        
        int TotalPosts();

        IEnumerable<Post> AllPosts();


        void AddPost(Post post);
    }
}
