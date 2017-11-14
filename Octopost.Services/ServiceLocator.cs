﻿namespace Octopost.Services
{
    using System;

    public static class ServiceLocator
    {
        private static Func<IServiceProvider> serviceProvider;

        public static void SetServiceLocator(Func<IServiceProvider> serviceLocator)
        {
            ServiceLocator.serviceProvider = serviceLocator;
        }

        public static IServiceProvider Instance => ServiceLocator.serviceProvider?.Invoke();
    }
}
