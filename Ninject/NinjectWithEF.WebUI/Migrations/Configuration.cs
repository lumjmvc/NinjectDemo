using System.Collections.Generic;
using NinjectWithEF.Domain.Models;

namespace NinjectWithEF.WebUI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using NinjectWithEF.Domain.Concrete;

    internal sealed class Configuration : DbMigrationsConfiguration<EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "NinjectWithEF.Domain.Concrete.EFDbContext";
        }

        protected override void Seed(EFDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            AddSampleData(context);
        }

        private void AddSampleData(EFDbContext context)
        {
            var posts = new List<Post>()
            {
                new Post() {Title = "Good no meals", Content = " Stufss to read .....fsdfsdf......."},
                new Post() {Title = "Bed nite stories", Content = "Stufss to read .........dadadc..."},
                new Post() {Title = "team beam", Content = "Stufss to read .........v vbfsbdfsgs..."},
                new Post() {Title = "nite gals", Content = "Stufss to read .......dfafvbdfsgsfasf....."},
                new Post() {Title = "Good no dasdsvmeals", Content = " Stufss to read .....fsdfsdf......."},
                new Post() {Title = "Bed nitdasdsade stories", Content = "Stufss to read .........dadadc..."},
                new Post() {Title = "team bdasdasdasdeam", Content = "Stufss to read .........v vbfsbdfsgs..."},
                new Post() {Title = "nite gdasdasdc adcals", Content = "Stufss to read .......dfafvbdfsgsfasf....."},
                new Post() {Title = "Good ndasdasdasvsdfbo meals", Content = " Stufss to read .....fsdfsdf......."},
                new Post() {Title = "Bed nite fsdfsdfsfsfstories", Content = "Stufss to read .........dadadc..."},
                new Post() {Title = "team bdsfdffdfssfssfdsfseam", Content = "Stufss to read .........v vbfsbdfsgs..."},
                new Post() {Title = "nite jkuigdfbg gals", Content = "Stufss to read .......dfafvbdfsgsfasf....."},
                new Post() {Title = "Good no fsdfsgfs fvdasdsvmeals", Content = " Stufss to read .....fsdfsdf......."},
                new Post() {Title = "Bed nitdavvdbdnbsdsade stories", Content = "Stufss to read .........dadadc..."},
                new Post() {Title = "team bdasdfsfsv vfsfdfsasdasdeam", Content = "Stufss to read .........v vbfsbdfsgs..."},
                new Post() {Title = "nite gdasdadadsfsggsdc adcals", Content = "Stufss to read .......dfafvbdfsgsfasf....."},

            };

            posts.ForEach(s => context.Posts.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();
        }
    }
}
