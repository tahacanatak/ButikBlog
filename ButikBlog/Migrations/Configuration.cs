namespace ButikBlog.Migrations
{
    using ButikBlog.Models;
    using ButikBlog.Utility;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ButikBlog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

        }

        protected override void Seed(ButikBlog.Models.ApplicationDbContext context)
        {
            //isi bittigi icin false a donduruldu
            var autoGeneratoreSlugs = false;

            #region Mevcut Kategori ve Yazilarin sluglar�n� olustur

            if (autoGeneratoreSlugs)
            {
                foreach (var item in context.Categories)
                {
                    if (string.IsNullOrEmpty(item.Slug))
                    {
                        item.Slug = UrlService.URLFriendly(item.CategoryName);
                    }
                }

                foreach ( var item in context.Posts)
                {
                    if (string.IsNullOrEmpty(item.Slug))
                    {
                        item.Slug = UrlService.URLFriendly(item.Title);
                    }
                }
            }
            #endregion

            #region Tum Kullanicilari IsEnable true yapildi
            //foreach (var item in context.Users)
            //{
            //    item.IsEnabled = true;
            //}
            //return; 
            #endregion

            #region Admin Rol�n� ve Kullan�c�s�n� olu�tur

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "tahacan.atak@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "tahacan.atak@gmail.com",
                    Email = "tahacan.atak@gmail.com"
                };

                manager.Create(user, "Ankara1.");
                manager.AddToRole(user.Id, "Admin");

                //Olu�turulan bu kullan�c�ya ait yaz�lar ekleyelim:
                #region Kategoriler ve Yaz�lar

                if (!context.Categories.Any())
                {
                    Category cat1 = new Category
                    {
                        CategoryName = "Gezi Yaz�lar�"
                    };
                    cat1.Posts = new List<Post>();
                    cat1.Posts.Add(new Post
                    {
                        Title = "Gezi Yaz�s� 1",
                        AuthorId = user.Id,
                        Content = @"<p>Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.</p>
<p>Placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante.</p>
<p>Fusce non varius purus aenean nec magna felis fusce vestibulum velit mollis odio sollicitudin lacinia aliquam posuere, sapien elementum lobortis tincidunt, turpis dui ornare nisl, sollicitudin interdum turpis nunc eget.</p>",
                        CreationTime = DateTime.Now
                    });
                    cat1.Posts.Add(new Post
                    {
                        Title = "Gezi Yaz�s� 2",
                        AuthorId = user.Id,
                        Content = @"<p>Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.</p>
<p>Placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante.</p>
<p>Fusce non varius purus aenean nec magna felis fusce vestibulum velit mollis odio sollicitudin lacinia aliquam posuere, sapien elementum lobortis tincidunt, turpis dui ornare nisl, sollicitudin interdum turpis nunc eget.</p>",
                        CreationTime = DateTime.Now
                    });

                    Category cat2 = new Category
                    {
                        CategoryName = "�� Yaz�lar�"
                    };
                    cat2.Posts = new List<Post>();
                    cat2.Posts.Add(new Post
                    {
                        Title = "�� Yaz�s� 1",
                        AuthorId = user.Id,
                        Content = @"<p>Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.</p>
<p>Placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante.</p>
<p>Fusce non varius purus aenean nec magna felis fusce vestibulum velit mollis odio sollicitudin lacinia aliquam posuere, sapien elementum lobortis tincidunt, turpis dui ornare nisl, sollicitudin interdum turpis nunc eget.</p>",
                        CreationTime = DateTime.Now
                    });
                    cat2.Posts.Add(new Post
                    {
                        Title = "�� Yaz�s� 2",
                        AuthorId = user.Id,
                        Content = @"<p>Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.</p>
<p>Placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante.</p>
<p>Fusce non varius purus aenean nec magna felis fusce vestibulum velit mollis odio sollicitudin lacinia aliquam posuere, sapien elementum lobortis tincidunt, turpis dui ornare nisl, sollicitudin interdum turpis nunc eget.</p>",
                        CreationTime = DateTime.Now
                    });

                    context.Categories.Add(cat1);
                    context.Categories.Add(cat2);
                }
                #endregion
            }
            #endregion

            #region Admin kullan�c�s�na 77 yeni yaz� ekle

         
            if (!context.Categories.Any(x => x.CategoryName == "Di�er"))
            {
                ApplicationUser admin = context.Users
                    .Where(x => x.UserName == "tahacan.atak@gmail.com")
                    .FirstOrDefault();

                if (admin != null)
                {
                    if (!context.Categories.Any(x => x.CategoryName == "Di�er"))
                    {
                        Category diger = new Category
                        {
                            CategoryName = "Di�er",
                            Posts = new HashSet<Post>() // hashset ayn� �eyi bir daha ekletmez.Listte olur
                        };

                        for (int i = 1; i <= 77; i++)
                        {
                            diger.Posts.Add(new Post
                            {
                                Title = "Gezi Yaz�s� " + i,
                                AuthorId = admin.Id,
                                Content = @"<p>Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.</p>
<p>Placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante.</p>",
                                CreationTime = DateTime.Now.AddMinutes(i)
                            });
                        }

                        context.Categories.Add(diger);
                        

                    }
                }
            }


            #endregion


        }
    }
}
