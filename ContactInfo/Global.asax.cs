﻿using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using ContactInfo.EFModel;
using ContactInfo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ContactInfo
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RegisterAutofac();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);           
        }

        private void RegisterAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // manual registration of types;
            builder.RegisterType<ContactRepository>().As<IContactRepository>();

            //builder.RegisterType<UnitOfWork>.As<IUnitOfWork>();
            builder.RegisterType<ContactDBContext>();

            // For property injection using Autofac
            // builder.RegisterType<QuoteService>().PropertiesAutowired();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver =
                 new AutofacWebApiDependencyResolver(container);   
        }
    }
}
