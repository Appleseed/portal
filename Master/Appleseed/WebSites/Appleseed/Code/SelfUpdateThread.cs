using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Appleseed.Framework;
using Microsoft.AspNet.SignalR;
using NuGet;
using SelfUpdater.Controllers;
using SelfUpdater.Models;

namespace Appleseed.Code
{
    public class SelfUpdateThread
    {

        public void CheckForSelfUpdates()
        {
            var sb = new StringBuilder();
            try
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<SelfUpdater.SignalR.SelfUpdaterHub>();

                var context = new SelfUpdaterEntities();

                var packagesToUpdate = context.SelfUpdatingPackages.AsQueryable();

                //updateNeeded = (packagesToUpdate.Count() > 0);

                if (packagesToUpdate.Any())
                {
                   
                    
                    var packagesToInstall = packagesToUpdate.Where(packages => packages.Install == true);

                    foreach (var package in packagesToInstall)
                    {
                        try
                        {
                            var projectManager =
                                GetProjectManagers().Where(p => p.SourceRepository.Source == package.Source).First();


                            hub.Clients.All.openPopUp();

                            projectManager.addLog("Installing " + package.PackageId);

                            ErrorHandler.Publish(LogLevel.Info,
                                                 String.Format("SelfUpdater: Installing {0} to version {1}",
                                                               package.PackageId, package.PackageVersion));

                            projectManager.InstallPackage(package.PackageId, new SemanticVersion(package.PackageVersion));

                            context.SelfUpdatingPackages.DeleteObject(package);

                            sb.AppendFormat("The package {0} version {1} was correctly installed", package.PackageId,
                                            package.PackageVersion);
                            sb.AppendLine();
                        }
                        catch(Exception e)
                        {
                            ErrorHandler.Publish(LogLevel.Error, e);
                            sb.AppendFormat("There was an error installing package {0} version {1} . Exception: {2}", package.PackageId,
                                            package.PackageVersion, e.Message);
                            sb.AppendLine();
                        }
                    }

                    var packagesToUpgrade = packagesToUpdate.Where(packages => packages.Install == false);
                    foreach (var package in packagesToUpgrade)
                    {
                        try
                        {
                            hub.Clients.All.openPopUp();

                            var projectManager =
                                GetProjectManagers().First(p => p.SourceRepository.Source == package.Source);

                            projectManager.addLog("Updating " + package.PackageId);

                            IPackage installedPackage = GetInstalledPackage(projectManager, package.PackageId);

                            IPackage update = projectManager.GetUpdatedPackage(installedPackage);

                            projectManager.UpdatePackage(update);

                            context.SelfUpdatingPackages.DeleteObject(package);

                            sb.AppendFormat("The package {0} version {1} was correctly updated", package.PackageId,
                                            update.Version);
                            sb.AppendLine();
                        }
                        catch(Exception e)
                        {
                            ErrorHandler.Publish(LogLevel.Error, e);
                            sb.AppendFormat("There was an error updating package {0} version {1} . Exception: {2}", package.PackageId,
                                            package.PackageVersion, e.Message);
                            sb.AppendLine();
                        }
                    }

                    //var pManager = GetProjectManagers().First();
                    //var installedPackages = GetInstalledPackages(pManager);
                    //var packagesList = installedPackages.GroupBy(x => x.Id);
                    //var installednotLatest = new List<IPackage>();
                    //foreach (var package in packagesList)
                    //{
                    //    var version = package.Max(x => x.Version);
                    //    installednotLatest.AddRange(package.Where(pack => pack.Version != version));
                    //}

                    //foreach (var package in installednotLatest)
                    //{
                    //    pManager.RemovePackageFromLocalRepository(package);
                    //}

                    context.SaveChanges();

                    var config = WebConfigurationManager.OpenWebConfiguration("~/");
                    var section = config.GetSection("system.web/httpRuntime");
                    ((HttpRuntimeSection)section).WaitChangeNotification = 10;
                    ((HttpRuntimeSection)section).MaxWaitChangeNotification = 10;
                    config.Save();

                    sb.AppendLine("All changes were applied succesfully");
                    
                }
                else
                {
                    ErrorHandler.Publish(LogLevel.Info, "SelfUpdater: Nothing to update");
                }


                hub.Clients.All.reloadPage();
                
            }
            catch (Exception exc)
            {

                ErrorHandler.Publish(LogLevel.Error, exc);

                sb.AppendFormat("There was an error on updating. Exception: {0}", exc.Message);
                sb.AppendLine();
                
            }

            try
            {
                var body = sb.ToString();
                var emailDir = ConfigurationManager.AppSettings["SelfUpdaterMailconfig"];
                if (!string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(emailDir))
                {
                    var mail = new MailMessage();
                    mail.From = new MailAddress("info@appleseedportal.net");

                   
                    mail.To.Add(new MailAddress(emailDir));
                    mail.Subject = string.Format("Updates Manager changes");

                    mail.Body = body;
                    mail.IsBodyHtml = false;

                    using (var client = new SmtpClient())
                    {
                        client.Send(mail);

                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandler.Publish(LogLevel.Error, e);
            }



        }


        private WebProjectManager[] GetProjectManagers()
        {
            var remoteSources = ConfigurationManager.AppSettings["PackageSource"] ?? @"D:\";
            var managers = new List<WebProjectManager>();
            foreach (var remoteSource in remoteSources.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                managers.Add(new WebProjectManager(remoteSource, HttpRuntime.AppDomainAppPath));
            }

            return managers.ToArray();
        }

        private IPackage GetInstalledPackage(WebProjectManager projectManager, string packageId)
        {
            IPackage package = projectManager.GetInstalledPackages(true).Where(d => d.Id == packageId).FirstOrDefault();

            if (package == null)
            {
                throw new InvalidOperationException(string.Format("The package for package ID '{0}' is not installed in this website. Copy the package into the App_Data/packages folder.", packageId));
            }
            return package;
        }

        private IEnumerable<IPackage> GetInstalledPackages(WebProjectManager projectManager)
        {
            var packages = projectManager.GetInstalledPackages(true);

            return packages;
        }

    }
}