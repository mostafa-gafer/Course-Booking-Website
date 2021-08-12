using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using mvcIdentity.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(mvcIdentity.Startup))]
namespace mvcIdentity
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRoles();
            createUsers();
        }
        ///* create default user and roles when app starr up*/
        public void createRoles()
        {
            //role manager to manager role table 
            //identity role that object of role that is row we want to add
            //role store to make me store row object of identity role to database
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            //object from identity role 
            IdentityRole role ;
            //check if role exict or not for admins
            if (!roleManager.RoleExists("Admins"))
            {
                role = new IdentityRole();
                //create role name admin
                role.Name = "Admins";
                //add to role manager
                roleManager.Create(role);

            }
            //check if role exict or not for publisher
            if (!roleManager.RoleExists("publishers"))
            {
                role = new IdentityRole();
                //create role name apublishers
                role.Name = "publishers";
                //add to role manager
                roleManager.Create(role);

            }


        }
        public void createUsers()
        {
            //UserManager to manage user table
            //applicationuser object of user thah  a row we want to add
            //user store to store user row in database
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //to create obect from user
            ApplicationUser user = new ApplicationUser();
            user.Email = "mostafa.fathy85.mf@gmail.com";
            user.UserName = "mostafa";
            //inser email and password and check if exist or not
            var check = userManager.Create(user, "Mo@123");
            if (check.Succeeded)
            {
                //insert user to admins role
                userManager.AddToRole(user.Id, "Admins");
            }
        }

    }
}
