using AToko.DataContexts;
using AToko.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AToko.Controllers
{
    public class AccountSupplierController : Controller
    {
     

        
         private  IdentityDb context = new IdentityDb();
        


        // GET: AccountSupplier
        [AllowAnonymous]
        public ActionResult AccountSupplier()
        {
            var a = (from b in context.Roles
                                 select new { b.Id});
                  

            ViewBag.Role = new SelectList (context.Roles.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountSupplier(AccountSupplier model)
        {
            if (ModelState.IsValid)
            {
                var userStore = new UserStore<IdentityUser>();
                var userManager = new UserManager<IdentityUser>(userStore);
               //var user = await userManager.FindAsync(model.Username, model.Password);

               var  user = new IdentityUser { UserName = model.Username };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync( user.Id, model.Role);
                    return RedirectToAction("Index", "Home");
                }



            }
            return View(model);
        }
        
        //[AllowAnonymous]
        //public ActionResult AccountSupplier()
        //{


        //    ViewBag.Role = new SelectList(context.Roles.ToList(), "Name", "Name");
        //    return View();
        //}

        ////
        //// POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AccountSupplier(AccountSupplier model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userStore = new UserStore<IdentityUser>();
        //        var userManager = new UserManager<IdentityUser>(userStore);
        //        //            var user = await userManager.FindAsync(model.Username, model.Password);

        //        //            user = new IdentityUser { UserName = model.Username };
        //        //            var result = await userManager.CreateAsync(user, model.Password);
        //        var user = new IdentityUser { UserName = model.Username };
        //        var result = await userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

        //            //Assign Role to user Here 
        //            await userManager.AddToRoleAsync(user.Id, model.Role);
        //            //Ends Here

        //            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //            // Send an email with this link
        //            // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //            // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

        //            return RedirectToAction("Index", "Home");
        //        }
        //       // AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}




    }
}