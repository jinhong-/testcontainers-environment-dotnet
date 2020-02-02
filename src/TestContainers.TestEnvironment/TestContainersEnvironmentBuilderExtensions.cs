using System;
using System.Linq;
using System.Reflection;
using Docker.DotNet;
using Microsoft.Extensions.DependencyInjection;
using TestContainers.Container.Abstractions;
using TestContainers.Container.Abstractions.Images;
using TestContainers.Container.Abstractions.Networks;

namespace TestContainers.TestEnvironment
{
    public static class TestContainersEnvironmentBuilderExtensions
    {
        public static ITestContainersEnvironmentBuilder RegisterContainer<TContainer>(
            this ITestContainersEnvironmentBuilder builder,
            string imageName,
            Action<TContainer> configureContainer = null)
            where TContainer : IContainer
            => builder.RegisterContainer(p =>
            {
                var image = ActivatorUtilities.CreateInstance<GenericImage>(p,
                    p.GetRequiredService<ITestEnvironmentContextAccessor>().Context.DockerClient);
                image.ImageName = imageName;
                return image;
            }, configureContainer);

        public static ITestContainersEnvironmentBuilder RegisterContainer<TContainer>(
            this ITestContainersEnvironmentBuilder builder,
            Func<IServiceProvider, IImage> createImage,
            Action<TContainer> configureContainer = null)
            where TContainer : IContainer
            => builder.RegisterContainer(p =>
            {
                var constructor = GetSuitableConstructorForActivation(typeof(TContainer));

                var additionalParameters = constructor.GetParameters()
                    .Select(parameter => (object) (parameter.ParameterType switch
                    {
                        var t when t == typeof(IImage) => createImage(p),
                        var t when t == typeof(IDockerClient) => p.GetRequiredService<ITestEnvironmentContextAccessor>()
                            .Context.DockerClient,
                        _ => null
                    }))
                    .Where(parameterValue => parameterValue != null).ToArray();

                var container = ActivatorUtilities.CreateInstance<TContainer>(p, additionalParameters);
                configureContainer?.Invoke(container);
                return container;
            });

        public static ITestContainersEnvironmentBuilder RegisterContainer(
            this ITestContainersEnvironmentBuilder builder,
            Func<IServiceProvider, IContainer> createContainer)
        {
            builder.Services.AddTransient(p =>
            {
                var container = createContainer(p);
                container.Network = p.GetService<INetwork>();
                return container;
            });
            return builder;
        }

        public static ITestContainersEnvironmentBuilder ConfigureNetwork(this ITestContainersEnvironmentBuilder builder,
            Func<IServiceProvider, IContainer> createNetwork)
        {
            builder.Services.AddSingleton(createNetwork);
            return builder;
        }

        private static ConstructorInfo GetSuitableConstructorForActivation(Type type)
        {
            int bestLength = -1;
            ConstructorInfo bestContructor = null;

            foreach (var constructor in type.GetConstructors()
                .Where(c => !c.IsStatic && c.IsPublic))
            {
                if (constructor.IsDefined(typeof(ActivatorUtilitiesConstructorAttribute), false))
                {
                    return constructor;
                }

                var length = constructor.GetParameters().Length;
                if (length > bestLength)
                {
                    bestLength = length;
                    bestContructor = constructor;
                }
            }

            return bestContructor;
        }
    }
}