﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch.Tallichet.WindMobile.Viewmodel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                if (!SimpleIoc.Default.IsRegistered<IMainViewModel>())
                {
                    // Create design time view services and models
                    SimpleIoc.Default.Register<IMainViewModel, Viewmodel.Design.DesignMainViewModel>();
                }
            }
            else
            {
                // Create runtime view services and models
                SimpleIoc.Default.Register<Service.NetworkService>();
                SimpleIoc.Default.Register<Service.LocationService>();
                SimpleIoc.Default.Register<IMainViewModel, Viewmodel.Runtime.MainViewModel>();
            }
        }

        public IMainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IMainViewModel>();
            }
        }

    }
}
